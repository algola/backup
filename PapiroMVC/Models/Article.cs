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
    public abstract partial class Article
    {
        public Article()
        {
            this.ArticleCosts = new HashSet<ArticleCost>();
        }
    
    	
    	[DataMember] 		
        public System.DateTime TimeStampTable { get; set; }
    	
    	[DataMember] 		
        public string CodArticle { get; set; }
    	
    	[DataMember] 		
        public string ArticleName { get; set; }
    	
    	[DataMember] 		
        public string CodSupplierBuy { get; set; }
    	
    	[DataMember] 		
        public string UnitOfMeasure { get; set; }
    	
    	[DataMember] 		
        public string CodSupplierMaker { get; set; }
    	
    	[DataMember] 		
        public Nullable<int> Selector { get; set; }
    
    	[DataMember]
        public virtual ICollection<ArticleCost> ArticleCosts { get; set; }
    	[DataMember]
        public virtual CustomerSupplier CustomerSupplierBuy { get; set; }
    	[DataMember]
        public virtual CustomerSupplier CustomerSupplierMaker { get; set; }
    }
}
