using System.Linq;
using System.Net;
using System.Web.Mvc;
using DocumentsExchange.WebUI.Exceptions;
using DocumentsExchange.WebUI.Helpers;
using NLog;

namespace DocumentsExchange.WebUI.Filters
{
    public class ValidationExceptionFilterAttribute : HandleErrorAttribute
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public override void OnException(ExceptionContext filterContext)
        {
            if (
                !filterContext.ExceptionHandled
                && filterContext.Exception != null
                && filterContext.HttpContext.Request.IsAjaxRequest()
                && filterContext.Exception is ValidationException
                )
            {
                filterContext.ExceptionHandled = true;

                ValidationException exception = (ValidationException)filterContext.Exception;
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                filterContext.Result = new JsonDotNetResult()
                {
                    Data = new
                    {
                        errors = exception.Errors.Select(x => new
                        {
                            x.Key,
                            Errors = x.Value
                        })
                    },

                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                };

                filterContext.HttpContext.Response.Clear();
                Logger.Info("Validation Failed: {0}", exception.Errors);
            }
            else
            {
                Logger.Info("Validation Failed. Not an ajax request. Skipped");

                //base.OnException(filterContext);
            }
        }
    }
}