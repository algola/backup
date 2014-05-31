using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class PrintedSheetArticleCostDetail : PrintedArticleCostDetail
    {

        public override void Copy(CostDetail to)
        {
            base.Copy(to);

            PrintedSheetArticleCostDetail to2 = (PrintedSheetArticleCostDetail)to;

            to2.CostPerKg = this.CostPerKg;
            to2.CostPerSheet = this.CostPerSheet;

            to = to2;
        }

        public PrintedSheetArticleCostDetail()
        {
            TypeOfCostDetail = CostDetailType.PrintedSheetArticleCostDetail;
        }

        public override void GetCostFromList(IQueryable<Article> articles)
        {
            //questo dovrebbe far ottenere il costo!!!!!!
            var extract = articles.GetArticlesByProductPartPrintableArticle(ProductPart.ProductPartPrintableArticles.FirstOrDefault(x => x.CodProductPartPrintableArticle == this.TaskCost.CodProductPartPrintableArticle));

            if (extract.FirstOrDefault() == null)
            {
                //se non trovo il
                throw (new NullReferenceException());
            }

            TypeOfQuantity = (int)extract.FirstOrDefault().TypeOfQuantity;
            var article = extract.FirstOrDefault();

            var aCost = article.ArticleCosts.OfType<SheetPrintableArticlePakedCost>().FirstOrDefault();
            CostPerKg = ((SheetPrintableArticleCost)aCost).CostPerKg;
            CostPerSheet = ((SheetPrintableArticleCost)aCost).CostPerSheet;
        }

        public override void CostDetailCostCodeRigen()
        {
            base.CostDetailCostCodeRigen();
        }

        public override double UnitCost(double qta)
        {
            if (!IsValid)
            {
                return 0;
            }

            //voglio visualizzare il costo al foglio

            if (Convert.ToDouble(CostPerKg) != 0)
            {
                //avendo il costo al kg... devo calcolare in uscita il costo al foglio
                //peso del foglio
                //questo dovrebbe far ottenere il costo!!!!!!
                var extract = _articles.GetArticlesByProductPartPrintableArticle(ProductPart.ProductPartPrintableArticles.FirstOrDefault(x => x.CodProductPartPrintableArticle == this.TaskCost.CodProductPartPrintableArticle));

                var mq = (this.ComputedBy.ProductPartPrinting.PrintingFormat.GetSide1() / 100) *
                    (this.ComputedBy.ProductPartPrinting.PrintingFormat.GetSide2() / 100) *
                    (double)this.ComputedBy.GainPrintingOnBuying.Makereadies.FirstOrDefault().CalculatedGain;

                var weightGr = (double)(extract.OfType<Printable>().FirstOrDefault().Weight ?? 0) * mq;
                return weightGr / 1000 * Convert.ToDouble(CostPerKg);
            }

            return Convert.ToDouble(CostPerSheet);
        }

    }
}