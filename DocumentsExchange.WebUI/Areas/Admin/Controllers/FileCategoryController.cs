using System;
using System.Data.Entity.Infrastructure;
using System.Web.Mvc;
using DocumentsExchange.BusinessLayer.Identity;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class FileCategoryController : Controller
    {

        private readonly IFileCategoryProvider _fileCategoryProvider;

        public FileCategoryController(IFileCategoryProvider fileCategoryProvider)
        {
            _fileCategoryProvider = fileCategoryProvider;
        }

        public ActionResult Index()
        {
            var categories = _fileCategoryProvider.GetAll().Result;
            return View(categories);
        }

        public PartialViewResult Create()
        {
            var fileCategory = new FileCategory() {CreatedDateTime = DateTime.UtcNow};
            return PartialView(fileCategory);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FileCategory fileCategory)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _fileCategoryProvider.Add(fileCategory).Result;
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


        public PartialViewResult Edit(int id)
        {
            var org = _fileCategoryProvider.Get(id).Result;
            return PartialView(org);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FileCategory fileCategory)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _fileCategoryProvider.Update(fileCategory).Result;
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

        public ActionResult Delete(int id)
        {
            var res = _fileCategoryProvider.Delete(id).Result;
            return RedirectToAction("Index", "AdminPanel");
        }
    }
}