using System.Web.Mvc;
using DocumentsExchange.BusinessLayer.Identity;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
using DocumentsExchange.DataLayer.Entity;
using DocumentsExchange.WebUI.Filters;
using Microsoft.AspNet.Identity;

namespace DocumentsExchange.WebUI.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly IUserProvider _userProvider;

        public HomeController(IUserProvider userProvider)
        {
            _userProvider = userProvider;
        }

        public ActionResult Index()
        {
            User user = _userProvider.Get(UserId).Result;
            return View(user);
        }
    }
}