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
    
    public partial class Profile
    {
        public Profile()
        {
            this.Orders = new HashSet<Order>();
            this.Modules = new HashSet<Module>();
        }
    
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string Base { get; set; }
        public string Refeere { get; set; }
        public string Culture { get; set; }
        public string Phone { get; set; }
        public string VatNumber { get; set; }
        public string TaxCode { get; set; }
        public string test { get; set; }
        public string Target { get; set; }
        public string Last3Dgt { get; set; }
        public string ExpiredM { get; set; }
        public string ExpiredY { get; set; }
        public string BrianTreeCustomerId { get; set; }
        public string BrainTreeToken { get; set; }
    
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Module> Modules { get; set; }
    }
}
