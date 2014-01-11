//using BWhereMvc.Models.Resources.Account;
//using BWhereMvc.Validation;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Data.Entity;
//using System.Globalization;
//using System.Web.Security;

//namespace PapiroMVC.Models
//{
//    public class UsersContext : DbContext
//    {
//        public UsersContext()
//            : base("DefaultConnection")
//        {
//        }

//        public DbSet<UserProfile> UserProfiles { get; set; }
//    }

//    [Table("UserProfile")]
//    public class UserProfile
//    {
//        [Key]
//        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
//        public int UserId { get; set; }
//        public string UserName { get; set; }
//    }

//    public class RegisterExternalLoginModel
//    {
//        [Required]
//        [Display(Name = "Nome utente")]
//        public string UserName { get; set; }

//        public string ExternalLoginData { get; set; }
//    }

//    public class LocalPasswordModel
//    {
//        [Required]
//        [DataType(DataType.Password)]
//        [Display(Name = "Password corrente")]
//        public string OldPassword { get; set; }

//        [Required]
//        [StringLength(100, ErrorMessage = "La lunghezza di {0} deve essere di almeno {2} caratteri.", MinimumLength = 6)]
//        [DataType(DataType.Password)]
//        [Display(Name = "Nuova password")]
//        public string NewPassword { get; set; }

//        [DataType(DataType.Password)]
//        [Display(Name = "Conferma nuova password")]
//        [Compare("NewPassword", ErrorMessage = "La nuova password e la password di conferma non corrispondono.")]
//        public string ConfirmPassword { get; set; }
//    }

//    public class RecoveryPasswordModel
//    {
//        [Required(ErrorMessageResourceType = typeof(Registration), ErrorMessageResourceName = "RequiredFieldEmail")]
//        [DisplayNameLocalized(typeof(Registration), "Email")]
//        [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(Registration), ErrorMessageResourceName = "ValidationEmail")]
//        public string Email { get; set; }
//    }

//    public class ChangePasswordModel
//    {
//        [Required(ErrorMessageResourceType = typeof(ResChangePasswordModel), ErrorMessageResourceName = "Required")]
//        [DataType(DataType.Password)]
//        [DisplayNameLocalized(typeof(ResChangePasswordModel), "OldPassword")]
//        public string OldPassword { get; set; }

//        [Required(ErrorMessageResourceType = typeof(ResChangePasswordModel), ErrorMessageResourceName = "Required")]
//        [StringLength(100, ErrorMessageResourceType = typeof(ResChangePasswordModel), ErrorMessageResourceName = "ValidationPassword", MinimumLength = 6)]
//        [DataType(DataType.Password)]
//        [DisplayNameLocalized(typeof(ResChangePasswordModel), "NewPassword")]
//        public string NewPassword { get; set; }

//        [DataType(DataType.Password)]
//        [DisplayNameLocalized(typeof(ResChangePasswordModel), "ConfirmPassword")]
//        [Compare("NewPassword", ErrorMessageResourceType = typeof(ResChangePasswordModel), ErrorMessageResourceName = "ValidationConfirmPassword")]
//        public string ConfirmPassword { get; set; }
//    }

//    public class LoginModel
//    {
//        [Required(ErrorMessageResourceType = typeof(ResLoginModel), ErrorMessageResourceName = "UserNameRequiredField")]
//        [DisplayNameLocalized(typeof(ResLoginModel), "UserName")]
//        public string UserName { get; set; }

//        [DataType(DataType.Password)]
//        [Required(ErrorMessageResourceType = typeof(ResLoginModel), ErrorMessageResourceName = "PasswordRequiredField")]
//        [DisplayNameLocalized(typeof(ResLoginModel), "Password")]

//        public string Password { get; set; }

//        [DisplayNameLocalized(typeof(ResLoginModel), "RememberMe")]
//        public bool RememberMe { get; set; }
//    }

//    public class RegisterModel
//    {
//        [Required(ErrorMessageResourceType = typeof(Registration), ErrorMessageResourceName = "RequiredFieldUserName")]
//        [DisplayNameLocalized(typeof(Registration), "UserName")]
//        public string UserName { get; set; }

//        [Required(ErrorMessageResourceType = typeof(Registration), ErrorMessageResourceName = "RequiredFieldEmail")]
//        [DisplayNameLocalized(typeof(Registration), "Email")]
//        [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(Registration), ErrorMessageResourceName = "ValidationEmail")]
//        public string Email { get; set; }

//        [Required(ErrorMessageResourceType = typeof(Registration), ErrorMessageResourceName = "RequiredPassword")]
//        [StringLength(100, ErrorMessageResourceType = typeof(Registration), ErrorMessageResourceName = "ValidationPassword", MinimumLength = 6)]
//        [DataType(DataType.Password)]
//        [DisplayNameLocalized(typeof(Registration), "Password")]
//        public string Password { get; set; }

//        [DataType(DataType.Password)]
//        [DisplayNameLocalized(typeof(Registration), "ConfirmPassword")]
//        [Compare("Password", ErrorMessageResourceType = typeof(Registration), ErrorMessageResourceName = "ValidationConfirmPassword")]
//        public string ConfirmPassword { get; set; }
//    }

//    public class ExternalLogin
//    {
//        public string Provider { get; set; }
//        public string ProviderDisplayName { get; set; }
//        public string ProviderUserId { get; set; }
//    }
//}
