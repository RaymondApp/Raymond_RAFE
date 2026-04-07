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
    [ExceptionFilter]
    public class HomeController : Controller
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

        public ActionResult Indexnew()
        {
            return View();
        }
        public ActionResult Index()
        {
            AwardMaster_BAL awardMaster_BAL = new AwardMaster_BAL();
            ViewBag.NominationActive = awardMaster_BAL.NominationActive(GetSessionUser());
            return View();
        }
        public ActionResult AllNomination()
        {
            Nomination_BAL objNomination_BAL = new Nomination_BAL();
            var model = objNomination_BAL.GetAllNomination(GetSessionUser());
            if (RAFE.Utility.WebCommon.GetUserRole(Request.Cookies["UserInfo"]).ToUpper() == "BU HR")
            {
                model.AllAwardYears = objNomination_BAL.GetAwardyearCurrent();
                model.Categories = objNomination_BAL.DownloadCategoryBUHR();
                model.Filters = objNomination_BAL.FiltersBUHR(model.Categories[0].text.ToString());
                Session["year"] = model.AllAwardYears[0].ToString();
            }
            else
            {
                model.AllAwardYears = objNomination_BAL.GetAwardyear();
                model.Categories = objNomination_BAL.DownloadCategory();
                model.Filters = objNomination_BAL.Filters(model.Categories[0].text.ToString(),GetSessionUser());
                Session["year"] = model.AllAwardYears[4].ToString();
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult GetSubCategory(string Category)
        {
            Nomination_BAL objNomination_BAL = new Nomination_BAL();
            var model = objNomination_BAL.Filters(Category,GetSessionUser());
            Session["subCategory"] = "";
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AllCategory(string Category,string subCategory,int Year)
        {
            Nomination_BAL objNomination_BAL = new Nomination_BAL();
            Session["year"] = Year;
           
            return PartialView("_nominationList", objNomination_BAL.GetAllCategory(GetSessionUser(), subCategory, Year, Category));
        }

        

        [HttpPost]
        public ActionResult AllNominationYear(string Year)
        {
            Nomination_BAL objNomination_BAL = new Nomination_BAL();
            Session["year"] = Year;
            return PartialView("_nominationList", objNomination_BAL.GetAllNomination(GetSessionUser(),Year));
        }
        [HttpPost]
        public ActionResult NumberOfNomination(string awardtypeid,string nominationId)
        {
            Nomination_BAL objNomination_BAL = new Nomination_BAL();
            //return Json(model,JsonRequestBehavior.AllowGet);
            return Json(objNomination_BAL.GetNumberOfNomination(awardtypeid, nominationId, GetSessionUser()),JsonRequestBehavior.AllowGet);
        }

        [Route("CXOPanel")]
        [HttpGet]
        public ActionResult CXOPanel()
        {
            Nomination_BAL objNomination_BAL = new Nomination_BAL();
            AwardMaster_BAL objAwardMaster_BAL = new AwardMaster_BAL();
            ViewBag.AwardsType = objAwardMaster_BAL.GetAllAwardType(-1);
            ViewBag.Functions = objAwardMaster_BAL.GetAllFunctions();
            return View(objNomination_BAL.GetAllForCXO(GetSessionUser()));
        }
        [Route("CXOPanel")]
        [HttpPost]
        public PartialViewResult CXOPanel(string AwardTypeID, string functionName)
        {
            Nomination_BAL objNomination_BAL = new Nomination_BAL();
            return PartialView("_nominationList", objNomination_BAL.GetAllForCXO(GetSessionUser(), AwardTypeID, functionName));
        }
        [HttpGet]
        public ActionResult NominationForm(int? id)
        {
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            AwardModel objAwardModel;
            AwardMaster_BAL objAwardMaster_BAL = new AwardMaster_BAL();
            if (id != null)
            {
                Nomination_BAL objNomination_BAL = new Nomination_BAL();
                objAwardModel = objNomination_BAL.GetNominationDetail(Convert.ToInt32(id), GetSessionUser());
                ViewBag.AwardsType = objAwardMaster_BAL.GetAllAwardType(objAwardModel.AwardCategory);

            }
            else
            {
                objAwardModel = objAwardMaster_BAL.GetBasicAwardDetails(GetSessionUser());
                if (objAwardModel.AwardCategoryList != null)
                    ViewBag.AwardsType = objAwardMaster_BAL.GetAllAwardType(objAwardModel.AwardCategoryList[0].AwardCategoryId);
                else
                    ViewBag.AwardsType = null;
            }
            RAFE.Models.AwardPortion objAwardPortion = objAwardMaster_BAL.GetAwardTime(GetSessionUser());

            objAwardModel.AwardYearList = objAwardPortion.years;
            objAwardModel.AwardQuarterList = objAwardPortion.quarters;

            objAwardModel.Approvers = nomination_BAL.GetFHList();

            ViewBag.Error = "Page Load";
            return View(objAwardModel);
        }
        [HttpPost]
        public ActionResult NominationForm(AwardModel objAwardModel, string iscxo)
        {
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            string approverName = objAwardModel.financeHeadName;
            ResultMessage resultMessage = new ResultMessage();
            if (objAwardModel.StatusID == 0 && ModelState.IsValid)
            {
                Nomination_BAL objNomination_BAL = new Nomination_BAL();
                string user = GetSessionUser();
                string autoURL = "";
                objAwardModel.StatusID = 0;
                Mail_BAL.GetMail(objAwardModel.NominationID, objAwardModel.StatusID, user, autoURL);
                resultMessage = objNomination_BAL.SaveNomination(objAwardModel, GetSessionUser());
               
            }
            else if (objAwardModel.StatusID > 0)
            {
                Nomination_BAL objNomination_BAL = new Nomination_BAL();
                resultMessage = objNomination_BAL.SaveNomination(objAwardModel, GetSessionUser());
            }
            if (resultMessage.isSuccess)
            {
                if (objAwardModel.StatusID == 0 && objAwardModel.isSubmitted == 0)
                {
                    objAwardModel.financeHeadName = approverName;
                    TempData["status"] = "1";
                    TempData["message"] = "Nomination saved";
                    return RedirectToAction("NominationForm", new { id = Convert.ToInt32(resultMessage.statusMessage) });

                }
                //else if (iscxo != null && iscxo.ToLower() == "true")
                //{
                //    return RedirectToAction("CXOPanel", new { iscxo = true });
                //}
                else
                {
                    return RedirectToAction("AllNomination");
                }         
            }
            else
            {
                AwardMaster_BAL objAwardMaster_BAL = new AwardMaster_BAL();
                objAwardModel = objAwardMaster_BAL.SetAwardDetails(GetSessionUser(), objAwardModel);
                objAwardModel.validUser = true;
                ViewBag.AwardsType = objAwardMaster_BAL.GetAllAwardType(objAwardModel.AwardCategory);
                RAFE.Models.AwardPortion objAwardPortion = objAwardMaster_BAL.GetAwardTime(GetSessionUser());
                objAwardModel.AwardYearList = objAwardPortion.years;
                objAwardModel.AwardQuarterList = objAwardPortion.quarters;
                ViewBag.Error = resultMessage.statusMessage;
            }
            objAwardModel.Approvers = nomination_BAL.GetFHList();
            objAwardModel.financeHeadName = approverName;
            return View(objAwardModel);
        }
        [HttpPost]
        public ActionResult GetAwardData(string id, string DataSection)
        {
            AwardMaster_BAL objAwardMaster_BAL = new AwardMaster_BAL();
            if (DataSection == "AwardType")
            {
                return Json(objAwardMaster_BAL.GetAllAwardType(Convert.ToInt32(id)));
            }
            else if (DataSection == "AwardSection")
            {
                AwardModel objAwardModel = new AwardModel();
                objAwardModel.SectionList = objAwardMaster_BAL.GetAwardSection(Convert.ToInt32(id)).SectionsList;
                objAwardModel.additionalSection = objAwardMaster_BAL.GetAwardAdditionalSection(Convert.ToInt32(id));
                return PartialView("_AwardSection", objAwardModel);
            }
            else if (DataSection == "AwardHistory")
            {
                Nomination_BAL objNomination_BAL = new Nomination_BAL();
                return PartialView("_AwardHistory", objNomination_BAL.GetAllAwardHistory(id));
            }
            else if (DataSection == "EmployeeForAward")
            {
                UserMaster_BAL userMaster_BAL = new UserMaster_BAL();
                return Json(userMaster_BAL.GetAllEmployeeListForAward(GetSessionUser(), Convert.ToInt32(id)));
            }
            Response.End();
            return View();
        }

        [HttpPost]
        public ActionResult CheckAlreadyNominated(int awardYear, string awardQuarter, int awardTypeID, string nominee)
        {
            Nomination_BAL objNomination_BAL = new Nomination_BAL();
            return Json(objNomination_BAL.CheckAlreadyNominated(awardYear, awardQuarter, awardTypeID, nominee));
        }
        [Route("process_note")]
        public FileResult processNote(string id, string name)
        {
            AwardMaster_BAL objAwardMaster_BAL = new AwardMaster_BAL();
            AwardTypeModel objAwardTypeModel = objAwardMaster_BAL.getAwardTypeByAwardTypeID(Convert.ToInt32(id));
            string processNoteURL = Server.MapPath("~/ProcessNote/" + objAwardTypeModel.PPTUrl);
            if (!System.IO.File.Exists(processNoteURL))
            {
                processNoteURL = Server.MapPath("~/ProcessNote/Prism_RR_Process_Note.pdf");
            }
            byte[] FileBytes = System.IO.File.ReadAllBytes(processNoteURL);
            if (System.IO.Path.GetExtension(objAwardTypeModel.PPTUrl).ToLower() == ".pptx")
                return File(FileBytes, "application/vnd.openxmlformats-officedocument.presentationml.presentation");
            else
                return File(FileBytes, "application/pdf");

        }

        [HttpGet]
        public ActionResult approve(string id, string status)
        {

            AwardModel objAwardModel = new AwardModel();
            HttpCookie reqCookies = Request.Cookies["UserInfo"];

            AwardMaster_BAL objAwardMaster_BAL = new AwardMaster_BAL();



            string nominationID = "";

            try
            {
                nominationID = Utility.StringCipher.Decrypt(id, reqCookies.Value.ToString().Split('~')[3]);
            }
            finally
            {
                nominationID = "";
            }

            if (nominationID != "")
            {
                Nomination_BAL objNomination_BAL = new Nomination_BAL();
                objAwardModel = objNomination_BAL.GetNominationDetail(Convert.ToInt32(id), GetSessionUser());
                ViewBag.AwardsType = objAwardMaster_BAL.GetAllAwardType(objAwardModel.AwardCategory);
            }
            if (status == "true")
            {
                ResultMessage resultMessage = new ResultMessage();
                if (objAwardModel.StatusID > 0)
                {
                    Nomination_BAL objNomination_BAL = new Nomination_BAL();
                    objAwardModel.StatusID = Convert.ToInt32(RAFE.Models.NominationStage.Approved);
                    resultMessage = objNomination_BAL.SaveNomination(objAwardModel, GetSessionUser());
                }                
            }          
            return View(objAwardModel);
        }

        public ActionResult UpdateNomination(string nomination, string action)
        {
            return View();
        }

        [Route("Dejavu")]
        public ActionResult Dejavu()
        {
            return View();
        }

        [Route("LastWinners")]
        public ActionResult LastWinners()
        {
            return View();
        }
        [Route("JuryPanel")]
        public ActionResult JuryPanel()
        {
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            return View(nomination_BAL.NominationList(GetSessionUser(), "GET_ALL_shortlisted"));
        }


        [Route("NominationRevert")]
        public ActionResult NominationRevert(int id)
        {
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            return Json(nomination_BAL.Revert(id, GetSessionUser()),
                JsonRequestBehavior.AllowGet);
        }

        //public int SentNotification()
        //{
        //    int Result = 0;
        //    try
        //    {
        //        string strFrom = string.Empty;
        //        string strTo = string.Empty;
        //        string strCc = string.Empty;
        //        string Subject = string.Empty;
        //        string Body = string.Empty;
        //        string Cc1 = string.Empty;
        //        string Bcc = string.Empty;
        //        string Bcc1 = string.Empty;
        //    }
        //}
        [HttpPost]
        public ActionResult Profiler(string email,string otheremail)
        {
            if(email == "" && otheremail == "")
            {
                string msg = "<div style='text-align:center; display:flex; justify-content:center; align-items:center;'><b style='font-size:30px;'>PLEASE SELECT EMPLOYEE FIRST</b></div>";
                return Content(msg,"text/html");
            }
            else
            {
                UserMaster_BAL userMaster_BAL = new UserMaster_BAL();
                UserModel userModel = new UserModel();
                //userModel.NominationId = nominationId;
                userModel = userMaster_BAL.GetDetailsForProfiler(email, otheremail);
                return PartialView("Profiler", userModel);
            }
        }
        public ActionResult AdminPage()
        {
            AwardMaster_BAL awardMaster_BAL = new AwardMaster_BAL();
            List<AwardAdminModel> awardAdminModels = new List<AwardAdminModel>();
            awardAdminModels = awardMaster_BAL.GetAdminControl();
            return View(awardAdminModels);
        }

        [HttpPost]
        public JsonResult ChangeStatus(string id, string Counter)
        {

            AwardMaster_BAL awardMaster_BAL = new AwardMaster_BAL();
            int val = awardMaster_BAL.ChangeStatus(Convert.ToInt32(id), Convert.ToInt32(Counter));
            return Json(val, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ChangeMemberStatus(string id, string Counter)
        {

            AwardMaster_BAL awardMaster_BAL = new AwardMaster_BAL();
            int val = awardMaster_BAL.ChangeMemberStatus(Convert.ToInt32(id), Convert.ToInt32(Counter));
            return Json(val, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteRow(string id)
        {
            AwardMaster_BAL awardMaster_BAL = new AwardMaster_BAL();
            int val = awardMaster_BAL.DeleteEntry(Convert.ToInt32(id));
            return RedirectToAction("AdminPage");
        }
        public ActionResult FinanceApproval(string id, string Counter)
        {
            AwardMaster_BAL awardMaster_BAL = new AwardMaster_BAL();
            int val = awardMaster_BAL.ChangeFinance(Convert.ToInt32(id), Convert.ToInt32(Counter));
            return Json(val, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult AddEdit(AwardAdminModel awardAdminModel)
        {
            Nomination_BAL objNomination_BAL = new Nomination_BAL();
            //AwardAdminModel awardAdminModel = new AwardAdminModel();
            return View(awardAdminModel);
        }
        public ActionResult AwardCommittee()
        {
            Nomination_BAL objNomination_BAL = new Nomination_BAL();
            var model = objNomination_BAL.GetAwardCommitteeNomination(GetSessionUser());
            model.AllAwardYears = objNomination_BAL.GetAwardyearCurrent();
            model.Categories = objNomination_BAL.DownloadCategoryAC();
            model.Filters = objNomination_BAL.Filters(model.Categories[0].text.ToString(),GetSessionUser());
            Session["year"] = model.AllAwardYears[0].ToString();
            return View(model);
        }

        public ActionResult BUCommittee()
        {
            Nomination_BAL objNomination_BAL = new Nomination_BAL();
            var model = objNomination_BAL.GetBUCommitteeNomination(GetSessionUser());
            model.AllAwardYears = objNomination_BAL.GetAwardyearCurrent();
            model.Categories = objNomination_BAL.DownloadCategoryAC();
            model.Filters = objNomination_BAL.Filters(model.Categories[0].text.ToString(),GetSessionUser());
            Session["year"] = model.AllAwardYears[0].ToString();
            return View(model);
        }
        public ActionResult JuryCommittee()
        {
            Nomination_BAL objNomination_BAL = new Nomination_BAL();
            var model = objNomination_BAL.GetJuryCommitteeNomination(GetSessionUser());
            model.AllAwardYears = objNomination_BAL.GetAwardyearCurrent();
            model.Categories = objNomination_BAL.DownloadCategoryJURY();
            model.Filters = objNomination_BAL.Filters(model.Categories[0].text.ToString(),GetSessionUser());
            Session["year"] = model.AllAwardYears[0].ToString();
            return View(model);
        }
        public ActionResult Details(int nominationId)
        {
            AwardMaster_BAL awardMaster_BAL = new AwardMaster_BAL();
            var model = awardMaster_BAL.GetDetails(nominationId,GetSessionUser());
            
            return PartialView("Details",model);
        }

        public ActionResult BUCommitteeDetails(int nominationId)
        {
            AwardMaster_BAL awardMaster_BAL = new AwardMaster_BAL();
            var model = awardMaster_BAL.GetDetails(nominationId, GetSessionUser());

            return PartialView("BUCommitteeDetails", model);
        }

        [HttpPost]
        public ActionResult BUCommiteeSubmit(int StatusID, int NominationID, string Comments, string Shortlisted, string NeedMoreInformation)
        {
            if (NeedMoreInformation != null)
            {
                StatusID = 5;
            }
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            ResultMessage resultMessage = new ResultMessage();
            resultMessage = nomination_BAL.Approval(StatusID, NominationID, GetSessionUser(), Comments);
            Nomination_BAL objNomination_BAL = new Nomination_BAL();
            var model = objNomination_BAL.GetBUCommitteeNomination(GetSessionUser());
            model.AllAwardYears = objNomination_BAL.GetAwardyearCurrent();
            model.Categories = objNomination_BAL.DownloadCategoryAC();
            model.Filters = objNomination_BAL.Filters(model.Categories[0].text.ToString(),GetSessionUser());
            return View("BUCommittee", model);
        }
        public ActionResult JuryDetails(int nominationId)
        {
            AwardMaster_BAL awardMaster_BAL = new AwardMaster_BAL();
            var model = awardMaster_BAL.GetJuryDetails(nominationId, GetSessionUser());
            return PartialView("JuryDetails", model);
        }

        //Saurabh Kumar Date- 19/05/2023
        public ActionResult JuryFeedback(int nominationId, string awardTyp, string nominee)
        {
            TempData["awardTyp"] = awardTyp;
            TempData["nominee"] = nominee;
            string senderMail = RAFE.Utility.WebCommon.GetUserEmail(Request.Cookies["UserInfo"]);
            TempData["NominationId"] = nominationId;
            AwardMaster_BAL awardMaster_BAL = new AwardMaster_BAL();
            var model = awardMaster_BAL.AskJuryFeedbackDetails(nominationId,GetSessionUser(), senderMail);
            return PartialView("_JuryFeedback",model);
        }

        //Saurabh Kumar Date- 22/05/2023
        [HttpPost]
        public JsonResult FDBAsk(string ReceiverEmailId, string AskFeedback)
        {
            int NominationId = Convert.ToInt32(TempData["NominationId"]);
            string SenderEmailId= RAFE.Utility.WebCommon.GetUserEmail(Request.Cookies["UserInfo"]);
            if (ReceiverEmailId != "")
            {
                //Mail_BAL.GetMail(NominationId, 3, ReceiverEmailId, "");
                string mailBody = "Dear Saurabh Kumar" + ",<br>";
                mailBody = mailBody + "<br>";
                mailBody = mailBody + "Thank you for sharing the nomination. Listed below is" + "/are the name(s) of nominee(s) under the category of" + "<b>#AwardCategory#</b>" + ".<br>";
                mailBody = mailBody + "<ul>";
                mailBody = mailBody + "#employee#";
                mailBody = mailBody + "</ul>";
                mailBody = mailBody + "It has now been shared to the next level for review." + "<br>";
                mailBody = mailBody + "<br>";
                mailBody = mailBody + "<a href=" + "https://raymondcentral.com/rafe/>" + "Click Here" + "</a>" + "for" + "details." + "<br>";
                mailBody = mailBody + "<br>";
                mailBody = mailBody + "Thanks & regards" + "<br>";
                mailBody = mailBody + "Team RAFE 2023";
                Utility.EmailLibrary.sendEmail(ReceiverEmailId, "", "Give Feedback", mailBody);
                Nomination_BAL nomination_BAL = new Nomination_BAL();
                int result = nomination_BAL.FDBAsking(NominationId, GetSessionUser(), GetSessionUser(), ReceiverEmailId, SenderEmailId, AskFeedback, TempData["awardTyp"].ToString(), TempData["nominee"].ToString());
            }
            else
            {
                string eMail = RAFE.Utility.WebCommon.GetUserEmail(Request.Cookies["UserInfo"]);
                AwardMaster_BAL awardMaster_BAL = new AwardMaster_BAL();
                var model = awardMaster_BAL.AskJuryFeedbackDetails(NominationId, GetSessionUser(), eMail);
                return Json("error");
            }
            return Json("success");
        }
        //Saurabh Kumar Date: 31/05/2023
        [HttpGet]
        public ActionResult GiveFeedBack()
        {
            string eMail = RAFE.Utility.WebCommon.GetUserEmail(Request.Cookies["UserInfo"]);
            GiveFeedBack ObjGFB = new GiveFeedBack();
            AwardMaster_BAL awardMaster_BAL = new AwardMaster_BAL();

            ObjGFB.GiveFeedBackList=awardMaster_BAL.GiveFeedBacks(eMail);
            //ObjGFB.NominationId =ObjGFB.
            return View(ObjGFB);
        }
        //Saurabh Kumar Date:12/06/2023
        public ActionResult SendFeedBack(int nominationId)
        {
            TempData["nominationId"] = nominationId;
            
            return PartialView("_SendFeedBack");
        }
        //Saurabh Kumar Date:13/06/2023
        [HttpPost]
        public JsonResult SaveFeedback(string GiveFeedback)
        {
            string mail= RAFE.Utility.WebCommon.GetUserEmail(Request.Cookies["UserInfo"]);
            int nominationId = Convert.ToInt32(TempData["nominationId"]);
            AwardMaster_BAL awardMaster_BAL = new AwardMaster_BAL();
            var model = awardMaster_BAL.SaveFeedback(nominationId, GiveFeedback);
            return Json("success");
            
        }
        //Saurabh Kumar Date:13/06/2023
        [HttpGet]
        public ActionResult SeeFeedback()
        {
            string eMail = RAFE.Utility.WebCommon.GetUserEmail(Request.Cookies["UserInfo"]);
            GiveFeedBack ObjGFB = new GiveFeedBack();
            AwardMaster_BAL awardMaster_BAL = new AwardMaster_BAL();

            ObjGFB.GiveFeedBackList = awardMaster_BAL.SeeFeedBacks(eMail);
            //ObjGFB.NominationId =ObjGFB.
            return View(ObjGFB);
        }
        [HttpPost]
        public ActionResult FDBSend(int NominationID, string Comments)
        {
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            int result = nomination_BAL.JCSaving(NominationID, Comments, GetSessionUser(), GetSessionUser());
            return RedirectToAction("JuryCommittee");

        }
        public FileResult Download(string Name)
        {
            //@"c:\folder\myfile.ext"
            
            string filepath = ConfigurationManager.AppSettings["PhysicalPath"].ToString()+ "\\File\\" + Name;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);
            string fileName = Name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        [HttpPost]
        public ActionResult ACSubmit(int StatusID, int NominationID, string commentsd, string Shortlisted , string NeedMoreInformation)
        {
           if(NeedMoreInformation != null)
            {
                StatusID = 5;
            }
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            ResultMessage resultMessage = new ResultMessage();
            resultMessage = nomination_BAL.Approval(StatusID, NominationID, GetSessionUser(), commentsd);
            Nomination_BAL objNomination_BAL = new Nomination_BAL();
            var model = objNomination_BAL.GetAwardCommitteeNomination(GetSessionUser());
            model.AllAwardYears = objNomination_BAL.GetAwardyearCurrent();
            model.Categories = objNomination_BAL.DownloadCategoryAC();
            model.Filters = objNomination_BAL.Filters(model.Categories[0].text.ToString(),GetSessionUser());
            return View("AwardCommittee",model);
        }
        [HttpPost]
        public ActionResult JCSave(int NominationID, string Comments)
        {
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            int result = nomination_BAL.JCSaving(NominationID, Comments, GetSessionUser(), GetSessionUser());
            return RedirectToAction("JuryCommittee");
            
        }
        [HttpPost]
        public ActionResult AwardCategory(string Category, string subCategory, int Year)
        {
            Nomination_BAL objNomination_BAL = new Nomination_BAL();
            Session["year"] = Year;
            return PartialView("_awardcommitteelist", objNomination_BAL.GetAwardCommitteeCategory(GetSessionUser(), subCategory, Year, Category));
        }
        public ActionResult Comments(int nominationId,string createdby)
        {
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            AwardModel model = new AwardModel();
            if(String.IsNullOrEmpty(createdby))
            {
                createdby = "ALL MEMBERS";
            }
            model.Comments = nomination_BAL.GetACComments(nominationId, createdby);  //call database function here
            model.NominationID = nominationId;
            model.ACNames = nomination_BAL.ACMembers(nominationId);
            return PartialView("_accomments", model);
        }
        [HttpPost]
        public JsonResult ACommitteeComments(int nominationId,string createdby)
        {
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            if (String.IsNullOrEmpty(createdby))
            {
                createdby = "ALL MEMBERS";
            }
            return Json(nomination_BAL.GetACComments(nominationId, createdby), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ACSave(int nominationId, string Comments,AwardModel awardModel)
        {
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            int result = nomination_BAL.ACSaving(nominationId, Comments, GetSessionUser(), awardModel.ACName);
            return RedirectToAction("AwardCommittee");
        }
        public ActionResult ACRatings(int nominationId)
        {
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            AwardModel model = new AwardModel();
            model.NominationID = nominationId;
            //model.RatingList = nomination_BAL.ACRatings(nominationId);
            return PartialView("_acratings", nomination_BAL.ACRatings(nominationId));
        }
        public JsonResult ACRatingsSave(List<RatingHeads> model)
        {
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            for(int i = 0; i < model.Count; i++)
            {
                nomination_BAL.ACRatingsSaving(model[i].nominationid, model[i].ratingname, model[i].weightage, model[i].score,(model[i].score)*(model[i].weightage)/100);
            }
         //   int result = nomination_BAL.ACRatingsSaving(nominationId, model.RatingNames, score);
            return Json("AwardCommittee",JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AwardName_Update(int awardtypeid,string awardname,string awarddesc)
        {
            Nomination_BAL nomination_BAL = new Nomination_BAL();  
            int model = nomination_BAL.AwardNameUpdate(awardtypeid,awardname,awarddesc);
            //return RedirectToAction("AdminPage", "Home");
            //return RedirectToAction("AdminPage");
            return Json("AwardCommittee",JsonRequestBehavior.AllowGet);
        }
        public ActionResult ACMapping(int? id)
        {
            //AwardMaster_BAL awardMaster_BAL = new AwardMaster_BAL();
            //List<ACMember> aCMember = new List<ACMember>();
            //aCMember = awardMaster_BAL.GetACMember();
            ////ViewBag.AwardNameView = awardMaster_BAL.ACMapping();
            //return PartialView("_acmapping", aCMember);
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            AwardModel objAwardModel;
            AwardMaster_BAL objAwardMaster_BAL = new AwardMaster_BAL();
            if (id != null)
            {
                Nomination_BAL objNomination_BAL = new Nomination_BAL();
                objAwardModel = objNomination_BAL.GetNominationDetail(Convert.ToInt32(id), GetSessionUser());
                ViewBag.AwardsType = objAwardMaster_BAL.GetAllAwardType(objAwardModel.AwardCategory);

            }
            else
            {
                objAwardModel = objAwardMaster_BAL.GetBasicAwardDetails(GetSessionUser());
                if (objAwardModel.AwardCategoryList != null)
                    ViewBag.AwardsType = objAwardMaster_BAL.GetAllAwardType(objAwardModel.AwardCategoryList[0].AwardCategoryId);
                else
                    ViewBag.AwardsType = null;
            }
            RAFE.Models.AwardPortion objAwardPortion = objAwardMaster_BAL.GetAwardTime(GetSessionUser());

            objAwardModel.AwardYearList = objAwardPortion.years;
            objAwardModel.AwardQuarterList = objAwardPortion.quarters;

            objAwardModel.Approvers = nomination_BAL.GetFHList();

            ViewBag.Error = "Page Load";
            return PartialView("_acmapping",objAwardModel);
        }

        [HttpPost]
        public ActionResult download1(List1 objList)
        {
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            DataTable dt = new DataTable();
            try
            {
                dt = nomination_BAL.ExcelExport();
                dt.Columns.Remove("ChartColor");
                string attachment = "attachment; filename=Nomination Details " + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day /*+ DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second*/ + ".xls ";
                HttpContext.Response.Clear();
                HttpContext.Response.AddHeader("content-disposition", attachment);
                HttpContext.Response.ContentType = "application/vnd.ms-excel";
                string sTab = "";
                foreach (DataColumn dc in dt.Columns)
                {
                    HttpContext.Response.Write(sTab + dc.ColumnName);
                    sTab = "\t";        
                }
                HttpContext.Response.Write("\n");
                int i;
                foreach (DataRow dr in dt.Rows)
                {
                    sTab = "";
                    for (i = 0; i < dt.Columns.Count; i++)
                    {
                        HttpContext.Response.Write(sTab + dr[i].ToString());
                        sTab = "\t";
                    }
                    HttpContext.Response.Write("\n");
                }
                HttpContext.Response.End();
            }
            catch (Exception)
            {

            }
            return View(nomination_BAL.GetNominationDetails(GetSessionUser()));
        }

        [HttpPost]
        public ActionResult download2(List2 objList)
        {
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            DataTable dt = new DataTable();
            try
            {
                dt = nomination_BAL.ExcelExport2();
                dt.Columns.Remove("ChartColor");
                string attachment = "attachment; filename=Award Type Status" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + ".xls ";
                HttpContext.Response.Clear();
                HttpContext.Response.AddHeader("content-disposition", attachment);
                HttpContext.Response.ContentType = "application/vnd.ms-excel";
                string sTab = "";
                foreach (DataColumn dc in dt.Columns)
                {
                    HttpContext.Response.Write(sTab + dc.ColumnName);
                    sTab = "\t";
                }
                HttpContext.Response.Write("\n");
                int i;
                foreach (DataRow dr in dt.Rows)
                {
                    sTab = "";
                    for (i = 0; i < dt.Columns.Count; i++)
                    {
                        HttpContext.Response.Write(sTab + dr[i].ToString());
                        sTab = "\t";
                    }
                    HttpContext.Response.Write("\n");
                }
                HttpContext.Response.End();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(nomination_BAL.GetNominationDetails(GetSessionUser()));
        }
        [HttpPost]
        public ActionResult download3(List2 objList)
        {
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            DataTable dt = new DataTable();
            try
            {
                dt = nomination_BAL.ExcelExport3();
                dt.Columns.Remove("isActive");
                string attachment = "attachment; filename=Previous Year Winners" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + ".xls ";
                HttpContext.Response.Clear();
                HttpContext.Response.AddHeader("content-disposition", attachment);
                HttpContext.Response.ContentType = "application/vnd.ms-excel";
                string sTab = "";
                foreach (DataColumn dc in dt.Columns)
                {
                    HttpContext.Response.Write(sTab + dc.ColumnName);
                    sTab = "\t";
                }
                HttpContext.Response.Write("\n");
                int i;
                foreach (DataRow dr in dt.Rows)
                {
                    sTab = "";
                    for (i = 0; i < dt.Columns.Count; i++)
                    {
                        HttpContext.Response.Write(sTab + dr[i].ToString());
                        sTab = "\t";
                    }
                    HttpContext.Response.Write("\n");
                }
                HttpContext.Response.End();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }
        public ActionResult PreviousYearWinners()
        {
            AwardMaster_BAL awardMaster_BAL = new AwardMaster_BAL();
            AwardMaster objawadmaster = new AwardMaster();
            objawadmaster.YearList = awardMaster_BAL.YearList("1");
            objawadmaster.PreviousYearWinner = awardMaster_BAL.GetPreviousWinners();
            return View(objawadmaster);
        }
    }
}