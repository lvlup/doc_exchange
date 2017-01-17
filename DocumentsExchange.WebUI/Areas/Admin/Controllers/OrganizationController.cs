using System;
using System.Data.Entity.Infrastructure;
using System.Web.Mvc;
using DocumentsExchange.BusinessLayer.Identity;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class OrganizationController : Controller
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

        public PartialViewResult Create()
        {
            var org = new Organization() {CreatedDateTime = DateTime.UtcNow};
            return PartialView(org);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Organization org)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _organizationProvider.Add(org).Result;
                }
                else
                {
                    return PartialView();
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Невозможно сохранить изменения. Попробуйте позже.");
            }

            return RedirectToAction("Index", "AdminPanel");
        }


        public PartialViewResult Edit(int orgId)
        {
            var org = _organizationProvider.Get(orgId).Result;
            return PartialView(org);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Organization org)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _organizationProvider.Update(org).Result;
                }
                else
                {
                    return PartialView();
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Невозможно сохранить изменения. Попробуйте позже.");
            }

            return RedirectToAction("Index","AdminPanel");
        }

        public ActionResult Delete(int id)
        {
            var res = _organizationProvider.Delete(id).Result;
            return RedirectToAction("Index", "AdminPanel");
        }
    }
}