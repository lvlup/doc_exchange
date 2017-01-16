using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using DocumentsExchange.BusinessLayer.Identity;
using DocumentsExchange.DataLayer.Entity;
using DocumentsExchange.DataLayer.Identity;
using DocumentsExchange.WebUI.ViewModels.Auth;
using Microsoft.AspNet.Identity;

namespace DocumentsExchange.WebUI.Controllers
{
    public class RolesController : Controller
    {
        private readonly ApplicationUserManager _userManager;
        private readonly AppRoleManager _roleManager;

        public RolesController(ApplicationUserManager userManager, AppRoleManager roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public ActionResult Index()
        {
            return View(_roleManager.Roles);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create([Required]string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result
                    = await _roleManager.CreateAsync(new AppRole(name));

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            return View(name);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            AppRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error", result.Errors);
                }
            }
            else
            {
                return View("Error", new string[] { "Роль не найдена" });
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            AppRole role = await _roleManager.FindByIdAsync(id);
            int[] memberIDs = role.Users.Select(x => x.UserId).ToArray();

            IEnumerable<User> members
                = _userManager.Users.Where(x => memberIDs.Any(y => y == x.Id));

            IEnumerable<User> nonMembers = _userManager.Users.Except(members);

            return View(new RoleEditModel
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [HttpPost]
        public async Task<ActionResult> Edit(RoleModificationModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (int userId in model.IdsToAdd ?? new int[] { })
                {
                    result = await _userManager.AddToRoleAsync(userId, model.RoleName);

                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }

                foreach (int userId in model.IdsToDelete ?? new int[] { })
                {
                    result = await _userManager.RemoveFromRoleAsync(userId,
                    model.RoleName);

                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }

                return RedirectToAction("Index");

            }
            return View("Error", new string[] { "Роль не найдена" });
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}