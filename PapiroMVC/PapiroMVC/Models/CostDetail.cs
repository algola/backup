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
    
    public partial class CostDetail
    {
        public CostDetail()
        {
            this.Computes = new HashSet<CostDetail>();
            this.GainPrintingOnBuyings = new HashSet<ProductPartPrintingGain>();
        }
    
        public System.DateTime TimeStampTable { get; set; }
        public string CodCostDetail { get; set; }
        public string CodTaskExecutorSelected { get; set; }
        public string CodCost { get; set; }
        public string CodProductPart { get; set; }
        public string CodComputedBy { get; set; }
        public Nullable<double> Starts { get; set; }
        public Nullable<double> GainForRun { get; set; }
        public Nullable<double> GainForRunForPrintableArticle { get; set; }
        public Nullable<double> GainForMqRun { get; set; }
        public Nullable<double> GainForMqRunForPrintableArticle { get; set; }
        public Nullable<int> TypeOfQuantity { get; set; }
        public Nullable<double> GainForWeigthRun { get; set; }
        public Nullable<double> GainForWeigthRunForPrintableArticle { get; set; }
        public Nullable<int> Error { get; set; }
        public string Guid { get; set; }
    
        public virtual Cost TaskCost { get; set; }
        public virtual ProductPart ProductPart { get; set; }
        public virtual TaskExecutor TaskexEcutorSelected { get; set; }
        public virtual ICollection<CostDetail> Computes { get; set; }
        public virtual CostDetail ComputedBy { get; set; }
        public virtual ProductPartPrinting ProductPartPrinting { get; set; }
        public virtual ICollection<ProductPartPrintingGain> GainPrintingOnBuyings { get; set; }
    }
}