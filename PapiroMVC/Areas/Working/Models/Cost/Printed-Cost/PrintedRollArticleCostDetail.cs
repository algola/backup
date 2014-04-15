using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class PrintedRollArticleCostDetail : PrintedArticleCostDetail
    {
        public PrintedRollArticleCostDetail()
        {
            TypeOfCostDetail = CostDetailType.PrintedRollArticleCostDetail;
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

            var aCost = article.ArticleCosts.OfType<RollPrintableArticleStandardCost>().FirstOrDefault();
            CostPerMq = ((RollPrintableArticleCost)aCost).CostPerMq;
            CostPerMl = ((RollPrintableArticleCost)aCost).CostPerMl;
        }

        public override void CostDetailCostCodeRigen()
        {
            base.CostDetailCostCodeRigen();
        }

        public override void UpdateCoeff()
        {
            base.UpdateCoeff();
            //voglio calcolare il prezzo al Ml e il prezzo al Mq!!!!
            GetCostFromList(_articles);
        }

        public override double UnitCost(double qta)
        {
            if (!IsValid)
            {
                return 0;
            }

            return (Convert.ToDouble(CostPerMq));
        }

        public override double Quantity(double qta)
        {
            double ret;

            //var ret = base.Quantity(qta);
            //questo dovrebbe far ottenere il costo!!!!!!
            var extract = _articles.GetArticlesByProductPartPrintableArticle(ProductPart.ProductPartPrintableArticles.FirstOrDefault(x => x.CodProductPartPrintableArticle == this.TaskCost.CodProductPartPrintableArticle));
            var article = (RollPrintableArticle)extract.FirstOrDefault();

            double mqMat = 0;

            if ((QuantityType)(ComputedBy.TypeOfQuantity??0) == QuantityType.RunLengthMlTypeOfQuantity)
            {
                //prendo i ml li moltiplico per la banca
                mqMat = ComputedBy.Quantity(qta) * ComputedBy.ProductPartPrinting.PrintingFormat.GetSide1()/100;
            }
            else
            {
                //prendo i ml li moltiplico per la banca
                mqMat = ComputedBy.Quantity(qta) * ComputedBy.ProductPartPrinting.PrintingFormat.GetSide1() * ComputedBy.ProductPartPrinting.PrintingFormat.GetSide1() / 10000;
            }

            switch ((QuantityType)(TypeOfQuantity ?? 0))
            {
                case QuantityType.MqWorkTypeOfQuantity:
                    ret = Math.Ceiling(mqMat);
                    break;
                default:
                    ret = base.Quantity(qta);
                    break;
            }

            return ret;

        }


    }
}