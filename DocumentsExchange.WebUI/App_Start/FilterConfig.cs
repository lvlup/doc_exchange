using System.Web.Mvc;
using DocumentsExchange.WebUI.Filters;

namespace DocumentsExchange.WebUI.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ValidationExceptionFilterAttribute());
        }
    }
}