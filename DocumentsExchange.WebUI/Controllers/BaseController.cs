using System.Web.Mvc;
using DocumentsExchange.WebUI.Helpers;
using Microsoft.AspNet.Identity;

namespace DocumentsExchange.WebUI.Controllers
{
    public abstract class BaseController : Controller
    {
        public int UserId => HttpContext.User.Identity.GetUserId<int>();

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonDotNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }
    }
}