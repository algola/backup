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
    
    public partial class Cost
    {
        public Cost()
        {
            this.CostDetails = new HashSet<CostDetail>();
        }
    
        public System.DateTime TimeStampTable { get; set; }
        public string CodCost { get; set; }
        public string CodDocumentProduct { get; set; }
        public string CodProductTask { get; set; }
        public string CodProductPartTask { get; set; }
        public string CodProductPartPrintableArticle { get; set; }
        public string Description { get; set; }
        public Nullable<double> Quantity { get; set; }
        public string UnitCost { get; set; }
        public string TotalCost { get; set; }
        public Nullable<bool> ForceZero { get; set; }
        public Nullable<bool> Hidden { get; set; }
    
        public virtual DocumentProduct DocumentProduct { get; set; }
        public virtual ProductPartsPrintableArticle ProductPartsPrintableArticle { get; set; }
        public virtual ProductPartTask ProductPartTask { get; set; }
        public virtual ProductTask ProductTask { get; set; }
        public virtual ICollection<CostDetail> CostDetails { get; set; }
    }
}