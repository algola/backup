using System;
using System.Linq;
using PapiroMVC.Models;
using PapiroMVC.DbCodeManagement;
using System.Threading;

namespace Services
{
    public class ArticleRepository : GenericRepository<dbEntities, Article>, IArticleRepository
    {
        public string GetNewCode(Article a, ICustomerSupplierRepository customerSupplierRepository, string supplierMaker, string supplyerBuy)
        {
            CustomerSupplier[] customerSuppliers = customerSupplierRepository.GetAll().ToArray();

            var filteredItems = customerSuppliers.Where(
                item => item.BusinessName.IndexOf(supplierMaker, StringComparison.InvariantCultureIgnoreCase) >= 0);

            if (filteredItems.Count() == 0) throw new Exception();

            a.CodSupplierMaker = filteredItems.First().CodCustomerSupplier;

            customerSuppliers = customerSupplierRepository.GetAll().ToArray();

            var filteredItems2 = customerSuppliers.Where(
                item => item.BusinessName.IndexOf(supplyerBuy, StringComparison.InvariantCultureIgnoreCase) >= 0);

            if (filteredItems2.Count() == 0) throw new Exception();

            //if #suppliers < 1 then no supplier has selected correctly and then thow error
            a.CodSupplierBuy = filteredItems2.First().CodCustomerSupplier;

            var codes = (from COD in this.GetAll() select COD.CodArticle).ToArray().OrderBy(x => x, new SemiNumericComparer());
            var csCode = codes.Count() != 0 ? codes.Last() : "0";

            return AlphaCode.GetNextCode(csCode);
        }

