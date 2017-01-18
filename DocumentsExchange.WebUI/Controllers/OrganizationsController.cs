using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.WebUI.Controllers
{
    [Authorize]
    public class OrganizationsController : BaseController
    {
        private readonly IOrganizationProvider _organizationProvider;

        public OrganizationsController(IOrganizationProvider organizationProvider)
        {
            _organizationProvider = organizationProvider;
        }

        public PartialViewResult Index()
        {
            //var org = new Organization() { CreatedDateTime = DateTime.Now, IsActive = true, Name = "test org" };

            //var r1 = _organizationProvider.Add(org).Result;

            var model =  _organizationProvider.GetUseOrganizations(UserId).Result;
            return PartialView(model);
        }

        public ActionResult Get(int id)
        {
            return  RedirectToAction("Index", "Table", new {id = id});
        }
    }
}