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
    public partial class CustomerSupplierBase
    {
    	
    	[DataMember] 		
        public System.DateTime TimeStampTable { get; set; }
    	
    	[DataMember] 		
        public string CodCustomerSupplier { get; set; }
    	
    	[DataMember] 		
        public string CodCustomerSupplierBase { get; set; }
    	
    	[DataMember] 		
        public string CodTypeOfBase { get; set; }
    	
    	[DataMember] 		
        public string Address { get; set; }
    	
    	[DataMember] 		
        public string City { get; set; }
    	
    	[DataMember] 		
        public string PostalCode { get; set; }
    	
    	[DataMember] 		
        public string Country { get; set; }
    	
    	[DataMember] 		
        public string Phone { get; set; }
    	
    	[DataMember] 		
        public string Fax { get; set; }
    	
    	[DataMember] 		
        public string Email { get; set; }
    	
    	[DataMember] 		
        public string Note { get; set; }
    	
    	[DataMember] 		
        public string Referee { get; set; }
    	
    	[DataMember] 		
        public string Pec { get; set; }
    	
    	[DataMember] 		
        public string Province { get; set; }
    
    	[DataMember]
        public virtual CustomerSupplier CustomerSupplier { get; set; }
    	[DataMember]
        public virtual TypeOfBase TypeOfBase { get; set; }
    }
}