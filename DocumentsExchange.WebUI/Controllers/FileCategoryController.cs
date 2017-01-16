using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocumentsExchange.WebUI.Controllers
{
    public class FileCategoryController : Controller
    {
        // GET: FileCategory
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Create()
        {
            return PartialView();
        }
    }
}