using Basilisk.DataAccess.Models;
using Basilisk.ViewModel.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.Repository
{
    public class TransactionRepository
    {
        public static void TransactionOrder(DetailCheckOutViewModel model)
        {
            using (BasiliskTFContext _context = new BasiliskTFContext())
            {
                _context.Database.BeginTransaction();
                try
                {
                    var carts = CartRepository.GetRepository().GetAll().ToList();
                    var product = ProductRepository.GetRepository().GetAll().ToList();

                    var orders = OrderRepository.GetRepository().GetAll().ToList();
                    var delivery = ShipperRepository.GetRepository().GetSingle(model.DeliveryId);
                    var orderDetail = OrderDetailRepository.GetRepository().GetAll().ToList();

                    string invoiceNumber = "";
                    DateTime tanggal = DateTime.Now;
                    string bulanTahun = tanggal.ToString("MM-yy");
                    bool cekBulanTahun = orders.Any(o => (bulanTahun == o.InvoiceNumber.Substring(0, 5)));
                    var lastOrder = orders.Where(o => o.OrderDate.Year == tanggal.Year && o.OrderDate.Month == tanggal.Month).OrderByDescending(a => a.InvoiceNumber).FirstOrDefault();
                    string lastNum = lastOrder.InvoiceNumber.Substring(6, 4);

                    if (cekBulanTahun)
                    {
                        invoiceNumber = $"{bulanTahun}-{(int.Parse(lastNum) + 1).ToString("D4")}";
                    }
                    else
                    {
                        invoiceNumber = $"{bulanTahun}-0001";
                    }

                    Order order = new Order
                    {
                        InvoiceNumber = invoiceNumber,
                        CustomerId = model.CustomerId,
                        SalesEmployeeNumber = "J101",
                        OrderDate = DateTime.Now,
                        DeliveryId = model.DeliveryId,
                        DeliveryCost = delivery.Cost,
                        DestinationAddress = model.DestinationAddress,
                        DestinationCity = model.DestinationCity,
                        DestinationPostalCode = model.DestinationPostalCode
                    };
                    OrderRepository.GetRepository().Insert(order);
                    //int[] numbers = { 1, 2, 3 };
                    //int value = numbers[3];
                    for (int index = 0; index < model.CartList.Count(); index++)
                    {
                        for (int prod = 0; prod < model.CartList[index].Products.Count(); prod++)
                        {
                            var ordet = new OrderDetail
                            {
                                InvoiceNumber = invoiceNumber,
                                ProductId = model.CartList[index].Products[prod].ProductId,
                                UnitPrice = model.CartList[index].Products[prod].Price,
                                Quantity = model.CartList[index].Products[prod].Qty,
                                Discount = 0
                            };

                            var produk = ProductRepository.GetRepository().GetSingle(model.CartList[index].Products[prod].ProductId);
                            produk.Stock -= model.CartList[index].Products[prod].Qty;
                            OrderDetailRepository.GetRepository().Insert(ordet);

                            var cart = CartRepository.GetRepository().GetAll().SingleOrDefault(a => a.CustomerId == model.CustomerId && a.ProductId == model.CartList[index].Products[prod].ProductId);
                            CartRepository.GetRepository().Delete(cart.Id);
                        }
                    }
                    _context.Database.CommitTransaction();
                }
                catch (Exception)
                {
                    _context.Database.RollbackTransaction();
                    throw;
                }
                
            }
        }
    }
}
