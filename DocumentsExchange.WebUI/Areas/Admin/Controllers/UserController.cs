using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
using DocumentsExchange.DataLayer.Entity;
using DocumentsExchange.WebUI.Controllers;
using DocumentsExchange.WebUI.Exceptions;
using DocumentsExchange.WebUI.ViewModels;

namespace DocumentsExchange.WebUI.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserProvider _userProvider;

        public UserController(IUserProvider userProvider)
        {
            _userProvider = userProvider;
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Users()
        {
            var users = _userProvider.GetAll().Result;

            var userVm = new UserViewModel();

            if (users != null) userVm.Users = new List<User>(users);

            return PartialView(userVm);
        }

        public PartialViewResult Create()
        {
            CreateUserViewModel createViewModel = new CreateUserViewModel
            {
                OrganizationInfoes = _userProvider.GetOrganizations().Result
            };

            return PartialView(createViewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateUserViewModel userModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ValidationException(ModelState);
                }

                User user = new User()
                {
                    UserName = userModel.UserName,
                    FirstName = userModel.FirstName,
                    LastName = userModel.LastName,
                    IsActive = userModel.IsActive,
                    RoleList = userModel.Role,
                    OrganizationIds = userModel.Organizations
                };

                var result = await _userProvider.Add(user, userModel.Password);
                if (!result)
                    throw new Exception("Невозможно сохранить изменения");

                return Json(new { Success = true });
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Невозможно сохранить изменения. Попробуйте позже.");
            }

            return Json(new { Success = false });
        }

        public async Task<PartialViewResult> Edit(int userId)
        {
            var user = await _userProvider.GetForEdit(userId);
            var editUserViewModel = new EditUserViewModel()
            {
                FirstName = user.User.FirstName,
                LastName = user.User.LastName,
                UserName = user.User.UserName,
                IsActive = user.User.IsActive,
                Id = user.User.Id,
                Role = user.User.RoleList.Split(new [] { "," }, StringSplitOptions.RemoveEmptyEntries)[0].Trim(),
                OrganizationInfoes = user.Organizations
            };

            return PartialView(editUserViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditUserViewModel userModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ValidationException(ModelState);
                }

                User user = new User()
                {
                    Id = userModel.Id,
                    UserName = userModel.UserName,
                    FirstName = userModel.FirstName,
                    LastName = userModel.LastName,
                    IsActive = userModel.IsActive,
                    RoleList = userModel.Role,
                    OrganizationIds = userModel.Organizations
                };

                var result = await _userProvider.Update(user);
                if (!result)
                    throw new Exception("Невозможно сохранить изменения");

                return Json(new { Success = true });
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Невозможно сохранить изменения. Попробуйте позже.");
            }

            return Json(new { Success = false });
        }

        public async Task<PartialViewResult> PasswordReset(int userId)
        {
            return await Task.FromResult(PartialView(new PasswordResetViewModel() { Id = userId }));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> PasswordReset(PasswordResetViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ValidationException(ModelState);
                }
                
                var result = await _userProvider.ResetPassword(model.Id, model.Password);
                if (!result)
                    throw new Exception("Невозможно сохранить изменения");

                return Json(new { Success = true });
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Невозможно сохранить изменения. Попробуйте позже.");
            }

            return Json(new { Success = false });
        }


        public ActionResult Delete(int id)
        {
            var res = _userProvider.Delete(id).Result;
            return RedirectToAction("Index", "AdminPanel");
        }
    }
}