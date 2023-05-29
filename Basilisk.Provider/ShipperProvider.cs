using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basilisk.DataAccess.Models;
using Basilisk.Repository;
using Basilisk.ViewModel.Category;
using Basilisk.ViewModel.Deliveries;
using Basilisk.ViewModel.Orders;
using Basilisk.ViewModel.Region;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Basilisk.Provider
{
    public class ShipperProvider : BaseProvider
    {
        private static ShipperProvider _intance = new ShipperProvider();
        public static CultureInfo indoFormat = CultureInfo.CreateSpecificCulture("id-ID");

        public static string judul = "Delivery Form";
        public static ShipperProvider GetProvider()
        {
            return _intance;
        }

        private IEnumerable<GridDeliveryViewModel> GetDataIndex(string searchName)
        {
            var delivery = ShipperRepository.GetRepository().GetAll().ToList();
            var shipper = (from del in delivery
                           where del.CompanyName.Contains(searchName)
                           select new GridDeliveryViewModel
                           {
                               Id = del.Id,
                               CompanyName = del.CompanyName,
                               Phone = del.Phone,
                               Cost = del.Cost,
                               StringCost = del.Cost.ToString("C2", indoFormat)
                           }).ToList();
            return shipper;

        }

        public IndexDeliveryViewModel GetIndex(int page, string searchName)
        {
            searchName = string.IsNullOrEmpty(searchName) ? "" : searchName;

            IEnumerable<GridDeliveryViewModel> data = GetDataIndex(searchName);
            int totalDelivery = data.Count();
            int totalData = data.Count();
            data = data.Skip(GetSkip(page)).Take(TotalDataPerPage);

            var model = new IndexDeliveryViewModel
            {
                SearchName = searchName,
                GridDelivery = data,
                TotalData = totalData,
                TotalHalaman = GetHalaman(totalData),
                TotalDelivery = totalDelivery,
                Judul = judul
            };

            return model;
        }

        public UpsertDeliveryViewModel GetUpdate(long idDelivery)
        {
            var delivery = ShipperRepository.GetRepository().GetSingle(idDelivery);
            var oldDelivery = delivery;
            var model = new UpsertDeliveryViewModel
            {
                Id = idDelivery,
                CompanyName = oldDelivery.CompanyName,
                Phone = oldDelivery.Phone,
                Cost = oldDelivery.Cost,
                Judul = judul
            };
            return model;

        }


        public void SetSave(UpsertDeliveryViewModel model)
        {


            try
            {
                if (model.Id == 0)
                {
                    var delivery = new Delivery()
                    {
                        CompanyName = model.CompanyName,
                        Phone = model.Phone,
                        Cost = model.Cost
                    };
                    ShipperRepository.GetRepository().Insert(delivery);
                }
                else
                {
                    var oldDelivery = ShipperRepository.GetRepository().GetSingle(model.Id);
                    MapingModel(oldDelivery, model);

                    ShipperRepository.GetRepository().Update(oldDelivery);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void Delete(long idDelivery)
        {

            try
            {
                var entityModel = ShipperRepository.GetRepository().GetSingle(idDelivery);
                ShipperRepository.GetRepository().Delete(entityModel);
            }
            catch
            {
                throw;
            }

        }

        public DetailDeliveryViewModel GetDetail(long idDelivery)
        {

            var delivery = ShipperRepository.GetRepository().GetAll().ToList();
            var order = OrderRepository.GetRepository().GetAll().ToList();
            var customer = CustomerRepository.GetRepository().GetAll().ToList();
            var model = new DetailDeliveryViewModel
            {
                Delivery = (from d in delivery
                            where d.Id == idDelivery
                            select new GridDeliveryViewModel
                            {
                                Id = d.Id,
                                CompanyName = d.CompanyName,
                                Cost = d.Cost,
                                Phone = d.Phone
                            }).SingleOrDefault(),

                ListOrder = (from ord in order
                             join c in customer on ord.CustomerId equals c.Id
                             where ord.DeliveryId == idDelivery
                             select new GridOrderViewModel
                             {
                                 CustomerName = c.CompanyName,
                                 InvoiceNumber = ord.InvoiceNumber,
                                 OrderDate = ord.OrderDate.ToString("dd-MMMM-yyyy", indoFormat),
                                 DesAddress = ord.DestinationAddress,
                                 ShippedDate = ord.ShippedDate,
                                 StringShippedDate = ord.ShippedDate == null ? "Belum Dikirim" : ord.ShippedDate.Value.ToString("dd-MMMM-yyyy", new CultureInfo("id-ID")),
                                 StringDueDate = ord.DueDate == null ? "Belum Diterima" : ord.DueDate.Value.ToString("dd-MMMM-yyyy", new CultureInfo("id-ID")),
                                 DesCity = ord.DestinationCity,
                                 Ongkir = ord.DeliveryCost,
                                 DesCost = ord.DeliveryCost.ToString("C", indoFormat)
                             }).ToList()
            };

            decimal totalUangMasuk = 0m;
            foreach (var item in model.ListOrder)
            {
                totalUangMasuk += item.Ongkir;
            }

            model.TotalUangMasuk = totalUangMasuk;

            return model;

        }

        public void GetShippedDate(string invoiceNumber)
        {
            var order = OrderRepository.GetRepository().GetAll().ToList();
            var ord = order.Any(o => o.ShippedDate == null && o.InvoiceNumber == invoiceNumber);
            if (ord)
            {
                var orders = order.SingleOrDefault(o => o.ShippedDate == null && o.InvoiceNumber == invoiceNumber);
                orders.ShippedDate = DateTime.Now;
                OrderRepository.GetRepository().Update(orders);
            }


        }

        public DetailDeliveryViewModel GetDetailInvoice(string invoiceNumber, long idDelivery)
        {
            var delivery = ShipperRepository.GetRepository().GetAll().ToList();
            var customer = CustomerRepository.GetRepository().GetAll().ToList();
            var order = OrderRepository.GetRepository().GetAll().ToList();
            var orderDetail = OrderDetailRepository.GetRepository().GetAll().ToList();
            var product = ProductRepository.GetRepository().GetAll().ToList();

            var model = new DetailDeliveryViewModel
            {
                Delivery = (from del in delivery
                            join ord in order on del.Id equals ord.DeliveryId
                            where ord.InvoiceNumber == invoiceNumber
                            select new GridDeliveryViewModel
                            {
                                InvoiceNumber = invoiceNumber,
                                Id = del.Id,
                                CompanyName = del.CompanyName,
                                Phone = del.Phone,
                                Cost = ord.DeliveryCost,
                                StringCost = ord.DeliveryCost.ToString("C", new CultureInfo("id-ID")),
                            }).SingleOrDefault(),

                ListOrder = (from ord in order
                             join ordet in orderDetail on ord.InvoiceNumber equals ordet.InvoiceNumber
                             join prod in product on ordet.ProductId equals prod.Id
                             where ord.InvoiceNumber == invoiceNumber
                             select new GridOrderViewModel
                             {
                                 InvoiceNumber = ordet.InvoiceNumber,
                                 ProductName = prod.Name,
                                 ShippedDate = ord.ShippedDate,
                                 StringShippedDate = ord.ShippedDate == null ? "Belum Dikirim" : ord.ShippedDate.Value.ToString("dd-MMMM-yyyy", new CultureInfo("id-ID")),
                                 StringDueDate = ord.DueDate == null ? "Belum Diterima" : ord.DueDate.Value.ToString("dd-MMMM-yyyy", new CultureInfo("id-ID")),
                                 OrderDate = ord.OrderDate.ToString("dd-MMMM-yyyy", new CultureInfo("id-ID"))
                             }).ToList()
            };

            decimal totalUangMasuk = 0m;
            foreach (var item in model.ListOrder)
            {
                totalUangMasuk += item.Ongkir;
            }

            model.TotalUangMasuk = totalUangMasuk;

            return model;

        }
    }
}
