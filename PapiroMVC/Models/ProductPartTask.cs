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
    
    public partial class ProductPartTask
    {
        public ProductPartTask()
        {
            this.Costs = new HashSet<Cost>();
        }
    
        public System.DateTime TimeStampTable { get; set; }
        public string CodProductPartTask { get; set; }
        public string CodProductPart { get; set; }
        public string CodOptionTypeOfTask { get; set; }
        public Nullable<bool> Hidden { get; set; }
        public Nullable<int> IndexOf { get; set; }
    
        public virtual ProductPart ProductPart { get; set; }
        public virtual OptionTypeOfTask OptionTypeOfTask { get; set; }
        public virtual ICollection<Cost> Costs { get; set; }
    }
}
