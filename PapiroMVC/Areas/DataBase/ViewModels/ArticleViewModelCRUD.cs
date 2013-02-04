using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PapiroMVC.Models
{
    //public class ArticleCostBinder : IModelBinder
    //{
    //    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    //    {
    //        var values = (ValueProviderCollection)bindingContext.ValueProvider;

    //        ArticleCost ret;
    //        var y = values.GetValue(bindingContext.ModelName + ".TypeOfArticleCost");

    //        var x = ArticleCost.ArticleCostType.SheetPrintableArticlePakedCost;
    //        if (x == ArticleCost.ArticleCostType.SheetPrintableArticlePakedCost)
    //            ret = new SheetPrintableArticlePakedCost();
    //        else

    //            if (x == ArticleCost.ArticleCostType.SheetPrintableArticlePalletCost)
    //                ret = new SheetPrintableArticlePalletCost();
    //            else
    //                if (x == ArticleCost.ArticleCostType.SheetPrintableArticleCuttedCost)
    //                    ret = new SheetPrintableArticleCuttedCost();
    //                else
    //                    if (x == ArticleCost.ArticleCostType.RollPrintableArticleStandardCost)
    //                        ret = new RollPrintableArticleStandardCost();
    //                    else
    //                        if (x == ArticleCost.ArticleCostType.RollPrintableArticleCuttedCost)
    //                            ret = new RollPrintableArticleCuttedCost();
    //                        else
    //                            if (x == ArticleCost.ArticleCostType.RigidPrintableArticleStandardCost)
    //                                ret = new RollPrintableArticleCuttedCost();
    //                            else
    //                                if (x == ArticleCost.ArticleCostType.ObjectPrintableArticleStandardCost)
    //                                    ret = new RollPrintableArticleCuttedCost();
    //                                else
    //                                    ret = new SheetPrintableArticleCuttedCost();
    //        return ret;
    //    }
    //}

    public class ObjectPrintableArticleViewModel
    {
        ObjectPrintableArticle article;
        public ObjectPrintableArticle Article
        {
            get
            {
                if (article == null)
                {
                    article = new ObjectPrintableArticle();
                    article.ArticleCosts.Add(new ObjectPrintableArticleStandardCost());
                }
                return article;
            }
            set
            {
                article = value;
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

    public class RollPrintableArticleViewModel
    {
        RollPrintableArticle article;
        public RollPrintableArticle Article
        {
            get
            {
                if (article == null)
                {
                    article = new RollPrintableArticle();
                    article.ArticleCosts.Add(new RollPrintableArticleCuttedCost());
                    article.ArticleCosts.Add(new RollPrintableArticleStandardCost());
                }
                return article;
            }
            set
            {
                article = value;
            }
        }

        public RollPrintableArticleCuttedCost RollPrintableArticleCuttedCost
        {
            get
            {
                return (RollPrintableArticleCuttedCost)this.Article.ArticleCosts.First(x => x.TypeOfArticleCost == ArticleCost.ArticleCostType.RollPrintableArticleCuttedCost);
            }
        }
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

    public class RigidPrintableArticleViewModel
    {
        RigidPrintableArticle article;
        public RigidPrintableArticle Article
        {
            get
            {
                if (article == null)
                {
                    article = new RigidPrintableArticle();
                    article.ArticleCosts.Add(new RigidPrintableArticleStandardCost());
                }
                return article;
            }
            set
            {
                article = value;
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