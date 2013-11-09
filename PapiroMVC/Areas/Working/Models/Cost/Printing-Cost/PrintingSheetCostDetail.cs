using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public class PrintingSheetCostDetail : CostDetail
    {
        /// <summary>
        /// this property contains informations over printing partial
        /// </summary>
        public ProductPartPrintingSheet ProductPartPrinting { get; set; }

        public PrintingSheetCostDetail()
        {
            TypeOfCostDetail = CostDetailType.PrintingSheetCostDetail;
        }

        /// <summary>
        /// Elenco dei possibili formati di acquisto 
        /// </summary>
        public List<String> BuyingFormats { get; set; }

        /// <summary>
        /// Formato di acqusto selezionato su cui calcolare i formati macchina
        /// </summary>
        public String BuyingFormat { get; set; }

        /// <summary>
        /// Formato macchina
        /// </summary>
        public String PrintingFormat { get; set; }

        //resa per il formato d'acquisto
        public ProductPartPrintingSheetGainSingle GainPrintingOnBuying { get; set; }

        //every changes fire this update
        public override void Update()
        {
            if (ProductPartPrinting == null)
            {
                switch (ProductPart.TypeOfProductPart)
                {
                    case ProductPart.ProductPartType.ProductPartSingleSheet:
                        this.ProductPartPrinting = new ProductPartSingleSheetPrinting();
                        break;
                    case ProductPart.ProductPartType.ProductPartCoverSheet:
                        this.ProductPartPrinting = new ProductPartCoverSheetPrinting();
                        break;
                    case ProductPart.ProductPartType.ProductPartBookSheet:
                        this.ProductPartPrinting = new ProductPartBookSheetPrinting();
                        break;
                    case ProductPart.ProductPartType.ProductPartBlockSheet:
                        this.ProductPartPrinting = new ProductPartSingleSheetPrinting();
                        break;
                    default:
                        throw new Exception();
                        break;
                }
            }

            if (GainPrintingOnBuying == null)
            {
                GainPrintingOnBuying = new ProductPartPrintingSheetGainSingle();
            }

            this.GainPrintingOnBuying.LargerFormat = this.BuyingFormat;
            this.GainPrintingOnBuying.SmallerFormat = this.PrintingFormat;
            this.GainPrintingOnBuying.SubjectNumber = 1;
            this.GainPrintingOnBuying.CalculateGain();

            //devo anche rifare la messa in macchina della parte!!!
            if (this.ProductPartPrinting != null)
            {
                this.ProductPartPrinting.Part = this.ProductPart;
                this.ProductPartPrinting.PrintingFormat = this.PrintingFormat;
                this.ProductPartPrinting.Update();
            }
        }
    }
}