using Microsoft.AspNetCore.Mvc;
using Basilisk.DataAccess.Models;
using Basilisk.ViewModel.Category;
using Basilisk.ViewModel.Product;
using Basilisk.Provider;
using System.Globalization;
using Microsoft.VisualBasic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Basilisk.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CategoryController : BaseController
    {
        
        //private static BasiliskTFContext _context = new BasiliskTFContext();
        
        public IActionResult Index(int page=1,string searchName = "")
        {
            SetUsernameRole(User.Claims);
            var model = CategoryProvider.GetProvider().GetIndex(page, searchName);
            return View(model);
        }

        public IActionResult Detail(int id)
        {
            SetUsernameRole(User.Claims);
            var model = CategoryProvider.GetProvider().GetDetail(id);
            return View("DetailProduct", model);
        }

        public IActionResult DetailPopup(int id)
        {
            SetUsernameRole(User.Claims);
            var model = CategoryProvider.GetProvider().GetDetailCategory(id);
            return Json(model);
        }

        public IActionResult Add()
        {
            SetUsernameRole(User.Claims);
            var model = new CreateUpdateViewModel();
            return View(model);
        }

        public IActionResult AddModal()
        {
            SetUsernameRole(User.Claims);
            var model = new CreateUpdateViewModel();
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Add(CreateUpdateViewModel model)
        {
            CategoryProvider.GetProvider().PostAdd(model);
            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult AddPopUp([FromBody] CreateUpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CategoryProvider.GetProvider().PostAdd(model);
                    return Json(new { success = true, message = "Data berhasil ditambah" });
                }
                catch (Exception)
                {
                    return Json(new { success = false, message = "Data gagal ditambah" });
                }
            }

            var validationMessage = GetValidationMessage(ModelState);
            return Json(new { success = false, message = "Data gagal ditambah", validations = validationMessage });
            
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult EditPopUp([FromBody] CreateUpdateViewModel model)
        {
            try
            {
                CategoryProvider.GetProvider().PostUpdate(model);
                return Json(new { success = true, message = "Data berhasil diupdate" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Data gagal diupdate" });
            }

        }

        [HttpGet]
        public IActionResult Edit(long id)
        {
            SetUsernameRole(User.Claims);
            var model = CategoryProvider.GetProvider().GetUpdate(id);
            return View(model);
        }

        [HttpGet]
        public IActionResult EditModal(long id)
        {
            var model = CategoryProvider.GetProvider().GetEditPopup(id);
            return Json(model);
        }

        [HttpPost]
        public IActionResult Edit(CreateUpdateViewModel model)
        {
            CategoryProvider.GetProvider().PostUpdate(model);
            return RedirectToAction("Index");
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult Delete(long id)
        {
            var anyDependentProduct = CategoryProvider.GetProvider().GetDelete(id);
            if (!anyDependentProduct)
            {
                CategoryProvider.GetProvider().PostDelete(id);
                return RedirectToAction("Index");
            }
            int totalDependentProducts = CategoryProvider.GetProvider().GetCount(id);
            return RedirectToAction("FailDeleteValidation", new { totalProducts = totalDependentProducts });
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult DeletePopup(long id)
        {
            var anyDependentProduct = CategoryProvider.GetProvider().GetDelete(id);
            if (!anyDependentProduct)
            {
                CategoryProvider.GetProvider().PostDelete(id);
                return Json(new { success = false, message = "Tidak bisa hapus category" });
            }
            else
            {
                return Json(new { success = true, message = "Data berhasil dihapus" });
            }
        }

        [HttpGet]
        public IActionResult FailDeleteValidation(int totalProducts)
        {
            return View(totalProducts);
        }
    }
}
