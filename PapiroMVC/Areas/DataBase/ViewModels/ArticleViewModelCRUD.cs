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
                return (RollPrintableArticle)article;
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

        public WarehouseItem WarehouseArticle
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

                return (SheetPrintableArticle)article;
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
                return (NoPrintable)article;
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

    public class FoilViewModel : NoPrintableViewModel
    {

        public FoilViewModel()
        {
            Console.Write("");
        }
        public Foil Article
        {
            get
            {
                if (article == null)
                {
                    article = new Foil();
                    SupplierMaker = "";
                    SupplyerBuy = "";
                    article.ArticleCosts.Add(new NoPrintableArticleCostMq());


                }
                return (Foil)article;
            }
            set
            {
                article = value;
                SupplierMaker = article.CustomerSupplierMaker == null ? null : article.CustomerSupplierMaker.BusinessName;
                SupplyerBuy = article.CustomerSupplierBuy == null ? null : article.CustomerSupplierBuy.BusinessName;
            }
        }

        public NoPrintableArticleCostMq NoPrintableArticleCostMq
        {
            get
            {
                return (NoPrintableArticleCostMq)this.Article.ArticleCosts.First(x => x.TypeOfArticleCost == ArticleCost.ArticleCostType.NoPrintableArticleCostMq);
            }
        }

    }

    public class MeshViewModel : NoPrintableViewModel
    {

        public MeshViewModel()
        {
            Console.Write("");
        }
        public Mesh Article
        {
            get
            {
                if (article == null)
                {
                    article = new Mesh();
                    SupplierMaker = "";
                    SupplyerBuy = "";
                    article.ArticleCosts.Add(new NoPrintableArticleCostMq());

                }
                return (Mesh)article;
            }
            set
            {
                article = value;
                SupplierMaker = article.CustomerSupplierMaker == null ? null : article.CustomerSupplierMaker.BusinessName;
                SupplyerBuy = article.CustomerSupplierBuy == null ? null : article.CustomerSupplierBuy.BusinessName;
            }
        }

        public NoPrintableArticleCostMq NoPrintableArticleCostMq
        {
            get
            {
                return (NoPrintableArticleCostMq)this.Article.ArticleCosts.First(x => x.TypeOfArticleCost == ArticleCost.ArticleCostType.NoPrintableArticleCostMq);
            }
        }
    }


    public class AniloxViewModel : NoPrintableViewModel
    {

        public AniloxViewModel()
        {
            Console.Write("");
        }
        public Anilox Article
        {
            get
            {
                if (article == null)
                {
                    article = new Anilox();
                    SupplierMaker = "";
                    SupplyerBuy = "";
                    article.ArticleCosts.Add(new NoPrintableArticleCostMq());

                }
                return (Anilox)article;
            }
            set
            {
                article = value;
                SupplierMaker = article.CustomerSupplierMaker == null ? null : article.CustomerSupplierMaker.BusinessName;
                SupplyerBuy = article.CustomerSupplierBuy == null ? null : article.CustomerSupplierBuy.BusinessName;
            }
        }

        public NoPrintableArticleCostMq NoPrintableArticleCostMq
        {
            get
            {
                return (NoPrintableArticleCostMq)this.Article.ArticleCosts.First(x => x.TypeOfArticleCost == ArticleCost.ArticleCostType.NoPrintableArticleCostMq);
            }
        }
    }
    public class InkViewModel : NoPrintableViewModel
    {

        public InkViewModel()
        {
            Console.Write("");
        }
        public Ink Article
        {
            get
            {
                if (article == null)
                {
                    article = new Ink();
                    SupplierMaker = "";
                    SupplyerBuy = "";
                    article.ArticleCosts.Add(new NoPrintableArticleCostKg());


                }
                return (Ink)article;
            }
            set
            {
                article = value;
                SupplierMaker = article.CustomerSupplierMaker == null ? null : article.CustomerSupplierMaker.BusinessName;
                SupplyerBuy = article.CustomerSupplierBuy == null ? null : article.CustomerSupplierBuy.BusinessName;
            }


        }

        public NoPrintableArticleCostKg NoPrintableArticleCostKg
        {
            get
            {
                return (NoPrintableArticleCostKg)this.Article.ArticleCosts.First(x => x.TypeOfArticleCost == ArticleCost.ArticleCostType.NoPrintableArticleCostKg);
            }
        }

    }
}

