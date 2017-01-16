﻿using System;
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
                ClaimsIdentity ident = await _userManager.CreateIdentityAsync(user,
                    DefaultAuthenticationTypes.ApplicationCookie);

                var authManager = HttpContext.GetOwinContext().Authentication;
                authManager.SignOut();
                authManager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = false
                }, ident);

                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Home");
            }

            return View(login);
        }
    }
}