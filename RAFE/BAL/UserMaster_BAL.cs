using RAFE.DAL;
using RAFE.Models;
using RAFE.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace RAFE.BAL
{
    public class UserMaster_BAL
    {
        public ResultMessage UserLogin(LoginModel objLoginModel)
        {
            ResultMessage objResultMessage = new ResultMessage();           
            DataTable dt;
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@userName", SqlDbType.VarChar, 150).Value = objLoginModel.userName;
            command.Parameters.Add("@password", SqlDbType.VarChar, 150).Value = objLoginModel.password;
            command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "LOGIN";
            //string encPassword = clsEncrypt.Encrypt(objLoginModel.userName, objLoginModel.password);
            string encPassword = objLoginModel.password;
            using (dt = SQLHelper.GetDataTable(Constants.sp_user, command))
            {
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["isActive"].ToString().ToLower() == "true" || dt.Rows[0]["isActive"].ToString().ToLower() == "1")
                    {
                        if (dt.Rows[0][1].ToString().ToLower() == objLoginModel.userName.ToLower() && encPassword == dt.Rows[0][2].ToString())
                        {
                            objResultMessage.isSuccess = true;
                            objResultMessage.statusMessage = dt.Rows[0]["UserID"].ToString() + "~" + dt.Rows[0]["FullName"].ToString() + "~" + dt.Rows[0]["UserName"].ToString() + "~" + dt.Rows[0]["Role"].ToString();
                        }
                        else
                        {
                            objResultMessage.isSuccess = false;
                            objResultMessage.statusMessage = "Please enter your valid User-id and Password....!!!!";
                        }
                    }
                    else
                    {
                        objResultMessage.isSuccess = false;
                        objResultMessage.statusMessage = "Entered User-ID is inactive. Kindly contact to administration.";
                    }
                }
                else
                {
                    objResultMessage.isSuccess = false;
                    objResultMessage.statusMessage = "Entered enter User-id not exists....!!!!";
                }
            }           
            return objResultMessage;
        }
        public ResultMessage ChangePassword(ChangePasswordModel objLoginModel, int loginUserID)
        {
            ResultMessage objResultMessage = new ResultMessage();
            try
            {
                if (string.IsNullOrEmpty(objLoginModel.oldPassword))
                {
                    objResultMessage.isSuccess = false;
                    objResultMessage.statusMessage = "Please enter your active password.";
                }
                else if (string.IsNullOrEmpty(objLoginModel.newPassword))
                {
                    objResultMessage.isSuccess = false;
                    objResultMessage.statusMessage = "Please enter your new password.";
                }
                else if (objLoginModel.newPassword != objLoginModel.oldPassword)
                {
                    objResultMessage.isSuccess = false;
                    objResultMessage.statusMessage = "The password and its confirm are not the same.";
                }
                SqlCommand command = new SqlCommand();
                command.Parameters.Add("@password", SqlDbType.VarChar, 150).Value = objLoginModel.newPassword;
                command.Parameters.Add("@userID", SqlDbType.Int).Value = loginUserID;
                command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "GETUSER";
                using (DataTable dt = SQLHelper.GetDataTable(Constants.sp_user, command))
                {
                    if (dt.Rows.Count > 0 && dt.Rows[0]["password"].ToString() == objLoginModel.oldPassword)
                    {
                        command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "CHANGEPASSWORD";
                        if (SQLHelper.ExecuteScalarString(Constants.sp_user, command).Split('|')[0] == "SUCCESS")
                        {
                            objResultMessage.isSuccess = true;
                            objResultMessage.statusMessage = "Password Updated successfully";
                        }
                    }
                    else if (dt.Rows.Count > 0 && dt.Rows[0]["password"].ToString() != objLoginModel.oldPassword)
                    {
                        objResultMessage.isSuccess = false;
                        objResultMessage.statusMessage = "Incorrect old password.";
                    }
                    else
                    {
                        objResultMessage.isSuccess = false;
                        objResultMessage.statusMessage = "User not exists";
                    }
                }
            }
            catch (Exception)
            {
                objResultMessage.isSuccess = false;
                objResultMessage.statusMessage = "Please enter your valid User-id and Password....!!!!";
            }
            return objResultMessage;
        }
        public ResultMessage ForgotPassword(string userName)
        {
            ResultMessage objResultMessage = new ResultMessage();
            try
            {
                if (string.IsNullOrEmpty(userName))
                {
                    objResultMessage.isSuccess = false;
                    objResultMessage.statusMessage = "Please enter your user name.";
                }
                SqlCommand command = new SqlCommand();
                command.Parameters.Add("@userName", SqlDbType.VarChar, 150).Value = userName;
                command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "GETUSERBYUSERNAME";
                using (DataTable dt = SQLHelper.GetDataTable(Constants.sp_user, command))
                {
                    if (dt.Rows.Count > 0)
                    {
                        command.Parameters.Add("@password", SqlDbType.VarChar, 150).Value = WebCommon.RandomString(6);
                        command.Parameters.Add("@userID", SqlDbType.Int).Value = dt.Rows[0]["userID"].ToString();
                        command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "CHANGEPASSWORD";
                        if (SQLHelper.ExecuteScalarString(Constants.sp_user, command).Split('|')[0] == "SUCCESS")
                        {
                            objResultMessage.isSuccess = true;
                            objResultMessage.statusMessage = "A reset password has been sent to your registerd email.";
                        }
                    }
                }
            }
            catch (Exception)
            {
                objResultMessage.isSuccess = false;
                objResultMessage.statusMessage = "Please enter your valid User-id and Password....!!!!";
            }
            return objResultMessage;
        }
        private void ForgotPasswordEmail(string name, string password, string emailId)
        {
            string htmlCode;
            using (System.Net.WebClient client = new System.Net.WebClient()) // WebClient class inherits IDisposable
            {
                // Or you can get the file content without saving it
                htmlCode = client.DownloadString(ConfigurationManager.AppSettings["AbsolutePath"].ToString() + "/Content/EmailTemplate/forgotPassword.html");
            }
            string str = htmlCode.ToString().Replace("##Name##", name).Replace("##Password##", password).Replace("##URL##", ConfigurationManager.AppSettings["AbsolutePath"].ToString());
            EmailLibrary.sendEmail(emailId, "", "Forgot password request", str);
        }
        public ResultMessage GetUserDetailsForLogin(string userName)
        {
           ResultMessage objResultMessage = new ResultMessage();
                if (string.IsNullOrEmpty(userName))
                {
                    objResultMessage.isSuccess = false;
                    objResultMessage.statusMessage = "Please enter your user name.";
                }
                SqlCommand command = new SqlCommand();
                command.Parameters.Add("@userName", SqlDbType.VarChar, 150).Value = userName;
                command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "GETUSERFORLOGIN";
                using (DataTable dt = SQLHelper.GetDataTable(Constants.sp_user, command))
                {
                    if (dt.Rows.Count > 0)
                    {
                        objResultMessage.isSuccess = true;
                        objResultMessage.statusMessage = dt.Rows[0]["SamAccountName"].ToString() + "~" + dt.Rows[0]["Name"].ToString() + "~" + dt.Rows[0]["Role"].ToString() + "~" + dt.Rows[0]["EmailAddress"].ToString() + "~" + dt.Rows[0]["Title"].ToString() + "~" + dt.Rows[0]["Department"].ToString() + "~" + dt.Rows[0]["manager"].ToString() + "~" + dt.Rows[0]["Committee"].ToString();
                    }
                }
            return objResultMessage;
        }
        public ResultMessage GetUserDetailsForDiffLogin(string userName, string  BU_HR)
        {
            ResultMessage objResultMessage = new ResultMessage();
            if (string.IsNullOrEmpty(userName))
            {
                objResultMessage.isSuccess = false;
                objResultMessage.statusMessage = "Please enter your user name.";
            }
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@userName", SqlDbType.VarChar, 150).Value = userName;
            command.Parameters.Add("@BU_HR", SqlDbType.VarChar, 150).Value = BU_HR;
            command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "GETLOGINWITHDIFF";
            using (DataTable dt = SQLHelper.GetDataTable(Constants.sp_user, command))
            {
                if (dt.Rows.Count > 0)
                {
                    objResultMessage.isSuccess = true;
                    objResultMessage.statusMessage = dt.Rows[0]["SamAccountName"].ToString() + "~" + dt.Rows[0]["Name"].ToString() + "~" + dt.Rows[0]["Role"].ToString() + "~" + dt.Rows[0]["EmailAddress"].ToString() + "~" + dt.Rows[0]["Title"].ToString() + "~" + dt.Rows[0]["Department"].ToString() + "~" + dt.Rows[0]["manager"].ToString();
                }
            }
            return objResultMessage;
        }
        public ResultMessage GetUserIsCXO(string userName)
        {
            ResultMessage objResultMessage = new ResultMessage();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@userName", SqlDbType.VarChar, 150).Value = userName;
            command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "IS_USER_CXO";
            using (DataTable dt = SQLHelper.GetDataTable(Constants.sp_user, command))
            {
                if (dt.Rows.Count > 0)
                {
                    objResultMessage.isSuccess = true;
                    objResultMessage.statusMessage = "User is CXO";
                }
            }
            return objResultMessage;
        }
        public UserModel GetuserDetails(string userName)
        {
            UserModel objUserModel;
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@userName", SqlDbType.VarChar, 150).Value = userName;
            command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "GETUSERALLDETAIL";
            using (DataTable dt = SQLHelper.GetDataTable(Constants.sp_user, command))
            {
                if (dt.Rows.Count > 0)
                {
                    objUserModel = new UserModel();
                    objUserModel.Name = dt.Rows[0]["Name"].ToString();
                    //objUserModel.SurName = dt.Rows[0]["SurName"].ToString();
                    objUserModel.SamAccountName = dt.Rows[0]["SamAccountName"].ToString();
                    objUserModel.Description = dt.Rows[0]["Description"].ToString();
                    objUserModel.EmailAddress = dt.Rows[0]["EmailAddress"].ToString();
                    objUserModel.Company = dt.Rows[0]["Company"].ToString();
                    objUserModel.Title = dt.Rows[0]["Title"].ToString();
                    objUserModel.Department = dt.Rows[0]["Department"].ToString();
                    objUserModel.OfficePhone = dt.Rows[0]["OfficePhone"].ToString();
                    //objUserModel.City = dt.Rows[0]["City"].ToString();
                    //objUserModel.office = dt.Rows[0]["office"].ToString();
                    //objUserModel.manager = dt.Rows[0]["manager"].ToString();
                    //objUserModel.employeeID = dt.Rows[0]["employeeID"].ToString();
                    //objUserModel.employeeType = dt.Rows[0]["employeeType"].ToString();
                    //objUserModel.employeeNumber = dt.Rows[0]["employeeNumber"].ToString();
                    //objUserModel.division = dt.Rows[0]["division"].ToString();
                    //objUserModel.displayNamePrintable = dt.Rows[0]["displayNamePrintable"].ToString();
                    //objUserModel.RoleName = dt.Rows[0]["RoleName"].ToString();
                    //objUserModel.RoleID = dt.Rows[0]["RoleID"].ToString();
                    return objUserModel;
                }
            }
            return null;
        }
        public UserModel GetDetailsForProfiler(string email,string otheremail)
        {
            UserModel userProfilerModel;
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@email", SqlDbType.VarChar, 150).Value = email;
            command.Parameters.Add("@otheremail", SqlDbType.VarChar, 150).Value = otheremail;
            //command.Parameters.Add("@nominationid", SqlDbType.Int).Value = nominationId;
            using (DataTable dt = SQLHelper.GetDataTable(Constants.sp_Profiler, command))
            {
                if(dt.Rows.Count > 0)
                {
                    userProfilerModel = new UserModel();
                    userProfilerModel.Photo = dt.Rows[0]["Photo"].ToString();
                    userProfilerModel.Name = dt.Rows[0]["Name"].ToString();
                    userProfilerModel.employeeID = dt.Rows[0]["employeeID"].ToString();
                    userProfilerModel.Designation_Title = dt.Rows[0]["Designation_Title"].ToString();
                    userProfilerModel.EmailAddress = dt.Rows[0]["EmailAddress"].ToString();
                    userProfilerModel.HOD = dt.Rows[0]["HOD"].ToString();
                    userProfilerModel.Group_Name = dt.Rows[0]["Group_Name"].ToString();
                    userProfilerModel.Sub_Group = dt.Rows[0]["Sub_Group"].ToString();
                    userProfilerModel.Function = dt.Rows[0]["Function_Name"].ToString();
                    userProfilerModel.Location = dt.Rows[0]["Location"].ToString();
                    return userProfilerModel;
                }
            }
            return null;
        }
        public List<EmployeeModel> GetAllEmployeeListUnderSupervisor(string userName)
        {
            List<EmployeeModel> EmployeeList = new List<EmployeeModel>();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@userName", SqlDbType.VarChar, 150).Value = userName;
            command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "GETALLEMPLOYEELIST";
            using (DataTable dt = SQLHelper.GetDataTable(Constants.sp_user, command))
            {
                if (dt.Rows.Count > 0)
                {
                    EmployeeList = Utility.WebCommon.DataTableToList<EmployeeModel>(dt);                   
                }
                EmployeeList.Add(new EmployeeModel
                {
                    EmployeeID = "other",
                    EmployeeName = "Other"
                });
                return EmployeeList;
            }
        }
        public List<EmployeeModel> GetAllEmployeeListForAward(string userName, int awardTypeID)
        {
            List<EmployeeModel> EmployeeList = new List<EmployeeModel>();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@userName", SqlDbType.VarChar, 150).Value = userName;
            command.Parameters.Add("@awardTypeID", SqlDbType.Int).Value = awardTypeID;
            command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "GETALLEMPLOYEELIST_For_Award";
            using (DataTable dt = SQLHelper.GetDataTable(Constants.sp_user, command))
            {
                if (dt.Rows.Count > 0)
                {
                    EmployeeList = Utility.WebCommon.DataTableToList<EmployeeModel>(dt);
                }
                EmployeeList.Add(new EmployeeModel
                {
                    EmployeeID = "other",
                    EmployeeName = "Other"
                });
                return EmployeeList;
            }
        }
        public List<EmployeeModel> GetHOD(string userName)
        {
            List<EmployeeModel> HODList = new List<EmployeeModel>();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "GETHOD";
            command.Parameters.Add("@userName", SqlDbType.VarChar, 150).Value = userName;
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_user, command))
            {
                if (dt.Rows.Count > 0)
                {
                    HODList = RAFE.Utility.WebCommon.DataTableToList<EmployeeModel>(dt);
                }
            }
            return HODList;
        }
        public List<EmployeeModel> UserAutoComplete(string prifix, string searchFor)
        {
            List<EmployeeModel> UserListList = new List<EmployeeModel>();
            SqlCommand command = new SqlCommand();
            if (searchFor == "Ref-User")
            {
                command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "ref-autosearch";
            }
            else
            {
                command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "autosearch";
            }
            command.Parameters.Add("@fullName", SqlDbType.VarChar, 150).Value = prifix;
            using (DataTable dt = SQLHelper.GetDataTable(RAFE.Utility.Constants.sp_user, command))
            {
                if (dt.Rows.Count > 0)
                {
                    UserListList = RAFE.Utility.WebCommon.DataTableToList<EmployeeModel>(dt);
                }
            }
            return UserListList;
        }
        public bool ValidUser(string userName)
        {
            List<EmployeeModel> UserListList = new List<EmployeeModel>();
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("@userName", SqlDbType.VarChar, 150).Value = userName;
            return Convert.ToBoolean(SQLHelper.ExecuteScalar(RAFE.Utility.Constants.sp_ValidUser, command));            
        }
    }
}