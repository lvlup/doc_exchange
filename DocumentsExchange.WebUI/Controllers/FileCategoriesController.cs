using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
using DocumentsExchange.DataLayer.Entity;
using DocumentsExchange.WebUI.ViewModels;

namespace DocumentsExchange.WebUI.Controllers
{
    [Authorize]
    public class FileCategoriesController : Controller
    {
        private readonly IFileCategoryProvider _fileCategoryProvider;

        public FileCategoriesController(IFileCategoryProvider fileCategoryProvider)
        {
            _fileCategoryProvider = fileCategoryProvider;
        }

        public  PartialViewResult Index(int id)
        {
            //var category = new FileCategory() { CreatedDateTime = DateTime.Now, IsActive = true, Name = "Платежные поручения" };

            //var r = _fileCategoryProvider.Add(category).Result;

            var categories = _fileCategoryProvider.GetAll().Result;

            var fileCategoriesVm = new FileCategoriesViewModel()
            {
                OrganizationId = id,
                FileCategories = new List<FileCategory>(categories)
            };

            return PartialView(fileCategoriesVm);
        }

        public ActionResult Get(int id, int orgId)
        {
            return RedirectToAction("Index", "File", new { orgId = orgId, categoryId = id });
        }
    }
}