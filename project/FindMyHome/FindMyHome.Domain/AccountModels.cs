using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace FindMyHome.Domain
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class RegisterExternalLoginModel
    {
        [Required(
			ErrorMessageResourceType = typeof(Properties.Resources),
			ErrorMessageResourceName = "RequiredInputSwe")]
		[Display(ResourceType = typeof(Properties.Resources), Name = "UsernameInputSwe")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
		[Required(
			ErrorMessageResourceType = typeof(Properties.Resources),
			ErrorMessageResourceName = "RequiredInputSwe")]
        [DataType(DataType.Password)]
		[Display(ResourceType = typeof(Properties.Resources), Name = "CurrentPasswordInputSwe")]
        public string OldPassword { get; set; }

		[Required(
			ErrorMessageResourceType = typeof(Properties.Resources),
			ErrorMessageResourceName = "RequiredInputSwe")]
        [StringLength(100, 
			ErrorMessageResourceType = typeof(Properties.Resources),
			ErrorMessageResourceName = "PasswordToShortSwe",
			MinimumLength = 6)]
        [DataType(DataType.Password)]
		[Display(ResourceType = typeof(Properties.Resources), Name="NewPasswordInputSwe")]
        public string NewPassword { get; set; }
		
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Properties.Resources), Name="ConfirmNewPasswordInputSwe")]
        [Compare("NewPassword",
			ErrorMessageResourceType = typeof(Properties.Resources),
			ErrorMessageResourceName = "PasswordDontMatchSwe")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
		[Required(
			ErrorMessageResourceType = typeof(Properties.Resources),
			ErrorMessageResourceName = "RequiredInputSwe")]
		[Display(ResourceType = typeof(Properties.Resources), Name = "UsernameInputSwe")]
        public string UserName { get; set; }
		
		[Required(
			ErrorMessageResourceType = typeof(Properties.Resources),
			ErrorMessageResourceName = "RequiredInputSwe")]
        [DataType(DataType.Password)]
		[Display(ResourceType = typeof(Properties.Resources), Name = "PasswordInputSwe")]
        public string Password { get; set; }

		[Display(ResourceType = typeof(Properties.Resources), Name = "RememberMeInputSwe")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
		[Required(
			ErrorMessageResourceType = typeof(Properties.Resources),
			ErrorMessageResourceName = "RequiredInputSwe")]
		[Display(ResourceType = typeof(Properties.Resources), Name = "UsernameInputSwe")]
        public string UserName { get; set; }
		
		[Required(
			ErrorMessageResourceType = typeof(Properties.Resources),
			ErrorMessageResourceName = "RequiredInputSwe")]
        [StringLength(100, 
			ErrorMessageResourceType = typeof(Properties.Resources),
			ErrorMessageResourceName = "InputLengthErrorSwe",
			MinimumLength = 6)]
        [DataType(DataType.Password)]
		[Display(ResourceType = typeof(Properties.Resources), Name = "PasswordInputSwe")]
        public string Password { get; set; }
		
        [DataType(DataType.Password)]
		[Display(ResourceType = typeof(Properties.Resources), Name = "ConfirmPasswordInputSwe")]
        [Compare("Password", 
			ErrorMessageResourceType = typeof(Properties.Resources),
			ErrorMessageResourceName = "PasswordsDontMatchSwe")]
        public string ConfirmPassword { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
