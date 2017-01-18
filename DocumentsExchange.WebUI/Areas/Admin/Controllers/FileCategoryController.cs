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
    public class FileCategoryController : BaseController
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

        public ActionResult Caregories()
        {
            var categories = _fileCategoryProvider.GetAll().Result;

            return PartialView(categories);
        }

        public PartialViewResult Create()
        {
            var fileCategory = new FileCategory() {CreatedDateTime = DateTime.UtcNow};
            return PartialView(fileCategory);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(FileCategory fileCategory)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ValidationException(ModelState);
                }

                var result = await _fileCategoryProvider.Add(fileCategory);
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


        public PartialViewResult Edit(int id)
        {
            var org = _fileCategoryProvider.Get(id).Result;
            return PartialView(org);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(FileCategory fileCategory)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ValidationException(ModelState);
                }

                var result = await _fileCategoryProvider.Update(fileCategory);

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

        public ActionResult Delete(int id)
        {
            var res = _fileCategoryProvider.Delete(id).Result;
            return RedirectToAction("Index", "AdminPanel");
        }
    }
}