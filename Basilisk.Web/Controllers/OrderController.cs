using Basilisk.DataAccess.Models;
using Basilisk.Provider;
using Basilisk.ViewModel.OrderDetails;
using Basilisk.ViewModel.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Basilisk.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class OrderController : BaseController
    {
        public IActionResult Index(int page = 1,string searchName = "")
        {
            SetUsernameRole(User.Claims);
            var model = OrderProvider.GetProvider().GetIndex(page, searchName);
            return View(model);
        }

        public IActionResult Detail(string invoiceNumber)
        {
            SetUsernameRole(User.Claims);
            var model = OrderProvider.GetProvider().GetDetail(invoiceNumber);
            return View(model);
        }
    }
}
