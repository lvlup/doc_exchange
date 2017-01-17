using System.Web.Mvc;

namespace DocumentsExchange.WebUI.Controllers
{
    public class MessageController : Controller
    {
        // GET: Message
        public PartialViewResult Index(int orgId)
        {
            ViewBag.OrgId = orgId;
            return PartialView();
        }
    }
}