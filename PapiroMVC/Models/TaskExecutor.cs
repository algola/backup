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
    
    public abstract partial class TaskExecutor
    {
        public TaskExecutor()
        {
            this.SetTaskExecutorEstimatedOn = new HashSet<TaskEstimatedOn>();
        }
    
        public System.DateTime TimeStampTable { get; set; }
        public string CodTaskExecutor { get; set; }
        public string TaskExecutorName { get; set; }
        public string Version { get; set; }
        public Nullable<bool> Dismissed { get; set; }
        public string FormatMin { get; set; }
        public string FormatMax { get; set; }
        public Nullable<long> WeightMin { get; set; }
        public Nullable<long> WeightMax { get; set; }
        public Nullable<double> Pinza { get; set; }
        public Nullable<double> ControPinza { get; set; }
        public Nullable<double> Laterale { get; set; }
        public Nullable<bool> IsEstimatedOnTime { get; set; }
        public Nullable<bool> IsEstimatedOnRun { get; set; }
        public Nullable<bool> IsEstimatedOnMq { get; set; }
    
        public virtual ICollection<TaskEstimatedOn> SetTaskExecutorEstimatedOn { get; set; }
    }
}
