using RAFE.DAL;
using RAFE.Models;
using RAFE.Utility;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace RAFE.BAL
{
    public class Mail_BAL
    {
        public static void GetMail(int nominationID, int statusID, string userName, string autoUrl)
        {
            DataSet ds;
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@NominationID", SqlDbType.Int).Value = nominationID;
            command.Parameters.Add("@StatusID", SqlDbType.Int).Value = statusID;
            command.Parameters.Add("@UserID", SqlDbType.VarChar, 150).Value = userName;
            using (ds = SQLHelper.GetDataSet(Constants.sp_MailTemplate, command))
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    List<MailModel> mailList = Utility.WebCommon.DataTableToList<MailModel>(ds.Tables[0]);
                    foreach (var objMail in mailList)
                    {
                        Utility.EmailLibrary.sendEmail(objMail.ToMail, objMail.CCMail, objMail.MailSubject, objMail.MailBody);
                    }
                }
            }
        }
       

    }
}