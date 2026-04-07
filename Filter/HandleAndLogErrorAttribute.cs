using System.Net;
using System.Web.Mvc;

namespace RAFE.Filter
{
    public class HandleAndLogErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
                return;
            if (filterContext.Exception is HttpAntiForgeryException && filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    Data = new { success = false, error = System.Web.Security.FormsAuthentication.LoginUrl },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
                UpdateFilterContext(filterContext, 401);
                return;
            }
            else if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                // This section (if needed) would be for any AJAX requests that are
                // not via Ext.NET - you could remove this if you don't need it
                // In addition, you may want to change the Data to suit your needs
                filterContext.Result = new JsonResult
                {
                    Data = new { success = false, error = "Internal server error" },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

                UpdateFilterContext(filterContext);
            }
            else
            {
                base.OnException(filterContext);
            }
        }

        private static void UpdateFilterContext(ExceptionContext filterContext, int statusCode = (int)HttpStatusCode.InternalServerError)
        {
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = statusCode;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }
}