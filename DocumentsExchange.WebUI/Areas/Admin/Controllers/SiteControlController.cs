using System.Web.Mvc;
using DocumentsExchange.BusinessLayer.Identity;

namespace DocumentsExchange.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class SiteControlController : Controller
    {
        
        // GET: SiteControl
        public ActionResult Index()
        {
            return View();
        }
    }
}