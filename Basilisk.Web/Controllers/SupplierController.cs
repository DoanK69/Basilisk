using Basilisk.DataAccess.Models;
using Basilisk.Provider;
using Basilisk.ViewModel.Category;
using Basilisk.ViewModel.Product;
using Basilisk.ViewModel.Supplier;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Basilisk.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class SupplierController : BaseController
    {
        
        public IActionResult Index(int page = 1,string searchName = "")
        {
            SetUsernameRole(User.Claims);
            var model = SupplierProvider.GetProvider().GetIndex(page, searchName);
            return View(model);
        }

        public IActionResult Detail(int id)
        {
            SetUsernameRole(User.Claims);
            var model = SupplierProvider.GetProvider().GetDetail(id);
            return View("DetailSupplier", model);
        }

        public IActionResult DetailModal(int id)
        {
            var model = SupplierProvider.GetProvider().GetDetail(id);
            return Json(model);
        }

        [HttpGet]
        public IActionResult Add()
        {
            SetUsernameRole(User.Claims);
            var model = new CreateUpdateSupplierViewModel();
            return View("AddSupplier", model);
        }

        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public IActionResult Add(CreateUpdateSupplierViewModel model)
        {
            SupplierProvider.GetProvider().PostAdd(model);
            return RedirectToAction("Index");
        }

        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public IActionResult AddModal([FromBody] CreateUpdateSupplierViewModel model)
        {
            try
            {
                SupplierProvider.GetProvider().PostAdd(model);
                return Json(new { success = true, message = "Data berhasil ditambah" });
            }
            catch (Exception)
            {
                return Json(new { success = true, message = "Data gagal ditambah" });
            }
        }

        [HttpGet]
        public IActionResult Edit(long id, int page)
        {
            SetUsernameRole(User.Claims);
            var model = SupplierProvider.GetProvider().GetEdit(id, page);
            return View("EditSupplier", model);
        }

        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public IActionResult Edit(CreateUpdateSupplierViewModel model)
        {
            int page = model.CurrentPage;
            SupplierProvider.GetProvider().PostEdit(model);
            return RedirectToAction("Index", new { page });
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult Delete(long id, int page)
        {
            var checkProduct = SupplierProvider.GetProvider().GetDelete(id);
            if (checkProduct)
            {
                return View();
            }
            else
            {
                SupplierProvider.GetProvider().PostDelete(id);
            }
            return RedirectToAction("Index", new { page });
        }

    }
}
