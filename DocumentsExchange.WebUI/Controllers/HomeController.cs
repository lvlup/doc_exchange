using System.Web.Mvc;
using DocumentsExchange.BusinessLayer.Identity;
using DocumentsExchange.DataLayer.Entity;
using DocumentsExchange.WebUI.Filters;

namespace DocumentsExchange.WebUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationUserManager _userManager;

        public HomeController(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        public ActionResult Index()
        {
            User user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            return View(user);
        }
    }
}