using System;
using System.Data.Entity.Infrastructure;
using System.Web.Mvc;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.WebUI.Areas.Admin.Controllers
{
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
        public PartialViewResult Create(Organization org)
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

            return PartialView();
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
    }
}