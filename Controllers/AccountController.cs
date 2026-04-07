using RAFE.BAL;
using RAFE.Models;
using RAFE.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace RAFE.Controllers
{
    public class AccountController : Controller
    {
        #region Global Declaration

        UserMaster_BAL objUserMaster_BAL;
        ResultMessage objResultMessage;

        #endregion

        #region Common Method
        private int GetSessionUser()
        {
            string[] user = User.Identity.Name.Split('~');
            return Convert.ToInt32(user[0]);
        }
        
        #endregion


        #region Login
        [Route("login")]
        public ActionResult Index()
        {

            var user = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            return View();
        }

        [Route("login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginModel objLoginModel, string returnUrl)
        {
            objUserMaster_BAL = new UserMaster_BAL();
            objResultMessage = objUserMaster_BAL.UserLogin(objLoginModel);
            if (objResultMessage.isSuccess)
            {
                authenticate(objResultMessage.statusMessage, "Admin", false);
            }
            else
            {
                ViewBag.ErrorMsg = "Please enter your valid User-id and Password....!!!!";
                objLoginModel.password = "";
                return View(objLoginModel);
            }

            if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                      && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion

        #region Logoff

        [Route("logoff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }


        #endregion

        #region Change Password
        [Route("change-password")]
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel objChangePass)
        {
            try
            {
                objUserMaster_BAL = new UserMaster_BAL();
                if (ModelState.IsValid)
                {
                    objResultMessage = objUserMaster_BAL.ChangePassword(objChangePass, GetSessionUser());
                    if (objResultMessage.isSuccess)
                    {
                        return Json(objResultMessage.isSuccess);
                    }
                    else
                    {
                        return Json(objResultMessage.statusMessage);
                    }
                }
                else
                {
                    return Json("Please fill all required entries.");
                }
            }
            catch (Exception)
            {
                return Json("Server has encounter some error. Please try again later.");
            }
        }
        #endregion

        #region Forgot Password

        [Route("forgot-password")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult forgotpassword(FormCollection frm)
        {
            try
            {

                UserMaster_BAL objUser = new UserMaster_BAL();
                ResultMessage objResultMessage = objUser.ForgotPassword(frm[""].ToString());
                if (objResultMessage.isSuccess)
                {
                    return Json(objResultMessage.isSuccess);
                }
                else
                {
                    return Json(objResultMessage.statusMessage);
                }
            }
            catch (Exception)
            {
                return Json("Server has encounter some error. Please try again later.");
            }
        }

        #endregion

        #region Loin with different user
        [HttpPost]
        public ActionResult loginwithDiffUser(string email)
        {
            try
            {
                HttpCookie myCookie;
                string Bu_HR = "";
                if (Request.Cookies["UserInfo_Old"] == null)
                {
                    Bu_HR = WebCommon.GetUserName(Request.Cookies["UserInfo"]);
                }
                else
                {
                    Bu_HR = WebCommon.GetUserName(Request.Cookies["UserInfo_Old"]);
                }

                UserMaster_BAL objUser = new UserMaster_BAL();
                ResultMessage objResultMessage = objUser.GetUserDetailsForDiffLogin(email, Bu_HR);
                if (objResultMessage.isSuccess)
                {
                    if (Request.Cookies["UserInfo_Old"] == null)
                    {
                        myCookie = new HttpCookie("UserInfo_Old");
                        myCookie.Expires = DateTime.Now.AddMinutes(30);
                        myCookie.Value = Request.Cookies["UserInfo"].Value;
                        HttpContext.Response.Cookies.Add(myCookie);
                    }
                    if (Request.Cookies["UserInfo"] != null)
                    {
                        myCookie = Request.Cookies["UserInfo"];
                        myCookie.Value = objResultMessage.statusMessage;
                        HttpContext.Response.Cookies.Add(myCookie);
                    }

                    return Json(objResultMessage.isSuccess);
                }
                else
                {
                    return Json(objResultMessage.isSuccess);
                }
            }
            catch (Exception )
            {
                return Json("Server has encounter some error. Please try again later.");
            }
        }

#endregion

        #region Local Method
        public void authenticate(string userData, string role, bool isPesistant)
        {
            var data = userData.Split('~');
            var authTicket = new FormsAuthenticationTicket(Convert.ToInt32(data[0]), userData, DateTime.Now, DateTime.Now.Add(FormsAuthentication.Timeout), isPesistant, data[3]);
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            HttpContext.Response.Cookies.Add(authCookie);
        }

        #endregion
	}
}