using System;
using System.Configuration;
using System.Net.Mail;
using System.Threading;
using System.IO;
using System.Net;

namespace RAFE.Utility
{
    public class EmailLibrary
    {
        private static string SMTPHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();
        private static string SMTPUserName = ConfigurationManager.AppSettings["SMTPUserName"].ToString();
        private static string SMTPPassword = ConfigurationManager.AppSettings["SMTPPassword"].ToString();
        private static string SMTPDomain = ConfigurationManager.AppSettings["SMTPDomain"].ToString();
        private static int SMTPPortNo = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"].ToString());
        private static string SMTPFrom = ConfigurationManager.AppSettings["SMTPFrom"].ToString();

        public static void sendEmail(string strTo, string strCc, string strSubject, string strBody)
        {
            Thread email = new Thread(delegate ()
            {
                sendEmail(strTo, strCc, strSubject, strBody, "");
            });

            email.IsBackground = true;
            email.Start();
        }

        public static int sendEmail(string strTo, string strCc, string strSubject, string strBody, string ActualPathToAttachment = "")
        {
            MailMessage msg = new MailMessage();
            msg.IsBodyHtml = true;

            string[] to = (Convert.ToString(ConfigurationManager.AppSettings["ApplicationType"]) == "LIVE" ? strTo : Convert.ToString(ConfigurationManager.AppSettings["ToEmailId"])).Split(';');
            string[] CC1 = (Convert.ToString(ConfigurationManager.AppSettings["ApplicationType"]) == "LIVE" ? strCc : Convert.ToString(ConfigurationManager.AppSettings["CCEmailID"])).Split(';');


            //if (Convert.ToString(ConfigurationManager.AppSettings["ApplicationType"]) != "LIVE")
            //{
            //    strBody = strBody + "<br/>To - " + strTo + "<br/>CC - " + strCc;
            //}
            //strBody = strBody.Replace("#LOGINURL#", ConfigurationManager.AppSettings["AbsolutePath"].ToString());


            try

            {

                string from = "ApplicationAlerts@Raymond.in";
                string password = "schlljmwrrjrqfvp";

                msg.From = new MailAddress(from);

                foreach (string recipient in to)
                {
                    if (recipient.Trim() != "")
                        msg.To.Add(recipient);
                }

                if (strSubject != "Forgot password request")
                {

                    if (CC1 == null || CC1.Length == 0)
                    {
                        // Where CC1 is null or empty
                    }
                    else
                    {
                        foreach (string recipient in CC1)
                        {
                            if (!string.IsNullOrWhiteSpace(recipient)) // Ensure recipient is not null, empty, or whitespace
                            {
                                try
                                {
                                    msg.CC.Add(recipient); // Add valid recipient to the CC list
                                }
                                catch (Exception ex)
                                {
                                    // Error if adding the recipient fails                               
                                }
                            }
                            else
                            {
                                // The recipient is null, empty, or whitespace
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["DefaultCCEmailID"]))
                    {
                        string DefaultCCEmailID = Convert.ToString(ConfigurationManager.AppSettings["DefaultCCEmailID"]);
                        msg.CC.Add(new MailAddress(DefaultCCEmailID));
                    }

                }


                MailAddress addressBCC = new MailAddress("bhavana.pundir@Raymond.in");
                msg.Bcc.Add(addressBCC);
                string mailbody = strBody;
                msg.Subject = strSubject;
                msg.Body = mailbody;

                //if (ActualPathToAttachment != "" && File.Exists(ActualPathToAttachment))
                //    msg.Attachments.Add(new Attachment(ActualPathToAttachment));

                msg.BodyEncoding = System.Text.Encoding.UTF8;
                msg.IsBodyHtml = true;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                SmtpClient client = new SmtpClient("smtp.office365.com", 587);

                System.Net.NetworkCredential basicCredential1 = new System.Net.NetworkCredential(from, password);
                client.EnableSsl = true;
                client.UseDefaultCredentials = true;
                client.Credentials = basicCredential1;

                try
                {

                    client.Send(msg);

                }
                catch (Exception ex)
                {
                    string s = ex.Message + ' ' + ex.Source;
                }
            }

            catch (Exception ex)
            {
            }
            return 1;
        }


    }
}