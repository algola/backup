using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Security;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Account;

namespace PapiroMVC.Models
{

    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required(ErrorMessageResourceType = typeof(ResLoginModel), ErrorMessageResourceName = "UserNameRequiredField")]
        [DisplayNameLocalized(typeof(ResLoginModel), "UserName")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceType = typeof(ResLoginModel), ErrorMessageResourceName = "PasswordRequiredField")]
        [DisplayNameLocalized(typeof(ResLoginModel), "Password")]
        public string Password { get; set; }

        [DisplayNameLocalized(typeof(ResLoginModel), "RememberMe")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required(ErrorMessageResourceType = typeof(Registration), ErrorMessageResourceName = "RequiredFieldUserName")]
        [DisplayNameLocalized(typeof(Registration), "UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Registration), ErrorMessageResourceName = "RequiredFieldEmail")]
        [DisplayNameLocalized(typeof(Registration), "Email")]
        [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(Registration), ErrorMessageResourceName = "ValidationEmail")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Registration), ErrorMessageResourceName = "RequiredPassword")]
        [StringLength(100, ErrorMessageResourceType = typeof(Registration), ErrorMessageResourceName = "ValidationPassword", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [DisplayNameLocalized(typeof(Registration), "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [DisplayNameLocalized(typeof(Registration), "ConfirmPassword")]
        [Compare("Password", ErrorMessageResourceType = typeof(Registration), ErrorMessageResourceName = "ValidationConfirmPassword")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Registration), ErrorMessageResourceName = "RequiredFieldOrganizationName")]
        [DisplayNameLocalized(typeof(Registration), "OrganizationName")]
        public string OrganizationName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Registration), ErrorMessageResourceName = "RequiredFieldVatNumber")]
        [DisplayNameLocalized(typeof(Registration), "VatNumber")]
        public string VatNumber { get; set; }

        [Required(ErrorMessageResourceType = typeof(Registration), ErrorMessageResourceName = "RequiredFieldTaxCode")]
        [DisplayNameLocalized(typeof(Registration), "TaxCode")]
        public string TaxCode { get; set; }
        
    }
}
