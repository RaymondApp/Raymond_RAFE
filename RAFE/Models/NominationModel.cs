using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RAFE.Models
{
    public class NominationModel
    {
        public int AwardYear { get; set; }
        public string AwardQuarter { get; set; }
        public string employeenm { get; set; }
        public int AwardCategoryId { get; set; }
        public int AwardTypeId { get; set; }
        public bool isAuthorized { get; set; }
        public List<Nomination> Nominationlist { get; set; }
        public NominationModel()
        {
            Nominationlist = new List<Nomination>();
        }
        public List<int> AllAwardYears { get; set; }
        public List<Dropdown> Categories { get; set; }
        public List<SecondDropdown> Filters { get; set; }
        public string Category { get; set;}
        public string Filter { get; set; }
        public string Rating { get; set; }
        public RatingHeads ACScore { get; set; }
        
    }

    public class Nomination
    {
        public int SrNo { get; set; }
        public int NominationID { get; set; }
        public int AwardYear { get; set; }
        public string AwardQuarter { get; set; }
        public int AwardCategoryId { get; set; }
        public string AwardCategory { get; set; }
        public int AwardTypeId { get; set; }
        public string AwardType { get; set; }
        public bool isSubmitted { get; set; }
        public bool isApproved { get; set; }
        public string CurrentStatus { get; set; }
        public int Functionid { get; set; }
        public string FunctionName { get; set; }
        public string Rationale { get; set; }
        public string Nomenees { get; set; }
        public string Nominator { get; set; }
        public int StatusID { get; set; }
        public string Rating { get; set; }
        public string Comments { get; set; }
        public int weightage { get; set; }
        public decimal score { get; set; }
        public string ratingname { get; set; }
        public string Score_Details { get; set; }
        public string ACMember { get; set; }
    }
    
    public class Dropdown
    {
        public string text { get; set; }
        public string value { get; set; }
        public class OrderList
        {
            public List<Dropdown> Order { get; set; }
        }
    }
    public class SecondDropdown
    {
        public string text { get; set; }
        public string value { get; set; }
        public class SecondList
        {
            public List<SecondDropdown> Second { get; set; }
        }
    }
}