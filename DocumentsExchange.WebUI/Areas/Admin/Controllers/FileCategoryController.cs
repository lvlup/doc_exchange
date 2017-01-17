using System.Web.Mvc;

namespace DocumentsExchange.WebUI.Areas.Admin.Controllers
{
    public class FileCategoryController : Controller
    {
        // GET: FileCategory
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Create()
        {
            return PartialView();
        }
    }
}