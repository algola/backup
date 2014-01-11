using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class PrintedRigidArticleCostDetail : PrintedArticleCostDetail
    {
        public PrintedRigidArticleCostDetail()
        {
            TypeOfCostDetail = CostDetailType.PrintedRigidArticleCostDetail;
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

            var aCost = article.ArticleCosts.OfType<RigidPrintableArticleStandardCost>().FirstOrDefault();
            CostPerMq = ((RigidPrintableArticleCost)aCost).CostPerMq;
        }

        public override void CostDetailCostCodeRigen()
        {
            base.CostDetailCostCodeRigen();
        }

        public override double UnitCost(double qta)
        {
            return (Convert.ToDouble(CostPerMq));
        }

    }
}