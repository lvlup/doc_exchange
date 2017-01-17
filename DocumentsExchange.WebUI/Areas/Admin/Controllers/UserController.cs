using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Web.Mvc;
using DocumentsExchange.BusinessLayer.Identity;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
using DocumentsExchange.DataLayer.Entity;
using DocumentsExchange.WebUI.ViewModels;

namespace DocumentsExchange.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class UserController : Controller
    {
        private readonly IUserProvider _userProvider;

        public UserController(IUserProvider userProvider)
        {
            _userProvider = userProvider;
        }

        
        public ActionResult Index()
        {
            var users = _userProvider.GetAll().Result;

            var userVm = new UserViewModel();

            if (users != null) userVm.Users = new List<User>(users);

            return View(userVm);
        }

        public PartialViewResult Create()
        {
            return PartialView();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {

            try
            {
                if (ModelState.IsValid)
                {
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Невозможно сохранить изменения. Попробуйте позже.");
            }

            return View();
        }

        public PartialViewResult Edit(int userId)
        {
            var user = _userProvider.Get(userId).Result;
            return PartialView(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            return PartialView();
        }

        public ActionResult Delete(int id)
        {
            var res = _userProvider.Delete(id).Result;
            return RedirectToAction("Index", "AdminPanel");
        }
    }
}