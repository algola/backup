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
    
    public partial class Order
    {
        public Order()
        {
            this.OrderRows = new HashSet<OrderRow>();
            this.orderrows = new HashSet<orderrow1>();
        }
    
        public System.DateTime TimeStampTable { get; set; }
        public string CodOrder { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public Nullable<int> Status { get; set; }
        public string Price { get; set; }
        public Nullable<double> Discount { get; set; }
        public string Total { get; set; }
    
        public virtual ICollection<OrderRow> OrderRows { get; set; }
        public virtual Profile Profile { get; set; }
        public virtual ICollection<orderrow1> orderrows { get; set; }
    }
}
