using Basilisk.DataAccess.Models;
using Basilisk.Provider;
using Basilisk.ViewModel;
using Basilisk.ViewModel.Product;
using Basilisk.ViewModel.Supplier;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace Basilisk.Web.Controllers
{
    [Authorize(Roles = "Administrator, Salesman")]
    public class ProductController : BaseController
    {
        public IActionResult Index(int page = 1,string searchProduct = "", string searchCategory = "", string searchSupplier = "")
        {
            SetUsernameRole(User.Claims);
            var model = ProductProvider.GetProvider().GetIndex(page, searchProduct, searchCategory, searchSupplier);
            return View(model);
        }

        [HttpGet]
        public IActionResult Add()
        {
            SetUsernameRole(User.Claims);
            var model = ProductProvider.GetProvider().GetDataAdd();
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(UpdateViewModel model)
        {
            ProductProvider.GetProvider().PostAdd(model);
            return RedirectToAction("Index");
            
        }

        [HttpGet]
        public IActionResult Edit(long id)
        {
            SetUsernameRole(User.Claims);
            var model = ProductProvider.GetProvider().GetUpdate(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(UpdateViewModel model)
        {          
            ProductProvider.GetProvider().PostUpdate(model);
            return RedirectToAction("Index");
        }
    }
}
