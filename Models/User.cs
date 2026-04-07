using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RAFE.Models
{
    #region User Model
    public class UserModel
    {
        public List<UserModel> UserList { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string SamAccountName { get; set; }
        public string Description { get; set; }
        public string EmailAddress { get; set; }
        public string Company { get; set; }
        public string Title { get; set; }
        public string Department { get; set; }
        public string City { get; set; }
        public string office { get; set; }
        public string OfficePhone { get; set; }
        public string manager { get; set; }
        public string employeeID { get; set; }
        public string employeeType { get; set; }
        public string employeeNumber { get; set; }
        public string division { get; set; }
        public string displayNamePrintable { get; set; }
        public string RoleName { get; set; }
        public string RoleID { get; set; }
        public string Designation_Title { get; set; }
        public string HOD { get; set; }
        public string Group_Name { get; set; }
        public string Sub_Group { get; set; }
        public string Location { get; set; }
        public string Photo { get; set; }
        public string Function { get; set; }
        public int NominationId { get; set; }

       
    }
    #endregion

    #region Login Model
    public class LoginModel
    {
        [Required(ErrorMessage = "Please enter username.")]
        public string userName { get; set; }

        [Required(ErrorMessage = "Please enter password.")]
        public string password { get; set; }
    }
    #endregion
    
    #region Change Password Model
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Please enter your active password.")]
        public string oldPassword { get; set; }

        [Required(ErrorMessage = "Please enter your new password.")]
        public string newPassword { get; set; }

        [Required(ErrorMessage = "Please confirm your new password.")]
        [Compare("newPassword", ErrorMessage = "New password and confirm paswword are not same.")]
        public string confirmPassword { get; set; }
    }
    #endregion
    
    #region Set Password Model
    public class SetPassword
    {
        [Required(ErrorMessage = "Please enter your new password.")]
        public string newPassword { get; set; }
        [Required(ErrorMessage = "Please confirm your new password.")]
        public string confirmPassword { get; set; }
        public int userID { get; set; }
    }
    #endregion
}