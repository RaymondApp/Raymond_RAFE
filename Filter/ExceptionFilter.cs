using RAFE.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace RAFE.Filter
{
    public class ExceptionFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext expcontext)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Controller : " + expcontext.RouteData.Values["controller"]);
                sb.AppendLine("</br>" + "Action : " + expcontext.RouteData.Values["action"]);
                sb.AppendLine("</br>" + "Error : " + expcontext.Exception.Message);
                sb.AppendLine("</br>" + "StackTrace : " + expcontext.Exception.StackTrace);
                ClsLogging.Log(ClsLogging.LogType.Exception, sb.ToString().Replace("</br>", "\t\t\t"));

                if (expcontext.HttpContext.Request.IsAjaxRequest())
                {
                    expcontext.HttpContext.Response.ClearContent();
                    expcontext.HttpContext.Items["PartialPageError"] = true;
                }
                else
                {
                    expcontext.Result = new RedirectToRouteResult
                                            (new System.Web.Routing.RouteValueDictionary{
                                                { "action" , "Error" },
                                                { "controller" , "Common" }
                                        });
                }
                expcontext.ExceptionHandled = true;
            }
            catch (Exception)
            {

            }
        }
    }
}