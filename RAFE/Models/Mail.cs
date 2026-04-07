using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RAFE.Models
{
    public class MailModel
    {
        public int SrNo { get; set; }
        public string MailSubject { get; set; }
        public string MailBody { get; set; }
        public string ToMail { get; set; }
        public string ToName { get; set; }
        public string CCMail { get; set; }
    }
}