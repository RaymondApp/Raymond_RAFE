using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace RAFE.Utility
{
    public class WebCommon
    {
        private static Random random = new Random();

        /// <summary>
        /// Return application absolute path (URL)
        /// </summary>
        /// <returns>string</returns>
        public static string GetApplicationAbsolutePath()
        {
            try
            {
                return ConfigurationManager.AppSettings["AbsolutePath"].ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// Return application physical path  (drive location)
        /// </summary>
        /// <returns>string</returns>
        public static string GetApplicationPhysicalPath()
        {
            try
            {
                return ConfigurationManager.AppSettings["PhysicalPath"].ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }


        /// <summary>
        /// Return current status of nomination based on submittion flag and current status
        /// </summary>
        /// <param name="isSubmitted">submitted flag value in boolen</param>
        /// <param name="CurrentStatus">current status value in string</param>
        /// <returns>string</returns>
        public static string GetSubmitStatus(object isSubmitted, object CurrentStatus)
        {
            string ret = "<span class='label label-danger'>Draft Save</span>";
            try
            {
                if (isSubmitted != DBNull.Value && Convert.ToBoolean(isSubmitted) == true && CurrentStatus != DBNull.Value)
                    ret = Convert.ToString(CurrentStatus);

            }
            catch (Exception)
            {
            }
            return ret;
        }

        /// <summary>
        /// Return award status based on active flag and nomination last date
        /// </summary>
        /// <param name="isActive">Active flag value in int</param>
        /// <param name="nominationLastDate">Nomination last date value in datetime</param>
        /// <returns>string</returns>
        public static string GetAwardActiveStatus(object isActive, object nominationLastDate)
        {
            string ret = "<span class='label label-danger'>Inactive</span>";
            try
            {
                if (isActive != DBNull.Value && Convert.ToBoolean(isActive) == true)
                {
                    DateTime dt = DateTime.ParseExact(Convert.ToString(nominationLastDate), "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    if (dt < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                    {
                        ret = "<span class='label label-danger'>Nomination closed</span>";
                    }
                    else
                    {
                        ret = "<span class='label label-success'>Active</span>";
                    }
                }                  
            }
            catch (Exception)
            {
            }
            return ret;
        }

        /// <summary>
        /// Return a random string of no of character defind by length parameter
        /// </summary>
        /// <param name="length">no of character(int)</param>
        /// <returns>string value</returns>
        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Return datetime in customize formate
        /// </summary>
        /// <param name="obj">datetime value</param>
        /// <returns>string</returns>
        public static string TimeAgo(object obj)
        {

            try
            {
                if (obj == DBNull.Value || Convert.ToString(obj) == "")
                    return string.Empty;
                DateTime dt = Convert.ToDateTime(obj);
                TimeSpan span = DateTime.Now - dt;


                if (span.Days > 6)
                {
                    return dt.ToString("dd MMMM yyyy hh:mm tt");
                }
                if (span.Days == 0 && dt.Day != DateTime.Now.Day)
                {
                    return "yesterday " + dt.ToString("hh:mm tt");
                }
                if (span.Days > 0)
                {
                    return dt.ToString("dddd, hh:mm tt");
                }
                if (span.Hours > 0)
                {
                    if (DateTime.Now.Day == dt.Day)
                        return String.Format("about {0} {1} ago",
                        span.Hours, span.Hours == 1 ? "hour" : "hours");
                    else
                        return dt.ToString("dddd, hh:mm tt");
                }
                if (span.Minutes > 0)
                    return String.Format("about {0} {1} ago",
                    span.Minutes, span.Minutes == 1 ? "minute" : "minutes");
                if (span.Seconds > 5)
                    return String.Format("about {0} seconds ago", span.Seconds);
                if (span.Seconds <= 5)
                    return "just now";
                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Return date in 'dd-MM-yyyy' format
        /// </summary>
        /// <param name="obj">datetime value in datetime datat ype</param>
        /// <returns>string</returns>
        public static string DateTimeFormat1(object obj)
        {

            try
            {
                if (obj == DBNull.Value || Convert.ToString(obj) == "")
                    return string.Empty;
                DateTime dt = Convert.ToDateTime(obj);
                return dt.ToString("dd-MM-yyyy");
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Return date in '29th May 2015' format
        /// </summary>
        /// <param name="obj">Accept datetime format which need to be in required format</param>
        /// <returns>string</returns>
        public static string DateTimeFormat2(object obj)
        {

            try
            {
                if (obj == DBNull.Value || Convert.ToString(obj) == "")
                    return string.Empty;
                DateTime dt = Convert.ToDateTime(obj);
                string suffix = "<sup>" + ((dt.Day % 10 == 1 && dt.Day != 11) ? "st" : (dt.Day % 10 == 2 && dt.Day != 12) ? "nd" : (dt.Day % 10 == 3 && dt.Day != 13) ? "rd" : "th") + "</sup>";
                return string.Format("{0:dd}{1} {0:MMMM yyyy}", dt, suffix);
                //return dt.ToString("dd MMMM yyyy HH:mm tt");
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Return date in '29 May 2015' format
        /// </summary>
        /// <param name="obj">Accept datetime format which need to be in required format</param>
        /// <returns>string</returns>
        public static string DateTimeFormat3(object obj)
        {

            try
            {
                if (obj == DBNull.Value || Convert.ToString(obj) == "")
                    return string.Empty;
                DateTime dt = Convert.ToDateTime(obj);
                return dt.ToString("dd MMMM yyyy");
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Truncate string after perticular character defined in maxLength and return
        /// </summary>
        /// <param name="value">String value which we need to truncate</param>
        /// <param name="maxLength">Maximum character length</param>
        /// <returns>string</returns>
        public static string NormalizeLength(string value, int maxLength)
        {

            string ret = string.IsNullOrEmpty(value) ? "" : value.Length <= maxLength ? value : value.Substring(0, maxLength);
            return ret;
        }

        /// <summary>
        /// Return DataTable having column of passed base class of the supplied list 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lst"></param>
        /// <returns></returns>
        public static DataTable ListToDataTable<T>(List<T> lst)
        {
            System.Data.DataTable dataTable = new System.Data.DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in lst)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        /// <summary>
        /// Return List of <T> dynamic object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> DataTableToList<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        if (dr[column.ColumnName] != null || dr[column.ColumnName] != DBNull.Value)
                        {
                            pro.SetValue(obj, dr[column.ColumnName], null);
                        }
                        else
                            continue;
                }
            }
            return obj;
        }

        /// <summary>
        /// Return fullname of logged in user
        /// </summary>
        /// <param name="reqCookies">Cookies which we need to store logged in user basic information</param>
        /// <returns>Profile name in string</returns>
        public static string GetProfileName(System.Web.HttpCookie reqCookies)
        {
            if (reqCookies != null)
            {
                return reqCookies.Value.ToString().Split('~')[1];
            }
            else
                return "";
        }

        /// <summary>
        /// Return role of logged in user
        /// </summary>
        /// <param name="reqCookies">Cookies which we need to store logged in user basic information</param>
        /// <returns>Role in string</returns>
        public static string GetUserRole(System.Web.HttpCookie reqCookies)
        {
            if (reqCookies != null)
            {
                if (reqCookies.Value == "")
                    throw new HttpException(401, "Unauthorized access");
                else
                    return reqCookies.Value.ToString().Split('~')[2];
            }
            else
                return "";
        }
        /// <summary>
        /// Return Email iD of logged in user
        /// </summary>
        /// <param name="reqCookies">Cookies which we need to store logged in user basic information</param>
        /// <returns>Role in string</returns>
        public static string GetUserEmail(System.Web.HttpCookie reqCookies)
        {
            
            if (reqCookies != null)
            {
                if (reqCookies.Value == "")
                    throw new HttpException(401, "Unauthorized access");
                else
                    return reqCookies.Value.ToString().Split('~')[3];

            }
            else
                return "";
        }
        public static string GetUserCommittee(System.Web.HttpCookie reqCookies)
        {
            if (reqCookies != null)
            {
                if (reqCookies.Value == "")
                    throw new HttpException(401, "Unauthorized access");
                else
                    if (reqCookies.Value.ToString().Split('~').Length > 7)
                {
                    return reqCookies.Value.ToString().Split('~')[7];
                }
                else return "";
            }
            else
                return "";
        }



        /// <summary>
        /// Return Username of logged in user
        /// </summary>
        /// <param name="reqCookies">Cookies which we need to store logged in user basic information</param>
        /// <returns>Profile name in string</returns>
        public static string GetUserName(System.Web.HttpCookie reqCookies)
        {
            if (reqCookies != null)
            {
                return reqCookies.Value.ToString().Split('~')[0];
            }
            else
                return "";
        }
        /// <summary>
        /// Return List of Years in from (current year - 1) to (current year + 4)
        /// </summary>
        /// <returns></returns>
        public static List<string> GetYears()
        {
            List<string> objList = new List<string>();
            int currYear = DateTime.Now.Year;            
            for (int i = (currYear - 1); i < (currYear + 4); i++)
            {
                objList.Add(i.ToString());
            }
            return objList;
        }

        /// <summary>
        /// check whether user is CXO or not
        /// </summary>
        /// <param name="userName">string : userName</param>
        /// <returns>if user is CXO then true else false</returns>
        public static bool isUserCXO(string userName)
        {
            BAL.UserMaster_BAL objUserMaster_BAL = new BAL.UserMaster_BAL();
            return objUserMaster_BAL.GetUserIsCXO(userName.Split('~')[0]).isSuccess;
        }
    }
}