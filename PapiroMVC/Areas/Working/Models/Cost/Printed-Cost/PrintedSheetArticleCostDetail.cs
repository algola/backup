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


        public override double Quantity(double qta)
        {
            double ret;

            //var ret = base.Quantity(qta);
            //questo dovrebbe far ottenere il costo!!!!!!
            var extract = _articles.GetArticlesByProductPartPrintableArticle(ProductPart.ProductPartPrintableArticles.FirstOrDefault(x => x.CodProductPartPrintableArticle == this.TaskCost.CodProductPartPrintableArticle));
            var article = (SheetPrintableArticle)extract.FirstOrDefault();

            double mqMat = 0;
            double mlMat = 0;
            double runMat = 0;
            double kgMat = 0;

            var thisArticle = ProductPart.ProductPartPrintableArticles.FirstOrDefault(x => x.CodProductPartPrintableArticle == this.TaskCost.CodProductPartPrintableArticle);

            //devo ottenere i mq totali di materiale stampato ed uso un trucco... voglio il numero di fogli... lo moltiplico per la resa del materiale e per i mq
            var lastTypeOfQuantity = ComputedBy.TypeOfQuantity;
            ComputedBy.TypeOfQuantity = 0;
            mqMat = ComputedBy.QuantityMaterial(qta) * (GainForRunForPrintableArticle ?? 1) * ComputedBy.ProductPartPrinting.PrintingFormat.GetSide1() * ComputedBy.ProductPartPrinting.PrintingFormat.GetSide2() / 10000;
            ComputedBy.TypeOfQuantity = lastTypeOfQuantity;

            mlMat = mqMat / (ComputedBy.ProductPartPrinting.PrintingFormat.GetSide1() / 100);
            runMat = ComputedBy.QuantityMaterial(qta) / (double)this.ComputedBy.GainPrintingOnBuying.Makereadies.Average(x => x.CalculatedGain ?? 1);

            kgMat = mqMat * thisArticle.Weight ?? 0;
            kgMat /= 1000;

            this.CalculatedMq = mqMat;
            this.CalculatedMl = mlMat;
            this.CalculatedKg = kgMat;
            this.CalculatedRun = runMat;

            switch ((QuantityType)(TypeOfQuantity ?? 0))
            {
                case QuantityType.MqWorkTypeOfQuantity:
                    ret = Math.Ceiling(mqMat);
                    break;
                case QuantityType.NumberTypeOfQuantity:
                case QuantityType.RunTypeOfQuantity:
                    ret = Math.Ceiling(runMat);
                    break;
                default:
                    ret = base.Quantity(qta);
                    break;
            }

            return ret;


        }


    }
}