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
    using System.Runtime.Serialization;
    
    
    	
    [DataContract] 
    public abstract partial class Step
    {
    	
    	[DataMember] 		
        public System.DateTime TimeStampTable { get; set; }
    	
    	[DataMember] 		
        public string CodTaskEstimatedOn { get; set; }
    	
    	[DataMember] 		
        public long IdStep { get; set; }
    	
    	[DataMember] 		
        public Nullable<double> FromUnit { get; set; }
    	
    	[DataMember] 		
        public Nullable<double> ToUnit { get; set; }
    	
    	[DataMember] 		
        public Nullable<int> SelectorCostBW { get; set; }
    
    	[DataMember]
        public virtual TaskEstimatedOn taskexecutorestimatedon { get; set; }
    }
}