using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DocumentsExchange.BusinessLayer.Identity;

namespace DocumentsExchange.WebUI.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class AdminPanelController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }
    }
}