using System.Web;
using System.Web.Mvc;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
using DocumentsExchange.Common.Extensions;
using Microsoft.AspNet.Identity;

namespace DocumentsExchange.WebUI.Filters
{
    public class OrganizationAuthorizeAttribute : AuthorizeAttribute
    {
        public IOrganizationRelevanceValidator Validator { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            string orgId = httpContext.Request.QueryString.Get("orgId");
            if (orgId.IsNullOrEmpty())
                return true;

            return Validator.Check(httpContext.User.Identity.GetUserId<int>(), orgId.ToInt());
        }
    }
}