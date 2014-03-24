using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PapiroMVC.Models
{

    [MetadataType(typeof(RollPrintableArticleViewModelWizard_Metadata))]
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

    [MetadataType(typeof(SheetPrintableArticleViewModelWizard_Metadata))]
    public class SheetPrintableArticleViewModelWizard : ArticleViewModel
    {
        SheetPrintableArticle article;
        List<string> formats;
        List<double> weights;

        public SheetPrintableArticle Article
        {
            get
            {
                if (article == null)
                {
                    article = new SheetPrintableArticle();
                    article.Format = "10x10";
                    article.SheetPerPacked = 0;
                    article.SheetPerPallet = 0;
                    SupplierMaker = "";
                    SupplyerBuy = "";
//                    article.ArticleCosts.Add(new SheetPrintableArticleCuttedCost());
                    article.ArticleCosts.Add(new SheetPrintableArticlePakedCost());
                    article.ArticleCosts.Add(new SheetPrintableArticlePalletCost());
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
        /*
        public SheetPrintableArticleStandardCost SheetPrintableArticleStandardCost
        {
            get
            {
                return (SheetPrintableArticleStandardCost)this.Article.ArticleCosts.First(x => x.TypeOfArticleCost == ArticleCost.ArticleCostType.SheetPrintableArticleStandardCost);
            }
        }
        */

        public List<string> Formats
        {
            get
            {
                if (formats == null)
                {
                    formats = new List<string>();
                    formats.Add("");
                    formats.Add("");
                    formats.Add("");
                    formats.Add("");
                    formats.Add("");
                    formats.Add("");
                    formats.Add("");
                    formats.Add("");
                    formats.Add("");
                    formats.Add("");
                }
                return formats;
            }
            set
            {
                formats = value;
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

        //public SheetPrintableArticleCuttedCost SheetPrintableArticleCuttedCost
        //{
        //    get
        //    {
        //        return (SheetPrintableArticleCuttedCost)this.Article.ArticleCosts.First(x => x.TypeOfArticleCost == ArticleCost.ArticleCostType.SheetPrintableArticleCuttedCost);
        //    }
        //}

        public SheetPrintableArticlePalletCost SheetPrintableArticlePalletCost
        {
            get
            {
                return (SheetPrintableArticlePalletCost)this.Article.ArticleCosts.First(x => x.TypeOfArticleCost == ArticleCost.ArticleCostType.SheetPrintableArticlePalletCost);
            }
        }

        public SheetPrintableArticlePakedCost SheetPrintableArticlePakedCost
        {
            get
            {
                return (SheetPrintableArticlePakedCost)this.Article.ArticleCosts.First(x => x.TypeOfArticleCost == ArticleCost.ArticleCostType.SheetPrintableArticlePakedCost);
            }
        }


    }

}