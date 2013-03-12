using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;

namespace PapiroMVC.Models
{
    public partial class CustomerSupplier_MetaData
    {
        public System.DateTime TimeStampTable { get; set; }

        //soluzione per gestire il multilingua!!!!

        [DisplayNameLocalized(typeof(ResCustomerSupplier), "CodCustomerSupplier")]
        [Tooltip(typeof(ResCustomerSupplier), "CodCustomerSupplierToolTip")]        
        public string CodCustomerSupplier { get; set; }
        
        [Required(ErrorMessageResourceType=typeof(ResCustomerSupplier),ErrorMessageResourceName="RequiredField")]
        [DisplayNameLocalized(typeof(ResCustomerSupplier),"BusinessName")]
        [Tooltip(typeof(ResCustomerSupplier),"BusinessNameToolTip")]
        public string BusinessName { get; set; }

        [DisplayNameLocalized(typeof(ResCustomerSupplier),"VatNumber")]
        [Tooltip(typeof(ResCustomerSupplier), "VatNumberToolTip")]
        public string VatNumber { get; set; }

        [DisplayNameLocalized(typeof(ResCustomerSupplier),"TaxCode")]
        [Tooltip(typeof(ResCustomerSupplier), "TaxCodeToolTip")]
        public string TaxCode { get; set; }
        
        [DisplayNameLocalized(typeof(ResCustomerSupplier),"Outdated")]
        [Tooltip(typeof(ResCustomerSupplier), "OutdatedToolTip")]
        public Nullable<bool> Outdated { get; set; }
    }

}