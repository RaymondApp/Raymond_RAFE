using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD_Exe
{
    class Program
    {
        static void Main(string[] args)
        {

            String LdapUser = "raymond\\MossAdmin";
            String LdapUserPwd = "malumnahi";
            String domain = "Raymond.in";
            String Ldpconn = "LDAP://10.100.100.151";
            //String password = ConfigurationSettings.AppSettings["defultPwd"];



            DirectoryEntry LdapConnection = createDirectoryEntry(Ldpconn);   

            DirectorySearcher srchUserExist = new DirectorySearcher(LdapConnection);

            srchUserExist.Filter = "(&(objectCategory=person)(objectClass=user)(samaccountname=*))";
            srchUserExist.PropertiesToLoad.Add("Name");
            srchUserExist.PropertiesToLoad.Add("SamAccountName");
            srchUserExist.PropertiesToLoad.Add("Description");
            srchUserExist.PropertiesToLoad.Add("mail");
            srchUserExist.PropertiesToLoad.Add("Company");
            srchUserExist.PropertiesToLoad.Add("Title");
            srchUserExist.PropertiesToLoad.Add("Department");
            srchUserExist.PropertiesToLoad.Add("Enabled");
            srchUserExist.PropertiesToLoad.Add("l");
            srchUserExist.PropertiesToLoad.Add("telephoneNumber");
            srchUserExist.PropertiesToLoad.Add("UserPrincipalName");
            srchUserExist.PropertiesToLoad.Add("GivenName");
            srchUserExist.PropertiesToLoad.Add("SurName");
            srchUserExist.PropertiesToLoad.Add("mobile");
            srchUserExist.PropertiesToLoad.Add("physicalDeliveryOfficeName");


            srchUserExist.PropertiesToLoad.Add("manager");
            srchUserExist.PropertiesToLoad.Add("employeeID");
            srchUserExist.PropertiesToLoad.Add("employeeType");
            srchUserExist.PropertiesToLoad.Add("employeeNumber");
            srchUserExist.PropertiesToLoad.Add("division");
            srchUserExist.PropertiesToLoad.Add("displayNamePrintable");

    

    

            SearchResultCollection resultcoll;
            resultcoll = srchUserExist.FindAll();
            using (DataTable AdData = new DataTable())
            {
                AdData.Columns.Add("Name");
                AdData.Columns.Add("SamAccountName");
                AdData.Columns.Add("Description");
                AdData.Columns.Add("EmailAddress");
                AdData.Columns.Add("MobilePhone");
                AdData.Columns.Add("Company");
                AdData.Columns.Add("Title");
                AdData.Columns.Add("Department");

                DataColumn deDate = new DataColumn("Enabled", typeof(bool));
                deDate.AllowDBNull = true;
                AdData.Columns.Add(deDate);

                AdData.Columns.Add("City");
                AdData.Columns.Add("Office");
                AdData.Columns.Add("OfficePhone");
                AdData.Columns.Add("UserPrincipalName");
                AdData.Columns.Add("GivenName");
                AdData.Columns.Add("SurName");
                AdData.Columns.Add("manager");
                AdData.Columns.Add("employeeID");
                AdData.Columns.Add("employeeType");
                AdData.Columns.Add("employeeNumber");
                AdData.Columns.Add("division");
                AdData.Columns.Add("displayNamePrintable");

                DataRow dr = null;
                if (resultcoll != null)
                {
                    if (resultcoll.Count > 0)
                    {
                        foreach (SearchResult result in resultcoll)
                        {
                            dr = AdData.NewRow();
                            try
                            {

                                if (result.Properties.Contains("Name"))
                                {
                                    dr["Name"] = Convert.ToString(result.Properties["Name"][0]);
                                }
                                else
                                    dr["Name"] = "";

                                if (result.Properties.Contains("SamAccountName"))
                                {
                                    dr["SamAccountName"] = Convert.ToString(result.Properties["SamAccountName"][0]);
                                }
                                else
                                    dr["SamAccountName"] = "";

                                if (result.Properties.Contains("Description"))
                                {
                                    dr["Description"] = Convert.ToString(result.Properties["Description"][0]);
                                }
                                else
                                    dr["Description"] = "";

                                if (result.Properties.Contains("mail"))
                                {
                                    dr["EmailAddress"] = Convert.ToString(result.Properties["mail"][0]);
                                }
                                else
                                    dr["EmailAddress"] = "";

                                if (result.Properties.Contains("mobile"))
                                {
                                    dr["MobilePhone"] = Convert.ToString(result.Properties["mobile"][0]);
                                }
                                else
                                    dr["MobilePhone"] = "";

                                if (result.Properties.Contains("Company"))
                                {
                                    dr["Company"] = Convert.ToString(result.Properties["Company"][0]);
                                }
                                else
                                    dr["Company"] = "";

                                if (result.Properties.Contains("Title"))
                                {
                                    dr["Title"] = Convert.ToString(result.Properties["Title"][0]);
                                }
                                else
                                    dr["Title"] = "";

                                if (result.Properties.Contains("Department"))
                                {
                                    dr["Department"] = Convert.ToString(result.Properties["Department"][0]);
                                }
                                else
                                    dr["Department"] = "";

                                dr["Enabled"] = true;

                                if (result.Properties.Contains("l"))
                                {
                                    dr["City"] = Convert.ToString(result.Properties["l"][0]);
                                }
                                else
                                    dr["City"] = "";



                                if (result.Properties.Contains("physicalDeliveryOfficeName"))
                                {
                                    dr["Office"] = Convert.ToString(result.Properties["physicalDeliveryOfficeName"][0]);
                                }
                                else
                                    dr["Office"] = "";

                                if (result.Properties.Contains("telephoneNumber"))
                                {
                                    dr["OfficePhone"] = Convert.ToString(result.Properties["telephoneNumber"][0]);
                                }
                                else
                                    dr["OfficePhone"] = "";

                                if (result.Properties.Contains("UserPrincipalName"))
                                {
                                    dr["UserPrincipalName"] = Convert.ToString(result.Properties["UserPrincipalName"][0]);
                                }
                                else
                                    dr["UserPrincipalName"] = "";

                                if (result.Properties.Contains("GivenName"))
                                {
                                    dr["GivenName"] = Convert.ToString(result.Properties["GivenName"][0]);
                                }
                                else
                                    dr["GivenName"] = "";

                                if (result.Properties.Contains("SurName"))
                                {
                                    dr["SurName"] = Convert.ToString(result.Properties["sn"][0]);
                                }
                                else
                                    dr["SurName"] = "";

                                if (result.Properties.Contains("manager"))
                                {
                                    dr["manager"] = Convert.ToString(result.Properties["manager"][0]);
                                }
                                else
                                    dr["manager"] = "";


                                if (result.Properties.Contains("employeeID"))
                                {
                                    dr["employeeID"] = Convert.ToString(result.Properties["employeeID"][0]);
                                }
                                else
                                    dr["employeeID"] = "";



                                if (result.Properties.Contains("employeeType"))
                                {
                                    dr["employeeType"] = Convert.ToString(result.Properties["employeeType"][0]);
                                }
                                else
                                    dr["employeeType"] = "";


                                if (result.Properties.Contains("employeeNumber"))
                                {
                                    dr["employeeNumber"] = Convert.ToString(result.Properties["employeeNumber"][0]);
                                }
                                else
                                    dr["employeeNumber"] = "";


                                if (result.Properties.Contains("division"))
                                {
                                    dr["division"] = Convert.ToString(result.Properties["division"][0]);
                                }
                                else
                                    dr["division"] = "";

                                if (result.Properties.Contains("displayNamePrintable"))
                                {
                                    dr["displayNamePrintable"] = Convert.ToString(result.Properties["displayNamePrintable"][0]);
                                }
                                else
                                    dr["displayNamePrintable"] = "";

                            }
                            catch (Exception)
                            {

                            }
                            AdData.Rows.Add(dr);

                        }
                        SaveAdData(AdData);

                    }
                }
            }

        }

        public static DirectoryEntry createDirectoryEntry(String LdapConn)
        {
            // create and return new LDAP connection with desired settings  
            String LdapUser = "raymond\\MossAdmin";
            String LdapUserPwd = "malumnahi";
            DirectoryEntry ldapConnection = new DirectoryEntry(LdapConn, LdapUser, LdapUserPwd);
            ldapConnection.AuthenticationType = AuthenticationTypes.Secure;
            return ldapConnection;
        }




        public static void SaveAdData(DataTable dt)
        {
            try
            {
                string strConn = "Data Source=10.100.100.170;Initial Catalog=PRISM; User Id=xiuser; password=xiuser@123";
                using (SqlConnection SQLConn = new SqlConnection(strConn))
                {
                    SQLConn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = SQLConn;
                    cmd.Parameters.Add("@AdMasterData_TT", SqlDbType.Structured).Value = dt;
                    cmd.CommandText = "Job_UpdateAdMasterData";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();
                    SQLConn.Close();
                    SQLConn.Dispose();
                }
            }
            catch (Exception)
            { }
            finally
            { }
        }


        public class AdUser
        {
            public string SamAccountName { get; set; }
            public string UserPrincipalName { get; set; }
            public string Name { get; set; }
            public string GivenName { get; set; }
            public string SurName { get; set; }
            public string Description { get; set; }
            public string MobilePhone { get; set; }
            public string EmailAddress { get; set; }
            public string Title { get; set; }
            public string Department { get; set; }
            public string Company { get; set; }
            public string Office { get; set; }
            public string manager { get; set; }
            public string employeeID { get; set; }
            public string employeeType { get; set; }
            public string employeeNumber { get; set; }
            public string displayNamePrintable { get; set; }

        }
    }
}
