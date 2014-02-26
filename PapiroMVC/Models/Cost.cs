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
    public partial class Cost
    {
        public Cost()
        {
            this.CostDetails = new HashSet<CostDetail>();
        }
    
    	
    	[DataMember] 		
        public System.DateTime TimeStampTable { get; set; }
    	
    	[DataMember] 		
        public string CodCost { get; set; }
    	
    	[DataMember] 		
        public string CodDocumentProduct { get; set; }
    	
    	[DataMember] 		
        public string CodProductTask { get; set; }
    	
    	[DataMember] 		
        public string CodProductPartTask { get; set; }
    	
    	[DataMember] 		
        public string CodProductPartPrintableArticle { get; set; }
    	
    	[DataMember] 		
        public string Description { get; set; }
    	
    	[DataMember] 		
        public Nullable<double> Quantity { get; set; }
    	
    	[DataMember] 		
        public string UnitCost { get; set; }
    	
    	[DataMember] 		
        public string TotalCost { get; set; }
    	
    	[DataMember] 		
        public Nullable<bool> ForceZero { get; set; }
    	
    	[DataMember] 		
        public Nullable<bool> Hidden { get; set; }
    
    	[DataMember]
        public virtual DocumentProduct DocumentProduct { get; set; }
    	[DataMember]
        public virtual ProductPartsPrintableArticle ProductPartsPrintableArticle { get; set; }
    	[DataMember]
        public virtual ProductPartTask ProductPartTask { get; set; }
    	[DataMember]
        public virtual ProductTask ProductTask { get; set; }
    	[DataMember]
        public virtual ICollection<CostDetail> CostDetails { get; set; }
    }
}