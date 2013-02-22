using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PapiroMVC.Models
{

    [MetadataType(typeof(ArticleViewModelVizard_Metadata))]
    public class RollPrintableArticleViewModelWizard : ArticleViewModel
    {
        RollPrintableArticle article;
        List<double> widths;
        List<double> weights;

        public RollPrintableArticle Article
        {
            get
            {
                if (article == null)
                {
                    article = new RollPrintableArticle();
                    SupplierMaker = "";
                    SupplyerBuy = "";
                    article.ArticleCosts.Add(new RollPrintableArticleStandardCost());
                }
                return article;
            }
            set
            {
                article = value;
                SupplierMaker = article.CustomerSupplierMaker == null ? null : article.CustomerSupplierMaker.BusinessName;
                SupplyerBuy = article.CustomerSupplierBuy == null ? null : article.CustomerSupplierBuy.BusinessName;
            }
        }

        public RollPrintableArticleStandardCost RollPrintableArticleStandardCost
        {
            get
            {
                return (RollPrintableArticleStandardCost)this.Article.ArticleCosts.First(x => x.TypeOfArticleCost == ArticleCost.ArticleCostType.RollPrintableArticleStandardCost);
            }
        }

        public List<double> Widths 
        {
            get
            {
                if (widths == null)
                {
                    widths = new List<double>();
                    widths.Add(0);
                    widths.Add(0);
                    widths.Add(0);
                    widths.Add(0);
                    widths.Add(0);
                    widths.Add(0);
                    widths.Add(0);
                    widths.Add(0);
                    widths.Add(0);
                    widths.Add(0);
                }
                return widths;
            }
            set
            {
                widths = value;
            }
        }

        public List<double> Weights
        {
            get
            {
                if (weights == null)
                {
                    weights = new List<double>();
                    weights.Add(0);
                    weights.Add(0);
                    weights.Add(0);
                    weights.Add(0);
                    weights.Add(0);
                    weights.Add(0);
                    weights.Add(0);
                    weights.Add(0);
                    weights.Add(0);
                    weights.Add(0);
                }
                return weights;
            }
            set
            {
                weights = value;
            }
        }
    }

}