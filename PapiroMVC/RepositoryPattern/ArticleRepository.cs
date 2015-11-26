using System;
using System.Linq;
using PapiroMVC.Models;
using PapiroMVC.DbCodeManagement;
using System.Threading;
using PapiroMVC.Validation;

namespace Services
{

    public class ArticleRepository : GenericRepository<dbEntities, Article>, IArticleRepository
    {

        /// <summary>
        /// if supplier is noi in reporitory than create it
        /// </summary>
        public void SincroSupplier(Article a,
            ICustomerSupplierRepository customerSupplierRepository,
            string supplierMaker,
            string supplyerBuy)
        {


            if (!String.IsNullOrEmpty(supplierMaker) && !String.IsNullOrEmpty(supplyerBuy))
            {
                CustomerSupplier[] customerSuppliers = customerSupplierRepository.GetAll().ToArray();

                var filteredItems = customerSuppliers.Where(
                    item => !String.IsNullOrEmpty(item.BusinessName) && item.BusinessName.IndexOf(supplierMaker, StringComparison.InvariantCultureIgnoreCase) >= 0);

                if (filteredItems.Count() == 0)
                {
                    var sup = new Supplier();
                    sup.BusinessName = supplierMaker;
                    sup.CodCustomerSupplier = customerSupplierRepository.GetNewCode(sup);

                    customerSupplierRepository.Add(sup);
                    customerSupplierRepository.Save();
                    a.CodSupplierMaker = sup.CodCustomerSupplier;

                }
                else
                {

                    a.CodSupplierMaker = filteredItems.First().CodCustomerSupplier;

                }

                customerSuppliers = customerSupplierRepository.GetAll().ToArray();

                var filteredItems2 = customerSuppliers.Where(
                    item => !String.IsNullOrEmpty(item.BusinessName) && item.BusinessName.IndexOf(supplyerBuy, StringComparison.InvariantCultureIgnoreCase) >= 0);

                if (filteredItems2.Count() == 0)
                {
                    var sup = new Supplier();
                    sup.BusinessName = supplyerBuy;
                    sup.CodCustomerSupplier = customerSupplierRepository.GetNewCode(sup);

                    customerSupplierRepository.Add(sup);
                    customerSupplierRepository.Save();
                    a.CodSupplierBuy = sup.CodCustomerSupplier;
                }
                else
                {
                    a.CodSupplierBuy = filteredItems2.First().CodCustomerSupplier;
                }
            }


        }
        public string GetNewCode(Article a,
            ICustomerSupplierRepository customerSupplierRepository,
            string supplierMaker,
            string supplyerBuy)
        {

            this.SincroSupplier(a, customerSupplierRepository, supplierMaker, supplierMaker);
            var csCode = (from COD in this.GetAll() select COD.CodArticle).Max();

            return AlphaCode.GetNextCode(csCode ?? "0").PadLeft(6, '0');
        }

        private void ArticleCostCodeRigen(Article c)
        {

            c.ArticleName = c.ToString();
            c.TimeStampTable = DateTime.Now;

            foreach (var item in c.ArticleCosts)
            {
                item.TimeStampTable = DateTime.Now;
            }

            #region Magazzeno

            try
            {
                foreach (var warehouseInfo in c.WarehouseArticles)
                {
                    warehouseInfo.TimeStampTable = DateTime.Now;

                    warehouseInfo.CodArticle = c.CodArticle;
                    //tengo uguale tanto l'associazione è 1:1 + "A" davanti
                    warehouseInfo.CodWarehouseArticle = warehouseInfo.CodWarehouse + "A" + c.CodArticle;
                }

            }
            catch (Exception)
            { }
            #endregion




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
                        Convert.ToDouble(palletCost.CostPerSheet, Thread.CurrentThread.CurrentUICulture).ToString("#,0.00000", Thread.CurrentThread.CurrentUICulture);

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
                            Thread.CurrentThread.CurrentUICulture).ToString("#,0.00000", Thread.CurrentThread.CurrentUICulture);

