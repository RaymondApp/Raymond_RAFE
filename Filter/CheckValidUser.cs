using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace RAFE.Filter
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CheckValidUser : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.Request.Cookies["UserInfo"].Value =="")
            {
                filterContext.Result = new RedirectToRouteResult
                                           (new RouteValueDictionary{
                                                { "action" , "unauthorized" },
                                                { "controller" , "common" }
                                        });
            }
        }

    }
}