using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;

namespace PapiroMVC.Models
{
    public partial class CustomerSupplierEx_MetaData
    {
        public System.DateTime TimeStampTable { get; set; }

        //soluzione per gestire il multilingua!!!!
//        [Required(ErrorMessageResourceType=typeof(Strings),ErrorMessageResourceName="RequiredField")]
        
        [DisplayNameLocalized(typeof(Strings),"CodCustomerSupplier")]
        [Tooltip(typeof(Strings), "CodCustomerSupplierToolTip")]        
        public string CodCustomerSupplier { get; set; }
        
        [Required(ErrorMessageResourceType=typeof(Strings),ErrorMessageResourceName="RequiredField")]
        [DisplayNameLocalized(typeof(Strings),"BusinessName")]
        [Tooltip(typeof(Strings),"BusinessNameToolTip")]
        public string BusinessName { get; set; }

        [DisplayNameLocalized(typeof(Strings),"VatNumber")]
        [Tooltip(typeof(Strings), "VatNumberToolTip")]
        public string VatNumber { get; set; }

        [DisplayNameLocalized(typeof(Strings),"TaxCode")]
        [Tooltip(typeof(Strings), "TaxCodeToolTip")]
        public string TaxCode { get; set; }
        
        [DisplayNameLocalized(typeof(Strings),"Outdated")]
        [Tooltip(typeof(Strings), "OutdatedToolTip")]
        public Nullable<bool> Outdated { get; set; }
    }

}