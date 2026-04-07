using System;
using System.Configuration;
using System.Net.Mail;
using System.Threading;

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

        public static int send(string strTo, string strCc, string strSubject, string strBody)
        {
            try
            {
                SmtpClient MailObj;

                MailObj = new SmtpClient(SMTPHost, SMTPPortNo);
                System.Net.NetworkCredential Nw = new System.Net.NetworkCredential(SMTPUserName, SMTPPassword, SMTPDomain);
                MailObj.UseDefaultCredentials = true;

                MailObj.Timeout = 50000;

                MailObj.Credentials = Nw;

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(SMTPFrom);

                msg.IsBodyHtml = true;
                string[] TO = (Convert.ToString(ConfigurationManager.AppSettings["ApplicationType"]) == "LIVE" ? strTo : Convert.ToString(ConfigurationManager.AppSettings["ToEmailId"])).Split(';');
                string[] CC1 = (Convert.ToString(ConfigurationManager.AppSettings["ApplicationType"]) == "LIVE" ? strCc : Convert.ToString(ConfigurationManager.AppSettings["CCEmailID"])).Split(';');
                string[] BCC = Convert.ToString(ConfigurationManager.AppSettings["BCCEmailID"]).Split(';');

                if (TO.Length > 0)
                {
                    for (int icnt = 0; icnt < TO.Length; icnt++)
                    {
                        string id = TO[icnt].ToString();
                        if (id != "")
                        {
                            if (String.IsNullOrEmpty(TO[icnt]) == false && TO[icnt].ToString() != "")
                                msg.To.Add(new MailAddress(TO[icnt]));
                        }
                    }
                }
                msg.IsBodyHtml = true;
                if (CC1.Length > 0)
                {
                    for (int icnt = 0; icnt < CC1.Length; icnt++)
                    {
                        string id = CC1[icnt].ToString();
                        if (id != "")
                        {
                            if (String.IsNullOrEmpty(CC1[icnt]) == false)
                                msg.CC.Add(new MailAddress(CC1[icnt]));
                        }
                    }
                }
                if (BCC.Length > 0)
                {
                    for (int icnt = 0; icnt < BCC.Length; icnt++)
                    {
                        string id = BCC[icnt].ToString();
                        if (id != "")
                        {
                            if (String.IsNullOrEmpty(BCC[icnt]) == false)
                                msg.Bcc.Add(new MailAddress(BCC[icnt]));
                        }
                    }
                }

                if (Convert.ToString(ConfigurationManager.AppSettings["ApplicationType"]) != "LIVE")
                {
                    strBody = strBody + "<br/>To - " + strTo + "<br/>CC - " + strCc;
                }

                strBody = strBody.Replace("#LOGINURL#", ConfigurationManager.AppSettings["AbsolutePath"].ToString());

                msg.Subject = strSubject;
                msg.Body = strBody;

                MailObj.Send(msg);
                return 1;
            }
            catch (Exception)
            {
                ClsLogging.Log(ClsLogging.LogType.Exception, "Mail fail for : - " + strSubject);
                return 0;
            }
        }

        public static void sendEmail(string strTo, string strCc, string strSubject, string strBody)
        {
            Thread email = new Thread(delegate()
            {
                send(strTo, strCc, strSubject, strBody);
            });

            email.IsBackground = true;
            email.Start();
        }

    }
}