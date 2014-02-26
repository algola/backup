using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Security;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Account;

namespace PapiroMVC.Models
{

    public class RecoveryPasswordModel
    {
        [Required(ErrorMessageResourceType = typeof(PapiroMVC.Models.Resources.Account.Registration), ErrorMessageResourceName = "RequiredFieldEmail")]
        [DisplayNameLocalized(typeof(PapiroMVC.Models.Resources.Account.Registration), "Email")]
        [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(PapiroMVC.Models.Resources.Account.Registration), ErrorMessageResourceName = "ValidationEmail")]
        public string Email { get; set; }
    }

    public class ChangePasswordModel
    {
        [Required(ErrorMessageResourceType = typeof(ResChangePasswordModel), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Password)]
        [DisplayNameLocalized(typeof(ResChangePasswordModel), "OldPassword")]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(ResChangePasswordModel), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceType = typeof(ResChangePasswordModel), ErrorMessageResourceName = "ValidationPassword", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [DisplayNameLocalized(typeof(ResChangePasswordModel), "NewPassword")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [DisplayNameLocalized(typeof(ResChangePasswordModel), "ConfirmPassword")]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(ResChangePasswordModel), ErrorMessageResourceName = "ValidationConfirmPassword")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required(ErrorMessageResourceType = typeof(ResLoginModel), ErrorMessageResourceName = "UserNameRequiredField")]
        [DisplayNameLocalized(typeof(ResLoginModel),  "UserName")]
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
        [Required(ErrorMessageResourceType = typeof(PapiroMVC.Models.Resources.Account.Registration), ErrorMessageResourceName = "RequiredFieldUserName")]
        [RegularExpressionLocalizedAttribute(typeof(PapiroMVC.Models.Resources.Account.Registration), "UserNameRegex", "UserNameFormatValidation")]        
        [DisplayNameLocalized(typeof(PapiroMVC.Models.Resources.Account.Registration), "UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(PapiroMVC.Models.Resources.Account.Registration), ErrorMessageResourceName = "RequiredFieldEmail")]
        [DisplayNameLocalized(typeof(PapiroMVC.Models.Resources.Account.Registration), "Email")]
        [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(PapiroMVC.Models.Resources.Account.Registration), ErrorMessageResourceName = "ValidationEmail")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(PapiroMVC.Models.Resources.Account.Registration), ErrorMessageResourceName = "RequiredPassword")]
        [StringLength(100, ErrorMessageResourceType = typeof(PapiroMVC.Models.Resources.Account.Registration), ErrorMessageResourceName = "ValidationPassword", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [DisplayNameLocalized(typeof(PapiroMVC.Models.Resources.Account.Registration), "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [DisplayNameLocalized(typeof(PapiroMVC.Models.Resources.Account.Registration), "ConfirmPassword")]
        [Compare("Password", ErrorMessageResourceType = typeof(PapiroMVC.Models.Resources.Account.Registration), ErrorMessageResourceName = "ValidationConfirmPassword")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(PapiroMVC.Models.Resources.Account.Registration), ErrorMessageResourceName = "RequiredFieldOrganizationName")]
        [DisplayNameLocalized(typeof(PapiroMVC.Models.Resources.Account.Registration), "OrganizationName")]
        public string OrganizationName { get; set; }

        [Required(ErrorMessageResourceType = typeof(PapiroMVC.Models.Resources.Account.Registration), ErrorMessageResourceName = "RequiredFieldVatNumber")]
        [DisplayNameLocalized(typeof(PapiroMVC.Models.Resources.Account.Registration), "VatNumber")]
        public string VatNumber { get; set; }

//        [Required(ErrorMessageResourceType = typeof(PapiroMVC.Models.Resources.Account.Registration), ErrorMessageResourceName = "RequiredFieldTaxCode")]
        [DisplayNameLocalized(typeof(PapiroMVC.Models.Resources.Account.Registration), "TaxCode")]
        public string TaxCode { get; set; }

//        [Required(ErrorMessageResourceType = typeof(PapiroMVC.Models.Resources.Account.Registration), ErrorMessageResourceName = "RequiredFieldBase")]
        [DisplayNameLocalized(typeof(PapiroMVC.Models.Resources.Account.Registration), "Base")]
        public string Base { get; set; }

        //        [Required(ErrorMessageResourceType = typeof(PapiroMVC.Models.Resources.Account.Registration), ErrorMessageResourceName = "RequiredFieldRefeere")]
        [DisplayNameLocalized(typeof(PapiroMVC.Models.Resources.Account.Registration), "Refeere")]
        public string Refeere { get; set; }

        //        [Required(ErrorMessageResourceType = typeof(PapiroMVC.Models.Resources.Account.Registration), ErrorMessageResourceName = "RequiredFieldPhone")]
        [DisplayNameLocalized(typeof(PapiroMVC.Models.Resources.Account.Registration), "Phone")]
        public string Phone { get; set; }

        [DisplayNameLocalized(typeof(PapiroMVC.Models.Resources.Account.Registration), "Culture")]
        public string Culture { get; set; }


    }
}
