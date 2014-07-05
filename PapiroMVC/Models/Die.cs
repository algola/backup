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
    public partial class Die : NoPrintable
    {
    	
    	[DataMember] 		
        public string PrintingFormat { get; set; }
    	
    	[DataMember] 		
        public Nullable<double> Width { get; set; }
    	
    	[DataMember] 		
        public Nullable<double> Z { get; set; }
    	
    	[DataMember] 		
        public Nullable<double> DCut2 { get; set; }
    	
    	[DataMember] 		
        public Nullable<double> DCut1 { get; set; }
    	
    	[DataMember] 		
        public Nullable<int> MaxGain1 { get; set; }
    	
    	[DataMember] 		
        public Nullable<int> MaxGain2 { get; set; }
    	
    	[DataMember] 		
        public string Description { get; set; }
    	
    	[DataMember] 		
        public Nullable<int> FormatType { get; set; }
    	
    	[DataMember] 		
        public string CodDie { get; set; }
    	
    	[DataMember] 		
        public string Format { get; set; }
    }
}