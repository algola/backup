using PapiroMVC.Models.Resources.Account;
using PapiroMVC.Validation;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Profile_MetaData
    {
        [Tooltip(typeof(PapiroMVC.Models.Resources.Account.Registration), "OrganizationName")]
        [Required(ErrorMessageResourceType = typeof(PapiroMVC.Models.Resources.Account.Registration), ErrorMessageResourceName = "RequiredFieldOrganizationName")]
        [DisplayNameLocalized(typeof(PapiroMVC.Models.Resources.Account.Registration), "OrganizationName")]
        public string CompanyName { get; set; }

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