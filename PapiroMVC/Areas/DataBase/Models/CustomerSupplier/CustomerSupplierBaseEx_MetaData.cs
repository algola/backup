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
        [DisplayNameLocalized(typeof(Strings),"CodTypeOfBaseDropDown")]
        public string CodTypeOfBase { get; set; }
        [Required(ErrorMessageResourceType=typeof(Strings),ErrorMessageResourceName="RequiredField"), DisplayNameLocalized(typeof(Strings),"Address")]
        public string Address { get; set; }
        [Required(ErrorMessageResourceType=typeof(Strings),ErrorMessageResourceName="RequiredField"), DisplayNameLocalized(typeof(Strings),"City")]
        public string City { get; set; }
        [DisplayNameLocalized(typeof(Strings),"PostalCode")]
        public string PostalCode { get; set; }
        [DisplayNameLocalized(typeof(Strings),"Country")]
        public string Country { get; set; }
        [DisplayNameLocalized(typeof(Strings),"Phone")]
        public string Phone { get; set; }
        [DisplayNameLocalized(typeof(Strings),"Fax")]
        public string Fax { get; set; }
        [DisplayNameLocalized(typeof(Strings),"Email")]
        public string Email { get; set; }
        [DisplayNameLocalized(typeof(Strings),"Note")]
        public string Note { get; set; }
        [DisplayNameLocalized(typeof(Strings),"Referee")]
        public string Referee { get; set; }
        [DisplayNameLocalized(typeof(Strings),"Pec")]
        public string Pec { get; set; }
        [DisplayNameLocalized(typeof(Strings),"Province")]
        public string Province { get; set; }
    }
}