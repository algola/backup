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
    public abstract partial class Digital : PrinterMachine
    {
    	
    	[DataMember] 		
        public Nullable<bool> BWSide1 { get; set; }
    	
    	[DataMember] 		
        public Nullable<bool> BWSide2 { get; set; }
    	
    	[DataMember] 		
        public Nullable<bool> ColorSide1 { get; set; }
    	
    	[DataMember] 		
        public Nullable<bool> ColorSide2 { get; set; }
    	
    	[DataMember] 		
        public Nullable<long> ProofSheetFirstStart { get; set; }
    	
    	[DataMember] 		
        public Nullable<long> ProofSheetSecondsStart { get; set; }
    	
    	[DataMember] 		
        public Nullable<double> ProductionWaste { get; set; }
    }
}