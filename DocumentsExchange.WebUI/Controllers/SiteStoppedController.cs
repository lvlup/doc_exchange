using System.Web.Mvc;
using DocumentsExchange.BusinessLayer.Services.Interfaces;

namespace DocumentsExchange.WebUI.Controllers
{
    public class SiteStoppedController : Controller
    {
        private readonly IAdminProvider _adminProvider;

        public SiteStoppedController(IAdminProvider adminProvider)
        {
            _adminProvider = adminProvider;
        }

        public ActionResult Index()
        {
            if (_adminProvider.GetWebSiteState().Result.IsActive)
                return RedirectToAction("Index", "Home");

            return View();
        }
    }
}