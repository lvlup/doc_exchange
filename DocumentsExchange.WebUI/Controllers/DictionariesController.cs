using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocumentsExchange.WebUI.Controllers
{
    public class DictionariesController : Controller
    {
        // GET: Dictionaries
        public ActionResult Index()
        {
            return View();
        }
    }
}