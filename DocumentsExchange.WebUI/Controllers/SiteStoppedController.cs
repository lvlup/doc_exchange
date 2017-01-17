using System.Web.Mvc;
using DocumentsExchange.BusinessLayer.Identity;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
using Microsoft.AspNet.Identity;

namespace DocumentsExchange.WebUI.Controllers
{
    public class SiteStoppedController : BaseController
    {
        private readonly IAdminProvider _adminProvider;
        private readonly IBlockIpService _blockIpService;
        private readonly ApplicationUserManager _userManager;

        public SiteStoppedController(IAdminProvider adminProvider, IBlockIpService blockIpService, ApplicationUserManager userManager)
        {
            _adminProvider = adminProvider;
            _blockIpService = blockIpService;
            _userManager = userManager;
        }

        public ActionResult Index()
        {
            if (_adminProvider.GetWebSiteState().Result.IsActive)
                return RedirectToAction("Index", "Home");

            return View();
        }

        public ActionResult LoginDeactivated()
        {
            var user = _userManager.FindById(UserId);
            if (user != null && user.IsActive)
                return RedirectToAction("Index", "Home");

            return View();
        }

        public ActionResult Banned()
        {
            if (!_blockIpService.IsIpBlocked(Request.UserHostAddress))
                return RedirectToAction("Index", "Home");

            return View();
        }
    }
}