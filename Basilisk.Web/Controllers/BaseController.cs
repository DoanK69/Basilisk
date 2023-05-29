using Basilisk.Utility;
using Basilisk.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;

namespace Basilisk.Web.Controllers
{
    public class BaseController : Controller
    {
        protected void SetUsernameRole(IEnumerable<Claim> claims)
        {
            foreach (var claim in claims)
            {
                if (claim.Type == ClaimTypes.NameIdentifier)
                {
                    ViewBag.Username = claim.Value;
                    ViewBag.Id = claim.Value;
                    ViewBag.EmployeeNumber = claim.Value;
                }
                if (claim.Type == ClaimTypes.Role)
                {
                    ViewBag.Role = claim.Value;
                    ViewBag.Menus = GlobalConfiguration.Menus().Where(m => m.Role == claim.Value);
                }
                //if (claim.ValueType == ClaimValueTypes)
                //{
                //    ViewBag.Menus = GlobalConfiguration.Menus();
                //}
            }
        }


        protected string GetUsername(IEnumerable<Claim> claims)
        {
            return claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
        }

        protected string GetRole(IEnumerable<Claim> claims)
        {
            return claims.SingleOrDefault(c => c.Type == ClaimTypes.Role).Value;
        }

        //protected void SetMenuByUsername(IEnumerable<Claim> claims)
        //{
        //    string role = GetRole(claims);
        //    ViewBag.Menus = GlobalConfiguration.GetMenuByRole().Where(m => m.Role == role);
        //}

        protected IEnumerable<ValidationViewModel> GetValidationMessage(ModelStateDictionary modelState)
        {
            var result = new List<ValidationViewModel>();
            foreach (KeyValuePair<string, ModelStateEntry> item in modelState)
            {
                if (item.Value.Errors.Count > 0)
                {
                    var propertyItem = item.Key;
                    var errorMessage = item.Value.Errors.FirstOrDefault().ErrorMessage;
                    result.Add(new ValidationViewModel
                    {
                        PropertyName = propertyItem,
                        MessageError = errorMessage
                    });
                }
            }
            return result;
        } 
    }
}
