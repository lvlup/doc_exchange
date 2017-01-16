using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocumentsExchange.WebUI.Controllers
{
    public class MessageController : Controller
    {
        // GET: Message
        public PartialViewResult Index()
        {
            return PartialView();
        }
    }
}