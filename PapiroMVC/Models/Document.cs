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
    public partial class Document
    {
        public Document()
        {
            this.DocumentProducts = new HashSet<DocumentProduct>();
        }
    
    	
    	[DataMember] 		
        public System.DateTime TimeStampTable { get; set; }
    	
    	[DataMember] 		
        public string CodDocument { get; set; }
    	
    	[DataMember] 		
        public string DocumentName { get; set; }
    	
    	[DataMember] 		
        public string CodCustomer { get; set; }
    	
    	[DataMember] 		
        public string Customer { get; set; }
    	
    	[DataMember] 		
        public Nullable<int> SelectorDocument { get; set; }
    	
    	[DataMember] 		
        public Nullable<int> Number { get; set; }
    	
    	[DataMember] 		
        public string Notes { get; set; }
    	
    	[DataMember] 		
        public Nullable<System.DateTime> DateDocument { get; set; }
    	
    	[DataMember] 		
        public string EstimateNumber { get; set; }
    	
    	[DataMember] 		
        public string EstimateNumberSerie { get; set; }
    
    	[DataMember]
        public virtual CustomerSupplier CustomerSupplier { get; set; }
    	[DataMember]
        public virtual ICollection<DocumentProduct> DocumentProducts { get; set; }
    }
}