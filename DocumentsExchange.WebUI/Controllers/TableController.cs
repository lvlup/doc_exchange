﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using DocumentsExchange.BusinessLayer.Identity;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
using DocumentsExchange.WebUI.ViewModels;

namespace DocumentsExchange.WebUI.Controllers
{
    [Authorize]
    public class TableController : Controller
    {
        private readonly IOrganizationProvider _organizationProvider;

        public TableController(IOrganizationProvider organizationProvider)
        {
            _organizationProvider = organizationProvider;
        }

        public PartialViewResult Index(int  id)
        {
            var org =  _organizationProvider.Get(id).Result;
            return PartialView(org);
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.Technician)]
        public PartialViewResult CallAddRecord(int id)
        {
            var tableNamesVm = new TableNamesViewModel() {OrganizationId = id};
            return PartialView(tableNamesVm);
        }
    }
}