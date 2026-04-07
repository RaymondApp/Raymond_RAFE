using System;
using System.Configuration;
using System.IO;

namespace RAFE.Utility
{
    public class ClsLogging
    {
        #region Log types

        public enum LogType
        {
            Process,
            Exception
        }

        #endregion

        #region Log

        public static void Log(LogType logType, String Message)
        {
            try
            {
                string FileName = "Prism_Log_" + DateTime.Now.ToString("ddMMyyyy") + ".txt";
                //string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "\\Log\\";
                string path = Utility.WebCommon.GetApplicationPhysicalPath() + "log\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                FileStream fs = new FileStream(path + FileName, FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter sr = new StreamWriter(fs);
                sr.BaseStream.Seek(0, SeekOrigin.End);
                sr.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:MM:ss tt") + " : " + Message);
                sr.WriteLine("---------------------------------------------------------------------------------------");
                sr.Close();
            }
            catch (Exception)
            {

            }
        }

        #endregion
    }
}