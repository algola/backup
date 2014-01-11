using PapiroMVC.Models.Resources.Account;
using PapiroMVC.Validation;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Profile_MetaData
    {
        [Tooltip(typeof(Registration), "OrganizationName")]
        [Required(ErrorMessageResourceType = typeof(Registration), ErrorMessageResourceName = "RequiredFieldOrganizationName")]
        [DisplayNameLocalized(typeof(Registration), "OrganizationName")]
        public string CompanyName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Registration), ErrorMessageResourceName = "RequiredFieldVatNumber")]
        [DisplayNameLocalized(typeof(Registration), "VatNumber")]
        public string VatNumber { get; set; }

        //        [Required(ErrorMessageResourceType = typeof(Registration), ErrorMessageResourceName = "RequiredFieldTaxCode")]
        [DisplayNameLocalized(typeof(Registration), "TaxCode")]
        public string TaxCode { get; set; }

        //        [Required(ErrorMessageResourceType = typeof(Registration), ErrorMessageResourceName = "RequiredFieldBase")]
        [DisplayNameLocalized(typeof(Registration), "Base")]
        public string Base { get; set; }

        //        [Required(ErrorMessageResourceType = typeof(Registration), ErrorMessageResourceName = "RequiredFieldRefeere")]
        [DisplayNameLocalized(typeof(Registration), "Refeere")]
        public string Refeere { get; set; }

        //        [Required(ErrorMessageResourceType = typeof(Registration), ErrorMessageResourceName = "RequiredFieldPhone")]
        [DisplayNameLocalized(typeof(Registration), "Phone")]
        public string Phone { get; set; }

        [DisplayNameLocalized(typeof(Registration), "Culture")]
        public string Culture { get; set; }


    }
}