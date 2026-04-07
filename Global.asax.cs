using RAFE.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace RAFE
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Session_Start(object sender, EventArgs e)
        {
            string UserName;
            string[] user = User.Identity.Name.Split('\\');
            if (user.Length == 1)
                UserName = user[0];
            else if (user.Length == 2)
                UserName  = user[1];
            else
                UserName = "";
            RAFE.BAL.UserMaster_BAL objUserMaster_BAL = new BAL.UserMaster_BAL();
            if (System.Configuration.ConfigurationManager.AppSettings["ApplicationType"].ToString().ToUpper() == "UAT")
            {
                UserName = System.Configuration.ConfigurationManager.AppSettings["AdUserName"].ToString();
            }

            UserName = "ashwiniacharya";//
            ResultMessage objResultMessage = objUserMaster_BAL.GetUserDetailsForLogin(UserName);
            if (objResultMessage.isSuccess)
            {
                HttpCookie myCookie;
                if (Request.Cookies["UserInfo"] != null)
                {
                    myCookie = Request.Cookies["UserInfo"];
                    myCookie.Value = objResultMessage.statusMessage;
                    Response.Cookies.Add(myCookie);
                }
                else
                {
                    myCookie = new HttpCookie("UserInfo");
                    myCookie.Value = objResultMessage.statusMessage;
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                if (Request.Cookies["UserInfo_Old"] != null)
                {
                    myCookie = Request.Cookies["UserInfo_Old"];
                    myCookie.Expires = DateTime.Now.AddHours(-1);
                    Response.Cookies.Add(myCookie);
                }
            }
            else
            {
                HttpCookie myCookie = new HttpCookie("UserInfo");
                myCookie.Value = "";
                myCookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(myCookie);
            }
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null && !authTicket.Expired)
                {
                    var roles = authTicket.UserData.Split(',');
                    HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new FormsIdentity(authTicket), roles);
                }
            }
        }

        protected void Application_EndRequest()
        {
            // Any AJAX request that ends in a redirect should get mapped to an unauthorized request
            // since it should only happen when the request is not authorized and gets automatically
            // redirected to the login page.
            var context = new HttpContextWrapper(Context);
            if (context.Response.StatusCode == 302 && context.Request.IsAjaxRequest())
            {
                context.Response.Clear();
                Context.Response.StatusCode = 401;
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            ////to check login 
            //Exception ex = Server.GetLastError();
            //if (ex is HttpAntiForgeryException)
            //{
            //    Response.Clear();
            //    Server.ClearError(); 
            //    Response.StatusCode = 401;
            //}

            //var serverError = Server.GetLastError() as HttpException;


            //if (serverError.GetHttpCode() == 404)
            //{
            //    Server.ClearError();
                
            //}
            //else
            //{
            //    HttpContext ctx = HttpContext.Current;
            //    Utility.ClsLogging.Log(Utility.ClsLogging.LogType.Exception, "Error Message : - " + ctx.Server.GetLastError().ToString());
            //    ctx.Response.Clear();
            //    RequestContext rc = ((MvcHandler)ctx.CurrentHandler).RequestContext;
            //    string controllerName = rc.RouteData.GetRequiredString("controller");
            //    IControllerFactory factory = ControllerBuilder.Current.GetControllerFactory();
            //    IController controller = factory.CreateController(rc, controllerName);
            //    ControllerContext cc = new ControllerContext(rc, (ControllerBase)controller);

            //    ViewResult viewResult = new ViewResult { ViewName = "error" };
            //    viewResult.ExecuteResult(cc);
            //    ctx.Server.ClearError();
            //}
        }
    }
}
