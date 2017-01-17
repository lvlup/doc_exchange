using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DocumentsExchange.BusinessLayer.Identity;

namespace DocumentsExchange.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class IpController : Controller
    {
        // GET: Admin/Ip
        public ActionResult Index()
        {
            return View();
        }



    }
}