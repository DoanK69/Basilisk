using Basilisk.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Basilisk.ViewModel.Salesmen;
using Basilisk.ViewModel;
using Basilisk.ViewModel.Region;
using Basilisk.ViewModel.SalesmanRegions;
using Basilisk.Provider;
using Microsoft.AspNetCore.Authorization;

namespace Basilisk.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class SalesmenController : BaseController
    {
        public IActionResult Index(int page = 1, string searchName = "")
        {
            SetUsernameRole(User.Claims);
            var model = SalesmenProvider.GetProvider().GetIndex(page, searchName);
            return View(model);
        }

        [HttpGet]
        public IActionResult Add()
        {
            SetUsernameRole(User.Claims);
            var model = SalesmenProvider.GetProvider().GetAdd();
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(UpsertSalesmenViewModel model)
        {
            if (ModelState.IsValid)
            {
                SalesmenProvider.GetProvider().PostAdd(model);
                return RedirectToAction("Index");
            }
            else
            {
                model = SalesmenProvider.GetProvider().DropDownSupplier(model);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Edit(string empNumber)
        {
            SetUsernameRole(User.Claims);
            var model = SalesmenProvider.GetProvider().GetUpdate(empNumber);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(UpsertSalesmenViewModel model)
        {
            SalesmenProvider.GetProvider().PostUpdate(model);
            return RedirectToAction("Index");
        }

        public IActionResult Detail(string empNumber)
        {
            SetUsernameRole(User.Claims);
            var model = SalesmenProvider.GetProvider().GetDetail(empNumber);
            return View(model);
        }


        [AcceptVerbs("GET", "POST")]
        public IActionResult Delete(string empNumber)
        {
            SalesmenProvider.GetProvider().PostDelete(empNumber);

            return RedirectToAction("Index");
        }
        //public int CountRegion(string empNumber)
        //{
        //    var region = (from sal in _context.Salesmen
        //                  join salReg in _context.SalesmenRegions on sal.EmployeeNumber equals salReg.SalesmanEmployeeNumber
        //                  join reg in _context.Regions on salReg.RegionId equals reg.Id
        //                  where sal.EmployeeNumber == empNumber
        //                  select new GridSalesmanRegionsViewModel
        //                  {
        //                      EmployeeNumber = sal.EmployeeNumber,
        //                      FirstName = sal.FirstName,
        //                      LastName = sal.LastName,
        //                      Level = sal.Level,
        //                      RegionId = reg.Id,
        //                      RegionCity = reg.City,
        //                      RegionRemark = reg.Remark
        //                  }).AsEnumerable();

        //    var hitung = region.Count();
        //    return hitung;
        //}
    }
}
