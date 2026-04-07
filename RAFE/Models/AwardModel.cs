using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using RAFE.Models;

namespace RAFE.Models
{
    public class AwardModel
    {
        public int NominationID { get; set; }
        public string NominatorSamAccountName { get; set; }
        [Required(ErrorMessage = "Please select award category.")]
        public int AwardCategory { get; set; }
        public List<AwardCataegory> AwardCategoryList { get; set; }
        [Required(ErrorMessage = "Please select award type.")]
        public int AwardType { get; set; }
        public List<AwardTypeModel> AwardTypeList { get; set; }
        public string AwardYear { get; set; }
        public List<int> AwardYearList { get; set; }
        public string AwardQuarter { get; set; }
        public List<string> AwardQuarterList { get; set; }
        public int EmpCount { get; set; }
        public int isSubmitted { get; set; }
        public bool isApproved { get; set; }

        [Required(ErrorMessage = "Please select employee for award.")]
        public string EmployeeID1 { get; set; }
        public string EmployeeName1 { get; set; }
        public string ImagePath1 { get; set; }
        public HttpPostedFileBase EmployeeImage1 { get; set; }
        public string EmployeeID2 { get; set; }
        public string EmployeeName2 { get; set; }
        public string EmployeeID3 { get; set; }
        public string EmployeeName3 { get; set; }
        public string EmployeeID4 { get; set; }
        public string EmployeeName4 { get; set; }
        public string EmployeeID5 { get; set; }
        public string EmployeeName5 { get; set; }
        public string EmployeeID6 { get; set; }
        public string EmployeeName6 { get; set; }
        public string EmployeeID7 { get; set; }
        public string EmployeeName7 { get; set; }
        public string EmployeeID8 { get; set; }
        public string EmployeeName8 { get; set; }
        public string EmployeeID9 { get; set; }
        public string EmployeeName9 { get; set; }
        public string EmployeeID10 { get; set; }
        public string EmployeeName10 { get; set; }

        //Shraddha
        public string Other_EmployeeID1 { get; set; }
        public string Other_EmployeeID2 { get; set; }
        public string Other_EmployeeID3 { get; set; }
        public string Other_EmployeeID4 { get; set; }
        public string Other_EmployeeID5 { get; set; }
        public string Other_EmployeeID6 { get; set; }
        public string Other_EmployeeID7 { get; set; }
        public string Other_EmployeeID8 { get; set; }
        public string Other_EmployeeID9 { get; set; }
        public string Other_EmployeeID10 { get; set; }
        public HttpPostedFileBase videoFile { get; set; }
        public string videoFileName { get; set; }
        public string videoFilePath { get; set; }
        public HttpPostedFileBase addFile { get; set; }
        public string addFileName { get; set; }
        public string addFilePath { get; set; }

        public HttpPostedFileBase financeFile { get; set; }
        public string financeFileName { get; set; }
        public string financeFilePath { get; set; }
        public string financeHeadName { get; set; }

        [Required(ErrorMessage = "Please enter the Reference details")]
        public string refName1 { get; set; }
        public string refName2 { get; set; }

        ///////////
        /// </summary>
        /// 
        public UserModel NominatorDetails { get; set; }
        public List<AwardSectonModel> SectionList { get; set; }
        public List<EmployeeModel> AllEmployeeList { get; set; }
        public List<RemarkModel> RemarkList { get; set; }
        public List<EmployeeModel> HODList { get; set; }

        [Required(ErrorMessage = "Please select HOD.")]

