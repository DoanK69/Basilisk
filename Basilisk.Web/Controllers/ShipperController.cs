using Microsoft.AspNetCore.Mvc;
using Basilisk.Provider;
using Basilisk.ViewModel.Deliveries;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Basilisk.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ShipperController : BaseController
    {
        public IActionResult Index(int page = 1, string searchName = "")
        {
            SetUsernameRole(User.Claims);
            var model = ShipperProvider.GetProvider().GetIndex(page, searchName);
            return View(model);
        }

        [HttpGet]
        public IActionResult Add()
        {
            SetUsernameRole(User.Claims);
            UpsertDeliveryViewModel model = new UpsertDeliveryViewModel();
            model.Judul = "Add Delivery";
            return View("Upsert", model);
        }

        [HttpGet]
        public IActionResult Edit(long idDelivery)
        {
            SetUsernameRole(User.Claims);
            var model = ShipperProvider.GetProvider().GetUpdate(idDelivery);
            return View("Upsert", model);
        }

        [HttpPost]
        public IActionResult Save(UpsertDeliveryViewModel model)
        {
            ShipperProvider.GetProvider().SetSave(model);
            return RedirectToAction("Index");
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult Delete(int idDelivery)
        {
            ShipperProvider.GetProvider().Delete(idDelivery);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Detail(long idDelivery)
        {
            SetUsernameRole(User.Claims);
            var model = ShipperProvider.GetProvider().GetDetail(idDelivery);
            return View("DetailOrder", model);
        }


        public IActionResult ShippedDate(string invoiceNumber, long idDelivery)
        {
            ShipperProvider.GetProvider().GetShippedDate(invoiceNumber);
            return RedirectToAction("Detail", "Shipper", new { idDelivery });
        }

        public IActionResult DetailInvoice(string invoiceNumber, long idDelivery)
        {

            var model = ShipperProvider.GetProvider().GetDetailInvoice(invoiceNumber, idDelivery);
            return View("DetailInvoiceDelivery", model);
        }
    }
}
