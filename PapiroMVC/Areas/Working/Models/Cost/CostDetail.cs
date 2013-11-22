using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PapiroMVC.Models
{
    public interface ICostDetail
    {
        void Update();
    }

    //in questa viewmodel carico il costo
    //ed espongo l'eleco delle macchine che possono eseguire il task

    //se è un articolo--> ?? decider

    //    [MetadataType(typeof(TaskCostDetail_MetaData))]
    public partial class CostDetail : ICostDetail
    {
        public enum CostDetailType : int
        {
            PrintingSheetCostDetail = 0,  //digital and litho sheet
            PrintingRollCostDetail = 1,  //digital and litho sheet
            PrintingPlotterCostDetail = 2, //plotter

            PrintedSheetArticleCostDetail = 10,
            PrintedRollArticleCostDetail = 11
        }

        public CostDetailType TypeOfCostDetail
        {
            get;
            protected set;
        }

        public List<TaskExecutor> TaskExecutors { get; set; }

        public virtual void Update()
        { 
        }

        public virtual void InitCostDetail(IQueryable<TaskExecutor> tskExec, IQueryable<Article> articles)
        {
       //     TaskCost = taskCost;
        }

        public ProductPartPrintingGain GainPrintingOnBuying 
        {
            get
            {
                return GainPrintingOnBuyings.FirstOrDefault();
            }
            set
            {
                GainPrintingOnBuyings.Clear();
                GainPrintingOnBuyings.Add(value);
            }
        
        }


        public virtual void CostDetailCostCodeRigen()
        {
            this.TimeStampTable = DateTime.Now;
 //           this.CodCostDetail = this.CodCost;

            if (this.GainPrintingOnBuying != null)
            {
                GainPrintingOnBuying.CodProductPartPrintingGain = CodCostDetail + "-CDET";
                GainPrintingOnBuying.TimeStampTable = DateTime.Now;

                var mrArray = GainPrintingOnBuying.Makereadies.ToList();
                foreach (var mr in GainPrintingOnBuying.Makereadies)
                {
                    mr.CodMakeready = GainPrintingOnBuying.CodProductPartPrintingGain + "-" + mrArray.IndexOf(mr);
                    mr.CodProductPartPrintingGain = GainPrintingOnBuying.CodProductPartPrintingGain;                   
                    mr.TimeStampTable = DateTime.Now;
                }
            }

            if (this.ProductPartPrinting != null)
            {
                ProductPartPrinting.CodProductPartPrinting = CodCostDetail;
                ProductPartPrinting.TimeStampTable = DateTime.Now;

                if (ProductPartPrinting.GainPartOnPrinting != null)
                {
                    ProductPartPrinting.GainPartOnPrinting.CodProductPartPrintingGain = ProductPartPrinting.CodProductPartPrinting;
                    ProductPartPrinting.GainPartOnPrinting.TimeStampTable = DateTime.Now;

                    var mrArray = ProductPartPrinting.GainPartOnPrinting.Makereadies.OrderBy(x => x.CodMakeready).ToList();
                    foreach (var mr in ProductPartPrinting.GainPartOnPrinting.Makereadies.OrderBy(x => x.CodMakeready))
                    {
                        mr.CodMakeready = ProductPartPrinting.GainPartOnPrinting.CodProductPartPrintingGain + "-" + mrArray.IndexOf(mr);
                        mr.CodProductPartPrintingGain = ProductPartPrinting.GainPartOnPrinting.CodProductPartPrintingGain;                   
                        mr.TimeStampTable = DateTime.Now;
                    }
                }
            }
        }
    }
}