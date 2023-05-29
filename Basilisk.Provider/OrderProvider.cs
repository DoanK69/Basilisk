using Basilisk.DataAccess.Models;
using Basilisk.Repository;
using Basilisk.ViewModel.Customer;
using Basilisk.ViewModel.OrderDetails;
using Basilisk.ViewModel.Orders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.Provider
{
    public class OrderProvider : BaseProvider
    {
        private static OrderProvider _intance = new OrderProvider();
        public static CultureInfo indoFormat = CultureInfo.CreateSpecificCulture("id-ID");

        public static OrderProvider GetProvider()
        {
            return _intance;
        }

        private IEnumerable<GridOrderViewModel> GetDataIndex()
        {
            var order = OrderRepository.GetRepository().GetAll().ToList();
            var customer = CustomerRepository.GetRepository().GetAll().ToList();
            var orderDetail = OrderDetailRepository.GetRepository().GetAll().ToList();

            var indoFormat = CultureInfo.CreateSpecificCulture("id-ID");
            var orders = (from ord in order
                          join cus in customer on ord.CustomerId equals cus.Id
                          join ordet in orderDetail on ord.InvoiceNumber equals ordet.InvoiceNumber into joined
                          from result in joined.DefaultIfEmpty()
                          group result by new
                          {
                              ord.InvoiceNumber,
                              cus.CompanyName,
                              ord.OrderDate,
                              ord.DeliveryCost
                          } into grup
                          select new GridOrderViewModel
                          {
                              InvoiceNumber = grup.Key.InvoiceNumber,
                              CustomerName = grup.Key.CompanyName,
                              OrderDate = grup.Key.OrderDate.ToString("dd-MMMM-yyyy", indoFormat),
                              TotalPembayaran = grup.Sum(od => od == null ? 0 : (od.UnitPrice * od.Quantity) - ((od.Discount / 100) * od.UnitPrice * od.Quantity)) != 0 ? (grup.Sum(od => (od.UnitPrice * od.Quantity) - ((od.Discount / 100) * od.UnitPrice * od.Quantity)) + grup.Key.DeliveryCost).ToString("C0") : "Rp.0,00"
                          }).ToList();


            return orders;

        }

        public IndexOrderViewModel GetIndex(int page, string searchName)
        {
            IEnumerable<GridOrderViewModel> orders = GetDataIndex();

            int totalOrders = orders.Count();

            if (!string.IsNullOrEmpty(searchName))
            {
                orders = orders.Where(a => a.CustomerName.Contains(searchName));
            }

            int totalHalaman = (int)Math.Ceiling(orders.Count() / (decimal)TotalDataPerPage);
            int totalData = orders.Count();
            int skip = (TotalDataPerPage * (page - 1));

            orders = orders.Skip(skip).Take(TotalDataPerPage);

            var model = new IndexOrderViewModel
            {
                SearchName = searchName,
                GridOrder = orders,
                TotalHalaman = totalHalaman,
                TotalData = totalData,
                TotalOrder = totalOrders
            };

            return model;
        }

        public DetailOrderViewModel GetDetail(string invoiceNumber)
        {
            var order = OrderRepository.GetRepository().GetAll();
            var salesman = SalesmenRepository.GetRepository().GetAll();
            var orderDetail = OrderDetailRepository.GetRepository().GetAll();

            var indoFormat = CultureInfo.CreateSpecificCulture("id-ID");
            var orders = (from ord in order
                          where ord.InvoiceNumber == invoiceNumber
                          select new GridOrderViewModel
                          {
                              InvoiceNumber = invoiceNumber,
                              CustomerName = ord.Customer.CompanyName,
                              SalesName = ord.SalesEmployeeNumberNavigation.FirstName,
                              OrderDate = ord.OrderDate.ToString("dd MMMM yyyy", indoFormat),
                              ShippedDate = ord.ShippedDate,
                              StringShippedDate = ord.ShippedDate == null ? "Belum dikirim" : ord.ShippedDate.Value.ToString("dd MMMM yyyy", indoFormat),
                              DueDate = ord.DueDate,
                              StringDueDate = ord.DueDate == null ? "Belum diterima" : ord.DueDate.Value.ToString("dd MMMM yyyy", indoFormat),
                              DesAddress = ord.DestinationAddress,
                              DesCity = ord.DestinationCity,
                              DesPostalCode = ord.DestinationPostalCode,
                              DesCost = ord.DeliveryCost.ToString("C0", indoFormat)
                          }).SingleOrDefault();

            var orderDetails = (from ordet in orderDetail
                                where ordet.InvoiceNumber == invoiceNumber
                                select new GridOrderDetailsViewModel
                                {
                                    ProductName = ordet.Product.Name,
                                    Qty = ordet.Quantity,
                                    Harga = ordet.UnitPrice.ToString("C", indoFormat),
                                    Diskon = (ordet.Discount / 100).ToString("P0")
                                }).ToList();

            var model = new DetailOrderViewModel()
            {
                GridOrder = orders,
                GridOrderDetails = orderDetails
            };

            return model;

        }
    }
}
