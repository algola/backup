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
    public partial class DocumentTaskCenter
    {
    	
    	[DataMember] 		
        public System.DateTime TimeStampTable { get; set; }
    	
    	[DataMember] 		
        public string CodDocumentTaskCenter { get; set; }
    	
    	[DataMember] 		
        public string CodTaskCenter { get; set; }
    	
    	[DataMember] 		
        public string CodDocument { get; set; }
    	
    	[DataMember] 		
        public string DocumentName { get; set; }
    	
    	[DataMember] 		
        public Nullable<int> IndexOf { get; set; }
    
    	[DataMember]
        public virtual Document document { get; set; }
    	[DataMember]
        public virtual TaskCenter taskcenter { get; set; }
    }
}