        private void ArticleCostCodeRigen(Article c)
        {
            c.TimeStampTable = DateTime.Now;

            foreach (var item in c.ArticleCosts)
            {
                item.TimeStampTable = DateTime.Now;
            }

            switch (c.TypeOfArticle)
            {
                   
                case Article.ArticleType.SheetPrintableArticle:

                    /*
                    #region Cutted
                    try
                    {
                        var cuttedCost = ((SheetPrintableArticleCuttedCost)c.ArticleCosts.First(x =>
                            x.TypeOfArticleCost == ArticleCost.ArticleCostType.SheetPrintableArticleCuttedCost));

                        cuttedCost.CostPerKg = cuttedCost.CostPerKg == null ?
                            null : Convert.ToDouble(cuttedCost.CostPerKg,
                            Thread.CurrentThread.CurrentUICulture).ToString("#,0.000", Thread.CurrentThread.CurrentUICulture);

                        cuttedCost.CostPerSheet = cuttedCost.CostPerSheet == null ?
                        null :
                        Convert.ToDouble(cuttedCost.CostPerSheet, Thread.CurrentThread.CurrentUICulture).ToString("#,0.000", Thread.CurrentThread.CurrentUICulture);

                        cuttedCost.CodArticle = c.CodArticle;
                        cuttedCost.CodArticleCost = c.CodArticle + "_CTD";

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    #endregion
                    */

                    #region Paked
                    try
                    {
                        var pakedCost = ((SheetPrintableArticlePakedCost)c.ArticleCosts.First(x =>
                            x.TypeOfArticleCost == ArticleCost.ArticleCostType.SheetPrintableArticlePakedCost));

                        pakedCost.CostPerKg = pakedCost.CostPerKg == null ?
                            null : Convert.ToDouble(pakedCost.CostPerKg,
                            Thread.CurrentThread.CurrentUICulture).ToString("#,0.000", Thread.CurrentThread.CurrentUICulture);

                        pakedCost.CostPerSheet = pakedCost.CostPerSheet == null ?
                        null :
                        Convert.ToDouble(pakedCost.CostPerSheet, Thread.CurrentThread.CurrentUICulture).ToString("#,0.000", Thread.CurrentThread.CurrentUICulture);

                        pakedCost.CodArticle = c.CodArticle;
                        pakedCost.CodArticleCost = c.CodArticle + "_PKC";
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    #endregion

                    #region Pallet
                    try
                    {
                        var palletCost = ((SheetPrintableArticlePalletCost)c.ArticleCosts.First(x =>
                        x.TypeOfArticleCost == ArticleCost.ArticleCostType.SheetPrintableArticlePalletCost));

                        palletCost.CostPerKg = palletCost.CostPerKg == null ?
                            null : Convert.ToDouble(palletCost.CostPerKg,
                            Thread.CurrentThread.CurrentUICulture).ToString("#,0.000", Thread.CurrentThread.CurrentUICulture);

                        palletCost.CostPerSheet = palletCost.CostPerSheet == null ?
                        null :
                        Convert.ToDouble(palletCost.CostPerSheet, Thread.CurrentThread.CurrentUICulture).ToString("#,0.0000", Thread.CurrentThread.CurrentUICulture);

                        palletCost.CodArticle = c.CodArticle;
                        palletCost.CodArticleCost = c.CodArticle + "_PLC";
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    #endregion

                    break;
                case Article.ArticleType.RollPrintableArticle:
                    #region Standard
                    try
                    {
                        var standardCost = ((RollPrintableArticleStandardCost)c.ArticleCosts.First(x =>
                        x.TypeOfArticleCost == ArticleCost.ArticleCostType.RollPrintableArticleStandardCost));

                        standardCost.CostPerMq = standardCost.CostPerMq == null ?
                            null : Convert.ToDouble(standardCost.CostPerMq,
                            Thread.CurrentThread.CurrentUICulture).ToString("#,0.000", Thread.CurrentThread.CurrentUICulture);

                        standardCost.CostPerMl = standardCost.CostPerMl == null ?
                        null :
                        Convert.ToDouble(standardCost.CostPerMl, Thread.CurrentThread.CurrentUICulture).ToString("#,0.0000", Thread.CurrentThread.CurrentUICulture);

                        standardCost.CodArticle = c.CodArticle;
                        standardCost.CodArticleCost = c.CodArticle + "_STC";
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    #endregion
                    break;
                case Article.ArticleType.RigidPrintableArticle:
                    #region Standard
                    try
                    {
                        var standardCost = ((RigidPrintableArticleStandardCost)c.ArticleCosts.First(x =>
                        x.TypeOfArticleCost == ArticleCost.ArticleCostType.RigidPrintableArticleStandardCost));

                        standardCost.CostPerMq = standardCost.CostPerMq == null ?
                            null : Convert.ToDouble(standardCost.CostPerMq,
                            Thread.CurrentThread.CurrentUICulture).ToString("#,0.000", Thread.CurrentThread.CurrentUICulture);

                        standardCost.CodArticle = c.CodArticle;
                        standardCost.CodArticleCost = c.CodArticle + "_STC";
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    #endregion
                    break;
                case Article.ArticleType.ObjectPrintableArticle:
                    break;
                default:
                    break;
            }
        }

        public override void Add(Article entity)
        {
            ArticleCostCodeRigen(entity);

            //cehck if name is just inserted
            var article = (from ART in this.GetAll() where ART.ArticleName == entity.ArticleName select ART);

            Console.Write(article.Count());
            
            if (article.Count() > 0)
            {
                //this.Edit(entity);
            }
            else
                base.Add(entity);
        }

        public override IQueryable<Article> GetAll()
        {
            Console.WriteLine(Context.Database.Connection.ConnectionString);
            return Context.articles.Include("articlecosts").Include("CustomerSupplierMaker").Include("CustomerSupplierBuy");
        }

        public override void Edit(Article entity)
        {
            ArticleCostCodeRigen(entity);

            foreach (var item in entity.ArticleCosts)
            {
                Context.Entry(item).State = System.Data.EntityState.Modified;
            }
            base.Edit(entity);
        }

        public Article GetSingle(string codArticle)
        {
            var query = Context.articles.Include("articlecosts").Include("CustomerSupplierMaker").Include("CustomerSupplierBuy").FirstOrDefault(x => x.CodArticle == codArticle);
            return query;
        }

        public override void SetDbName(string name)
        {
            base.SetDbName(name);
        }
    }
}