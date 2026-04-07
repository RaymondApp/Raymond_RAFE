using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RAFE.Models
{
    
    public class AwardMaster
    {
        public int SrNo { get; set; }
        public int AwardID { get; set; }
        [Required(ErrorMessage = "Please select award year.")]
        public int AwardYear { get; set; }
        [Required(ErrorMessage = "Please select award quarter.")]
        public string AwardQuarter { get; set; }
        [Required(ErrorMessage = "Please select last date for nomination.")]
        public string NominationLastDate { get; set; }
        public string ApprovalLastDate { get; set; }
        public int isActive { get; set; }
        public List<AwardMaster> AwardMasterList { get; set; }
        public string action { get; set; }
        public AwardMaster()
        {
            AwardMasterList = new List<AwardMaster>();
            PreviousYearWinner = new List<PreviousYearWinners>();
        }
        public List<PreviousYearWinners> PreviousYearWinner { get; set; }
        public List<Dropdown1> YearList { get; set; }
        public string Year { get; set; }
        
    }
    public class Dropdown1
    {
        public string text { get; set; }
        public string value { get; set; }
        public class OrderList
        {
            public List<Dropdown1> Order { get; set; }
        }
    }
    public class AwardTypeModel
    {
        public int AwardTypeID { get; set; }
        public string AwardType { get; set; }
        public int SectionCount { get; set; }
        public string  PPTUrl { get; set; }
        public List<AwardSectonModel> SectionsList { get; set; }
        public AwardTypeModel()
        {
            SectionsList = new List<AwardSectonModel>();
        }
        
    }
    public class AwardAdminModel
    {
        public int AwardTypeID { get; set; }
        public string AwardName { get; set; }
        public string AwardCategory { get; set; }
        public bool isActive { get; set; }
        public int isDeleted { get; set; }
        public int financeHeadApproval { get; set; }
        public List<AwardCataegory> AwardCategoryList { get; set; }

    }
    public class PreviousYearWinners
    {
        public int Id { get; set; }
        public string AwardCategory { get; set; }
        public string AwardName { get; set; }
        public string IndividualName { get; set; }
        public string Business { get; set; }
        public int AwardYear { get; set; }
        public int isActive { get; set; }
        //public List<int> AllAwardYears { get; set; }
    }
   
    
    public class AwardSectonModel
    {
        public int SectionID { get; set; }
        public string SectionText { get; set; }
        public string DescriptionText { get; set; }
        public string SectionInput { get; set; }
        public int CharacterLength { get; set; }
    }
    public class AwardCataegory
    {
        public int AwardCategoryId { get; set; }
        public string AwardCategory { get; set; }
        public bool isActive { get; set; }
    }

    public class AwardAdditionalSection
    {
        public int AdditionalPartID { get; set; }
        public string TitleText { get; set; }
        public string DescriptionText { get; set; }
        public bool isTabularForm { get; set; }
        public List<AwardAdditionalModel> AdditionalsList { get; set; }
        public AwardAdditionalSection()
        {
            AdditionalsList = new List<AwardAdditionalModel>();
        }

    }
    public class AwardAdditionalModel
    {
        public int AdditionalQuestionID { get; set; }
        public string TitleText { get; set; }
        public string DescriptionText { get; set; }
        public string UserInput { get; set; }
        public int CharacterLength { get; set; }
    }
}