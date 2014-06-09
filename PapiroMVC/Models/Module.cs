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
    
    public partial class Module
    {
        public Module()
        {
            this.OrderRows = new HashSet<OrderRow>();
        }
    
        public System.DateTime TimeStampTable { get; set; }
        public string CodModuleName { get; set; }
        public string Name { get; set; }
        public string CodModule { get; set; }
        public Nullable<System.DateTime> ActivationDate { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public string MontlyPrice { get; set; }
        public Nullable<double> Discount { get; set; }
        public Nullable<int> Status { get; set; }
    
        public virtual Profile Profile { get; set; }
        public virtual ICollection<OrderRow> OrderRows { get; set; }
    }
}