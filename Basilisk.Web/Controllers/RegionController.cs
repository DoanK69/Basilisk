using Basilisk.DataAccess.Models;
using Basilisk.ViewModel.Region;
using Microsoft.AspNetCore.Mvc;
using Basilisk.Provider;
using Microsoft.AspNetCore.Authorization;

namespace Basilisk.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RegionController : BaseController
    {
        public IActionResult Index(int page = 1, string searchCity = "")
        {
            SetUsernameRole(User.Claims);
            var model = RegionProvider.GetProvider().GetIndex(page, searchCity);
            return View(model);
        }

        [HttpGet]
        public IActionResult Add()
        {
            SetUsernameRole(User.Claims);
            var model = new UpsertRegionViewModel();
            return View("Upsert", model);
        }

        [HttpGet]
        public IActionResult Edit(long idRegion)
        {
            SetUsernameRole(User.Claims);
            var model = RegionProvider.GetProvider().GetEdit(idRegion);
            return View("Upsert", model);
        }

        [HttpPost]
        public IActionResult Save(UpsertRegionViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    RegionProvider.GetProvider().Save(model);
                }
                catch (Exception)
                {
                    throw;
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View("Upsert", model);
            }
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult Delete(int idRegion)
        {
            RegionProvider.GetProvider().Delete(idRegion);
            return RedirectToAction("Index");
        }          
    }
}
