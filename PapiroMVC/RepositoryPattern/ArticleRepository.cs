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

            a.CodSupplierMaker = filteredItems.Single().CodCustomerSupplier;

            customerSuppliers = customerSupplierRepository.GetAll().ToArray();

            var filteredItems2 = customerSuppliers.Where(
                item => item.BusinessName.IndexOf(supplyerBuy, StringComparison.InvariantCultureIgnoreCase) >= 0);

            if (filteredItems2.Count() == 0) throw new Exception();

            //if #suppliers < 1 then no supplier has selected correctly and then thow error
            a.CodSupplierBuy = filteredItems2.Single().CodCustomerSupplier;

            var codes = (from COD in this.GetAll() select COD.CodArticle).ToArray().OrderBy(x => x, new SemiNumericComparer());
            var csCode = codes.Count() != 0 ? codes.Last() : "0";

            return AlphaCode.GetNextCode(csCode);
        }

        private void ArticleCostCodeRigen(Article c)
        {
            switch (c.TypeOfArticle)
            {
                case Article.ArticleType.SheetPrintableArticle:

                    /*CUTTED
                    ((SheetPrintableArticleCuttedCost)c.ArticleCosts.First(x =>
                        x.TypeOfArticleCost == ArticleCost.ArticleCostType.SheetPrintableArticleCuttedCost)).CodArticle = c.CodArticle;
                    ((SheetPrintableArticleCuttedCost)c.ArticleCosts.First(x =>
                        x.TypeOfArticleCost == ArticleCost.ArticleCostType.SheetPrintableArticleCuttedCost)).CodArticleCost = c.CodArticle + "_CTC";
                    */
                    #region Paked
                    var pakedCost = ((SheetPrintableArticlePakedCost)c.ArticleCosts.First(x =>
                        x.TypeOfArticleCost == ArticleCost.ArticleCostType.SheetPrintableArticlePakedCost));

                    pakedCost.CostPerKg = pakedCost.CostPerKg == null?
                        null:Convert.ToDouble(pakedCost.CostPerKg,
                        Thread.CurrentThread.CurrentUICulture).ToString("#,0.000",Thread.CurrentThread.CurrentUICulture);
                    
                        pakedCost.CostPerSheet = pakedCost.CostPerSheet == null ? 
                        null : 
                        Convert.ToDouble(pakedCost.CostPerSheet, Thread.CurrentThread.CurrentUICulture).ToString("#,0.000", Thread.CurrentThread.CurrentUICulture);

                    pakedCost.CodArticle = c.CodArticle;
                    pakedCost.CodArticleCost = c.CodArticle + "_PKC";

                    #endregion

                    #region Pallet
                    var palletCost = ((SheetPrintableArticlePalletCost)c.ArticleCosts.First(x =>
                        x.TypeOfArticleCost == ArticleCost.ArticleCostType.SheetPrintableArticlePalletCost));

                    palletCost.CostPerKg = palletCost.CostPerKg == null?
                        null:Convert.ToDouble(palletCost.CostPerKg,
                        Thread.CurrentThread.CurrentUICulture).ToString("#,0.000",Thread.CurrentThread.CurrentUICulture);
                    
                        palletCost.CostPerSheet = palletCost.CostPerSheet == null ? 
                        null : 
                        Convert.ToDouble(palletCost.CostPerSheet, Thread.CurrentThread.CurrentUICulture).ToString("#,0.0000", Thread.CurrentThread.CurrentUICulture);

                    palletCost.CodArticle = c.CodArticle;
                    palletCost.CodArticleCost = c.CodArticle + "_PLC";

                    #endregion

                    break;
                case Article.ArticleType.RollPrintableArticle:
                    break;
                case Article.ArticleType.RigidPrintableArticle:
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