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
    
    public partial class ProductTask
    {
        public ProductTask()
        {
            this.costs = new HashSet<Cost>();
            this.productpartstoproducttasks = new HashSet<productpartstoproducttask>();
        }
    
        public System.DateTime TimeStampTable { get; set; }
        public string CodProductTask { get; set; }
        public string CodProduct { get; set; }
        public string ProductTaskName { get; set; }
        public Nullable<int> SelectorProductTask { get; set; }
        public string CodOptionTypeOfTask { get; set; }
        public Nullable<bool> Hidden { get; set; }
        public Nullable<int> IndexOf { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual OptionTypeOfTask OptionTypeOfTask { get; set; }
        public virtual ICollection<Cost> costs { get; set; }
        public virtual ICollection<productpartstoproducttask> productpartstoproducttasks { get; set; }
    }
}