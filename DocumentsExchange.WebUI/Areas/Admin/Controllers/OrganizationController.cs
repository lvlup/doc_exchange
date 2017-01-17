using System.Web.Mvc;

namespace DocumentsExchange.WebUI.Areas.Admin.Controllers
{
    public class OrganizationController : Controller
    {
        // GET: Organization
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