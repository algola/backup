using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class PrintingSheetCostDetail : PrintingCostDetail
    {

        public PrintingSheetCostDetail()
        {
            TypeOfCostDetail = CostDetailType.PrintingSheetCostDetail;
        }

        /// <summary>
        /// Elenco dei possibili formati di acquisto 
        /// </summary>
        public List<String> BuyingFormats { get; set; }

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

            ((ProductPartPrintingSheetGainSingle)GainPrintingOnBuying).LargerFormat = this.BuyingFormat;
            ((ProductPartPrintingSheetGainSingle)GainPrintingOnBuying).SmallerFormat = this.PrintingFormat;
            ((ProductPartPrintingSheetGainSingle)GainPrintingOnBuying).SubjectNumber = 1;
            ((ProductPartPrintingSheetGainSingle)GainPrintingOnBuying).CalculateGain();

            //devo anche rifare la messa in macchina della parte!!!
            if (this.ProductPartPrinting != null)
            {
                this.ProductPartPrinting.Part = this.ProductPart;
                this.ProductPartPrinting.PrintingFormat = this.PrintingFormat;
                this.ProductPartPrinting.Update();
            }
        }

        public override void InitCostDetail(IQueryable<TaskExecutor> tskExec, IQueryable<Article> articles, Cost taskCost)
        {
            ICollection<ProductPartsPrintableArticle> productPartPrintabelArticles = new List<ProductPartsPrintableArticle>();

            base.InitCostDetail(tskExec, articles,taskCost);
            productPartPrintabelArticles = taskCost.ProductPartTask.ProductPart.ProductPartPrintableArticles;

            #region Format
            List<string> formats = new List<string>();
            //
            //voglio sapere quali sono i formati degli articoli
            foreach (var item in productPartPrintabelArticles)
            {
                var formatList = articles.OfType<SheetPrintableArticle>()
                            .Where(x => x.TypeOfMaterial == item.TypeOfMaterial &&
                                x.Color == item.Color &&
                                x.NameOfMaterial == item.NameOfMaterial)
                                    .Select(x => x.Format);

                formats = formats.Union(formatList.ToList()).ToList();
            }

            BuyingFormats = formats;

            #endregion
        }


        public override void InitCostDetail2(IQueryable<TaskExecutor> tskExec, IQueryable<Article> articles)
        {
            ICollection<ProductPartsPrintableArticle> productPartPrintabelArticles = new List<ProductPartsPrintableArticle>();

            base.InitCostDetail2(tskExec, articles);
            productPartPrintabelArticles = TaskCost.ProductPartTask.ProductPart.ProductPartPrintableArticles;

            #region Format
            List<string> formats = new List<string>();
            //
            //voglio sapere quali sono i formati degli articoli
            foreach (var item in productPartPrintabelArticles)
            {
                var formatList = articles.OfType<SheetPrintableArticle>()
                            .Where(x => x.TypeOfMaterial == item.TypeOfMaterial &&
                                x.Color == item.Color &&
                                x.NameOfMaterial == item.NameOfMaterial)
                                    .Select(x => x.Format);

                formats = formats.Union(formatList.ToList()).ToList();
            }

            BuyingFormats = formats;

            #endregion
        }
    }
}