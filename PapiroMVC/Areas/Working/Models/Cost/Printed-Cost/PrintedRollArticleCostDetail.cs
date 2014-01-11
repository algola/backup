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

    }
}