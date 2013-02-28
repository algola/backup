using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PapiroMVC.Models
{
  
    /// <summary>
    /// data used to create and edit
    /// </summary>
    public class ObjectPrintableArticleViewModel : ArticleViewModel
    {
        ObjectPrintableArticle article;
        public ObjectPrintableArticle Article
        {
            get
            {
                if (article == null)
                {
                    article = new ObjectPrintableArticle();
                    SupplierMaker = "";
                    SupplyerBuy = "";
                    article.ArticleCosts.Add(new ObjectPrintableArticleStandardCost());
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
        public ObjectPrintableArticleStandardCost ObjectPrintableArticleStandardCost
        {
            get
            {
                return (ObjectPrintableArticleStandardCost)this.Article.ArticleCosts.First(x => x.TypeOfArticleCost == ArticleCost.ArticleCostType.ObjectPrintableArticleStandardCost);
            }
        }
    }

    public class RollPrintableArticleViewModel : ArticleViewModel
    {
        RollPrintableArticle article;
        public RollPrintableArticle Article
        {
            get
            {
                if (article == null)
                {
                    article = new RollPrintableArticle();
                    SupplyerBuy = "";
                    SupplierMaker = "";
//                    article.ArticleCosts.Add(new RollPrintableArticleCuttedCost());
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

        /*
        public RollPrintableArticleCuttedCost RollPrintableArticleCuttedCost
        {
            get
            {
                return (RollPrintableArticleCuttedCost)this.Article.ArticleCosts.First(x => x.TypeOfArticleCost == ArticleCost.ArticleCostType.RollPrintableArticleCuttedCost);
            }
        }
        */

        public RollPrintableArticleStandardCost RollPrintableArticleStandardCost
        {
            get
            {
                return (RollPrintableArticleStandardCost)this.Article.ArticleCosts.First(x => x.TypeOfArticleCost == ArticleCost.ArticleCostType.RollPrintableArticleStandardCost);
            }
        }
    }

    [MetadataType(typeof(ArticleViewModel_Metadata))]
    public abstract class ArticleViewModel
    {
        //this field is used to link on view suppplier to article
        //autocomplete is called to recognize supplier
        //after editing / creation / etc.. we have to find supplier by name and link code to article
                
        public string SupplierMaker { get; set; }
        public string SupplyerBuy { get; set; }
    
    }

    public class SheetPrintableArticleViewModel : ArticleViewModel
    {
        SheetPrintableArticle article;
        public SheetPrintableArticle Article
        {
            get
            {
                if (article == null)
                {
                    article = new SheetPrintableArticle();
                    SupplyerBuy = "";
                    SupplierMaker = "";
                    article.ArticleCosts.Add(new SheetPrintableArticleCuttedCost());
                    article.ArticleCosts.Add(new SheetPrintableArticlePalletCost());
                    article.ArticleCosts.Add(new SheetPrintableArticlePakedCost());
                }
                return article;
            }
            set 
            {
                article = value;
                SupplierMaker = article.CustomerSupplierMaker==null?null:article.CustomerSupplierMaker.BusinessName;
                SupplyerBuy = article.CustomerSupplierBuy==null?null:article.CustomerSupplierBuy.BusinessName;
            }
        }

        public  SheetPrintableArticleCuttedCost SheetPrintableArticleCuttedCost
        {
            get
            {
                return (SheetPrintableArticleCuttedCost) this.Article.ArticleCosts.First(x=>x.TypeOfArticleCost==ArticleCost.ArticleCostType.SheetPrintableArticleCuttedCost);
            }
        }
        public  SheetPrintableArticlePalletCost SheetPrintableArticlePalletCost
        {
            get
            {
                return (SheetPrintableArticlePalletCost)this.Article.ArticleCosts.First(x => x.TypeOfArticleCost == ArticleCost.ArticleCostType.SheetPrintableArticlePalletCost);
            }
        }
        public  SheetPrintableArticlePakedCost SheetPrintableArticlePakedCost
        {
            get
            {
                return (SheetPrintableArticlePakedCost)this.Article.ArticleCosts.First(x => x.TypeOfArticleCost == ArticleCost.ArticleCostType.SheetPrintableArticlePakedCost);
            }
        }
    }

    public class RigidPrintableArticleViewModel : ArticleViewModel
    {
        RigidPrintableArticle article;
        public RigidPrintableArticle Article
        {
            get
            {
                if (article == null)
                {
                    article = new RigidPrintableArticle();
                    SupplierMaker = "";
                    SupplyerBuy = "";
                    article.ArticleCosts.Add(new RigidPrintableArticleStandardCost());
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

        public RigidPrintableArticleStandardCost RigidPrintableArticleStandardCost
        {
            get
            {
                return (RigidPrintableArticleStandardCost)this.Article.ArticleCosts.First(x => x.TypeOfArticleCost == ArticleCost.ArticleCostType.RigidPrintableArticleStandardCost);
            }
        }
    }

}