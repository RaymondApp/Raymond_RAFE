using RAFE.DAL;
using RAFE.Models;
using RAFE.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Text;

namespace RAFE.BAL
{
    public class Nomination_BAL
    {
        string videoFilename = "";
        string addFilename = "";
        string financeFilename = "";
        /// <summary>
        /// Save Nomination form
        /// </summary>
        /// <param name="objAwardModel"> contating all the nomimnation details inculding draft save or submitt</param>
        /// <returns></returns>
        public ResultMessage SaveNomination(AwardModel objAwardModel, string userName)
        {
            ResultMessage resultMessage = new ResultMessage();
            string EmployeeIDs = "", OtherEmployeeIDs = "";
            string pic = "";
            #region Employee 1 details
            if (!string.IsNullOrEmpty(objAwardModel.EmployeeID1))
            {
                EmployeeIDs = objAwardModel.EmployeeID1;

                if (objAwardModel.EmployeeID1.ToLower() == "other")
                {
                    OtherEmployeeIDs = objAwardModel.Other_EmployeeID1;
                }
            }


            pic = SaveUpload(objAwardModel.EmployeeImage1, DateTime.Now.Ticks.ToString(), objAwardModel.ImagePath1, "Image");

            if(objAwardModel.videoFile != null )
            {
                if (objAwardModel.videoFile.ContentLength < 10000000)
                {
                    objAwardModel.videoFilePath = SaveUpload(objAwardModel.videoFile, objAwardModel.videoFileName, objAwardModel.videoFilePath, "Video");
                }
                else
                {
                    objAwardModel.videoFilePath = "";
                }
            }
            else
            {
                objAwardModel.videoFilePath = SaveUpload(objAwardModel.videoFile, objAwardModel.videoFileName, objAwardModel.videoFilePath, "Video");
            }


            objAwardModel.videoFileName = videoFilename;

            objAwardModel.addFilePath = SaveUpload(objAwardModel.addFile, objAwardModel.addFileName, objAwardModel.addFilePath, "File");

            objAwardModel.addFileName = addFilename;

            objAwardModel.financeFilePath = SaveUpload(objAwardModel.financeFile, objAwardModel.financeFileName, objAwardModel.financeFilePath, "financeFile");

            objAwardModel.financeFileName = financeFilename;
            #endregion

            #region Employee 2 details
            if (objAwardModel.EmpCount > 1)
            {
                if (!string.IsNullOrEmpty(objAwardModel.EmployeeID2))
                {
                    if (string.IsNullOrEmpty(EmployeeIDs))
                    {
                        EmployeeIDs = objAwardModel.EmployeeID2;
                    }
                    else
                    {
                        EmployeeIDs = EmployeeIDs + "," + objAwardModel.EmployeeID2;
                    }

                    if (objAwardModel.EmployeeID2.ToLower() == "other")
                    {
                        OtherEmployeeIDs = OtherEmployeeIDs + "," + objAwardModel.Other_EmployeeID2;
                    }
                    else
                    {
                        OtherEmployeeIDs = OtherEmployeeIDs + ",";
                    }

                }
            }
            #endregion

            #region Employee 3 details
            if (objAwardModel.EmpCount > 2)
            {
                if (!string.IsNullOrEmpty(objAwardModel.EmployeeID3))
                {
                    if (string.IsNullOrEmpty(EmployeeIDs))
                    {
                        EmployeeIDs = objAwardModel.EmployeeID3;
                    }
                    else
                    {
                        EmployeeIDs = EmployeeIDs + "," + objAwardModel.EmployeeID3;
                    }

                    if (objAwardModel.EmployeeID3.ToLower() == "other")
                    {
                        OtherEmployeeIDs = OtherEmployeeIDs + "," + objAwardModel.Other_EmployeeID3;
                    }
                    else
                    {
                        OtherEmployeeIDs = OtherEmployeeIDs + ",";
                    }
                }
            }
            #endregion

            #region Employee 4 details
            if (objAwardModel.EmpCount > 3)
            {
                if (!string.IsNullOrEmpty(objAwardModel.EmployeeID4))
                {
                    if (string.IsNullOrEmpty(EmployeeIDs))
                    {
                        EmployeeIDs = objAwardModel.EmployeeID4;
                    }
                    else
                    {
                        EmployeeIDs = EmployeeIDs + "," + objAwardModel.EmployeeID4;
                    }

                    if (objAwardModel.EmployeeID4.ToLower() == "other")
                    {
                        OtherEmployeeIDs = OtherEmployeeIDs + "," + objAwardModel.Other_EmployeeID4;
                    }
                    else
                    {
                        OtherEmployeeIDs = OtherEmployeeIDs + ",";
                    }
                }

            }
            #endregion

            #region Employee 5 details
            if (objAwardModel.EmpCount > 4)
            {
                if (!string.IsNullOrEmpty(objAwardModel.EmployeeID5))
                {
                    if (string.IsNullOrEmpty(EmployeeIDs))
                    {
                        EmployeeIDs = objAwardModel.EmployeeID5;
                    }
                    else
                    {
                        EmployeeIDs = EmployeeIDs + "," + objAwardModel.EmployeeID5;
                    }

                    if (objAwardModel.EmployeeID5.ToLower() == "other")
                    {
                        OtherEmployeeIDs = OtherEmployeeIDs + "," + objAwardModel.Other_EmployeeID5;
                    }
                    else
                    {
                        OtherEmployeeIDs = OtherEmployeeIDs + ",";
                    }
                }
            }
            #endregion

            #region Employee 6 details
            if (objAwardModel.EmpCount > 5)
            {
                if (!string.IsNullOrEmpty(objAwardModel.EmployeeID6))
                {
                    if (string.IsNullOrEmpty(EmployeeIDs))
                    {
                        EmployeeIDs = objAwardModel.EmployeeID6;
                    }
                    else
                    {
                        EmployeeIDs = EmployeeIDs + "," + objAwardModel.EmployeeID6;
                    }

                    if (objAwardModel.EmployeeID6.ToLower() == "other")
                    {
                        OtherEmployeeIDs = OtherEmployeeIDs + "," + objAwardModel.Other_EmployeeID6;
                    }
                    else
                    {
                        OtherEmployeeIDs = OtherEmployeeIDs + ",";
                    }
                }
            }
            #endregion

            #region Employee 7 details
            if (objAwardModel.EmpCount > 6)
            {
                if (!string.IsNullOrEmpty(objAwardModel.EmployeeID7))
                {
                    if (string.IsNullOrEmpty(EmployeeIDs))
                    {
                        EmployeeIDs = objAwardModel.EmployeeID7;
                    }
                    else
                    {
                        EmployeeIDs = EmployeeIDs + "," + objAwardModel.EmployeeID7;
                    }

                    if (objAwardModel.EmployeeID7.ToLower() == "other")
                    {
                        OtherEmployeeIDs = OtherEmployeeIDs + "," + objAwardModel.Other_EmployeeID7;
                    }
                    else
                    {
                        OtherEmployeeIDs = OtherEmployeeIDs + ",";
                    }
                }
            }
            #endregion

            #region Employee 8 details
            if (objAwardModel.EmpCount > 7)
            {
                if (!string.IsNullOrEmpty(objAwardModel.EmployeeID8))
                {
                    if (string.IsNullOrEmpty(EmployeeIDs))
                    {
                        EmployeeIDs = objAwardModel.EmployeeID8;
                    }
                    else
                    {
                        EmployeeIDs = EmployeeIDs + "," + objAwardModel.EmployeeID8;
                    }

                    if (objAwardModel.EmployeeID8.ToLower() == "other")
                    {
                        OtherEmployeeIDs = OtherEmployeeIDs + "," + objAwardModel.Other_EmployeeID8;
                    }
                    else
                    {
                        OtherEmployeeIDs = OtherEmployeeIDs + ",";
                    }
                }
            }
            #endregion

            #region Employee 9 details
            if (objAwardModel.EmpCount > 8)
            {
                if (!string.IsNullOrEmpty(objAwardModel.EmployeeID9))
                {
                    if (string.IsNullOrEmpty(EmployeeIDs))
                    {
                        EmployeeIDs = objAwardModel.EmployeeID9;
                    }
                    else
                    {
                        EmployeeIDs = EmployeeIDs + "," + objAwardModel.EmployeeID9;
                    }

                    if (objAwardModel.EmployeeID9.ToLower() == "other")
                    {
                        OtherEmployeeIDs = OtherEmployeeIDs + "," + objAwardModel.Other_EmployeeID9;
                    }
                    else
                    {
                        OtherEmployeeIDs = OtherEmployeeIDs + ",";
                    }
                }
            }
            #endregion

            #region Employee 10 details
            if (objAwardModel.EmpCount > 9)
            {
                if (!string.IsNullOrEmpty(objAwardModel.EmployeeID10))
                {
                    if (string.IsNullOrEmpty(EmployeeIDs))
                    {
                        EmployeeIDs = objAwardModel.EmployeeID10;
                    }
                    else
                    {
                        EmployeeIDs = EmployeeIDs + "," + objAwardModel.EmployeeID10;
                    }

                    if (objAwardModel.EmployeeID10.ToLower() == "other")
                    {
                        OtherEmployeeIDs = OtherEmployeeIDs + "," + objAwardModel.Other_EmployeeID10;
                    }
                    else
                    {
                        OtherEmployeeIDs = OtherEmployeeIDs + ",";
                    }
                }
            }
            #endregion

            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@NominationID", SqlDbType.Int).Value = objAwardModel.NominationID;
            command.Parameters.Add("@AwardTypeID", SqlDbType.Int).Value = objAwardModel.AwardType;
            command.Parameters.Add("@awardYear", SqlDbType.Int).Value = objAwardModel.AwardYear;
            command.Parameters.Add("@AwardQuarter", SqlDbType.VarChar, 150).Value = objAwardModel.AwardQuarter;
            command.Parameters.Add("@employeenm", SqlDbType.VarChar, 2000).Value = EmployeeIDs;
            command.Parameters.Add("@otheremployeenm", SqlDbType.VarChar, 2000).Value = OtherEmployeeIDs;
            command.Parameters.Add("@empphotopath", SqlDbType.VarChar, 2000).Value = pic;
            command.Parameters.Add("@videoFileName", SqlDbType.VarChar, 2000).Value = objAwardModel.videoFileName;
            command.Parameters.Add("@videoFilePath", SqlDbType.VarChar, 2000).Value = objAwardModel.videoFilePath;
            command.Parameters.Add("@addFileName", SqlDbType.VarChar, 500).Value = objAwardModel.addFileName;
            command.Parameters.Add("@addFilePath", SqlDbType.VarChar, 500).Value = objAwardModel.addFilePath;
            command.Parameters.Add("@refName1", SqlDbType.VarChar, 250).Value = objAwardModel.refName1;
            command.Parameters.Add("@refName2", SqlDbType.VarChar, 250).Value = objAwardModel.refName2;
            command.Parameters.Add("@isSubmitted", SqlDbType.Int).Value = objAwardModel.isSubmitted;
            command.Parameters.Add("@financeFilePath", SqlDbType.VarChar, 500).Value = objAwardModel.financeFilePath;
            command.Parameters.Add("@financeFileName", SqlDbType.VarChar, 250).Value = objAwardModel.financeFileName;
            //To check if nomination is submitted at initial level or at approval level?
            // if statusid = 0 then form is submitted at initial level
            if (objAwardModel.isSubmitted == 1 && objAwardModel.StatusID == 0)
            {
                command.Parameters.Add("@ApprovalLevel", SqlDbType.Int).Value = 1;
                command.Parameters.Add("@StatusID", SqlDbType.Int).Value = 4;
            }
            //else submitted from approval level and status may be any one like approved, rejected or need for more info
            else if (objAwardModel.isSubmitted == 1 && objAwardModel.StatusID > 0)
            {
                command.Parameters.Add("@StatusID", SqlDbType.Int).Value = objAwardModel.StatusID;
            }
            command.Parameters.Add("@HOD", SqlDbType.VarChar, 250).Value = objAwardModel.HOD;
            command.Parameters.Add("@Remarks", SqlDbType.VarChar, 2000).Value = objAwardModel.Remarks;
            command.Parameters.Add("@userID", SqlDbType.VarChar, 250).Value = userName;
            command.Parameters.Add("@AwardSection_TT", SqlDbType.Structured).Value = Utility.WebCommon.ListToDataTable(objAwardModel.SectionList);
            command.Parameters.Add("@Additional_Answer_TT", SqlDbType.Structured).Value = Utility.WebCommon.ListToDataTable(objAwardModel.additionalSection.AdditionalsList);

            if (objAwardModel.NominationID > 0 && (objAwardModel.StatusID == 0 || objAwardModel.StatusID == 14))
            {
                command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "UPDATE";
                command.Parameters.Add("@financeHeadName", SqlDbType.VarChar, 150).Value = objAwardModel.financeHeadName;
            }
            else if (objAwardModel.NominationID == 0 && objAwardModel.StatusID == 0)
            {
                command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "SAVE";
                command.Parameters.Add("@financeHeadName", SqlDbType.VarChar, 150).Value = objAwardModel.financeHeadName;
            }
            else if (objAwardModel.NominationID > 0 && objAwardModel.StatusID > 0)
            {
                command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "Approval";
            }
            string retStr = SQLHelper.ExecuteScalarString(RAFE.Utility.Constants.sp_Nomination, command);
            if (retStr.Split('|')[0].ToUpper() == "SUCCESS")
            {
                resultMessage.isSuccess = true;
                resultMessage.statusMessage = retStr.Split('|')[1];
                if (objAwardModel.isSubmitted == 1 && objAwardModel.StatusID == 0)
                {
                    Mail_BAL.GetMail(Convert.ToInt32(retStr.Split('|')[1].ToUpper()), Convert.ToInt32(NominationStage.Pending), userName, "");
                }
                else if (objAwardModel.isSubmitted == 1 && objAwardModel.StatusID > 0)
                {
                    Mail_BAL.GetMail(Convert.ToInt32(retStr.Split('|')[1].ToUpper()), objAwardModel.StatusID, userName, "");
                }
                //return true;
                return resultMessage;
            }
            else
            {
                if (retStr.Split('|').Length > 1)
                {
                    resultMessage.isSuccess = false;
                    resultMessage.statusMessage = retStr.Split('|')[1].ToString();
                }
                else
                {
                    resultMessage.isSuccess = false;
                    resultMessage.statusMessage = retStr;
                }
                return resultMessage;
            }
        }
        //internal DataSet DownloadByCategory(string category, string subCategory, string v, string year)
        //{
        //    throw new NotImplementedException();
        //}
        /// <summary>
        /// Get All the details of nomination form on the basis of Nomination id and user id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public AwardModel GetNominationDetail(int id, string userID)
        {
            UserMaster_BAL objUserMaster_BAL = new UserMaster_BAL();
            AwardModel objAwardModel = new AwardModel();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@NominationId", SqlDbType.Int).Value = id;
            command.Parameters.Add("@userID", SqlDbType.VarChar, 150).Value = userID;
            command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "GETNOMINATIONDETAIL";
            using (DataSet ds = SQLHelper.GetDataSet(RAFE.Utility.Constants.sp_Nomination, command))
            {
                AwardMaster_BAL objAwardMaster_BAL = new AwardMaster_BAL();
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    objAwardModel.NominationID = Convert.ToInt32(dt.Rows[0]["NominationID"].ToString());
                    objAwardModel.AwardQuarter = dt.Rows[0]["AwardQuarter"].ToString();
                    objAwardModel.AwardYear = dt.Rows[0]["AwardYear"].ToString();
                    objAwardModel.AwardCategory = Convert.ToInt32(dt.Rows[0]["AwardCategoryID"].ToString());
                    objAwardModel.AwardType = Convert.ToInt32(dt.Rows[0]["AwardTypeID"].ToString());
                    objAwardModel.videoFileName = Convert.ToString(dt.Rows[0]["videoFileName"].ToString());
                    objAwardModel.videoFilePath = Convert.ToString(dt.Rows[0]["videoFilePath"].ToString());
                    objAwardModel.addFileName = Convert.ToString(dt.Rows[0]["addFileName"].ToString());
                    objAwardModel.addFilePath = Convert.ToString(dt.Rows[0]["addFilePath"].ToString());
                    objAwardModel.refName1 = Convert.ToString(dt.Rows[0]["refName1"].ToString());
                    objAwardModel.refName2 = Convert.ToString(dt.Rows[0]["refName2"].ToString());
                    objAwardModel.financeFilePath = Convert.ToString(dt.Rows[0]["financeFilePath"].ToString());
                    objAwardModel.financeFileName = Convert.ToString(dt.Rows[0]["financeFileName"].ToString());
                    //*Arpit
                    objAwardModel.financeHeadName = Convert.ToString(dt.Rows[0]["financeHeadName"].ToString());
                    objAwardModel.Remarks = Convert.ToString(dt.Rows[0]["Remarks"].ToString());

                    objAwardModel.isSubmitted = dt.Rows[0]["StatusID"].ToString() == "5" ? -1 : Convert.ToInt32(dt.Rows[0]["isSubmitted"]);
                    objAwardModel.isApproved = Convert.ToBoolean(dt.Rows[0]["isApproved"] == DBNull.Value ? false : true);
                    objAwardModel.HOD = dt.Rows[0]["HOD"].ToString();
                    objAwardModel.NominatorDetails = objUserMaster_BAL.GetuserDetails(dt.Rows[0]["createdby"].ToString());
                    objAwardModel.HODList = objUserMaster_BAL.GetHOD(dt.Rows[0]["createdby"].ToString());
                    string[] empl = ds.Tables[0].Rows[0]["employeenm"].ToString().Trim().Split(',');
                    string[] otherempl = ds.Tables[0].Rows[0]["otheremployeenm"].ToString().Trim().Split(',');
                    objAwardModel.ImagePath1 = ds.Tables[0].Rows[0]["empphotopath"].ToString().Trim();
                    objAwardModel.ACComments = Convert.ToString(dt.Rows[0]["Comments"].ToString());
                    objAwardModel.Score_Details = Convert.ToString(dt.Rows[0]["Score_Details"].ToString());
                    objAwardModel.ACMember = Convert.ToString(dt.Rows[0]["ACMember"].ToString());
                    objAwardModel.EmpCount = empl.Length;
                    for (int i = 0; i < empl.Length; i++)
                    {
                        if (i == 0)
                        {
                            objAwardModel.EmployeeID1 = empl[i];
                            if (empl[i].ToLower() == "other")
                            {
                                objAwardModel.Other_EmployeeID1 = otherempl[i];
                            }
                        }
                        else if (i == 1)
                        {
                            objAwardModel.EmployeeID2 = empl[i];
                            if (empl[i].ToLower() == "other")
                            {
                                objAwardModel.Other_EmployeeID2 = otherempl[i];
                            }
                        }
                        else if (i == 2)
                        {
                            objAwardModel.EmployeeID3 = empl[i];
                            if (empl[i].ToLower() == "other")
                            {
                                objAwardModel.Other_EmployeeID3 = otherempl[i];
                            }
                        }
                        else if (i == 3)
                        {
                            objAwardModel.EmployeeID4 = empl[i];
                            if (empl[i].ToLower() == "other")
                            {
                                objAwardModel.Other_EmployeeID4 = otherempl[i];
                            }
                        }
                        else if (i == 4)
                        {
                            objAwardModel.EmployeeID5 = empl[i];
                            if (empl[i].ToLower() == "other")
                            {
                                objAwardModel.Other_EmployeeID5 = otherempl[i];
                            }
                        }
                        else if (i == 5)
                        {
                            objAwardModel.EmployeeID6 = empl[i];
                            if (empl[i].ToLower() == "other")
                            {
                                objAwardModel.Other_EmployeeID6 = otherempl[i];
                            }
                        }
                        else if (i == 6)
                        {
                            objAwardModel.EmployeeID7 = empl[i];
                            if (empl[i].ToLower() == "other")
                            {
                                objAwardModel.Other_EmployeeID7 = otherempl[i];
                            }
                        }
                        else if (i == 7)
                        {
                            objAwardModel.EmployeeID8 = empl[i];
                            if (empl[i].ToLower() == "other")
                            {
                                objAwardModel.Other_EmployeeID8 = otherempl[i];
                            }
                        }
                        else if (i == 8)
                        {
                            objAwardModel.EmployeeID9 = empl[i];
                            if (empl[i].ToLower() == "other")
                            {
                                objAwardModel.Other_EmployeeID9 = otherempl[i];
                            }
                        }
                        else if (i == 9)
                        {
                            objAwardModel.EmployeeID10 = empl[i];
                            if (empl[i].ToLower() == "other")
                            {
                                objAwardModel.Other_EmployeeID10 = otherempl[i];
                            }
                        }
                    }
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    objAwardModel.AwardCategoryList = Utility.WebCommon.DataTableToList<AwardCataegory>(ds.Tables[2]);
                }
                if (ds.Tables[3].Rows.Count > 0)
                {
                    objAwardModel.AwardTypeList = Utility.WebCommon.DataTableToList<AwardTypeModel>(ds.Tables[3]);
                }
                if (ds.Tables[4].Rows.Count > 0)
                {
                    objAwardModel.SectionList = Utility.WebCommon.DataTableToList<AwardSectonModel>(ds.Tables[4]);
                }
                if (ds.Tables[6].Rows.Count > 0)
                {
                    objAwardModel.additionalSection.AdditionalPartID = Convert.ToInt32(ds.Tables[6].Rows[0]["AdditionalPartID"]);
                    objAwardModel.additionalSection.TitleText = ds.Tables[6].Rows[0]["AdditionalTitle"].ToString();
                    objAwardModel.additionalSection.DescriptionText = ds.Tables[6].Rows[0]["AdditionalDescription"].ToString();
                    objAwardModel.additionalSection.isTabularForm = Convert.ToBoolean(ds.Tables[6].Rows[0]["isTabularForm"]);
                    objAwardModel.additionalSection.AdditionalsList = Utility.WebCommon.DataTableToList<AwardAdditionalModel>(ds.Tables[6]);
                }
                if (ds.Tables[7].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[7].Rows)
                    {
                        for (int i = 0; i < objAwardModel.additionalSection.AdditionalsList.Count; i++)
                        {
                            if (objAwardModel.additionalSection.AdditionalsList[i].AdditionalQuestionID == Convert.ToInt32(dr["AdditionalQuestionID"]))
                            {
                                objAwardModel.additionalSection.AdditionalsList[i].UserInput = dr["UserInput"].ToString();
                            }
                        }
                    }
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        for (int i = 0; i < objAwardModel.SectionList.Count; i++)
                        {
                            if (objAwardModel.SectionList[i].SectionID == Convert.ToInt32(dr["SectionID"]))
                            {
                                objAwardModel.SectionList[i].SectionInput = dr["Description"].ToString();
                            }
                        }
                    }
                }
                if (ds.Tables[5].Rows.Count > 0)
                {
                    objAwardModel.AllEmployeeList = Utility.WebCommon.DataTableToList<EmployeeModel>(ds.Tables[5]);
                }
                objAwardModel.AllEmployeeList.Add(new EmployeeModel
                {
                    EmployeeID = "other",
                    EmployeeName = "Other"
                });
                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        for (int i = 0; i < objAwardModel.SectionList.Count; i++)
                        {
                            if (objAwardModel.SectionList[i].SectionID == Convert.ToInt32(dr["SectionID"]))
                            {
                                objAwardModel.SectionList[i].SectionInput = dr["Description"].ToString();
                            }
                        }
                    }
                }
                if (ds.Tables.Count > 8 && ds.Tables[8].Rows.Count > 0 && ds.Tables[8].Rows[0][0].ToString() == "1")
                {
                    objAwardModel.isEditable = true;
                }
                if (ds.Tables.Count > 9 && ds.Tables[9].Rows.Count > 0)
                {
                    objAwardModel.UserRole = ds.Tables[9].Rows[0][0].ToString();
                }
                if (ds.Tables[10].Rows.Count > 0)
                {
                    //AuthorList.Add(new Author("Mahesh Chand", 35, "A Prorammer's Guide to ADO.NET", true, new DateTime(2003, 7, 10)));
                    List<RemarkModel> remarkList = new List<RemarkModel>();
                    for(int i = 0; i < ds.Tables[10].Rows.Count; i++)
                    {
                        RemarkModel model = new RemarkModel();
                        model.AppStepID = ds.Tables[10].Rows[i]["AppStepID"].ToString();
                        model.NominationID = ds.Tables[10].Rows[i]["NominationID"].ToString();
                        model.Level = ds.Tables[10].Rows[i]["Level"].ToString();
                        model.StatusID = ds.Tables[10].Rows[i]["StatusID"].ToString();
                        model.Remarks = ds.Tables[10].Rows[i]["Remarks"].ToString();
                        model.UpdatedBy = ds.Tables[10].Rows[i]["UpdatedBy"].ToString();
                        if(ds.Tables[10].Rows[i]["Remarks"].ToString() != "")
                        {
                            remarkList.Add(model);
                        }
                    }
                    objAwardModel.RemarkList = remarkList;
                }
            }
            objAwardModel.validUser = objUserMaster_BAL.ValidUser(userID);
            return objAwardModel;
        }
        /// <summary>
        /// Check already Nominated for the same award for same year and same quarter
        /// </summary>
        /// <param name="awardYear">award year</param>
        /// <param name="awardQuarter">award quarter</param>
        /// <param name="awardTypeID"> award type</param>
        /// <param name="nominee">nominee samaccountname</param>
        /// <returns></returns>
        public bool CheckAlreadyNominated(int awardYear, string awardQuarter, int awardTypeID, string nominee)
        {
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@AwardTypeID", SqlDbType.Int).Value = awardTypeID;
            command.Parameters.Add("@awardYear", SqlDbType.Int).Value = awardYear;
            command.Parameters.Add("@AwardQuarter", SqlDbType.VarChar, 150).Value = awardQuarter;
            command.Parameters.Add("@employeenm", SqlDbType.VarChar, 150).Value = nominee;
            command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "ALREADYNOMINATED";
            return SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_Nomination, command).Rows.Count > 0 ? true : false;
        }
        /// <summary>
        /// Get all the nominee in list for the loggenIn user
        /// </summary>
        /// <param name="userID">LoggedIn user samaccountname</param>
        /// <returns></returns>
        public NominationModel GetAllNomination(string userID)
        {
            NominationModel objNominationModel = new NominationModel();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@userID", SqlDbType.VarChar, 150).Value = userID;
            command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "GETALLNOMINATION";
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_Nomination, command))
            {
                objNominationModel.Nominationlist = Utility.WebCommon.DataTableToList<Nomination>(dt);
            }
            UserMaster_BAL objUserMaster_BAL = new UserMaster_BAL();
            objNominationModel.isAuthorized = objUserMaster_BAL.ValidUser(userID);
            return objNominationModel;
        }
        public NominationModel GetAwardCommitteeNomination(string userID)
        {
            NominationModel objNominationModel = new NominationModel();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@userID", SqlDbType.VarChar, 150).Value = userID;
            command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "GETAWARDCOMMITTEENOMINATION";
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_Nomination, command))
            {
                objNominationModel.Nominationlist = Utility.WebCommon.DataTableToList<Nomination>(dt);
            }
            UserMaster_BAL objUserMaster_BAL = new UserMaster_BAL();
            objNominationModel.isAuthorized = objUserMaster_BAL.ValidUser(userID);
            return objNominationModel;
        }

        public NominationModel GetBUCommitteeNomination(string userID)
        {
            NominationModel objNominationModel = new NominationModel();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@userID", SqlDbType.VarChar, 150).Value = userID;
            command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "GETBUCOMMITTEENOMINATION";

            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_Nomination, command))
            {
                objNominationModel.Nominationlist = Utility.WebCommon.DataTableToList<Nomination>(dt);
            }

            UserMaster_BAL objUserMaster_BAL = new UserMaster_BAL();
            objNominationModel.isAuthorized = objUserMaster_BAL.ValidUser(userID);
            return objNominationModel;
        }
        public NominationModel GetJuryCommitteeNomination(string userID)
        {
            NominationModel objNominationModel = new NominationModel();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@userID", SqlDbType.VarChar, 150).Value = userID;
            command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "GETJURYCOMMITTEENOMINATION";
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_Nomination, command))
            {
                objNominationModel.Nominationlist = Utility.WebCommon.DataTableToList<Nomination>(dt);
            }
            UserMaster_BAL objUserMaster_BAL = new UserMaster_BAL();
            objNominationModel.isAuthorized = objUserMaster_BAL.ValidUser(userID);
            return objNominationModel;
        }
        public NominationModel GetAllNomination(string userID, string Year)
        {
            NominationModel objNominationModel = new NominationModel();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@userID", SqlDbType.VarChar, 150).Value = userID;
            command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "GETALLNOMINATION";
            command.Parameters.Add("@AwardYear", SqlDbType.Int).Value = Year;
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_Nomination, command))
            {
                objNominationModel.Nominationlist = Utility.WebCommon.DataTableToList<Nomination>(dt);
            }
            UserMaster_BAL objUserMaster_BAL = new UserMaster_BAL();
            objNominationModel.isAuthorized = objUserMaster_BAL.ValidUser(userID);
            return objNominationModel;
        }
        public NominationModel GetAllCategory(string userID, string subCategory, int Year, string Category)
        {
            NominationModel objNominationModel = new NominationModel();
            SqlCommand command = new SqlCommand();
            StringBuilder str = new StringBuilder();
            if (userID.ToString().ToLower().Trim() == "mayaj")
            {
                str.Append(" AND ( AN.createdby = '" + userID.ToString().ToLower().Trim() + "' OR Nom.BU_HR = 'Sonasaje' ) ");
            }
            else if (userID.ToString().ToLower().Trim() == "AnnPradhan")
            {
                str.Append(" AND ( AN.createdby = '" + userID.ToString().ToLower().Trim() + "' OR Nom.BU_HR = 'SadhanaGopal' )");
            }
            else if (userID.ToString().ToLower().Trim() == "VijayRaj1")
            {
                str.Append(" AND ( AN.createdby = '" + userID.ToString().ToLower().Trim() + "' OR Nom.BU_HR = 'BhushanKulkarni' )");
            }
            else if (userID.ToString().ToLower().Trim() != "ashwiniacharya" && userID.ToString().ToLower().Trim() != "surbhisolanki" && userID.ToString().ToLower().Trim() != "srikanthpb" && userID.ToString().ToLower().Trim() != "suchitajain")
            {
                str.Append(" AND ( AN.createdby = '" + userID.ToString().ToLower().Trim() + "' OR Nom.BU_HR = '" + userID.ToString().ToLower().Trim() + "' OR  AN.HOD = '" + userID.ToString().ToLower().Trim() + "'  )");
            }
            if (Category == "1")
            {

            }
            else if (Category == "2")
            {
                str.Append(" AND AC_M.awardcategoryid = '" + subCategory.ToString() + "'");
            }
            else if (Category == "3")
            {
                str.Append(" AND Nom.Sub_Group='" + subCategory.ToString() + "'  ");
            }
            else if (Category == "4")
            {
                str.Append(" AND AN.[AwardTypeId] = '" + subCategory.ToString() + "'");
            }
            if (Year.ToString() != "")
            {
                str.Append(" AND AN.AwardYear='" + Year.ToString() + "'  ");
            }
            command.Parameters.Add("@Where", SqlDbType.VarChar).Value = str.ToString();
            //command.Parameters.Add("@category", SqlDbType.Int).Value = category;
            //command.Parameters.Add("@subCategory", SqlDbType.VarChar, 150).Value = subCategory;
            //command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "GETALLNOMINATION";
            //command.Parameters.Add("@userID", SqlDbType.VarChar, 150).Value = userID;
            //command.Parameters.Add("@AwardYear", SqlDbType.Int).Value = Year;
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_GridChange, command))
            {
                objNominationModel.Nominationlist = Utility.WebCommon.DataTableToList<Nomination>(dt);
            }
            UserMaster_BAL objUserMaster_BAL = new UserMaster_BAL();
            objNominationModel.isAuthorized = objUserMaster_BAL.ValidUser(userID);
            return objNominationModel;
        }
        public NominationModel GetAwardCommitteeCategory(string userID, string subCategory, int Year, string Category)
        {
            NominationModel objNominationModel = new NominationModel();
            SqlCommand command = new SqlCommand();
            StringBuilder str = new StringBuilder();
            if (userID.ToString().ToLower().Trim() == "mayaj")
            {
                str.Append(" AND ( AN.createdby = '" + userID.ToString().ToLower().Trim() + "' OR Nom.BU_HR = 'Sonasaje' ) ");
            }
            else if (userID.ToString().ToLower().Trim() == "AnnPradhan")
            {
                str.Append(" AND ( AN.createdby = '" + userID.ToString().ToLower().Trim() + "' OR Nom.BU_HR = 'SadhanaGopal' )");
            }
            else if (userID.ToString().ToLower().Trim() == "VijayRaj1")
            {
                str.Append(" AND ( AN.createdby = '" + userID.ToString().ToLower().Trim() + "' OR Nom.BU_HR = 'BhushanKulkarni' )");
            }
            else if (userID.ToString().ToLower().Trim() != "ashwiniacharya" && userID.ToString().ToLower().Trim() != "surbhisolanki" && userID.ToString().ToLower().Trim() != "srikanthpb" && userID.ToString().ToLower().Trim() != "suchitajain")
            {
                str.Append(" AND ( AN.createdby = '" + userID.ToString().ToLower().Trim() + "' OR Nom.BU_HR = '" + userID.ToString().ToLower().Trim() + "' OR  AN.HOD = '" + userID.ToString().ToLower().Trim() + "'  )");
            }
            if (Category == "1")
            {

            }
            else if (Category == "2")
            {
                str.Append(" AND AC_M.awardcategoryid = '" + subCategory.ToString() + "'");
            }
            else if (Category == "3")
            {
                str.Append(" AND Nom.Sub_Group='" + subCategory.ToString() + "'  ");
            }
            else if (Category == "4")
            {
                str.Append(" AND AN.[AwardTypeId] = '" + subCategory.ToString() + "'");
            }
            if (Year.ToString() != "")
            {
                str.Append(" AND AN.AwardYear='" + Year.ToString() + "'  ");
            }
            command.Parameters.Add("@Where", SqlDbType.VarChar).Value = str.ToString();
            //command.Parameters.Add("@category", SqlDbType.Int).Value = category;
            //command.Parameters.Add("@subCategory", SqlDbType.VarChar, 150).Value = subCategory;
            //command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "GETALLNOMINATION";
            //command.Parameters.Add("@userID", SqlDbType.VarChar, 150).Value = userID;
            //command.Parameters.Add("@AwardYear", SqlDbType.Int).Value = Year;
            command.Parameters.Add("@action", SqlDbType.VarChar).Value = "AWARDCOMMITTEE";
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_GridChange, command))
            {
                objNominationModel.Nominationlist = Utility.WebCommon.DataTableToList<Nomination>(dt);
            }
            UserMaster_BAL objUserMaster_BAL = new UserMaster_BAL();
            objNominationModel.isAuthorized = objUserMaster_BAL.ValidUser(userID);
            return objNominationModel;
        }
        public DataSet DownloadByYear(string userID, string Year)
        {
            NominationModel objNominationModel = new NominationModel();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@UserID", SqlDbType.VarChar, 150).Value = userID;
            command.Parameters.Add("@CMD", SqlDbType.VarChar, 150).Value = "GETALLNOMINATION";
            command.Parameters.Add("@year", SqlDbType.Int).Value = Year;
            DataSet dt = SQLHelper.GetDataSet(RAFE.Utility.Constants.sp_GetNominationInExcel_year, command);
            return dt;
        }

        public DataSet DownloadByCategory(int category, string subCategory, string userId, string year)
        {
            NominationModel objNominationModel = new NominationModel();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@category", SqlDbType.Int).Value = category;
            command.Parameters.Add("@subCategory", SqlDbType.VarChar, 150).Value = subCategory;
            command.Parameters.Add("@userID", SqlDbType.VarChar, 150).Value = userId;
            command.Parameters.Add("@year", SqlDbType.Int).Value = year;
            DataSet dt = SQLHelper.GetDataSet(RAFE.Utility.Constants.sp_CategoryDownload, command);
            return dt;
        }
        public DataSet DownloadForJury(int category, string subCategory, string userId, string year)
        {
            NominationModel objNominationModel = new NominationModel();
            SqlCommand command = new SqlCommand();
            //command.Parameters.Add("@category", SqlDbType.Int).Value = category;
            //command.Parameters.Add("@subCategory", SqlDbType.VarChar, 150).Value = subCategory;
            //command.Parameters.Add("@userID", SqlDbType.VarChar, 150).Value = userId;
            //command.Parameters.Add("@year", SqlDbType.Int).Value = year;
            command.Parameters.Add("@userID", SqlDbType.VarChar, 150).Value = userId;
            command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "GETJURYCOMMITTEENOMINATION";
            DataSet dt = SQLHelper.GetDataSet(RAFE.Utility.Constants.sp_Nomination, command);
            return dt;
        }
        /// <summary>
        /// Get only winning araed history of nominee
        /// </summary>
        /// <param name="nominee">nominee SamAccountName</param>
        /// <returns></returns>
        public NominationModel GetAllAwardHistory(string nominee)
        {
            NominationModel objNominationModel = new NominationModel();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@employeenm", SqlDbType.VarChar, 150).Value = nominee;
            command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "GETALLPREVIOUSHISTORY";
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_Nomination, command))
            {
                objNominationModel.employeenm = nominee;
                objNominationModel.Nominationlist = Utility.WebCommon.DataTableToList<Nomination>(dt);
            }
            return objNominationModel;
        }

        /// <summary>
        /// To save uploaded file
        /// </summary>
        /// <param name="obj">File</param>
        /// <param name="Name">file name to save</param>
        /// <param name="previousImage">previous file name</param>
        /// <returns></returns>
        private string SaveUpload(HttpPostedFileBase obj, string Name, string previousImage, string fileType)
        {
            string filePath = "";
            string filename = previousImage;
            if (obj != null && obj.ContentLength > 0)
            {
                string path = Utility.WebCommon.GetApplicationPhysicalPath() + fileType + "\\";
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                Guid g = Guid.NewGuid();
                string[] str = obj.FileName.Split('\\');
                filename = g.ToString() + System.IO.Path.GetExtension(str[str.Length - 1]);
                if (fileType.ToLower() == "video")
                    videoFilename = filename;
                else if (fileType.ToLower() == "file")
                    addFilename = filename;
                else if (fileType.ToLower() == "financefile")
                    financeFilename = filename;
                filePath = path + filename;
                if (File.Exists(filePath))
                    File.Delete(filePath);
                    obj.SaveAs(filePath);
            }
            else
            if (fileType.ToLower() == "video")
                videoFilename = Name;
            else if (fileType.ToLower() == "file")
                addFilename = Name;
            else if (fileType.ToLower() == "financefile")
                financeFilename = filename;
            return filename;
        }

        /// <summary>
        /// Get all the nominee in list for the loggenIn user
        /// </summary>
        /// <param name="userID">LoggedIn user samaccountname</param>
        /// <returns></returns>
        public NominationModel GetAllForCXO(string userID, string AwardTypeId = null, string functionName = null)
        {
            NominationModel objNominationModel = new NominationModel();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@functionName", SqlDbType.VarChar, 250).Value = functionName;
            command.Parameters.Add("@AwardTypeId", SqlDbType.VarChar, 250).Value = AwardTypeId;
            command.Parameters.Add("@userID", SqlDbType.VarChar, 150).Value = userID;
            command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "GET_ALL_FOR_CXO";
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_Nomination, command))
            {
                objNominationModel.Nominationlist = Utility.WebCommon.DataTableToList<Nomination>(dt);
            }
            return objNominationModel;
        }
        public NominationFormApprovalValue GetNumberOfNomination(string awardtypeid, string nominationId, string userid)
        {
            NominationFormApprovalValue nominationFormApprovalValue = new NominationFormApprovalValue();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@awardtypeid", SqlDbType.VarChar, 250).Value = awardtypeid;
            //command.Parameters.Add("@awardCategory", SqlDbType.VarChar, 250).Value = awardCategory;
            command.Parameters.Add("@nominationId", SqlDbType.VarChar, 150).Value = nominationId;
            command.Parameters.Add("@userid", SqlDbType.VarChar, 150).Value = userid;
            //DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_NumberOfNomination, command);
            DataSet ds = SQLHelper.GetDataSet(RAFE.Utility.Constants.sp_NumberOfNomination, command);
            if (ds.Tables.Count > 0)
            {

                nominationFormApprovalValue.nominationcount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                nominationFormApprovalValue.financeapproval = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                return nominationFormApprovalValue;
            }
            else
            {
                return nominationFormApprovalValue;
            }
        }


        /// <summary>
        /// To Get Image path store in database.
        /// </summary>
        /// <param name="empId"> employee id  </param>
        /// <param name="empimg"> image path stored in database separated by (,)Quamma </param>
        /// <returns> return the employee image full path save at form submit.</returns>
        private string GetImagePath(string empId, string empimg)
        {
            string[] imgArray = empimg.Split(',');
            foreach (string img in imgArray)
            {
                if (img.Contains(empId))
                    return img;
            }
            return "";
        }

        public DataSet DataForExport(string dataType, string userID, string Year)
        {
            NominationModel objNominationModel = new NominationModel();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@CMD", SqlDbType.VarChar, 250).Value = dataType;
            //command.Parameters.Add("@CMD", SqlDbType.VarChar, 250).Value = "GETALLNOMINATION";
            command.Parameters.Add("@userID", SqlDbType.VarChar, 150).Value = userID;
            command.Parameters.Add("@year", SqlDbType.Int).Value = Year;
            return SQLHelper.GetDataSet(RAFE.Utility.Constants.sp_GetNominationInExcel, command);
        }

        public NominationModel NominationList(string userID, string CMD)
        {
            NominationModel objNominationModel = new NominationModel();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@userID", SqlDbType.VarChar, 150).Value = userID;
            command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = CMD;
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_Nomination_List, command))
            {
                objNominationModel.Nominationlist = Utility.WebCommon.DataTableToList<Nomination>(dt);
            }

            UserMaster_BAL objUserMaster_BAL = new UserMaster_BAL();
            objNominationModel.isAuthorized = objUserMaster_BAL.ValidUser(userID);
            return objNominationModel;
        }
        public bool Revert(int id, string userID)
        {
            NominationModel objNominationModel = new NominationModel();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@NominationID", SqlDbType.VarChar, 250).Value = id;
            command.Parameters.Add("@action", SqlDbType.VarChar, 250).Value = "Revert";
            command.Parameters.Add("@userID", SqlDbType.VarChar, 150).Value = userID;
            return Convert.ToBoolean(Convert.ToInt32(SQLHelper.ExecuteScalarString(RAFE.Utility.Constants.sp_Nomination, command)));
        }
        public dynamic GetAwardyear()
        {
            AwardPortion objAwardPortion = new AwardPortion();

            SqlCommand command = new SqlCommand();
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_AwardYear, command))
            {
                var distinctYear = dt.AsEnumerable()
                       .Select(row => new
                       {
                           AwardYear = row.Field<int>("AwardYear")
                       }).Distinct().ToList();

                foreach (var i in distinctYear)
                {
                    objAwardPortion.years.Add(i.AwardYear);
                }
            }
            return objAwardPortion.years;
        }
        public dynamic GetAwardyearCurrent()
        {
            AwardPortion objAwardPortion = new AwardPortion();

            SqlCommand command = new SqlCommand();
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_AwardYearCurrent, command))
            {
                var distinctYear = dt.AsEnumerable()
                       .Select(row => new
                       {
                           AwardYear = row.Field<int>("AwardYear")
                       }).Distinct().ToList();

                foreach (var i in distinctYear)
                {
                    objAwardPortion.years.Add(i.AwardYear);
                }
            }
            return objAwardPortion.years;
        }
        public List<Dropdown> DownloadCategory()
        {
            List<Dropdown> obj_dropdown = new List<Dropdown>();
            SqlCommand command = new SqlCommand();
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_DownloadCategory, command))
            {
                foreach (DataRow rw in dt.Rows)
                {
                    obj_dropdown.Add(new Dropdown
                    {
                        value = rw["Categories"].ToString(),
                        text = rw["Category_Id"].ToString(),
                    });
                }
            }
            return obj_dropdown;
        }
        public List<Dropdown> DownloadCategoryBUHR()
        {
            List<Dropdown> obj_dropdown = new List<Dropdown>();
            SqlCommand command = new SqlCommand();
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_DownloadCategoryBUHR, command))
            {
                foreach (DataRow rw in dt.Rows)
                {
                    obj_dropdown.Add(new Dropdown
                    {
                        value = rw["Categories"].ToString(),
                        text = rw["Category_Id"].ToString(),
                    });
                }
            }
            return obj_dropdown;
        }
        public List<Dropdown> DownloadCategoryAC()
        {
            List<Dropdown> obj_dropdown = new List<Dropdown>();
            SqlCommand command = new SqlCommand();
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_DownloadCategoryAC, command))
            {
                foreach (DataRow rw in dt.Rows)
                {
                    obj_dropdown.Add(new Dropdown
                    {
                        value = rw["Categories"].ToString(),
                        text = rw["Category_Id"].ToString(),
                    });
                }
            }
            return obj_dropdown;
        }
        public List<Dropdown> DownloadCategoryJURY()
        {
            List<Dropdown> obj_dropdown = new List<Dropdown>();
            SqlCommand command = new SqlCommand();
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_DownloadCategoryJury, command))
            {
                foreach (DataRow rw in dt.Rows)
                {
                    obj_dropdown.Add(new Dropdown
                    {
                        value = rw["Categories"].ToString(),
                        text = rw["Category_Id"].ToString(),
                    });
                }
            }
            return obj_dropdown;
        }
        public List<SecondDropdown> Filters(string filter,string userId)
        {
            List<SecondDropdown> obj_seconddropdown = new List<SecondDropdown>();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@para1", SqlDbType.VarChar).Value = filter;
            command.Parameters.Add("@username", SqlDbType.VarChar).Value = userId;
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_SecondDropdown, command))
            {
                if (dt != null)
                {
                    foreach (DataRow rw in dt.Rows)
                    {
                        obj_seconddropdown.Add(new SecondDropdown
                        {
                            value = rw["value"].ToString(),
                            text = rw["text"].ToString(),
                        });
                    }
                }
            }
            return obj_seconddropdown;
        }
        public List<SecondDropdown> FiltersBUHR(string filter)
        {
            List<SecondDropdown> obj_seconddropdown = new List<SecondDropdown>();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@para1", SqlDbType.VarChar).Value = filter;
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_SecondDropdownBUHR, command))
            {
                if (dt != null)
                {
                    foreach (DataRow rw in dt.Rows)
                    {
                        obj_seconddropdown.Add(new SecondDropdown
                        {
                            value = rw["value"].ToString(),
                            text = rw["text"].ToString(),
                        });
                    }
                }
            }
            return obj_seconddropdown;
        }
        public List<FHList> GetFHList()
        {
            List<FHList> obj_list = new List<FHList>();
            SqlCommand command = new SqlCommand();
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_financeHeadList, command))
            {
                foreach (DataRow rw in dt.Rows)
                {
                    obj_list.Add(new FHList
                    {
                        value = rw["UserName"].ToString(),
                        text = rw["FullName"].ToString(),
                    });
                }
            }
            return obj_list;
        }
        public Dashboard GetNominationDetails(string userId)
        {
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            Dashboard model = new Dashboard();
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Add("@action", SqlDbType.VarChar, 50).Value = "OVERALL";
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_Dashboard, cmd))
            {
                foreach (DataRow rw in dt.Rows)
                {
                    model.lsPieChart.Add(new PRPieChart
                    {
                        name = rw["Status"].ToString(),
                        y = Convert.ToInt32(rw["Total"].ToString()),
                        color = rw["ChartColor"].ToString()
                    });
                }
            }
            using (DataTable dt1 = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_AwardDashboard, cmd))
            {
                foreach (DataRow rw in dt1.Rows)
                {
                    model.PieChart2nd.Add(new PRPieChart
                    {
                        name = rw["AwardType"].ToString(),
                        y = Convert.ToInt32(rw["Total"].ToString()),
                        color = rw["ChartColor"].ToString()
                    });
                }
            }
            Dashboard_Nomination(ref model, userId);
            return model;
        }
        public Dashboard GetBUDashboard(string userId)
        {
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            Dashboard model = new Dashboard();
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Add("@action", SqlDbType.VarChar, 50).Value = "BU_SPECIFIC";
            cmd.Parameters.Add("@BUHR", SqlDbType.VarChar, 50).Value = userId;
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_Dashboard, cmd))
            {
                foreach (DataRow rw in dt.Rows)
                {
                    model.lsPieChart.Add(new PRPieChart
                    {
                        name = rw["Status"].ToString(),
                        y = Convert.ToInt32(rw["Total"].ToString()),
                        color = rw["ChartColor"].ToString()
                    });
                }
            }
            Dashboard_Nomination(ref model, userId);
            return model;
        }
        public DataTable ExcelExport()
        {
            SqlCommand command = new SqlCommand();
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            return SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_Dashboard, command);
        }
        public DataTable ExcelExport2()
        {
            SqlCommand command = new SqlCommand();
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            return SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_AwardDashboard, command);
        }
        public DataTable ExcelExport3()
        {
            SqlCommand command = new SqlCommand();
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            return SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_GetPreviousYearWinners, command);
        }
        public ResultMessage Approval(int StatusID, int NominationID, string UserID, string Remarks)
        {
            AwardModel objAwardModel = new AwardModel();
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            ResultMessage resultMessage = new ResultMessage();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@StatusID", SqlDbType.Int).Value = StatusID;
            command.Parameters.Add("@NominationID", SqlDbType.Int).Value = NominationID;
            command.Parameters.Add("@userid", SqlDbType.VarChar,100).Value = UserID;
            command.Parameters.Add("@Remarks", SqlDbType.VarChar, 2000).Value = Remarks;
            command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "Approval";
            string retStr = SQLHelper.ExecuteScalarString(RAFE.Utility.Constants.sp_Nomination, command);
            if (retStr.Split('|')[0].ToUpper() == "SUCCESS")
            {
                resultMessage.isSuccess = true;
                resultMessage.statusMessage = retStr.Split('|')[1];
                if (objAwardModel.isSubmitted == 1 && objAwardModel.StatusID == 0)
                {
                    Mail_BAL.GetMail(Convert.ToInt32(retStr.Split('|')[1].ToUpper()), Convert.ToInt32(NominationStage.Pending), UserID, "");
                }
                return resultMessage;
            }
            else
            {
                resultMessage.isSuccess = false;
                resultMessage.statusMessage = "Action failed. Try after some time.";
                return resultMessage;
            }
        }
        public int JCSaving(int NominationID, string Comments, string UserID,string name)
        {
            AwardModel objAwardModel = new AwardModel();
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@NominationID", SqlDbType.Int).Value = NominationID;
            command.Parameters.Add("@Comments", SqlDbType.VarChar,20000).Value = Comments;
            command.Parameters.Add("@CreatedBy", SqlDbType.VarChar,200).Value = name;
            int retStr = SQLHelper.ExecuteSP(RAFE.Utility.Constants.sp_JuryComments_Insert, command);
            return retStr;
        }
        //Saurabh Kumar Date- 19/05/2023
        public int FDBAsking(int NominationID, string UserID, string name, string ReceverMail, string SenderEmailId, string AskFeedback, string awardTyp,string nominee)
        {
            AwardModel objAwardModel = new AwardModel();
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@EmailId", SqlDbType.VarChar).Value = ReceverMail;
            DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.SP_GetRecieverDetails, command);
            command.Parameters.Clear();
            command.Parameters.Add("@Action", SqlDbType.VarChar).Value = "INSERT";
            command.Parameters.Add("@NominationID", SqlDbType.Int).Value = NominationID;
            command.Parameters.Add("@AskFeedback", SqlDbType.VarChar, 20000).Value = AskFeedback;
            command.Parameters.Add("@SenderName", SqlDbType.VarChar, 200).Value = name;
            command.Parameters.Add("@ReceiverEmailId", SqlDbType.VarChar).Value = ReceverMail;
            command.Parameters.Add("@SenderEmailId", SqlDbType.VarChar).Value = SenderEmailId;
            command.Parameters.Add("@ReceiverName", SqlDbType.VarChar).Value = dt.Rows[0]["FullName"].ToString();
            command.Parameters.Add("@GiveFeedback", SqlDbType.VarChar).Value = " ";
            command.Parameters.Add("@SendingDate", SqlDbType.DateTime).Value = DateTime.Now;
            command.Parameters.Add("@ReceivingDate", SqlDbType.DateTime).Value = DateTime.Now;
            command.Parameters.Add("@Updated", SqlDbType.DateTime).Value = DateTime.Now;
            command.Parameters.Add("@IsActive", SqlDbType.Int).Value = 1;
            command.Parameters.Add("@IsDeleted", SqlDbType.Int).Value = 1;
            int retStr = SQLHelper.ExecuteSP(RAFE.Utility.Constants.sp_JuryAskFeedback, command);
            //Code for Send a mail for Give Feedback
            string mailBody = "Dear"+ dt.Rows[0]["FullName"].ToString() + ",<br>";
            mailBody = mailBody + "<br>";
            mailBody = mailBody + "Thank you for sharing the nomination. Listed below is" + "/are the name(s) of nominee(s) under the category of" + "<b>"+ awardTyp + "</b>" + ".<br>";
            mailBody = mailBody + "<ul>";
            mailBody = mailBody + nominee;
            mailBody = mailBody + "</ul>";
            mailBody = mailBody + "It has now been shared to the next level for review." + "<br>";
            mailBody = mailBody + "<br>";
            mailBody = mailBody + "<a href=" + "https://raymondcentral.com/rafe/>" + "Click Here" + "</a>" + "for" + "details." + "<br>";
            mailBody = mailBody + "<br>";
            mailBody = mailBody + "Thanks & regards" + "<br>";
            mailBody = mailBody + "Team RAFE 2023";
            Utility.EmailLibrary.sendEmail(ReceverMail, "", "Give Feedback", mailBody);
            return retStr;
        }
        public int ACSaving(int nominationId, string Comments, string UserID,string name)
        {
            AwardModel objAwardModel = new AwardModel();
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@NominationID", SqlDbType.Int).Value = nominationId;
            command.Parameters.Add("@Comments", SqlDbType.VarChar, 20000).Value = Comments;
            command.Parameters.Add("@CreatedBy", SqlDbType.VarChar, 200).Value = name;
            int retStr = SQLHelper.ExecuteSP(RAFE.Utility.Constants.sp_AwardCommitteeComments_Insert, command);
            return retStr;
        }
        public List<FHList> ACMembers(int NominationId)
        {
            List<FHList> obj_dropdown = new List<FHList>();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@NominationId", SqlDbType.Int).Value = NominationId;
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_GetAwardCommitteeMembers, command))
            {
                foreach (DataRow rw in dt.Rows)
                {
                    obj_dropdown.Add(new FHList
                    {
                        value = rw["Id"].ToString(),
                        text = rw["AwardCommitteeMember"].ToString(),
                    });
                }
            }
            return obj_dropdown;
        }
        public string GetACComments(int nominationId,string createdby)
        {
            AwardModel objAwardModel = new AwardModel();
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@NominationID", SqlDbType.Int).Value = nominationId;
            command.Parameters.Add("@CreatedBy", SqlDbType.VarChar, 20000).Value = createdby;
            string retStr = SQLHelper.ExecuteScalarString(RAFE.Utility.Constants.sp_GetAwardCommitteeComments, command);
            return retStr;
        }
        public List<RatingHeads> ACRatings(int NominationId)
        {
            List<RatingHeads> obj_dropdown = new List<RatingHeads>();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@NominationId", SqlDbType.Int).Value = NominationId;
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_GetAwardCommitteeRatings, command))
            {
                foreach (DataRow rw in dt.Rows)
                {
                    obj_dropdown.Add(new RatingHeads
                    {
                        Id = Convert.ToInt32(rw["Id"]),
                        ratingname = rw["RatingHeads"].ToString(),
                        weightage = Convert.ToInt32(rw["Percentage"]),
                        nominationid = Convert.ToInt32(rw["NominationID"]),
                        score = Convert.ToDecimal(rw["score"]),
                    });
                }
            }
            return obj_dropdown;
        }
        public int ACRatingsSaving(int nominationId,string RatingHead, int Weightage, decimal Score,decimal CalcScore)
        {
            AwardModel objAwardModel = new AwardModel();
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@NominationID", SqlDbType.Int).Value = nominationId;
            command.Parameters.Add("@RatingHead", SqlDbType.VarChar, 500).Value = RatingHead.ToString().Replace("\n","").Trim();
            command.Parameters.Add("@Weightage", SqlDbType.Int).Value = Weightage;
            command.Parameters.Add("@Score", SqlDbType.Decimal).Value = Score;
            command.Parameters.Add("@FinalScore", SqlDbType.Decimal).Value = CalcScore;
            int retStr = SQLHelper.ExecuteSP(RAFE.Utility.Constants.sp_AwardCommitteeScores_Insert, command);
            return retStr;
        }
        public int AwardNameUpdate(int awardtypeid,string awardname,string awarddesc)
        {
            AwardAdminModel awardAdminModel = new AwardAdminModel();
            Nomination_BAL nomination_BAL = new Nomination_BAL();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@awardtypeid", SqlDbType.Int).Value = awardtypeid;
            command.Parameters.Add("@awardname", SqlDbType.VarChar, 100).Value = awardname;
            command.Parameters.Add("@awarddesc", SqlDbType.VarChar, 100).Value = awarddesc;
            int retStr = SQLHelper.ExecuteSP(RAFE.Utility.Constants.sp_Update_AwardName, command);
            return retStr;
        }

        public Dashboard Dashboard_Nomination(ref Dashboard dashboard,string userId)
        {
            dashboard.List = new List<Status>();
            dashboard.business = new List<string>();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@BU_HR", SqlDbType.VarChar, 100).Value = userId;
            command.Parameters.Add("@id", SqlDbType.Int).Value = 8;
            DataTable ds = SQLHelper.GetDataTable("sp_DashboardBusiness", command);
            if (ds.Rows.Count > 0)
            {
                List<string> business = new List<string>();
                List<Status> List = new List<Status>();
                Status obj = new Status();
                obj.name = "Shortlisted Nomination";
                obj.data = new List<int>();
                for(int i = 0; i < ds.Rows.Count; i++)
                {
                    obj.data.Add(Convert.ToInt32(ds.Rows[i]["shortlistednomination"].ToString()));
                }
                List.Add(obj);
                obj = new Status();
                obj.name = "Nomination";
                obj.data = new List<int>();
                for(int i = 0; i < ds.Rows.Count; i++)
                {
                    obj.data.Add(Convert.ToInt32(ds.Rows[i]["totalnomination"].ToString()));
                }
                List.Add(obj);
                for(int i = 0; i < ds.Rows.Count; i++)
                {
                    business.Add(ds.Rows[i]["business"].ToString());
                }
                dashboard.List = List;
                dashboard.business = business;
                
            }
            return dashboard;
        }
    }
}