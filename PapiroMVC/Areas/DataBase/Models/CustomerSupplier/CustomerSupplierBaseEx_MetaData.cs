using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;

namespace PapiroMVC.Models
{
    public partial class CustomerSupplierBase_MetaData
    {
        public System.DateTime TimeStampTable { get; set; }
        public string CodCustomerSupplier { get; set; }
        public string CodCustomerSupplierBase { get; set; }
        [DisplayNameLocalized(typeof(ResCustomerSupplierBase), "CodTypeOfBaseDropDown")]
        public string CodTypeOfBase { get; set; }
        [Required(ErrorMessageResourceType=typeof(ResCustomerSupplierBase),ErrorMessageResourceName="RequiredField"), 
        DisplayNameLocalized(typeof(ResCustomerSupplierBase),"Address")]
        public string Address { get; set; }
        [Required(ErrorMessageResourceType=typeof(ResCustomerSupplierBase),ErrorMessageResourceName="RequiredField"), 
        DisplayNameLocalized(typeof(ResCustomerSupplierBase),"City")]
        public string City { get; set; }
        [DisplayNameLocalized(typeof(ResCustomerSupplierBase),"PostalCode")]
        public string PostalCode { get; set; }
        [DisplayNameLocalized(typeof(ResCustomerSupplierBase),"Country")]
        public string Country { get; set; }
        [DisplayNameLocalized(typeof(ResCustomerSupplierBase),"Phone")]
        public string Phone { get; set; }
        [DisplayNameLocalized(typeof(ResCustomerSupplierBase),"Fax")]
        public string Fax { get; set; }
        [DisplayNameLocalized(typeof(ResCustomerSupplierBase),"Email")]
        public string Email { get; set; }
        [DisplayNameLocalized(typeof(ResCustomerSupplierBase),"Note")]
        public string Note { get; set; }
        [DisplayNameLocalized(typeof(ResCustomerSupplierBase),"Referee")]
        public string Referee { get; set; }
        [DisplayNameLocalized(typeof(ResCustomerSupplierBase),"Pec")]
        public string Pec { get; set; }
        [DisplayNameLocalized(typeof(ResCustomerSupplierBase),"Province")]
        public string Province { get; set; }
    }
}