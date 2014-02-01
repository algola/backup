﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class PrintingRollCostDetail : PrintingCostDetail
    {
        public PrintingRollCostDetail()
        {
            TypeOfCostDetail = CostDetailType.PrintingRollCostDetail;
        }

       
        /// <summary>
        /// Elenco dei possibili larghezze di acquisto 
        /// </summary>
        public List<Double> BuyingWidths { get; set; }

        //every changes fire this update
        public override void Update()
        {
            if (ProductPartPrinting == null)
            {
                switch (ProductPart.TypeOfProductPart)
                {
                    case ProductPart.ProductPartType.ProductPartSinglePlotter:
                        this.ProductPartPrinting = new ProductPartSingleRollPrinting();
                        break;
                    default:
                        throw new NotImplementedException();
                        break;
                }
            }

            if (GainPrintingOnBuying == null)
            {
                GainPrintingOnBuying = new ProductPartPrintingRollGainSingle();
            }

            ((ProductPartPrintingRollGainSingle)GainPrintingOnBuying).Width = this.BuyingWidth??1;
            //this.GainPrintingOnBuying.SmallerFormat = this.PrintingFormat;
            //this.GainPrintingOnBuying.SubjectNumber = 1;
            //this.GainPrintingOnBuying.CalculateGain();

            //devo anche rifare la messa in macchina della parte!!!
            if (this.ProductPartPrinting != null)
            {
                this.ProductPartPrinting.Part = this.ProductPart;
                ((ProductPartRollPrinting)this.ProductPartPrinting).Width = this.BuyingWidth??1;
                this.ProductPartPrinting.Update();
            }
        }

        public override List<PrintedArticleCostDetail> GetRelatedPrintedCostDetail(IQueryable<Article> articles, IQueryable<Cost> costs)
        {
            List<PrintedArticleCostDetail> lst = new List<PrintedArticleCostDetail>();

            foreach (var item in this.ProductPart.ProductPartPrintableArticles)
            {
                var x = new PrintedRollArticleCostDetail();
                x.ComputedBy = this;
                x.ProductPart = this.ProductPart;

                x.Guid = this.Guid;
                this.Computes.Add(x);
            }

            return lst;
        }

    }
}