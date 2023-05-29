using Basilisk.Provider;
using Basilisk.ViewModel.Login;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Basilisk.Web.Controllers
{
    public class AccountController : BaseController
    {   
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            LoginViewModel model = new LoginViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(string? returnUrl, LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (AccountProvider.Getprovider().IsAuthentication(model))
                {
                    var claims = new List<Claim>
                    {
                    /*                 
                     * claim untuk menyimpan data login
                     */
                    new Claim("username", model.Username),
                    new Claim (ClaimTypes.NameIdentifier, model.Username),
                    new Claim (ClaimTypes.Role, AccountProvider.Getprovider().GetRoleName(model.Username))
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(principal);

                    if (returnUrl == null)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("LoginFailed");
                }

            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult LoginFailed()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Index()
        {
            SetUsernameRole(User.Claims);
            var model = AccountProvider.Getprovider().GetIndex();
            return View(model);
        }

        [HttpGet]
        public IActionResult Daftar()
        {
            SetUsernameRole(User.Claims);
            var model = AccountProvider.Getprovider().GetAdd();
            return View(model);
        }

        public IActionResult Edit(string username)
        {
            SetUsernameRole(User.Claims);
            var model = AccountProvider.Getprovider().GetUpdate(username);
            return View(model);
        }

        [HttpPost]
        public IActionResult Daftar(RegisterViewModel model)
        {
            AccountProvider.Getprovider().PostAdd(model);
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public IActionResult Edit(RegisterViewModel model)
        {
            AccountProvider.Getprovider().PostUpdate(model);
            return RedirectToAction("Index");
        }


    }
}
