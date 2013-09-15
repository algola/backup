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
    
    public partial class ProductPart
    {
        public ProductPart()
        {
            this.ProductPartPrintableArticles = new HashSet<ProductPartsPrintableArticle>();
            this.ProductPartTasks = new HashSet<ProductPartTask>();
            this.ProductPartsToProductTasks = new HashSet<productpartstoproducttask>();
        }
    
        public System.DateTime TimeStampTable { get; set; }
        public string CodProductPart { get; set; }
        public string CodProduct { get; set; }
        public string ProductPartName { get; set; }
        public string PrintingType { get; set; }
        public string Format { get; set; }
        public Nullable<int> ServicesNumber { get; set; }
        public string CodProductPart_ { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual ICollection<ProductPartsPrintableArticle> ProductPartPrintableArticles { get; set; }
        public virtual ICollection<ProductPartTask> ProductPartTasks { get; set; }
        public virtual ICollection<productpartstoproducttask> ProductPartsToProductTasks { get; set; }
    }
}