                        standardCost.CostPerMl = standardCost.CostPerMl == null ?
                            null : Convert.ToDouble(standardCost.CostPerMl, Thread.CurrentThread.CurrentUICulture).ToString("#,0.00000", Thread.CurrentThread.CurrentUICulture);

                        standardCost.CostPerKg = standardCost.CostPerKg == null ?
                            null : Convert.ToDouble(standardCost.CostPerKg,
                            Thread.CurrentThread.CurrentUICulture).ToString("#,0.00000", Thread.CurrentThread.CurrentUICulture);

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
                case Article.ArticleType.NoPrintable:
                    #region Standard
                    try
                    {
                        var CostStandard = ((NoPrintableArticleCostStandard)c.ArticleCosts.First(x =>
                        x.TypeOfArticleCost == ArticleCost.ArticleCostType.NoPrintableArticleCostStandard));

                        CostStandard.CostPerUnit = CostStandard.CostPerUnit == null ?
                            null : Convert.ToDouble(CostStandard.CostPerUnit,
                            Thread.CurrentThread.CurrentUICulture).ToString("#,0.000", Thread.CurrentThread.CurrentUICulture);

                        CostStandard.CodArticle = c.CodArticle;
                        CostStandard.CodArticleCost = c.CodArticle + "_STC";
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    #endregion
                    break;
                case Article.ArticleType.Ink:
                    #region Standard
                    try
                    {
                        var CostStandard = ((NoPrintableArticleCostKg)c.ArticleCosts.First(x =>
                        x.TypeOfArticleCost == ArticleCost.ArticleCostType.NoPrintableArticleCostKg));

                        CostStandard.CostPerKg = CostStandard.CostPerKg == null ?
                            null : Convert.ToDouble(CostStandard.CostPerKg,
                            Thread.CurrentThread.CurrentUICulture).ToString("#,0.000", Thread.CurrentThread.CurrentUICulture);

                        CostStandard.CodArticle = c.CodArticle;
                        CostStandard.CodArticleCost = c.CodArticle + "_STK";
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    #endregion
                    break;
                case Article.ArticleType.Foil:
                case Article.ArticleType.Mesh:
                case Article.ArticleType.Anilox:
                    #region Standard
                    try
                    {
                        var CostStandard = ((NoPrintableArticleCostMq)c.ArticleCosts.First(x =>
                        x.TypeOfArticleCost == ArticleCost.ArticleCostType.NoPrintableArticleCostMq));

                        CostStandard.CostPerMq = CostStandard.CostPerMq == null ?
                            null : Convert.ToDouble(CostStandard.CostPerMq,
                            Thread.CurrentThread.CurrentUICulture).ToString("#,0.000", Thread.CurrentThread.CurrentUICulture);

                        CostStandard.CodArticle = c.CodArticle;
                        CostStandard.CodArticleCost = c.CodArticle + "_STM";
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    #endregion
                    break;
                default:
                    break;
            }



        }

        public override void Add(Article entity)
        {

            ArticleCostCodeRigen(entity);

            ////cehck if name is just inserted
            //var article = (from ART in this.GetAll() where ART.ArticleName == entity.ArticleName select ART);

            //Console.Write(article.Count());

            //if (article.Count() > 0)
            //{
            //    //this.Edit(entity);
            //}
            //else
            base.Add(entity);



        }

        public override IQueryable<Article> GetAll()
        {


            //We have to fix NoUseInEstimateCalculation
            var toFix = Context.articles.OfType<Printable>().Where(x => x.NoUseInEstimateCalculation == null).ToList();

            foreach (var item in toFix)
            {
                item.NoUseInEstimateCalculation = false;
                Edit(item);
            }
            if (toFix.Count() > 0)
            {
                Save();
            }




            var numWare = Context.warehouseSpec.Count();

            //update with new warehouse
            var lstProd = Context.articles.AsNoTracking().Include("articlecosts").Where(x => x.WarehouseArticles.Count < numWare).ToList();

            foreach (var article in lstProd)
            {
                if (numWare != article.WarehouseArticles.Count())
                {
                    foreach (var warehouse in Context.warehouseSpec)
                    {
                        //Does article esist in Warehouse?
                        var res = article.WarehouseArticles.Where(x => x.CodWarehouse == warehouse.CodWarehouse);
                        //no warehouse
                        if (res.Count() == 0)
                        {
                            var newP = new WarehouseArticle
                            {
                                CodWarehouse = warehouse.CodWarehouse,
                                CodArticle = article.CodArticle
                            };

                            article.WarehouseArticles.Add(newP);
                            //this.Context.Set<WarehouseArticle>().Add(newP);
                        }
                    }
                }
                this.ArticleCostCodeRigen(article);

                this.Edit(article);

                Save();
            }


            Console.WriteLine(Context.Database.Connection.ConnectionString);
            return Context.articles.Include("articlecosts").Include("CustomerSupplierMaker").Include("CustomerSupplierBuy").Include("warehousearticles");
        }

        public virtual IQueryable<Article> GetForImport()
        {
            Console.WriteLine(Context.Database.Connection.ConnectionString);
            return Context.articles.Include("articlecosts");
        }

        public override void Edit(Article entity)
        {
            ArticleCostCodeRigen(entity);

            var fromBD = Context.articles.SingleOrDefault(p => p.CodArticle == entity.CodArticle);
            if (fromBD != null)
            {
                Context.Entry(fromBD).CurrentValues.SetValues(entity);
                Context.Entry(fromBD).State = System.Data.Entity.EntityState.Modified;

                foreach (var item in entity.WarehouseArticles)
                {
                    var fromBDW = Context.warehousearticles.SingleOrDefault(p => p.CodWarehouseArticle == item.CodWarehouseArticle);
                    if (fromBDW != null)
                    {
                        Context.Entry(fromBDW).CurrentValues.SetValues(item);
                        Context.Entry(fromBDW).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        Context.Entry(item).State = System.Data.Entity.EntityState.Added;
                    }
                }


                foreach (var item in entity.ArticleCosts)
                {
                    var fromBDC = Context.articlecost.SingleOrDefault(p => p.CodArticleCost == item.CodArticleCost);
                    if (fromBDC != null)
                    {
                        Context.Entry(fromBDC).CurrentValues.SetValues(item);
                        Context.Entry(fromBDC).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        Context.Entry(item).State = System.Data.Entity.EntityState.Added;
                    }
                }


            }
            else
            {
                Context.Entry(entity).State = System.Data.Entity.EntityState.Added;
            }



            //foreach (var item in entity.ArticleCosts)
            //{
            //    Context.Entry(item).State = System.Data.Entity.EntityState.Modified;
            //}

            //foreach (var item in entity.WarehouseArticles)
            //{
            //        var fromBDC = Context.warehousearticles.SingleOrDefault(p => p.CodArticle == entity.CodArticle);
            //        if (fromBDC != null)
            //        {
            //            Context.Entry(item).State = System.Data.Entity.EntityState.Modified;
            //        }
            //        else
            //        {
            //            Context.Entry(item).State = System.Data.Entity.EntityState.Added;
            //        }                
            //}

            //base.Edit(entity);
        }

        public new Article GetSingle(string codArticle)
        {
            var article = Context.articles.Include("articlecosts").Include("CustomerSupplierMaker").Include("CustomerSupplierBuy").Include("warehousearticles").FirstOrDefault(x => x.CodArticle == codArticle);



            if (article == null)
            {
                return null;
            }


            var numWare = Context.warehouseSpec.Count();

            if (numWare != article.WarehouseArticles.Count())
            {
                foreach (var warehouse in Context.warehouseSpec)
                {
                    //Does article esist in Warehouse?
                    var res = article.WarehouseArticles.Where(x => x.CodWarehouse == warehouse.CodWarehouse);
                    //no warehouse
                    if (res.Count() == 0)
                    {
                        var newP = new WarehouseArticle
                        {
                            CodWarehouse = warehouse.CodWarehouse,
                            CodArticle = article.CodArticle
                        };

                        article.WarehouseArticles.Add(newP);
                        this.Context.Set<WarehouseArticle>().Add(newP);
                    }
                }
            }
            this.ArticleCostCodeRigen(article);
            Save();

            return article;
        }

        public override void SetDbName(string name)
        {
            base.SetDbName(name);
        }

    }
}