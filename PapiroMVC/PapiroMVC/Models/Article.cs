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
    
    public abstract partial class Article
    {
        public Article()
        {
            this.ArticleCosts = new HashSet<ArticleCost>();
        }
    
        public System.DateTime TimeStampTable { get; set; }
        public string CodArticle { get; set; }
        public string ArticleName { get; set; }
        public string CodSupplierBuy { get; set; }
        public string UnitOfMeasure { get; set; }
        public string CodSupplierMaker { get; set; }
        public Nullable<int> Selector { get; set; }
    
        public virtual ICollection<ArticleCost> ArticleCosts { get; set; }
        public virtual CustomerSupplier CustomerSupplierBuy { get; set; }
        public virtual CustomerSupplier CustomerSupplierMaker { get; set; }
    }
}