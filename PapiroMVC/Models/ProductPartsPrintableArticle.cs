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
    public abstract partial class ProductPartsPrintableArticle
    {
        public ProductPartsPrintableArticle()
        {
            this.Costs = new HashSet<Cost>();
        }
    
    	
    	[DataMember] 		
        public System.DateTime TimeStampTable { get; set; }
    	
    	[DataMember] 		
        public string CodProductPartPrintableArticle { get; set; }
    	
    	[DataMember] 		
        public string CodProductPart { get; set; }
    	
    	[DataMember] 		
        public string ProductPartPrintableArticleName { get; set; }
    	
    	[DataMember] 		
        public string TypeOfMaterial { get; set; }
    	
    	[DataMember] 		
        public string NameOfMaterial { get; set; }
    	
    	[DataMember] 		
        public string Color { get; set; }
    	
    	[DataMember] 		
        public Nullable<long> Weight { get; set; }
    	
    	[DataMember] 		
        public string Adhesive { get; set; }
    
    	[DataMember]
        public virtual ProductPart ProductPart { get; set; }
    	[DataMember]
        public virtual ICollection<Cost> Costs { get; set; }
    }
}
