using ClosedXML.Excel;
using RAFE.BAL;
using RAFE.Filter;
using RAFE.Models;
using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace RAFE.Controllers
{

    public class CommonController : Controller
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

        #region Get Menu For LoggedIn User
        [ChildActionOnly]
        [Authorize]
        public PartialViewResult GetNevigationMenu()
        {
            Menu_BAL objMenu = new Menu_BAL();
            MenuModel objMenuModel = objMenu.GetMenu(GetSessionUser());
            return PartialView("_NevigationMenu", objMenuModel);
        }
        #endregion

        [HttpPost]
        public JsonResult AutoCompleteList(string Prefix, string MasterName)
        {
            if (MasterName == "User" || MasterName == "Ref-User")
            {
                UserMaster_BAL userMaster_BAL = new UserMaster_BAL();
                return Json(userMaster_BAL.UserAutoComplete(Prefix, MasterName), JsonRequestBehavior.AllowGet);
            }
            return Json("ok");
        }

        #region Error Page

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult unauthorized()
        {
            return View();
        }

        #endregion

        #region Logged In User
        [ChildActionOnly]
        [Authorize]
        public PartialViewResult GetLoggedinUserDetails()
        {
            UserMaster_BAL userMaster_BAL = new UserMaster_BAL();
            return PartialView("_LoggedInUser", userMaster_BAL.GetuserDetails(GetSessionUser()));
        }
        #endregion

        [ExceptionFilter]
        public ActionResult downloadvideo(string id, string fileType)
        {
            Nomination_BAL objNomination_BAL = new Nomination_BAL();
            AwardModel awardModel = objNomination_BAL.GetNominationDetail(Convert.ToInt32(id), GetSessionUser());
            string fileName = "";
            string filepath ="";
            if (fileType.ToLower() == "video")
            {
                filepath = Utility.WebCommon.GetApplicationPhysicalPath() + "video\\" + awardModel.videoFilePath;
                fileName = awardModel.videoFileName;
            }

            else if (fileType.ToLower() == "file")
            {
                filepath = Utility.WebCommon.GetApplicationPhysicalPath() + "file\\" + awardModel.addFilePath;
                fileName = awardModel.addFileName;
            }
            else if (fileType.ToLower() == "financefile")
            {
                filepath = Utility.WebCommon.GetApplicationPhysicalPath() + "financeFile\\" + awardModel.financeFileName;
                fileName = awardModel.financeFilePath;
            }

            byte[] filedata = System.IO.File.ReadAllBytes(filepath);
            string contentType = MimeMapping.GetMimeMapping(filepath);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = filepath,
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filedata, contentType);
        }

        [ExceptionFilter]
        public ActionResult downloadData(string dataType)
        {

            download(dataType);
            return View();
        }
        //public ActionResult downloadByCategory()
        private DataSet download(string dataType)
        {
            int i = 0;
            DataSet ds = new DataSet();
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            using (XLWorkbook wb = new XLWorkbook())
            {
                if (Session["category"] != null && Session["subcategory"].ToString() != "")
                {
                     ds = nomination_BAL.DownloadByCategory(Convert.ToInt32(Session["category"].ToString()), Session["subcategory"].ToString(), GetSessionUser(), Session["year"].ToString());
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataTable dt in ds.Tables)
                        {
                            if (dt.Rows.Count > 0 && ds.Tables.Count == ds.Tables[ds.Tables.Count - 1].Rows.Count + 1)
                            {
                                if (i < ds.Tables[ds.Tables.Count - 1].Rows.Count)
                                    dt.TableName = ds.Tables[ds.Tables.Count - 1].Rows[i][0].ToString();
                            }
                            if (dt.Rows.Count > 0)
                                wb.Worksheets.Add(dt);
                            i++;
                        }
                    }
                }
                else if (Session["year"] != null && dataType.ToLower() == "list")
                {
                    String Year = Session["year"].ToString();
                      ds = nomination_BAL.DownloadByYear(GetSessionUser(), Year);
                    wb.Worksheets.Add(ds.Tables[0]);
                }
                //else if (Session["category") == "Business Wise" && Session["subcategory"].ToString != "")
                //{
                //    String Year = Session["year"].ToString();
                //}
                else
                {
                    String Year = "0";
                    if (Session["year"] != null)
                    {
                        Year = Session["year"].ToString();
                    }
                    using ( ds = nomination_BAL.DataForExport(dataType, GetSessionUser(), Year))

                    {

                        if (dataType.ToLower() == "indetail")
                        {
                            foreach (DataTable dt in ds.Tables)
                            {
                                if (dt.Rows.Count > 0 && ds.Tables.Count == ds.Tables[ds.Tables.Count - 1].Rows.Count + 1)
                                {
                                    if (i < ds.Tables[ds.Tables.Count - 1].Rows.Count)
                                        dt.TableName = ds.Tables[ds.Tables.Count - 1].Rows[i][0].ToString();
                                }
                                if (dt.Rows.Count > 0)
                                    wb.Worksheets.Add(dt);
                                i++;
                            }
                        }
                        else if (dataType.ToLower() == "list")
                        {
                            wb.Worksheets.Add(ds.Tables[0]);
                        }
                    }

                }
                //Export the Excel file.
                if (wb.Worksheets.Count > 0)
                {
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=" + DateTime.Now.Ticks.ToString() + ".xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
                else
                {

                }
            }
            return ds;
        }
        private void downloadForJury(string datatype)
        {
            DataSet ds = new DataSet();
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            using (XLWorkbook wb = new XLWorkbook())
            {
                ds = nomination_BAL.DownloadForJury(Convert.ToInt32(Session["category"].ToString()), Session["subcategory"].ToString(), GetSessionUser(), Session["year"].ToString());
                wb.Worksheets.Add(ds.Tables[0]);

                if (wb.Worksheets.Count > 0)
                {
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=" + DateTime.Now.Ticks.ToString() + ".xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
                else
                {

                }
            }
        }
        public ActionResult DataDownload(FormCollection formCollection)
        {
           
            //string category = formCollection["category"].ToString();
            Session["category"] = formCollection["category"].ToString();
            
            string datatype = formCollection["master"].ToString();

            Session["year"] = formCollection["year"].ToString();
            if (formCollection["category"].ToString() == "1" && formCollection["subcategory"] == null)
            {
                
                download(formCollection["master"].ToString());
            }
            if(formCollection["subcategory"] != null)
            {
                //string subcategory = formCollection["subcategory"].ToString();
                Session["subcategory"] = formCollection["subcategory"].ToString();
                download(formCollection["master"].ToString());
            }
            return View();
        }
        public ActionResult DownloadForJury(FormCollection formCollection)
        {
            //string category = formCollection["category"].ToString();
            Session["category"] = formCollection["category"].ToString();

            string datatype = formCollection["master"].ToString();

            Session["year"] = formCollection["year"].ToString();
            if (formCollection["category"].ToString() == "1" && formCollection["subcategory"] == null)
            {
                downloadForJury(formCollection["master"].ToString());
            }
            if (formCollection["subcategory"] != null)
            {
                //string subcategory = formCollection["subcategory"].ToString();
                Session["subcategory"] = formCollection["subcategory"].ToString();
                downloadForJury(formCollection["master"].ToString());
            }
            return View();
        }
    }
}