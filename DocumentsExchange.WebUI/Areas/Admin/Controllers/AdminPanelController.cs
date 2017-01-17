﻿using System.Web.Mvc;
using DocumentsExchange.BusinessLayer.Identity;

namespace DocumentsExchange.WebUI.Areas.Admin.Controllers
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