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

            //devo gestire qui se c'è una quadratura minima e se l'articolo deve essere arrotondato al mq successivo

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

            return (Convert.ToDouble(CostPerMq));
        }

        public override double Quantity(double qta)
        {
            var ret = base.Quantity(qta);
            //questo dovrebbe far ottenere il costo!!!!!!
            var extract = _articles.GetArticlesByProductPartPrintableArticle(ProductPart.ProductPartPrintableArticles.FirstOrDefault(x => x.CodProductPartPrintableArticle == this.TaskCost.CodProductPartPrintableArticle));
            var article = (RigidPrintableArticle)extract.FirstOrDefault();

            if (article.FromMinFormat != null)
            {
                var to = article.FromMinFormat.GetSide1() * article.FromMinFormat.GetSide1() / 10000;
                ret = ret <= to ? to : ret;
            }

            if (article.ToNexMq??false)
            {
                ret = Math.Ceiling(ret);
            }

            return ret;

        }

    }
}