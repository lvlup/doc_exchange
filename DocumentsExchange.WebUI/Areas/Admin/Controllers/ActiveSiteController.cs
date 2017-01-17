using System.Threading.Tasks;
using System.Web.Mvc;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.WebUI.Areas.Admin.Controllers
{
    public class ActiveSiteController : Controller
    {
        private readonly IAdminProvider _adminProvider;

        public ActiveSiteController(IAdminProvider adminProvider)
        {
            _adminProvider = adminProvider;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> StopSite(WebSiteState state)
        {
            return Json(new
            {
                Success = await _adminProvider.SetWebSiteState(state.IsActive)
            });
        }


        // GET: Admin/ActiveSite
        public ActionResult Index()
        {
            return View(_adminProvider.GetWebSiteState().Result);
        }
    }
}