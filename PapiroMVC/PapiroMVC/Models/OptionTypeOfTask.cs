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
    
    public partial class OptionTypeOfTask
    {
        public OptionTypeOfTask()
        {
            this.ProductTasks = new HashSet<ProductTask>();
            this.ProductPartTasks = new HashSet<ProductPartTask>();
            this.TaskExecutorEstimatedOns = new HashSet<TaskEstimatedOn>();
        }
    
        public System.DateTime TimeStampTable { get; set; }
        public string CodOptionTypeOfTask { get; set; }
        public string CodTypeOfTask { get; set; }
        public string OptionName { get; set; }
        public Nullable<int> IdexOf { get; set; }
    
        public virtual TypeOfTask TypeOfTask { get; set; }
        public virtual ICollection<ProductTask> ProductTasks { get; set; }
        public virtual ICollection<ProductPartTask> ProductPartTasks { get; set; }
        public virtual ICollection<TaskEstimatedOn> TaskExecutorEstimatedOns { get; set; }
    }
}