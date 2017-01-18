using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DocumentsExchange.BusinessLayer.Identity;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
using DocumentsExchange.WebUI.Areas.Admin.ViewModels;
using DocumentsExchange.WebUI.Controllers;

namespace DocumentsExchange.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class IpController : BaseController
    {
        private readonly IBlockIpService _blockIpService;

        public IpController(IBlockIpService blockIpService)
        {
            _blockIpService = blockIpService;
        }

        // GET: Admin/Ip
        public ActionResult Index()
        {
            return View(new IpViewModel() { Content = _blockIpService.GetFileContents() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateIpList(IpViewModel ipViewModel)
        {
           bool result = _blockIpService.UpdateFileWithipList(ipViewModel.Content);

            return Json(new {Success = result});
        }


    }
}