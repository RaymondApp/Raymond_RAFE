using RAFE.BAL;
using RAFE.Filter;
using RAFE.Models;
using RAFE.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RAFE.Controllers
{
    [Authorize]
    [CheckValidUser]
  
    public class DashboardController : Controller
    {
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
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NominationStatus()
        {
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            return View(nomination_BAL.GetNominationDetails(GetSessionUser()));
        }
        public ActionResult BU_Dashboard()
        {
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            return View(nomination_BAL.GetBUDashboard(GetSessionUser()));
        }
        public ActionResult Line()
        {
            Dashboard dashboard = new Dashboard();
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            return View(nomination_BAL.Dashboard_Nomination(ref dashboard, GetSessionUser()));
        }
    }
}