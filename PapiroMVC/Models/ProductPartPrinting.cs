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
    
    public partial class ProductPartPrinting
    {
        public ProductPartPrinting()
        {
            this.GainPartOnPrintings = new HashSet<ProductPartPrintingGain>();
        }
    
        public System.DateTime TimeStampTable { get; set; }
        public string CodProductPartPrinting { get; set; }
        public string PrintingFormat { get; set; }
        public string CodProductPart { get; set; }
    
        public virtual CostDetail CostDetail { get; set; }
        public virtual ProductPartPrintingGain GainPartOnPrinting___ { get; set; }
        public virtual ProductPart Part { get; set; }
        public virtual ICollection<ProductPartPrintingGain> GainPartOnPrintings { get; set; }
    }
}