        public string HOD { get; set; }
        public int StatusID { get; set; }
        public bool isEditable { get; set; }
        public string Remarks { get; set; }
        public string NominationLevel { get; set; }
        public string UserRole { get; set; }
        public AwardAdditionalSection additionalSection { get; set; }
        public bool validUser { get; set; }
        public AwardModel()
        {
            SectionList = new List<AwardSectonModel>();
            additionalSection = new AwardAdditionalSection();
            NominatorDetails = new UserModel();
            AllEmployeeList = new List<EmployeeModel>();
        }
        public List<FHList> Approvers { get; set; }
        public string Comments { get; set; }
        public List<FHList> AllComments { get; set; }
        public List<FHList> ACNames { get; set; }
        public string ACName { get; set; }
        public List<RatingHeads> RatingList { get; set; }
        public string ACComments { get; set; }
        public string Score_Details { get; set; }
        public string ACMember { get; set; }
    }
    public class RatingHeads
    {
        public int Id { get; set; }
        public int awardtypeid { get; set; }
        public int nominationid { get; set; }
        public string ratingheadid { get; set; }
        public int weightage { get; set; }
        public decimal score { get; set; }
        public string ratingname { get; set; }
    }
    public class NominationFormApprovalValue
    {
        public int nominationcount { get; set; }
        public int financeapproval { get; set; }
    }
    public class FHList
    {
        public string value { get; set; }
        public string text { get; set; }
    }

    public class EmployeeModel
    {
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string ImagePath { get; set; }
        public HttpPostedFileBase EmployeeImage { get; set; }
    }
    public class RemarkModel
    {
        public string AppStepID { get; set; }
        public string NominationID { get; set; }
        public string Level { get; set; }
        public string StatusID { get; set; }
        public string Remarks { get; set; }
        public string UpdatedBy { get; set; }

    }

    public class AwardYearList
    {
        public List<string> MyProperty { get; set; }
    }

    public class AwardPortion
    {
        public List<int> years { get; set; }
        public List<string> quarters { get; set; }
        public AwardPortion()
        {
            years = new List<int>();
            quarters = new List<string>();
        }
    }
    public class Dashboard
    {
        public Dashboard()
        {        
            lsPieChart = new List<PRPieChart>();
            PieChart2nd = new List<PRPieChart>();
        }
        public string value { get; set; }
        public string text { get; set; }
        public List<PRPieChart> lsPieChart { get; set; }
        public List<PRPieChart> PieChart2nd { get; set; }
        public List<PRPieChart> PRBarChart { get; set; }
        public List<string> business { get; set; }
        public List<Status> List { get; set; }

    }
    
    public class Status
    {
        public string name { get; set; }
        public List<int> data { get; set; }
    }
    public class List1
    {
        public List<PRPieChart> Order1 { get; set; }
    }

    public class List2
    {
        public List<PRPieChart> Order2 { get; set; }
    }
    public class PRPieChart
    {
        public string name { get; set; }
        public int y { get; set; }
        public string color { get; set; }
    }
    //Saurabh Kumar
    public class JuryModel
    {
        public int NominationID { get; set; }
        public string Comments { get; set; } 
    }
    public class JuryFeedback
    {
        public string Action { get; set; }
        public int NominationId { get; set; }
        public string SenderName { get; set; }
        public string SenderEmailId { get; set; }
        public string ReceiverName{get;set;}
        public string ReceiverEmailId { get; set; }
        public string AskFeedback { get; set; }
        public string GiveFeedback { get; set; }
        public DateTime SendingDate { get; set; }
        public DateTime ReceivingDate { get; set; }
        public DateTime Update { get; set; }
        public int IsActive { get; set; }
	    public int IsDeleted { get; set; }
    }

    //Saurabh Kumar Date:30/05/2023
    public class GiveFeedBack
    {
        public string SrNo { get; set; }
        public int NominationId { get; set; }
        //public int awardtypeid { get; set; }
        public string Nominee { get; set; }
        public string AwardType { get; set; }
        public string AwardCategory { get; set; }
        public int AwardYear { get; set; }
        public string FunctionName { get; set; }
        public string AskFeedback { get; set; }
        public string GiveFeedback { get; set; }
        public string CurrentStatus { get; set; }
        public string ReceiverName { get; set; }

        public List<GiveFeedBack> GiveFeedBackList { get; set; }
        public GiveFeedBack()
        {
            GiveFeedBackList = new List<GiveFeedBack>();
        }
    }
    public class Requirefeedback
    {

    }
    public class ACMember
    {
        public int Id { get; set; }
        public string AwardCommitteeMember { get; set; }
        public string EmailId { get; set; }
        public int awardtypeid { get; set; }
        public List<FHList> AwardName { get; set; }
        public int isActive { get; set; }
    }
}