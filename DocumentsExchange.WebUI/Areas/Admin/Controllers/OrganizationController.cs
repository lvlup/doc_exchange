using System;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using System.Web.Mvc;
using DocumentsExchange.BusinessLayer.Identity;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
using DocumentsExchange.DataLayer.Entity;
using DocumentsExchange.WebUI.Controllers;
using DocumentsExchange.WebUI.Exceptions;

namespace DocumentsExchange.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class OrganizationController : BaseController
    {

        private readonly IOrganizationProvider _organizationProvider;


        public OrganizationController(IOrganizationProvider organizationProvider)
        {
            _organizationProvider = organizationProvider;
        }

        public ActionResult Index()
        {
            var orgs = _organizationProvider.GetAll().Result;
            return View(orgs);
        }

        public ActionResult Organizations()
        {
            var orgs = _organizationProvider.GetAll().Result;

            return PartialView(orgs);
        }

        public PartialViewResult Create()
        {
            var org = new Organization() {CreatedDateTime = DateTime.UtcNow};
            return PartialView(org);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Organization org)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ValidationException(ModelState);
                }

                var result = await _organizationProvider.Add(org);
                if (!result)
                    throw new Exception("Невозможно сохранить изменения");

                return Json(new { Success = true });

            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Невозможно сохранить изменения. Попробуйте позже.");
            }

            return Json(new { Success = false });
        }


        public PartialViewResult Edit(int orgId)
        {
            var org = _organizationProvider.Get(orgId).Result;
            return PartialView(org);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Organization org)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ValidationException(ModelState);
                }

                var result = await _organizationProvider.Update(org);
                if(!result)
                    throw new Exception("Невозможно сохранить изменения");

                return Json(new { Success = true });
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Невозможно сохранить изменения. Попробуйте позже.");
            }

            return Json(new { Success = false });
        }

        public ActionResult Delete(int id)
        {
            var res = _organizationProvider.Delete(id).Result;
            return RedirectToAction("Index", "AdminPanel");
        }
    }
}