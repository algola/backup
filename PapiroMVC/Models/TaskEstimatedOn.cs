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
    
    public abstract partial class TaskEstimatedOn
    {
        public TaskEstimatedOn()
        {
            this.steps = new HashSet<Step>();
        }
    
        public System.DateTime TimeStampTable { get; set; }
        public string CodTaskExecutor { get; set; }
        public string CodPrinterEstimatedOn { get; set; }
        public Nullable<double> CostForfait { get; set; }
        public Nullable<double> CostForfaitForSheet { get; set; }
    
        public virtual TaskExecutor printermachines { get; set; }
        public virtual ICollection<Step> steps { get; set; }
    }
}
