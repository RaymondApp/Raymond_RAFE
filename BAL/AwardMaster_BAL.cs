using RAFE.DAL;
using RAFE.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace RAFE.BAL
{
    public class AwardMaster_BAL
    {
        /// <summary>
        /// Get All Award Masters 
        /// </summary>
        /// <param name="isActive">if want all active then 1 else return all inculding active and inactive</param>
        /// <returns>List of AwardMaster</returns>
        public AwardMaster GetAllAwardMaster(int isActive)
        {
            AwardMaster objAwardMaster = new AwardMaster();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@isActive", SqlDbType.Int).Value = isActive;
            command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "GETALLAWARDS";
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_awardMaster, command))
            {
                if (dt.Rows.Count > 0)
                {
                    objAwardMaster.AwardMasterList = Utility.WebCommon.DataTableToList<AwardMaster>(dt);
                }
            }
            return objAwardMaster;

        }

        /// <summary>
        /// Return award master details of selected awrad by AwardId
        /// </summary>
        /// <param name="awardId">Award id of which we wand details</param>
        /// <returns>award master object</returns>
        public AwardMaster GetAwardByAwardId(int awardId)
        {
            AwardMaster objAwardMaster = new AwardMaster();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@AwardId", SqlDbType.Int).Value = awardId;
            command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "AWARD";
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_awardMaster, command))
            {
                if (dt.Rows.Count > 0)
                {
                    objAwardMaster.AwardID = Convert.ToInt32(dt.Rows[0]["AwardID"]);
                    objAwardMaster.AwardYear = Convert.ToInt32(dt.Rows[0]["AwardYear"]);
                    objAwardMaster.AwardQuarter = Convert.ToString(dt.Rows[0]["AwardQuarter"]);
                    objAwardMaster.NominationLastDate = Convert.ToString(dt.Rows[0]["NominationLastDate"]);
                    objAwardMaster.ApprovalLastDate = Convert.ToString(dt.Rows[0]["ApprovalLastDate"]);
                    objAwardMaster.isActive = Convert.ToInt32(dt.Rows[0]["isActive"]);

                }
            }
            return objAwardMaster;

        }

        /// <summary>
        /// Save Award master detail is awradId exists in db then it update the exiting record else insert the new recod.
        /// </summary>
        /// <param name="objAwardMaster">object of award master having requred details</param>
        /// <returns>return success with awardId i.e "SUCCESS|(awardid)" if executed successfully else returen error message</returns>
        public string SaveAward(AwardMaster objAwardMaster)
        {
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@AwardID", SqlDbType.Int).Value = objAwardMaster.AwardID;
            command.Parameters.Add("@AwardYear", SqlDbType.Int).Value = objAwardMaster.AwardYear;
            command.Parameters.Add("@NominationLastDate", SqlDbType.DateTime).Value = DateTime.ParseExact(objAwardMaster.NominationLastDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            command.Parameters.Add("@ApprovalLastDate", SqlDbType.DateTime).Value = DateTime.ParseExact(objAwardMaster.ApprovalLastDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            command.Parameters.Add("@isActive", SqlDbType.Int).Value = objAwardMaster.isActive;
            command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "SAVE";
            return SQLHelper.ExecuteScalarString(RAFE.Utility.Constants.sp_awardMaster, command);
        }

        /// <summary>
        /// Return all the award sections with correspondent to passed award type id
        /// </summary>
        /// <param name="AwardType">Award Type ID of which section we want</param>
        /// <returns>List of ward sections</returns>
        public AwardTypeModel GetAwardSection(int AwardType)
        {
            AwardTypeModel objAwardMasterModel = new AwardTypeModel();
            DataTable dt;
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@awardtypeid", SqlDbType.Int).Value = AwardType;
            command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "GETAWARDSECTION";
            using (dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_awardMaster, command))
            {
                if (dt.Rows.Count > 0)
                {
                    objAwardMasterModel.AwardTypeID = Convert.ToInt32(dt.Rows[0]["awardtypeid"]);
                    objAwardMasterModel.AwardType = Convert.ToString(dt.Rows[0]["awardtype"]);
                    objAwardMasterModel.SectionCount = dt.Rows.Count;
                    objAwardMasterModel.SectionsList = Utility.WebCommon.DataTableToList<AwardSectonModel>(dt);
                }
            }
            return objAwardMasterModel;
        }

        /// <summary>
        /// Return all the award sections with correspondent to passed award type id
        /// </summary>
        /// <param name="AwardType">Award Type ID of which section we want</param>
        /// <returns>List of ward sections</returns>
        public AwardAdditionalSection GetAwardAdditionalSection(int AwardType)
        {
            AwardAdditionalSection awardAdditionalSection = new AwardAdditionalSection();
            DataTable dt;
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@awardtypeid", SqlDbType.Int).Value = AwardType;
            command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "GETAWARD_Addtional_SECTION";
            using (dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_awardMaster, command))
            {
                if (dt.Rows.Count > 0)
                {
                    awardAdditionalSection.TitleText = Convert.ToString(dt.Rows[0]["AdditionalTitle"]);
                    awardAdditionalSection.DescriptionText = Convert.ToString(dt.Rows[0]["AdditionalDescription"]);
                    awardAdditionalSection.isTabularForm = Convert.ToBoolean(dt.Rows[0]["isTabularForm"]);
                    awardAdditionalSection.AdditionalsList = Utility.WebCommon.DataTableToList<AwardAdditionalModel>(dt);
                }
            }
            return awardAdditionalSection;
        }


        /// <summary>
        /// Return all award type for correspondent to passed award category i.e for Indivisual or Team category.
        /// </summary>
        /// <param name="awardCategoryId">Award category id for which we want all award type</param>
        /// <returns>List of AwardTypeModel</returns>
        public List<AwardTypeModel> GetAllAwardType(int awardCategoryId)
        {
            List<AwardTypeModel> objAwardMasterModel = new List<AwardTypeModel>();
            DataTable dt;
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@awardcategoryid", SqlDbType.Int).Value = awardCategoryId;
            command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "GETAWARDTYPES";
            using (dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_awardMaster, command))
            {
                if (dt.Rows.Count > 0)
                {
                    objAwardMasterModel = Utility.WebCommon.DataTableToList<AwardTypeModel>(dt);


                    //foreach (DataRow dr in dt.Rows)
                    //{
                    //    objAwardMasterModel.Add(new AwardTypeModel()
                    //    {
                    //        AwardTypeID = Convert.ToInt32(dr["AwardTypeID"]),
                    //        AwardType = Convert.ToString(dr["AwardType"]),
                    //        SectionCount = Convert.ToInt32(dr["SectionCount"])
                    //    });
                    //}
                }
            }
            return objAwardMasterModel;
        }

        /// <summary>
        /// Return all award category
        /// </summary>
        /// <returns>List of AwardCategory</returns>
        public List<AwardCataegory> GetAwardCategory()
        {
            List<AwardCataegory> AwardCataegoryList = new List<AwardCataegory>();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "GETAWARDCATEGORY";
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_awardMaster, command))
            {
                if (dt.Rows.Count > 0)
                {
                    AwardCataegoryList = Utility.WebCommon.DataTableToList<AwardCataegory>(dt);
                }
            }

            return AwardCataegoryList;
        }

        /// <summary>
        /// Return bsic award details that requrd for the nomination form based on role of logged in user.
        /// </summary>
        /// <param name="userID">UserName need to pass</param>
        /// <returns>Object of AwardModel class </returns>
        public AwardModel GetBasicAwardDetails(string userID)
        {
            AwardModel objAwardModel = new AwardModel();
            UserMaster_BAL objUserMaster_BAL = new UserMaster_BAL();
            objAwardModel.NominatorDetails = objUserMaster_BAL.GetuserDetails(userID);
            objAwardModel.validUser = objUserMaster_BAL.ValidUser(userID);
            objAwardModel.AllEmployeeList = objUserMaster_BAL.GetAllEmployeeListUnderSupervisor(userID);
            objAwardModel.HODList = objUserMaster_BAL.GetHOD(userID);
            AwardMaster_BAL objAwardMaster_BAL = new AwardMaster_BAL();
            objAwardModel.AwardCategoryList = objAwardMaster_BAL.GetAwardCategory();
            if (objAwardModel.AwardCategoryList != null && objAwardModel.AwardCategoryList.Count > 0)
            {
                objAwardModel.AwardTypeList = objAwardMaster_BAL.GetAllAwardType(objAwardModel.AwardCategoryList[0].AwardCategoryId);
            }
            if (objAwardModel.AwardTypeList != null && objAwardModel.AwardTypeList.Count > 0)
            {
                objAwardModel.SectionList = objAwardMaster_BAL.GetAwardSection(objAwardModel.AwardTypeList[0].AwardTypeID).SectionsList;
                objAwardModel.additionalSection = objAwardMaster_BAL.GetAwardAdditionalSection(objAwardModel.AwardTypeList[0].AwardTypeID);
            }
            objAwardModel.EmpCount = 1;
            return objAwardModel;
        }


        /// <summary>
        /// Return the user filled details to mantain form   
        /// </summary>
        /// <param name="userID">UserName need to pass</param>
        /// <returns>Object of AwardModel class </returns>
        public AwardModel SetAwardDetails(string userID, AwardModel awardModel)
        {
            AwardModel objAwardModel = new AwardModel();
            UserMaster_BAL objUserMaster_BAL = new UserMaster_BAL();
            objAwardModel.NominatorDetails = objUserMaster_BAL.GetuserDetails(userID);
            objAwardModel.validUser = objUserMaster_BAL.ValidUser(userID);

            awardModel.AllEmployeeList = objUserMaster_BAL.GetAllEmployeeListUnderSupervisor(userID);
            awardModel.HODList = objUserMaster_BAL.GetHOD(userID);
            AwardMaster_BAL objAwardMaster_BAL = new AwardMaster_BAL();
            awardModel.AwardCategoryList = objAwardMaster_BAL.GetAwardCategory();

            awardModel.AwardTypeList = objAwardMaster_BAL.GetAllAwardType(awardModel.AwardCategory);
            objAwardModel.SectionList = objAwardMaster_BAL.GetAwardSection(awardModel.AwardType).SectionsList;

            foreach (var sec in objAwardModel.SectionList)
            {
                foreach (var formSec in awardModel.SectionList)
                {
                    if (formSec.SectionID == sec.SectionID)
                    {
                        formSec.CharacterLength = sec.CharacterLength;
                        formSec.DescriptionText = sec.DescriptionText;
                        formSec.SectionText = sec.SectionText;
                    }
                }
            }


            objAwardModel.additionalSection = objAwardMaster_BAL.GetAwardAdditionalSection(awardModel.AwardType);
            awardModel.additionalSection.DescriptionText = objAwardModel.additionalSection.DescriptionText;
            awardModel.additionalSection.AdditionalPartID = objAwardModel.additionalSection.AdditionalPartID;
            awardModel.additionalSection.TitleText = objAwardModel.additionalSection.TitleText;

            foreach (var asec in objAwardModel.additionalSection.AdditionalsList)
            {
                foreach (var formASec in awardModel.additionalSection.AdditionalsList)
                {
                    if (formASec.AdditionalQuestionID == asec.AdditionalQuestionID)
                    {
                        formASec.CharacterLength = asec.CharacterLength;
                        formASec.DescriptionText = asec.DescriptionText;
                        formASec.TitleText = asec.TitleText;
                    }
                }
            }

            return awardModel;
        }

        /// <summary>
        /// Return List of Years from basic award details for nomination form
        /// </summary>
        /// <returns>List<string> </returns>
        public dynamic GetAwardTime(string userID)
        {
            AwardPortion objAwardPortion = new AwardPortion();

            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@isActive", SqlDbType.Int).Value = 1;
            command.Parameters.Add("@userID", SqlDbType.VarChar, 150).Value = userID;
            command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "ALLACTIVEAWARDS";
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_awardMaster, command))
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

                var distinctQuarter = dt.AsEnumerable()
                       .Select(row => new
                       {
                           AwardQuarter = row.Field<string>("AwardQuarter")
                       }).Distinct().ToList();

                foreach (var i in distinctQuarter)
                {
                    objAwardPortion.quarters.Add(i.AwardQuarter);
                }
            }
            return objAwardPortion;
        }

        /// <summary>
        ///Return Quarters from basic details for nomination
        /// </summary>
        /// <returns>List<string></returns>
        public dynamic GetQuarter()
        {
            List<string> quarters = new List<string>();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@isActive", SqlDbType.Int).Value = 1;
            command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "ALLACTIVEAWARDS";
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_awardMaster, command))
            {
                var distinctValues = dt.AsEnumerable()
                       .Select(row => new
                       {
                           AwardQuarter = row.Field<string>("AwardQuarter")
                       }).Distinct().ToList();

                foreach (var i in distinctValues)
                {
                    quarters.Add(i.AwardQuarter);
                }
                return quarters;
            }
        }

        /// <summary>
        /// Return Award Type details by passing awardtypeId
        /// </summary>
        /// <param name="awardTypeID">award type id integer </param>
        /// <returns>AwardTypeModel object</returns>
        public AwardTypeModel getAwardTypeByAwardTypeID(int awardTypeID)
        {
            AwardTypeModel objAwardTypeModel = new AwardTypeModel();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@awardtypeid", SqlDbType.Int).Value = awardTypeID;
            command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "AWARDTYPE";
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_awardMaster, command))
            {
                if (dt.Rows.Count > 0)
                {
                    objAwardTypeModel.AwardType = dt.Rows[0]["AwardType"].ToString();
                    objAwardTypeModel.AwardTypeID = Convert.ToInt32(dt.Rows[0]["AwardTypeID"].ToString());
                    objAwardTypeModel.PPTUrl = dt.Rows[0]["PPTUrl"].ToString();
                }
            }
            return objAwardTypeModel;
        }

        /// <summary>
        /// Return all functions from all nominations
        /// </summary>
        /// <returns>List<string></returns>
        public dynamic GetAllFunctions()
        {
            List<string> functions = new List<string>();            
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "ALL_FUNCTIONS";
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_awardMaster, command))
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rw in dt.Rows)
                    {
                        functions.Add(rw["FunctionName"].ToString());
                    }
                }
            }
            return functions;
        }

        /// <summary>
        /// Nomination is active or not
        /// </summary>
        /// <returns>Bool</returns>
        public bool NominationActive(string userID)
        {
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@isActive", SqlDbType.Int).Value = 1;
            command.Parameters.Add("@userID", SqlDbType.VarChar, 150).Value = userID;
            command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "ALLACTIVEAWARDS";
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_awardMaster, command))
            {
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        //Changes by Arpit
        public List<AwardAdminModel> GetAdminControl()
        {
            List<AwardAdminModel> awardTypeModels = new List<AwardAdminModel>();
            SqlCommand command = new SqlCommand();
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_AdminPage,command))
            {
                if(dt.Rows.Count > 0)
                {
                    {
                        awardTypeModels = Utility.WebCommon.DataTableToList<AwardAdminModel>(dt);
                    }                   
                    return awardTypeModels;
                }
            }
            return awardTypeModels;
        }
        public int ChangeStatus(int id, int Counter)
        {
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@awardtypeid", SqlDbType.Int).Value = id;
            command.Parameters.Add("@counter", SqlDbType.Int).Value = Counter;
            return SQLHelper.ExecuteSP(RAFE.Utility.Constants.sp_isActive,command);
        }
        public int ChangeMemberStatus(int id, int Counter)
        {
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@id", SqlDbType.Int).Value = id;
            command.Parameters.Add("@counter", SqlDbType.Int).Value = Counter;
            return SQLHelper.ExecuteSP(RAFE.Utility.Constants.sp_CommitteeMemberStatus, command);
        }
        public int DeleteEntry(int id)
        {
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@awardtypeid", SqlDbType.Int).Value = id;
            //command.Parameters.Add("@counter", SqlDbType.Int).Value = Counter;
            return SQLHelper.ExecuteSP(RAFE.Utility.Constants.sp_isDeleted, command);
        }
        public int ChangeFinance(int id,int Counter)
        {
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@awardtypeid", SqlDbType.Int).Value = id;
            command.Parameters.Add("@counter", SqlDbType.Int).Value = Counter;
            return SQLHelper.ExecuteSP(RAFE.Utility.Constants.sp_financeHeadApproval, command);
        }
        public AwardModel GetDetails(int nominationId,string userId)
        {
            AwardModel awardModel = new AwardModel();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@NominationID",SqlDbType.Int).Value = nominationId;
            command.Parameters.Add("@userID", SqlDbType.VarChar,150).Value = userId;
            command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "GETNOMINATIONDETAIL";

            DataTable dt =  SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_Nomination, command);
            if (dt.Rows.Count > 0)
            {
                awardModel.NominationID = Convert.ToInt32(dt.Rows[0]["NominationID"].ToString());
                awardModel.addFileName = dt.Rows[0]["addFileName"].ToString();
                awardModel.videoFileName = dt.Rows[0]["videoFileName"].ToString();
                awardModel.financeFileName = dt.Rows[0]["financeFileName"].ToString();
            }
            return awardModel;
        }

        public JuryModel GetJuryDetails(int nominationId, string userId)
        {
            JuryModel awardModel = new JuryModel();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@NominationID", SqlDbType.Int).Value = nominationId;
            DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.SP_GetJuryComments, command);
            if (dt.Rows.Count > 0)
            {
                awardModel.NominationID = Convert.ToInt32(dt.Rows[0]["NominationID"].ToString());
                awardModel.Comments = dt.Rows[0]["Comments"].ToString();
            }
            return awardModel;
        }

        //Saurabh Kumar Date-19/05/2023
        //this is for Jury member ask feedback 
        public JuryFeedback AskJuryFeedbackDetails(int nominationId, string userId, string sendrMail)
        {
            JuryFeedback feedbackModel = new JuryFeedback();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@NominationID", SqlDbType.Int).Value = nominationId;
            command.Parameters.Add("@Action", SqlDbType.VarChar).Value = "SELECT";
            command.Parameters.Add("@SenderName", SqlDbType.VarChar).Value = " ";
            command.Parameters.Add("@SenderEmailId", SqlDbType.VarChar).Value = sendrMail;
            command.Parameters.Add("@ReceiverName", SqlDbType.VarChar).Value = " ";
            command.Parameters.Add("@ReceiverEmailId", SqlDbType.VarChar).Value = " ";
            command.Parameters.Add("@AskFeedback", SqlDbType.VarChar).Value = " ";
            command.Parameters.Add("@GiveFeedback", SqlDbType.VarChar).Value = " ";
            command.Parameters.Add("@SendingDate", SqlDbType.DateTime).Value = DateTime.Now;
            command.Parameters.Add("@ReceivingDate", SqlDbType.DateTime).Value = DateTime.Now;
            command.Parameters.Add("@Updated", SqlDbType.DateTime).Value = DateTime.Now;
            command.Parameters.Add("@IsActive", SqlDbType.Int).Value = 1;
            command.Parameters.Add("@IsDeleted", SqlDbType.Int).Value = 1;

            DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_JuryAskFeedback, command);
            if (dt.Rows.Count > 0)
            {
                feedbackModel.NominationId = Convert.ToInt32(dt.Rows[0]["NominationId"].ToString());
                feedbackModel.AskFeedback = dt.Rows[0]["AskFeedback"].ToString();
                feedbackModel.GiveFeedback = dt.Rows[0]["GiveFeedback"].ToString();
            }
            return feedbackModel;
        }
        //Saurabh Kumar Date:31/052023
        public List<GiveFeedBack> GiveFeedBacks(string eMail)
        {
            List<GiveFeedBack> obj_feedBacksList = new List<GiveFeedBack>();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@ReceiverEmailId", SqlDbType.VarChar).Value = eMail;
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.SP_GetDetailsForFeedback, command))
            {
                foreach (DataRow rw in dt.Rows)
                {
                    obj_feedBacksList.Add(new GiveFeedBack
                    {
                        SrNo = rw["SrNo"].ToString(),
                        NominationId = Convert.ToInt32(rw["NominationId"]),
                        //awardtypeid=Convert.ToInt32(rw["awardtypeid"]),
                        Nominee = rw["Nomenees"].ToString(),
                        AwardType=rw["AwardType"].ToString(),
                        AwardCategory=rw["AwardCategory"].ToString(),
                        AwardYear=Convert.ToInt32(rw["AwardYear"]),
                        FunctionName=rw["FunctionName"].ToString(),
                        AskFeedback=rw["AskFeedback"].ToString(),
                        GiveFeedback=rw["GiveFeedback"].ToString(),
                        CurrentStatus =rw["CurrentStatus"].ToString()
                    });
                }
            }
            return obj_feedBacksList;
        }
        //Saurabh Kumar Date:14/06/2023
        public List<GiveFeedBack> SeeFeedBacks(string eMail)
        {
            List<GiveFeedBack> obj_feedBacksList = new List<GiveFeedBack>();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@SenderEmailId", SqlDbType.VarChar).Value = eMail;
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.SP_GetDetailsForSeeFeedback, command))
            {
                foreach (DataRow rw in dt.Rows)
                {
                    obj_feedBacksList.Add(new GiveFeedBack
                    {
                        SrNo = rw["SrNo"].ToString(),
                        NominationId = Convert.ToInt32(rw["NominationId"]),
                        //awardtypeid=Convert.ToInt32(rw["awardtypeid"]),
                        AwardType = rw["AwardType"].ToString(),
                        AwardCategory = rw["AwardCategory"].ToString(),
                        AwardYear = Convert.ToInt32(rw["AwardYear"]),
                        FunctionName = rw["FunctionName"].ToString(),
                        AskFeedback = rw["AskFeedback"].ToString(),
                        GiveFeedback = rw["GiveFeedback"].ToString(),
                        CurrentStatus = rw["CurrentStatus"].ToString(),
                       ReceiverName = rw["ReceiverName"].ToString()
                    });
                }
            }
            return obj_feedBacksList;
        }
        public GiveFeedBack SaveFeedback(int NominationId, string GiveFeedback)
        {
            GiveFeedBack giveFeedBackModel = new GiveFeedBack();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@NominationId", SqlDbType.Int).Value = NominationId;
            command.Parameters.Add("@GiveFeedback", SqlDbType.VarChar).Value = GiveFeedback;
            //command.Parameters.Add("@Updated", SqlDbType.DateTime).Value = DateTime.Now;

            DataTable dt =SQLHelper.GetDataTable(RAFE.Utility.Constants.SP_SaveFeedback, command);
            return giveFeedBackModel;
        }
        public List<ACMember> GetACMember()
        {
            List<ACMember> CommitteeMemberList = new List<ACMember>();
            SqlCommand command = new SqlCommand();
            //command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "GETAWARDCATEGORY";
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_GetACMembers, command))
            {
                if (dt.Rows.Count > 0)
                {
                    CommitteeMemberList = Utility.WebCommon.DataTableToList<ACMember>(dt);
                }
            }
            return CommitteeMemberList;
        }
        public List<FHList> ACMapping()
        {
            List<FHList> obj_dropdown = new List<FHList>();
            SqlCommand command = new SqlCommand();
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_AdminPage, command))
            {
                foreach (DataRow rw in dt.Rows)
                {
                    obj_dropdown.Add(new FHList
                    {
                        value = rw["AwardTypeID"].ToString(),
                        text = rw["AwardName"].ToString(),
                    });
                }
            }
            return obj_dropdown;
        }
        public List<PreviousYearWinners> GetPreviousWinners()
        {
            List<PreviousYearWinners> previousYearWinners = new List<PreviousYearWinners>();
            SqlCommand command = new SqlCommand();
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_GetPreviousYearWinners, command))
            {
                if (dt.Rows.Count > 0)
                {
                    {
                        previousYearWinners = Utility.WebCommon.DataTableToList<PreviousYearWinners>(dt);
                    }
                    return previousYearWinners;
                }
            }
            return previousYearWinners;
        }
        public List<Dropdown1> YearList(string filter)
        {
            List<Dropdown1> obj_seconddropdown = new List<Dropdown1>();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@para1", SqlDbType.VarChar).Value = filter;
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_SecondDropdown, command))
            {
                if (dt != null)
                {
                    foreach (DataRow rw in dt.Rows)
                    {
                        obj_seconddropdown.Add(new Dropdown1
                        {
                            value = rw["value"].ToString(),
                            text = rw["text"].ToString(),
                        });
                    }
                }
            }
            return obj_seconddropdown;
        }
    }
}