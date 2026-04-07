using RAFE.BAL;
using RAFE.Models;
using System;
using System.Web.Mvc;

namespace RAFE.Controllers
{
    public class MasterController : Controller
    {
        // GET: Master
        AwardMaster_BAL objAwardMaster_Bal;

        public ActionResult AllAwards()
        {
            objAwardMaster_Bal = new AwardMaster_BAL();
            return View(objAwardMaster_Bal.GetAllAwardMaster(0));
        }

        [HttpGet]
        public ActionResult EditAward(int?id)
        {
            objAwardMaster_Bal = new AwardMaster_BAL();
            AwardMaster objAwardMaster = new AwardMaster();
            if (id != null)
            {
                objAwardMaster = objAwardMaster_Bal.GetAwardByAwardId(Convert.ToInt32(id));
                objAwardMaster.action = "Edit Award Details";
            }
            else
            {
                objAwardMaster.action = "Add Award Details";
            }
            return View(objAwardMaster);
        }

        [HttpPost]
        public ActionResult EditAward(AwardMaster objAwardMaster)
        {
            objAwardMaster_Bal = new AwardMaster_BAL();
            if(ModelState.IsValid)            
            {
                string ret = objAwardMaster_Bal.SaveAward(objAwardMaster);
                if (ret.Split(',')[0].ToString().ToUpper() == "SUCCESS")
                {
                    TempData["status"] = "1";
                    TempData["message"] = "Award details successfully saved.";
                    return RedirectToAction("GetAllAwards");
                }
                else
                {
                    TempData["status"] = "2";
                    TempData["message"] = "Failed to save award details. Try again later.";                    
                }
            }           
            return View(objAwardMaster);
        }
    }
}