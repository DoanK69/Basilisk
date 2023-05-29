using Basilisk.DataAccess.Models;
using Basilisk.Provider;
using Basilisk.ViewModel;
using Basilisk.ViewModel.Customer;
using Basilisk.ViewModel.Deliveries;
using Basilisk.ViewModel.Orders;
using Basilisk.ViewModel.Product;
using Basilisk.ViewModel.Supplier;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Cryptography;

namespace Basilisk.Web.Controllers
{
    
    public class CustomerController : BaseController
    {
        [Authorize(Roles = "Administrator, Salesman")]
        public IActionResult Index(int page = 1, string searchName = "")
        {
            SetUsernameRole(User.Claims);
            var model = CustomerProvider.GetProvider().GetIndex(page, searchName);
            return View(model);
        }

        [Authorize(Roles = "Administrator, Salesman, Customer")]
        public IActionResult Cart(int id)
        {
            SetUsernameRole(User.Claims);
            if (GetRole(User.Claims) == "Customer")
            {
                id = Convert.ToInt32(GetUsername(User.Claims));
            }
            
            var model = CustomerProvider.GetProvider().GetCart(id);           
            return View("CartDetail", model);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Salesman, Customer")]
        public IActionResult AddProduct(long id)
        {
            SetUsernameRole(User.Claims);
            if (GetRole(User.Claims) == "Customer")
            {
                id = Convert.ToInt64(GetUsername(User.Claims));
            }
            var model = CustomerProvider.GetProvider().GetAddProduct(id);
            return View(model);
        }

        [AcceptVerbs("GET", "POST")]
        [Authorize(Roles = "Administrator, Salesman, Customer")]
        public IActionResult PlusQuantity(int idCart, long id)
        {
            if (GetRole(User.Claims) == "Customer")
            {
                id = Convert.ToInt64(GetUsername(User.Claims));
            }
            CustomerProvider.GetProvider().PlusQuantity(idCart);
            return RedirectToAction("Cart", "Customer", new { id });
        }

        [AcceptVerbs("GET", "POST")]
        [Authorize(Roles = "Administrator, Salesman, Customer")]
        public IActionResult MinusQuantity(int idCart, long id)
        {
            if (GetRole(User.Claims) == "Customer")
            {
                id = Convert.ToInt64(GetUsername(User.Claims));
            }
            CustomerProvider.GetProvider().MinusQuantity(idCart);
            return RedirectToAction("Cart", "Customer", new { id });
        }

        [AcceptVerbs("GET", "POST")]
        [Authorize(Roles = "Administrator, Salesman, Customer")]
        public IActionResult Delete(int idCart, long id)
        {
            if (GetRole(User.Claims) == "Customer")
            {
                id = Convert.ToInt64(GetUsername(User.Claims));
            }
            CustomerProvider.GetProvider().Delete(idCart);
            return RedirectToAction("Cart", "Customer", new { id });
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Salesman, Customer")]
        public IActionResult AddProduct(long id, long idProd)
        {
            if (GetRole(User.Claims) == "Customer")
            {
                id = Convert.ToInt64(GetUsername(User.Claims));
            }
            CustomerProvider.GetProvider().PostAddProduct(id, idProd);            
            return RedirectToAction("AddProduct", "Customer", new {id});
        }

        [Authorize(Roles = "Administrator, Salesman, Customer")]
        public IActionResult CheckOut(IndexCartViewModel model)
        {
            SetUsernameRole(User.Claims);
            if (model.DetailCart != null)
            {
                DetailCheckOutViewModel modelCheckout = new DetailCheckOutViewModel();

                modelCheckout = CustomerProvider.GetProvider().GetDetailCheckOut(model, modelCheckout);

                return View("DetailCheckOut", modelCheckout);
            }
            else
            {
                long id = model.CustomerId;
                return RedirectToAction("Cart", "Customer", new { id });
            }            
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Salesman, Customer")]
        public IActionResult CheckOutDetail(DetailCheckOutViewModel model)
        {
            CustomerProvider.GetProvider().PostCheckOutDetail(model);
            long id = model.CustomerId;
            return RedirectToAction("Cart", "Customer", new { id });
        }

        [Authorize(Roles = "Administrator, Salesman, Customer")]
        public IActionResult CheckedCart(long id, bool status, int idCart)
        {
            if (GetRole(User.Claims) == "Customer")
            {
                id = Convert.ToInt64(GetUsername(User.Claims));
            }
            CustomerProvider.GetProvider().GetChecked(status, idCart);
            return RedirectToAction("Cart", new { id });
        }

        [Authorize(Roles = "Administrator, Salesman, Customer")]
        public IActionResult DetailCustomerOrder(long id)
        {
            SetUsernameRole(User.Claims);
            if (GetRole(User.Claims) == "Customer")
            {
                id = Convert.ToInt64(GetUsername(User.Claims));
            }
            var model = CustomerProvider.GetProvider().GetDetailCustomerOrder(id);
            return View(model);
        }

        [Authorize(Roles = "Administrator, Salesman, Customer")]
        public IActionResult DetailSalesmenOrder(string empNumber)
        {
            SetUsernameRole(User.Claims);
            if (GetRole(User.Claims) == "Salesman")
            {
                empNumber = GetUsername(User.Claims);
            }
            var model = CustomerProvider.GetProvider().GetDetailSalesmenOrder(empNumber);
            return View(model);
        }

        [Authorize(Roles = "Administrator, Salesman, Customer")]
        public IActionResult DueDate(string invoiceNumber, long id)
        {
            CustomerProvider.GetProvider().SetDueDate(invoiceNumber);
            if (GetRole(User.Claims) == "Customer")
            {
                id = Convert.ToInt64(GetUsername(User.Claims));
            }
            return RedirectToAction("DetailCustomerOrder", "Customer", new { id });
        }

        [Authorize(Roles = "Administrator, Salesman, Customer")]
        public IActionResult DetailInvoice(string invoiceNumber, long id)
        {
            if (GetRole(User.Claims) == "Customer")
            {
                id = Convert.ToInt64(GetUsername(User.Claims));
            }
            var model = CustomerProvider.GetProvider().GetDetailInovice(invoiceNumber, id);
            return View(model);
        }

        //public IActionResult DeliveryCost(long id)
        //{
        //    var cost = _context.Deliveries.SingleOrDefault(d => d.Id == id);
        //    return RedirectToAction("Cart");           
        //}
    }
}
