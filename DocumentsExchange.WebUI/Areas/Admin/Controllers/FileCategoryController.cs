using System;
using System.Data.Entity.Infrastructure;
using System.Web.Mvc;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.WebUI.Areas.Admin.Controllers
{
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
        public PartialViewResult Create(FileCategory fileCategory)
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

            return PartialView();
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
    }
}