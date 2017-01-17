using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DocumentsExchange.BusinessLayer.Identity;
using DocumentsExchange.DataLayer.Entity;
using DocumentsExchange.WebUI.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace DocumentsExchange.WebUI.Controllers
{
    public class AccountController : BaseController
    {
        private readonly ApplicationUserManager _userManager;

        public AccountController(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [Authorize]
        public ActionResult Logout()
        {
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();
            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel login, string returnUrl = "")
        {
            User user = await _userManager.FindAsync(login.Name, login.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Некорректное имя или пароль.");
            }
            else
            {
                user.ActivityDateTime = DateTime.UtcNow;
                _userManager.Update(user);

                if (!user.IsActive)
                {
                    return RedirectToAction("LoginDeactivated", "SiteStopped");
                }

                ClaimsIdentity ident = await _userManager.CreateIdentityAsync(user,
                    DefaultAuthenticationTypes.ApplicationCookie);

                ident.AddClaim(new Claim(ClaimTypes.GivenName, user.FullName));

                var authManager = HttpContext.GetOwinContext().Authentication;

                authManager.SignOut();
                authManager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = false
                }, ident);

                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Home", new { userId = user.Id });
            }

            return View(login);
        }
    }
}