using System.Web.Mvc;
using DocumentsExchange.BusinessLayer.Identity;

namespace DocumentsExchange.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class DictionariesController : Controller
    {
        // GET: Dictionaries
        public ActionResult Index()
        {
            return View();
        }
    }
}