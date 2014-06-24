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
    public partial class DocumentProduct
    {
        public DocumentProduct()
        {
            this.Costs = new HashSet<Cost>();
            this.Orders = new HashSet<Document>();
        }
    
    	
    	[DataMember] 		
        public System.DateTime TimeStampTable { get; set; }
    	
    	[DataMember] 		
        public string CodDocumentProduct { get; set; }
    	
    	[DataMember] 		
        public string CodDocument { get; set; }
    	
    	[DataMember] 		
        public string ProductName { get; set; }
    	
    	[DataMember] 		
        public string CodProduct { get; set; }
    	
    	[DataMember] 		
        public Nullable<int> Quantity { get; set; }
    	
    	[DataMember] 		
        public string UnitPrice { get; set; }
    	
    	[DataMember] 		
        public string TotalAmount { get; set; }
    
    	[DataMember]
        public virtual ICollection<Cost> Costs { get; set; }
    	[DataMember]
        public virtual Document Document { get; set; }
    	[DataMember]
        public virtual Product Product { get; set; }
    	[DataMember]
        public virtual ICollection<Document> Orders { get; set; }
    }
}
