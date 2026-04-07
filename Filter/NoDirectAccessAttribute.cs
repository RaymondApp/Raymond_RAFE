using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace RAFE.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class NoDirectAccessAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string act = filterContext.HttpContext.Request.RequestContext.RouteData.Values["action"].ToString();
            string con = filterContext.HttpContext.Request.RequestContext.RouteData.Values["controller"].ToString();

            if (filterContext.HttpContext.Request.UrlReferrer == null ||
                        filterContext.HttpContext.Request.Url.Host != filterContext.HttpContext.Request.UrlReferrer.Host)
            {
                filterContext.Result = new RedirectToRouteResult
                                           (new RouteValueDictionary{
                                                { "action" , "Index" },
                                                { "controller" , "Home" }
                                        });
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class URLValidForUser : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string act = filterContext.HttpContext.Request.RequestContext.RouteData.Values["action"].ToString();
            string con = filterContext.HttpContext.Request.RequestContext.RouteData.Values["controller"].ToString();




        }
    }
}