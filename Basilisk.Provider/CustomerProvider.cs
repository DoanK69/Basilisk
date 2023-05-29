using Basilisk.DataAccess.Models;
using Basilisk.Repository;
using Basilisk.ViewModel;
using Basilisk.ViewModel.Customer;
using Basilisk.ViewModel.Deliveries;
using Basilisk.ViewModel.Orders;
using Basilisk.ViewModel.Region;
using Basilisk.ViewModel.Salesmen;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Basilisk.Provider
{
    public class CustomerProvider : BaseProvider
    {
        private static CustomerProvider _intance = new CustomerProvider();
        public static CultureInfo indoFormat = CultureInfo.CreateSpecificCulture("id-ID");

        public static CustomerProvider GetProvider()
        {
            return _intance;
        }

        private IEnumerable<GridCustomerViewModel> GetDataIndex()
        {
            var customer = CustomerRepository.GetRepository().GetAll().ToList();
            var carts = CartRepository.GetRepository().GetAll().ToList();
            var customers = (from cus in customer
                             join cart in carts on cus.Id equals cart.CustomerId into cusCart
                             from result in cusCart.DefaultIfEmpty()
                             group result by new
                             {
                                 cus.Id,
                                 cus.CompanyName,
                                 cus.ContactPerson,
                                 cus.Address,
                                 cus.City,
                                 cus.Email,
                                 cus.Phone
                             } into grp
                             select new GridCustomerViewModel
                             {
                                 Id = grp.Key.Id,
                                 CompanyName = grp.Key.CompanyName,
                                 ContactPerson = grp.Key.ContactPerson,
                                 Address = grp.Key.Address,
                                 City = grp.Key.City,
                                 Email = grp.Key.Email,
                                 Phone = grp.Key.Phone,
                                 TotalProduct = grp.Sum(q => q == null ? 0 : q.Quantity)
                             }).ToList();

            return customers;

        }

        public IndexCustomerViewModel GetIndex(int page, string searchName)
        {
            IEnumerable<GridCustomerViewModel> customer = GetDataIndex();

            int totalCustomer = customer.Count();

            if (!string.IsNullOrEmpty(searchName))
            {
                customer = customer.Where(a => a.CompanyName.Contains(searchName));
            }

            int totalHalaman = (int)Math.Ceiling(customer.Count() / (decimal)TotalDataPerPage);
            int totalData = customer.Count();
            int skip = (TotalDataPerPage * (page - 1));

            customer = customer.Skip(skip).Take(TotalDataPerPage);

            var model = new IndexCustomerViewModel
            {
                SearchName = searchName,
                Grid = customer,
                TotalCustomer = totalCustomer,
                TotalData = totalData,
                TotalHalaman = totalHalaman
            };

            return model;
        }

        public List<DetailCartViewModel> GetDataCart(long id)
        {
            var product = ProductRepository.GetRepository().GetAll().ToList();
            var carts = CartRepository.GetRepository().GetAll().ToList();
            var supplier = SupplierRepository.GetRepository().GetAll().ToList();

            var indoFormat = CultureInfo.CreateSpecificCulture("id-ID");
            var keranjang = (from prod in product
                             join cart in carts on prod.Id equals cart.ProductId
                             join sup in supplier on prod.SupplierId equals sup.Id into b
                             from sup in b.DefaultIfEmpty()
                             where cart.CustomerId == id
                             group sup by new { sup.CompanyName, sup.Id, cart.CustomerId } into grp
                             select new DetailCartViewModel
                             {
                                 //CustomerId = grp.Key.CustomerId,
                                 CompanyName = grp.Key.CompanyName,
                                 Products = (from prod in product
                                             join cart in carts on prod.Id equals cart.ProductId
                                             where prod.SupplierId == grp.Key.Id
                                             select new DetailProductViewModel
                                             {
                                                 ProductId = prod.Id,
                                                 CartId = cart.Id,
                                                 ProductName = prod.Name,
                                                 Price = prod.Price,
                                                 StringPrice = prod.Price.ToString("C", indoFormat),
                                                 Qty = cart.Quantity,
                                                 Checked = cart.Checked,
                                                 TotalHarga = cart.Checked == true ? prod.Price * cart.Quantity : 0
                                             }).ToList()
                                 //ChekedAll = (carts.Where(c => c.Checked == false && c.CustomerId == id && c.Product.SupplierId == grp.Key.Id).Count()) ==
                                 //           (carts.Where(c => c.CustomerId == id && c.Product.SupplierId == grp.Key.Id).Count()) ? false : true
                             }).ToList();
            return keranjang;

        }

        public IndexCartViewModel GetCart(int id)
        {

            List<DetailCartViewModel> keranjang = GetDataCart(id);

            decimal totalHarga = 0m;
            foreach (var item in keranjang)
            {
                foreach (var prod in item.Products)
                {
                    totalHarga += prod.TotalHarga;
                }
            }

            var model = new IndexCartViewModel
            {
                CustomerId = id,
                DetailCart = keranjang,
                TotalHarga = totalHarga,
                StringTotalHarga = totalHarga.ToString("C", indoFormat)
            };

            return model;

        }

        public IndexProductCartViewModel GetAddProduct(long id)
        {

            var product = ProductRepository.GetRepository().GetAll().ToList();
            var carts = CartRepository.GetRepository().GetAll().ToList();

            var indoFormat = CultureInfo.CreateSpecificCulture("id-ID");
            var products = (from prod in product
                            select new ProductCartViewModel
                            {
                                CustomerId = id,
                                ProductId = prod.Id,
                                ProductName = prod.Name,
                                Harga = prod.Price.ToString("C", indoFormat)
                            }).ToList();

            var model = new IndexProductCartViewModel
            {
                CustomerId = id,
                ProductCart = products
            };

            return model;

        }

        public void PlusQuantity(int idCart)
        {
            var carts = CartRepository.GetRepository().GetSingle(idCart);
            var product = ProductRepository.GetRepository().GetSingle(carts.ProductId);

            string error = "";

            if (carts.Quantity >= product.Stock)
            {
                error = "Stock ga cukup";
            }
            else
            {
                carts.Quantity++;
                CartRepository.GetRepository().Update(carts);
            }

        }

        public void MinusQuantity(int idCart)
        {

            var cart = CartRepository.GetRepository().GetSingle(idCart);
            if (cart.Quantity <= 1)
            {
                CartRepository.GetRepository().Delete(cart.Id);
            }
            else
            {
                cart.Quantity--;
            }
            CartRepository.GetRepository().Update(cart);

        }

        public void Delete(int idCart)
        {
            var cart = CartRepository.GetRepository().GetSingle(idCart);
            CartRepository.GetRepository().Delete(cart.Id);
        }

        public void PostAddProduct(long id, long idProd)
        {
            var carts = CartRepository.GetRepository().GetAll().ToList();

            var cekKeranjang = carts.Any(c => c.CustomerId == id && idProd == c.ProductId);
            if (!cekKeranjang)
            {
                var cart = new Cart();
                cart.CustomerId = id;
                cart.ProductId = idProd;
                cart.Quantity = 1;
                cart.Checked = false;

                CartRepository.GetRepository().Insert(cart);
            }
            else
            {
                var cart = carts.SingleOrDefault(c => c.CustomerId == id && idProd == c.ProductId);
                cart.Quantity++;
                CartRepository.GetRepository().Update(cart);
            }

        }

        public void PostCheckOutDetail(DetailCheckOutViewModel model)
        {
            //using (var transaction = new TransactionScope())
            //{
            try
            {
                TransactionRepository.TransactionOrder(model);
                //var carts = CartRepository.GetRepository().GetAll().ToList();
                //var product = ProductRepository.GetRepository().GetAll().ToList();

                //var orders = OrderRepository.GetRepository().GetAll().ToList();
                //var delivery = ShipperRepository.GetRepository().GetSingle(model.DeliveryId);
                //var orderDetail = OrderDetailRepository.GetRepository().GetAll().ToList();

                //string invoiceNumber = "";
                //DateTime tanggal = DateTime.Now;
                //string bulanTahun = tanggal.ToString("MM-yy");
                //bool cekBulanTahun = orders.Any(o => (bulanTahun == o.InvoiceNumber.Substring(0, 5)));
                //var lastOrder = orders.Where(o => o.OrderDate.Year == tanggal.Year && o.OrderDate.Month == tanggal.Month).OrderByDescending(a => a.InvoiceNumber).FirstOrDefault();
                //string lastNum = lastOrder.InvoiceNumber.Substring(6, 4);

                //if (cekBulanTahun)
                //{
                //    invoiceNumber = $"{bulanTahun}-{(int.Parse(lastNum) + 1).ToString("D4")}";
                //}
                //else
                //{
                //    invoiceNumber = $"{bulanTahun}-0001";
                //}

                //Order order = new Order
                //{
                //    InvoiceNumber = invoiceNumber,
                //    CustomerId = model.CustomerId,
                //    SalesEmployeeNumber = "J101",
                //    OrderDate = DateTime.Now,
                //    DeliveryId = model.DeliveryId,
                //    DeliveryCost = delivery.Cost,
                //    DestinationAddress = model.DestinationAddress,
                //    DestinationCity = model.DestinationCity,
                //    DestinationPostalCode = model.DestinationPostalCode
                //};
                //OrderRepository.GetRepository().Insert(order);
                ////int[] numbers = { 1, 2, 3 };
                ////int value = numbers[3];
                //for (int index = 0; index < model.CartList.Count(); index++)
                //{
                //    for (int prod = 0; prod < model.CartList[index].Products.Count(); prod++)
                //    {
                //        var ordet = new OrderDetail
                //        {
                //            InvoiceNumber = invoiceNumber,
                //            ProductId = model.CartList[index].Products[prod].ProductId,
                //            UnitPrice = model.CartList[index].Products[prod].Price,
                //            Quantity = model.CartList[index].Products[prod].Qty,
                //            Discount = 0
                //        };

                //        var produk = ProductRepository.GetRepository().GetSingle(model.CartList[index].Products[prod].ProductId);
                //        produk.Stock -= model.CartList[index].Products[prod].Qty;
                //        OrderDetailRepository.GetRepository().Insert(ordet);

                //        var cart = CartRepository.GetRepository().GetAll().SingleOrDefault(a => a.CustomerId == model.CustomerId && a.ProductId == model.CartList[index].Products[prod].ProductId);
                //        CartRepository.GetRepository().Delete(cart.Id);
                //    }
                //}
                //transaction.Complete();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void GetChecked(bool status, int idCart)
        {
            var checkCart = CartRepository.GetRepository().GetSingle(idCart);
            checkCart.Checked = status;
            CartRepository.GetRepository().Update(checkCart);
        }

        public DetailCheckOutViewModel GetDetailCheckOut(IndexCartViewModel model, DetailCheckOutViewModel modelCheckout)
        {
            List<DetailCartViewModel> detailCarts = new List<DetailCartViewModel>();

            foreach (var detail in model.DetailCart)
            {
                var prodChecked = detail.Products.Where(p => p.Checked == true).ToList();
                if (prodChecked.Count() != 0)
                {
                    var newCart = new DetailCartViewModel();
                    newCart.CustomerId = detail.CustomerId;
                    newCart.CompanyName = detail.CompanyName;
                    newCart.ChekedAll = detail.ChekedAll;
                    newCart.Products = prodChecked;

                    detailCarts.Add(newCart);
                }
            }

            modelCheckout.CartList = detailCarts;
            modelCheckout.CustomerId = model.CustomerId;
            modelCheckout.DropDownDelivery = GetDelivery();
            modelCheckout.StringTotalHarga = model.TotalHarga.ToString("C", indoFormat);
            return modelCheckout;
        }

        public void SetDueDate(string invoiceNumber)
        {
            var order = OrderRepository.GetRepository().GetAll().ToList();
            var orderSingle = OrderRepository.GetRepository().GetAll();

            var ord = order.Any(o => o.DueDate == null && o.InvoiceNumber == invoiceNumber && o.ShippedDate != null);
            if (ord)
            {
                var orders = orderSingle.SingleOrDefault(o => o.DueDate == null && o.InvoiceNumber == invoiceNumber && o.ShippedDate != null);
                orders.DueDate = DateTime.Now;
                OrderRepository.GetRepository().Update(orders);
            }

        }

        public DetailDeliveryViewModel GetDetailInovice(string invoiceNumber, long id)
        {
            var orders = OrderRepository.GetRepository().GetAll().ToList();
            var deliveries = ShipperRepository.GetRepository().GetAll().ToList();
            var ordets = OrderDetailRepository.GetRepository().GetAll().ToList();
            var products = ProductRepository.GetRepository().GetAll().ToList();


            var model = new DetailDeliveryViewModel
            {
                Delivery = (from del in deliveries
                            join ord in orders on del.Id equals ord.DeliveryId
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

                ListOrder = (from ord in orders
                             join ordet in ordets on ord.InvoiceNumber equals ordet.InvoiceNumber
                             join prod in products on ordet.ProductId equals prod.Id
                             //from prod in grp.DefaultIfEmpty()
                             where ord.InvoiceNumber == invoiceNumber
                             //group prod by new { ord.OrderDate, ordet.InvoiceNumber, prod.Name, ord.ShippedDate, ord.DueDate, ord.Delivery.CompanyName } into grup
                             select new GridOrderViewModel
                             {
                                 InvoiceNumber = ord.InvoiceNumber,
                                 ProductName = prod.Name,
                                 ShippedDate = ord.ShippedDate,
                                 StringShippedDate = ord.ShippedDate == null ? "Belum Dikirim" : ord.ShippedDate.Value.ToString("dd-MMMM-yyyy", new CultureInfo("id-ID")),
                                 StringDueDate = ord.DueDate == null ? "Belum Diterima" : ord.DueDate.Value.ToString("dd-MMMM-yyyy", new CultureInfo("id-ID")),
                                 OrderDate = ord.OrderDate.ToString("dd-MMMM-yyyy", new CultureInfo("id-ID"))
                             }).ToList(),

                CustomerId = id
            };
            return model;

        }

        public DetailOrderCustomerViewModel GetDetailCustomerOrder(long id)
        {
            var customer = CustomerRepository.GetRepository().GetAll().ToList();
            var order = OrderRepository.GetRepository().GetAll().ToList();

            var model = new DetailOrderCustomerViewModel
            {
                Customer = (from cus in customer
                            where cus.Id == id
                            select new GridCustomerViewModel
                            {
                                Id = id,
                                CompanyName = cus.CompanyName,
                                ContactPerson = cus.ContactPerson,
                                Address = cus.Address,
                                City = cus.City,
                                Phone = cus.Phone
                            }).SingleOrDefault(c => c.Id == id),

                Order = (from ord in order
                         where ord.CustomerId == id
                         select new GridOrderViewModel
                         {
                             InvoiceNumber = ord.InvoiceNumber,
                             OrderDate = ord.OrderDate.ToString("dd-MMMM-yyyy", new CultureInfo("id-ID")),
                             ShippedDate = ord.ShippedDate,
                             StringShippedDate = ord.ShippedDate == null ? "Belum dikirim" : ord.ShippedDate.Value.ToString("dd-MMMM-yyyy", new CultureInfo("id-ID")),
                             DueDate = ord.DueDate,
                             StringDueDate = ord.DueDate == null ? "Belum diterima" : ord.DueDate.Value.ToString("dd-MMMM-yyyy", new CultureInfo("id-ID"))
                         }).ToList()
            };

            return model;

        }

        public DetailOrderCustomerViewModel GetDetailSalesmenOrder(string empNumber)
        {
            var salesmen = SalesmenRepository.GetRepository().GetAll().ToList();
            var order = OrderRepository.GetRepository().GetAll().ToList();

            var model = new DetailOrderCustomerViewModel
            {
                Salesmen = (from sal in salesmen
                            where sal.EmployeeNumber == empNumber
                            select new GridSalesmenViewModel
                            {
                                EmployeeNumber = empNumber,
                                FirstName = sal.FirstName,
                                LastName = sal.LastName
                            }).SingleOrDefault(s => s.EmployeeNumber == empNumber),

                Order = (from ord in order
                         where ord.SalesEmployeeNumber == empNumber
                         select new GridOrderViewModel
                         {
                             InvoiceNumber = ord.InvoiceNumber,
                             OrderDate = ord.OrderDate.ToString("dd-MMMM-yyyy", new CultureInfo("id-ID")),
                             ShippedDate = ord.ShippedDate,
                             StringShippedDate = ord.ShippedDate == null ? "Belum dikirim" : ord.ShippedDate.Value.ToString("dd-MMMM-yyyy", new CultureInfo("id-ID")),
                             DueDate = ord.DueDate,
                             StringDueDate = ord.DueDate == null ? "Belum diterima" : ord.DueDate.Value.ToString("dd-MMMM-yyyy", new CultureInfo("id-ID"))
                         }).ToList()
            };

            return model;

        }

        public List<DropDownListViewModel> GetDelivery()
        {
            var deliveries = ShipperRepository.GetRepository().GetAll().ToList();

            var result = deliveries.Select(a => new DropDownListViewModel
            {
                LongValue = a.Id,
                Text = $"{a.CompanyName} ({a.Cost.ToString("C", new CultureInfo("id-ID"))})"
            }).ToList();

            return result;


        }


    }
}
