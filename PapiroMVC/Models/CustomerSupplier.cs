//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PapiroMVC.Models
{
    using System;
    using System.Collections.Generic;
    
    public abstract partial class CustomerSupplier
    {
        public CustomerSupplier()
        {
            this.CustomerSupplierBases = new HashSet<CustomerSupplierBase>();
            this.articles = new HashSet<Article>();
            this.articles1 = new HashSet<Article>();
        }
    
        public System.DateTime TimeStampTable { get; set; }
        public string CodCustomerSupplier { get; set; }
        public string BusinessName { get; set; }
        public string VatNumber { get; set; }
        public string TaxCode { get; set; }
        public Nullable<bool> Outdated { get; set; }
    
        public virtual ICollection<CustomerSupplierBase> CustomerSupplierBases { get; set; }
        public virtual ICollection<Article> articles { get; set; }
        public virtual ICollection<Article> articles1 { get; set; }
    }
}
