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
    
    public abstract partial class ProductPartsPrintableArticle
    {
        public ProductPartsPrintableArticle()
        {
            this.costs = new HashSet<Cost>();
        }
    
        public System.DateTime TimeStampTable { get; set; }
        public string CodProductPartPrintableArticle { get; set; }
        public string CodProductPart { get; set; }
        public string ProductPartPrintableArticleName { get; set; }
        public string TypeOfMaterial { get; set; }
        public string NameOfMaterial { get; set; }
        public string Color { get; set; }
        public Nullable<long> Weight { get; set; }
    
        public virtual ProductPart ProductPart { get; set; }
        public virtual ICollection<Cost> costs { get; set; }
    }
}
