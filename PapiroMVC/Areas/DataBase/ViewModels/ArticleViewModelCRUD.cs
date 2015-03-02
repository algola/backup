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
                return (ObjectPrintableArticle)article;
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
                return (RollPrintableArticle) article;
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

        protected Article article;
        //this field is used to link on view suppplier to article
        //autocomplete is called to recognize supplier
        //after editing / creation / etc.. we have to find supplier by name and link code to article

        public string SupplierMaker { get; set; }
        public string SupplyerBuy { get; set; }

        public Warehouse WarehouseArticle
        {
            get
            {
                return this.article.WarehouseArticles.FirstOrDefault();
            }
        }

    }

    [MetadataType(typeof(SheetPrintableArticleViewModel_Metadata))]
    public class SheetPrintableArticleViewModel : ArticleViewModel
    {
        public SheetPrintableArticle Article
        {
            get
            {
                if (article == null)
                {
                    article = new SheetPrintableArticle();
                    SupplyerBuy = "";
                    SupplierMaker = "";
                    //CUTTED                    article.ArticleCosts.Add(new SheetPrintableArticleCuttedCost());
                    article.ArticleCosts.Add(new SheetPrintableArticlePalletCost());
                    article.ArticleCosts.Add(new SheetPrintableArticlePakedCost());

                    
                }
                
                return (SheetPrintableArticle) article;
            }
            set
            {
                article = value;
                SupplierMaker = article.CustomerSupplierMaker == null ? null : article.CustomerSupplierMaker.BusinessName;
                SupplyerBuy = article.CustomerSupplierBuy == null ? null : article.CustomerSupplierBuy.BusinessName;
            }
        }

        /*CUTTED
        public  SheetPrintableArticleCuttedCost SheetPrintableArticleCuttedCost
        {
            get
            {
                return (SheetPrintableArticleCuttedCost) this.Article.ArticleCosts.First(x=>x.TypeOfArticleCost==ArticleCost.ArticleCostType.SheetPrintableArticleCuttedCost);
            }
        }
        */

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

    public class RigidPrintableArticleViewModel : ArticleViewModel
    {

        public RigidPrintableArticleViewModel()
        {
            Console.Write("");
        }
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
                return (RigidPrintableArticle)article;
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


    public class NoPrintableViewModel : ArticleViewModel
    {

        public NoPrintableViewModel()
        {
            Console.Write("");
        }
        public NoPrintable Article
        {
            get
            {
                if (article == null)
                {
                    article = new NoPrintable();
                    SupplierMaker = "";
                    SupplyerBuy = "";
                    article.ArticleCosts.Add(new NoPrintableArticleCostStandard());
                    

                }
                return (NoPrintable) article;
            }
            set
            {
                article = value;
                SupplierMaker = article.CustomerSupplierMaker == null ? null : article.CustomerSupplierMaker.BusinessName;
                SupplyerBuy = article.CustomerSupplierBuy == null ? null : article.CustomerSupplierBuy.BusinessName;
            }
        }

        public NoPrintableArticleCostStandard NoPrintableArticleCostStandard
        {
            get
            {
                return (NoPrintableArticleCostStandard)this.Article.ArticleCosts.First(x => x.TypeOfArticleCost == ArticleCost.ArticleCostType.NoPrintableArticleCostStandard);
            }
        }
    }

}