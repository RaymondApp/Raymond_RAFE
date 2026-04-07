using RAFE.BAL;
using RAFE.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrismAward.Controllers
{

    [Authorize]
    public class AutoActionController : Controller
    {
        #region Common Method
        private string GetSessionUser()
        {
            HttpCookie reqCookies = Request.Cookies["UserInfo"];
            if (reqCookies != null)
            {
                return reqCookies.Value.ToString().Split('~')[0];
            }
            else
                return "";
        }

        #endregion
        [Route("autoaction")]
        public ActionResult Index(string ekey, string akey)
        {
            string dKey = "";
            string dAction = "";
            UserMaster_BAL userMaster_BAL = new UserMaster_BAL();
            string loginUserEmail = userMaster_BAL.GetUserDetailsForLogin(GetSessionUser()).statusMessage.Split('~')[3];

            dKey = StringCipher.Decrypt(ekey, loginUserEmail);

            dAction = StringCipher.Encrypt(akey, loginUserEmail);

            if (dAction == "reject")
            {

            }
            else
            {

            }


            return View();
        }
    }
}