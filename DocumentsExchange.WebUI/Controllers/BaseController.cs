using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace DocumentsExchange.WebUI.Controllers
{
    public abstract class BaseController : Controller
    {
        public int UserId => HttpContext.User.Identity.GetUserId<int>();
    }
}