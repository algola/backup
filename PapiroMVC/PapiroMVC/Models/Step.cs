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
    
    public abstract partial class Step
    {
        public System.DateTime TimeStampTable { get; set; }
        public string CodTaskEstimatedOn { get; set; }
        public long IdStep { get; set; }
        public Nullable<double> FromUnit { get; set; }
        public Nullable<double> ToUnit { get; set; }
        public Nullable<int> SelectorCostBW { get; set; }
    
        public virtual TaskEstimatedOn taskexecutorestimatedon { get; set; }
    }
}