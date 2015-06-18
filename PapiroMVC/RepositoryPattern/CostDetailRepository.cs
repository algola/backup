using System;
using System.Linq;
using PapiroMVC.Models;
using PapiroMVC.DbCodeManagement;
using System.Threading;
using System.Collections.Generic;
using System.Data.Entity;

namespace Services
{
    public class CostDetailRepository : GenericRepository<dbEntities, CostDetail>, ICostDetailRepository
    {
        private Dictionary<string, CostDetail> cache;
        protected Dictionary<string, CostDetail> Cache
        {
            get
            {
                if (cache == null)
                {
                    cache = new Dictionary<string, CostDetail>();
                }
                return cache;
            }
            set
            {
                cache = value;
            }
        }

        private Dictionary<string, CostDetail> cacheComputedBy;
        protected Dictionary<string, CostDetail> CacheComputedBy
        {
            get
            {
                if (cacheComputedBy == null)
                {
                    cacheComputedBy = new Dictionary<string, CostDetail>();
                }
                return cacheComputedBy;
            }
            set
            {
                cacheComputedBy = value;
            }
        }

        private Dictionary<string, ProductPartPrinting> cacheProductPartPrinting;
        protected Dictionary<string, ProductPartPrinting> CacheProductPartPrinting
        {
            get
            {
                if (cacheProductPartPrinting == null)
                {
                    cacheProductPartPrinting = new Dictionary<string, ProductPartPrinting>();
                }
                return cacheProductPartPrinting;
            }
            set
            {
                cacheProductPartPrinting = value;
            }
        }

        private void CostDetailCostCodeRigen(CostDetail c)
        {
            c.CostDetailCostCodeRigen();
        }

        public override void Add(CostDetail entity)
        {
            CostDetailCostCodeRigen(entity);

            var costDetail = Context.CostDetail.Where(x => x.CodCost == entity.CodCost);

            if (costDetail.Count() > 0)
            {
                this.Edit(entity);
            }
            else
            {
                this.EditAdd(entity);
            }
        }

        public override void Edit(CostDetail entity)
        {

            if (entity.TaskCost != null)
            {

                var fromBDC = Context.Costs.SingleOrDefault(p => p.CodCost == entity.CodCostDetail);
                if (fromBDC != null)
                {
                    Context.Entry(fromBDC).CurrentValues.SetValues(entity.TaskCost);
                    Context.Entry(fromBDC).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    Context.Entry(entity).State = System.Data.Entity.EntityState.Added;
                }
            }

            var fromBD = Context.CostDetail.SingleOrDefault(p => p.CodCostDetail == entity.CodCostDetail);

            if (fromBD != null)
            {

                Context.Entry(fromBD).CurrentValues.SetValues(entity);
                Context.Entry(fromBD).State = System.Data.Entity.EntityState.Modified;
                //se è' da modificare ma è contrassegnato come added faccio che lo segno come unmodificato

                if (entity.Computes != null)
                {
                    foreach (var item in entity.Computes)
                    {
                        item.CodComputedBy = entity.CodCostDetail;
                        var fromBDComp = Context.CostDetail.Single(p => p.CodCostDetail == item.CodCostDetail);
                        Context.Entry(fromBDComp).CurrentValues.SetValues(item);
                        Context.Entry(fromBDComp).State = System.Data.Entity.EntityState.Modified;

                    }
                }

                if (entity.ProductPartPrinting != null)
                {
                    var prdPartPrt = entity.ProductPartPrinting;
                    var fromBD2 = Context.ProductPartPrinting.Single(p => p.CodProductPartPrinting == prdPartPrt.CodProductPartPrinting);
                    Context.Entry(fromBD2).CurrentValues.SetValues(prdPartPrt);
                    Context.Entry(fromBD2).State = System.Data.Entity.EntityState.Modified;

                    if (prdPartPrt.GainPartOnPrinting != null)
                    {
                        var prdPartPrtGain = prdPartPrt.GainPartOnPrinting;
                        var fromBD3 = Context.ProductPartPrintingGain.Single(p => p.CodProductPartPrintingGain == prdPartPrtGain.CodProductPartPrintingGain);
                        Context.Entry(fromBD3).CurrentValues.SetValues(prdPartPrtGain);
                        Context.Entry(fromBD3).State = System.Data.Entity.EntityState.Modified;

                        //devo comportarmi in 2 diversi modi a seconda del fatto che le makereadies siano maggiori nel db o in memoria dopo le modifiche

                        //caso in cui le makereadies in memoria sono > di quelle del db
                        foreach (var mkr in prdPartPrtGain.Makereadies)
                        {
                            try
                            {
                                var fromBD4 = Context.Makereadies.Single(p => p.CodMakeready == mkr.CodMakeready);
                                Context.Entry(fromBD4).CurrentValues.SetValues(mkr);
                                Context.Entry(fromBD4).State = System.Data.Entity.EntityState.Modified;

                            }
                            catch (SystemException e)
                            {
                                Context.Entry(mkr).State = System.Data.Entity.EntityState.Added;
                            }
                        }

                        var isEccesso = Context.Makereadies.Where(x => x.CodProductPartPrintingGain == prdPartPrtGain.CodProductPartPrinting);

                        //caso in cui nel db ho dell'eccesso
                        foreach (var mkr in isEccesso)
                        {
                            if (prdPartPrtGain.Makereadies.Where(x => x.CodMakeready == mkr.CodMakeready).Count() == 0)
                            {
                                Context.Makereadies.Remove(mkr);
                                Context.Entry(mkr).State = System.Data.Entity.EntityState.Deleted;
                            }
                        }

                    }

                }

                if (entity.GainPrintingOnBuying != null)
                {
                    var prtGain = entity.GainPrintingOnBuying;
                    var fromBD3 = Context.ProductPartPrintingGain.Single(p => p.CodProductPartPrintingGainBuying == prtGain.CodProductPartPrintingGainBuying);
                    Context.Entry(fromBD3).CurrentValues.SetValues(prtGain);
                    Context.Entry(fromBD3).State = System.Data.Entity.EntityState.Modified;

                    foreach (var mkr in prtGain.Makereadies)
                    {
                        var fromBD4 = Context.Makereadies.Single(p => p.CodMakeready == mkr.CodMakeready);
                        Context.Entry(fromBD4).CurrentValues.SetValues(mkr);
                        Context.Entry(fromBD4).State = System.Data.Entity.EntityState.Modified;
                    }
                }


            }
            else
            {
                Context.Entry(entity).State = System.Data.Entity.EntityState.Added;
            }


            //*******************************************************************
            List<CostDetail> Added2 = Context.ChangeTracker.Entries()
            .Where(x => x.State == System.Data.Entity.EntityState.Added)
            .Select(x => x.Entity).OfType<CostDetail>().ToList();

            Console.WriteLine(Added2);
            //*******************************************************************



        }

        protected void EditAdd(CostDetail entity)
        {
            CostDetailCostCodeRigen(entity);


            //if (entity.TaskCost != null)
            //{
            //    var fromBDC = Context.Costs.SingleOrDefault(p => p.CodCost == entity.CodCostDetail);
            //    if (fromBDC != null)
            //    {
            //        Context.Entry(fromBDC).CurrentValues.SetValues(entity.TaskCost);
            //        Context.Entry(fromBDC).State = System.Data.Entity.EntityState.Modified;
            //    }
            //    else
            //    {
            //        Context.Entry(entity).State = System.Data.Entity.EntityState.Added;
            //    }
            //}

            var fromBD = Context.CostDetail.SingleOrDefault(p => p.CodCostDetail == entity.CodCostDetail);
            if (fromBD != null)
            {
                Context.Entry(fromBD).CurrentValues.SetValues(entity);
                Context.Entry(fromBD).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                Context.Set<CostDetail>().Add(entity);
                //foreach (var entry in Context.ChangeTracker.Entries<CostDetail>())
                //{
                //    if (entry.State == EntityState.Added && entry.Entity != entity)
                //    {
                //        entry.State = EntityState.Unchanged;
                //    }
                //}


                List<Object> modOrAdded = Context.ChangeTracker.Entries().Where(x =>
                    x.State == System.Data.Entity.EntityState.Added).Select(x => x.Entity).ToList();

                Console.Write(modOrAdded);


                //#region provo a togliere le cose superflue
                ////non aggiungo i productpartprinting se già sono presenti
                //foreach (var entry in Context.ChangeTracker.Entries<ProductPartPrinting>())
                //{
                //    try
                //    {
                //        var fromDBEntity = Context.ProductPartPrinting.FirstOrDefault(p => p.CodProductPartPrinting == entry.Entity.CodProductPartPrinting);
                //        if (fromDBEntity != null)
                //        {
                //            Context.Entry(fromDBEntity).State = System.Data.Entity.EntityState.Unchanged;
                //        }

                //    }
                //    catch (Exception)
                //    {
                //        entry.State = System.Data.Entity.EntityState.Unchanged;
                //    }
                //}


                ////non aggiungo i ProductPartPrintingGain se già sono presenti
                //foreach (var entry in Context.ChangeTracker.Entries<ProductPartPrintingGain>())
                //{
                //    var fromDBEntity = Context.ProductPartPrintingGain.SingleOrDefault(p => p.CodProductPartPrintingGain == entry.Entity.CodProductPartPrintingGain);
                //    if (fromDBEntity != null)
                //    {
                //        Context.Entry(fromDBEntity).State = System.Data.Entity.EntityState.Unchanged;
                //    }
                //}

                ////non aggiungo i Makeready se già sono presenti
                //foreach (var entry in Context.ChangeTracker.Entries<Makeready>())
                //{
                //    var fromDBEntity = Context.Makereadies.SingleOrDefault(p => p.CodMakeready == entry.Entity.CodMakeready);
                //    if (fromDBEntity != null)
                //    {
                //        Context.Entry(fromDBEntity).State = System.Data.Entity.EntityState.Unchanged;
                //    }
                //}

                //#endregion


                List<CostDetail> Added2 = Context.ChangeTracker.Entries()
                .Where(x => x.State == System.Data.Entity.EntityState.Added)
                .Select(x => x.Entity).OfType<CostDetail>().ToList();

                Console.WriteLine(Added2);


            }

            if (entity.Computes != null)
            {
                foreach (var item in entity.Computes)
                {
                    item.CodComputedBy = entity.CodCostDetail;
                    var fromBDComp = Context.CostDetail.SingleOrDefault(p => p.CodCostDetail == item.CodCostDetail);
                    if (fromBDComp != null)
                    {
                        Context.Entry(fromBDComp).CurrentValues.SetValues(item);
                        Context.Entry(fromBDComp).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        Context.Entry(item).State = System.Data.Entity.EntityState.Added;
                    }
                }
            }

            if (entity.ProductPartPrinting != null)
            {
                var prdPartPrt = entity.ProductPartPrinting;
                var fromBD2 = Context.ProductPartPrinting.SingleOrDefault(p => p.CodProductPartPrinting == prdPartPrt.CodProductPartPrinting);

                if (fromBD2 != null)
                {
                    Context.Entry(fromBD2).CurrentValues.SetValues(prdPartPrt);
                    Context.Entry(fromBD2).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    Context.Entry(prdPartPrt).State = System.Data.Entity.EntityState.Added;
                }

                if (prdPartPrt.GainPartOnPrinting != null)
                {
                    var prdPartPrtGain = prdPartPrt.GainPartOnPrinting;
                    var fromBD3 = Context.ProductPartPrintingGain.SingleOrDefault(p => p.CodProductPartPrintingGain == prdPartPrtGain.CodProductPartPrintingGain);

                    if (fromBD3 != null)
                    {
                        Context.Entry(fromBD3).CurrentValues.SetValues(prdPartPrtGain);
                        Context.Entry(fromBD3).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        Context.Entry(prdPartPrtGain).State = System.Data.Entity.EntityState.Added;
                    }

                    //devo comportarmi in 2 diversi modi a seconda del fatto che le makereadies siano maggiori nel db o in memoria dopo le modifiche

                    //caso in cui le makereadies in memoria sono > di quelle del db
                    foreach (var mkr in prdPartPrtGain.Makereadies)
                    {
                        try
                        {
                            var fromBD4 = Context.Makereadies.SingleOrDefault(p => p.CodMakeready == mkr.CodMakeready);
                            Context.Entry(fromBD4).CurrentValues.SetValues(mkr);
                        }
                        catch (SystemException e)
                        {
                            Context.Entry(mkr).State = System.Data.Entity.EntityState.Added;
                        }
                    }

                    var isEccesso = Context.Makereadies.Where(x => x.CodProductPartPrintingGain == prdPartPrtGain.CodProductPartPrinting);

                    //caso in cui nel db ho dell'eccesso
                    foreach (var mkr in isEccesso)
                    {
                        if (prdPartPrtGain.Makereadies.Where(x => x.CodMakeready == mkr.CodMakeready).Count() == 0)
                        {
                            Context.Makereadies.Remove(mkr);
                            Console.WriteLine("");
                        }
                    }

                }

            }

            if (entity.GainPrintingOnBuying != null)
            {
                var prtGain = entity.GainPrintingOnBuying;
                var fromBD3 = Context.ProductPartPrintingGain.SingleOrDefault(p => p.CodProductPartPrintingGainBuying == prtGain.CodProductPartPrintingGainBuying);

                if (fromBD3 != null)
                {
                    Context.Entry(fromBD3).CurrentValues.SetValues(prtGain);
                    Context.Entry(fromBD3).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    Context.Entry(prtGain).State = System.Data.Entity.EntityState.Added;
                }

                foreach (var mkr in prtGain.Makereadies)
                {
                    var fromBD4 = Context.Makereadies.SingleOrDefault(p => p.CodMakeready == mkr.CodMakeready);

                    if (fromBD4 != null)
                    {
                        Context.Entry(fromBD4).CurrentValues.SetValues(mkr);
                    }
                    else
                    {
                        Context.Entry(mkr).State = System.Data.Entity.EntityState.Added;
                    }


                }
            }

        }

        public override void SetDbName(string name)
        {
            base.SetDbName(name);
        }

        public override void Save()
        {
            List<Object> modOrAdded = Context.ChangeTracker.Entries()
                .Where(x => x.State == System.Data.Entity.EntityState.Modified
                || x.State == System.Data.Entity.EntityState.Added)
                .Select(x => x.Entity).ToList();

            var notAll = modOrAdded
                //  .Except(modOrAdded.OfType<Cost>())
               .Except(modOrAdded.OfType<CostDetail>())
               .Except(modOrAdded.OfType<ProductPartPrinting>())
               .Except(modOrAdded.OfType<ProductPartPrintingGain>())
               .Except(modOrAdded.OfType<Makeready>());

            foreach (var item in notAll)
            {
                Context.Entry(item).State = System.Data.Entity.EntityState.Modified;
            }

            base.Save();

        }

        public bool IsJustSaved(string id, Guid guid)
        {
            var g = guid.ToString("N");
            var rr = Context.CostDetail.SingleOrDefault(x => x.CodCostDetail == id);

            if (rr != null)
            {
                return (rr.Guid.ToString() == g);
            }
            else
            {
                return false;
            }
        }

        public CostDetail OGetSingleOld(string Cod)
        {
            try
            {
                return Cache[Cod];
            }

            catch (Exception)
            {

                var codProduct = String.Empty;

                var inizio = DateTime.Now;
                var ret = Context.CostDetail.Include(x => x.Computes).SingleOrDefault(x => x.CodCostDetail == Cod);

                if (ret != null)
                {
                    ret.GainPrintingOnBuyings = Context.ProductPartPrintingGain.Where(x => x.CodProductPartPrintingGainBuying == ret.CodCost).Include("Makereadies").ToList();
                    ret.ComputedBy = Context.CostDetail.SingleOrDefault(x => x.CodCost == ret.CodComputedBy);

                    ret.ProductPart = Context.ProductParts
                        .SingleOrDefault(x => x.CodProductPart == ret.CodProductPart);

                    codProduct = ret.ProductPart != null ? ret.ProductPart.CodProduct : "";

                    #region ProductPartPrinting
                    //cerco il productPartPrinting nella cache
                    try
                    {
                        var retPartPrinting = CacheProductPartPrinting[ret.CodCostDetail];
                        ret.ProductPartPrinting = retPartPrinting;
                    }
                    catch (Exception)
                    {
                        ret.ProductPartPrinting = Context.ProductPartPrinting.SingleOrDefault(x => x.CodProductPartPrinting == ret.CodCostDetail);

                        if (ret.ProductPartPrinting != null)
                        {
                            ret.ProductPartPrinting.Part = Context.ProductParts.SingleOrDefault(x => x.CodProductPart == ret.ProductPartPrinting.CodProductPart);
                            ret.ProductPartPrinting.GainPartOnPrintings = Context.ProductPartPrintingGain.Where(x => x.CodProductPartPrinting == ret.ProductPartPrinting.CodProductPartPrinting).Include("Makereadies").ToList();
                            CacheProductPartPrinting.Add(ret.CodCost, ret.ProductPartPrinting);
                        }

                    }
                    #endregion


                    ret.TaskCost = Context.Costs        //.SingleOrDefault(x => x.CodCost == ret.CodCostDetail)
                        .SingleOrDefault(x => x.CodCost == ret.CodCostDetail);

                    if (ret.TaskCost != null)
                    {
                        var c = ret.TaskCost;
                        c.ProductPartTask = Context.ProductPartTasks.Include("OptionTypeOfTask").SingleOrDefault(x => x.CodProductPartTask == c.CodProductPartTask);
                        if (c.ProductPartTask != null)
                        {
                            c.ProductPartTask.ProductPart = Context.ProductParts.SingleOrDefault(x => x.CodProductPart == c.ProductPartTask.CodProductPart);
                            if (c.ProductPartTask.ProductPart != null)
                            {
                                c.ProductPartTask.ProductPart.ProductPartPrintableArticles = Context.ProductPartsPrintableArticles.Include("Costs").Where(x => x.CodProductPart == c.ProductPartTask.ProductPart.CodProductPart).ToList();
                            }
                        
                        
                        
                        }

                        c.ProductTask = Context.ProductTasks.Include("OptionTypeOfTask").SingleOrDefault(x => x.CodProductTask == c.CodProductTask);

                        if (codProduct == "")
                            codProduct = c.ProductTask != null ? c.ProductTask.CodProduct : "";

                        c.ProductPartsPrintableArticle = Context.ProductPartsPrintableArticles.SingleOrDefault(x => x.CodProductPartPrintableArticle == c.CodProductPartPrintableArticle);
                        c.DocumentProduct = Context.DocumentProducts.SingleOrDefault(x => x.CodDocumentProduct == c.CodDocumentProduct);
                        if (c.DocumentProduct != null)
                        {
                            c.DocumentProduct.Document = Context.Documents.SingleOrDefault(x => x.CodDocument == c.DocumentProduct.CodDocument);
                            if (c.DocumentProduct.Document != null)
                            {
                            }
                            c.DocumentProduct.Costs = Context.Costs.Where(x => x.CodDocumentProduct == c.DocumentProduct.CodDocumentProduct).ToList();
                            //                    .Include(x => x.DocumentProduct.Document.DocumentProducts)
                            //                    .Include("DocumentProduct.Document.DocumentProducts.Costs")
                        }
                    }

                    if (ret.ComputedBy != null)
                    {
                        try
                        {
                            ret.ComputedBy = CacheComputedBy[ret.ComputedBy.CodCostDetail];
                            Console.WriteLine("ciao");
                        }
                        catch (Exception)
                        {
                            ret.ComputedBy = this.GetSingle(ret.ComputedBy.CodCost);
                        }
                    }

                    ////voglio caricare tutti i costi precedenti ed assegnarli a Previouses
                    //var gra = Context.ProductGraphLinks.Where(x => x.CodProduct == codProduct && x.CodItemGraphLink == ret.TaskCost.CodItemGraph);
                    //foreach (var item in gra)
                    //{
                    //    Console.WriteLine(item.CodItemGraphLink);
                    //}

                    ret.CodPartPrintingCostDetail = Context.Costs.Where(x => x.CodDocumentProduct == ret.TaskCost.CodDocumentProduct && x.CodItemGraph == "ST").Select(y => y.CodCost);
                    ret.TaskexEcutorSelected = Context.taskexecutors.Where(x => x.CodTaskExecutor == ret.CodTaskExecutorSelected).FirstOrDefault();

                }

                try
                {
                    CacheComputedBy.Add(ret.CodCost, ret);
                }
                catch (Exception)
                {

                }

                Console.Write(codProduct);
                var tempo = DateTime.Now.Subtract(inizio);
                Console.Write(tempo);

                return ret;
            }
        }


        public override CostDetail GetSingle(string Cod)
        {

            var codProduct = String.Empty;

            var inizio = DateTime.Now;
            var ret = Context.CostDetail.Include(x => x.Computes).SingleOrDefault(x => x.CodCostDetail == Cod);

            if (ret != null)
            {
                ret.GainPrintingOnBuyings = Context.ProductPartPrintingGain.Where(x => x.CodProductPartPrintingGainBuying == ret.CodCost).Include("Makereadies").ToList();
                ret.ComputedBy = Context.CostDetail.SingleOrDefault(x => x.CodCost == ret.CodComputedBy);

                ret.ProductPart = Context.ProductParts
                    .SingleOrDefault(x => x.CodProductPart == ret.CodProductPart);

                codProduct = ret.ProductPart != null ? ret.ProductPart.CodProduct : "";

                #region ProductPartPrinting
                //cerco il productPartPrinting nella cache
                try
                {
                    var retPartPrinting = CacheProductPartPrinting[ret.CodCostDetail];
                    ret.ProductPartPrinting = retPartPrinting;
                }
                catch (Exception)
                {
                    ret.ProductPartPrinting = Context.ProductPartPrinting.SingleOrDefault(x => x.CodProductPartPrinting == ret.CodCostDetail);

                    if (ret.ProductPartPrinting != null)
                    {
                        ret.ProductPartPrinting.Part = Context.ProductParts.SingleOrDefault(x => x.CodProductPart == ret.ProductPartPrinting.CodProductPart);
                        ret.ProductPartPrinting.GainPartOnPrintings = Context.ProductPartPrintingGain.Where(x => x.CodProductPartPrinting == ret.ProductPartPrinting.CodProductPartPrinting).Include("Makereadies").ToList();

                        if (ret.ProductPartPrinting.Part != null)
                        {
                            ret.ProductPartPrinting.Part.ProductPartPrintableArticles = Context.ProductPartsPrintableArticles.AsNoTracking().Where(x => x.CodProductPart == ret.ProductPartPrinting.Part.CodProductPart).ToList();
                        }

                    }

                }
                #endregion


                ret.TaskCost = Context.Costs        //.SingleOrDefault(x => x.CodCost == ret.CodCostDetail)
                    .AsNoTracking().SingleOrDefault(x => x.CodCost == ret.CodCostDetail);

                if (ret.TaskCost != null)
                {
                    var c = ret.TaskCost;
                    c.ProductPartTask = Context.ProductPartTasks.Include("OptionTypeOfTask").AsNoTracking().SingleOrDefault(x => x.CodProductPartTask == c.CodProductPartTask);
                    if (c.ProductPartTask != null)
                    {
                        c.ProductPartTask.ProductPart = Context.ProductParts.AsNoTracking().SingleOrDefault(x => x.CodProductPart == c.ProductPartTask.CodProductPart);
                        if (c.ProductPartTask.ProductPart != null)
                        {
                            c.ProductPartTask.ProductPart.ProductPartPrintableArticles = Context.ProductPartsPrintableArticles.Include("Costs").AsNoTracking().Where(x => x.CodProductPart == c.ProductPartTask.ProductPart.CodProductPart).ToList();
                        }

                        //Option of serigraphy / hotprinting
                        c.ProductPartTask.ProductPartTaskOptions = Context.ProductPartTaskOptions.AsNoTracking().Where(x => x.CodProductPartTask == c.ProductPartTask.CodProductPartTask).ToList();

                    }
                    c.ProductTask = Context.ProductTasks.Include("OptionTypeOfTask").AsNoTracking().SingleOrDefault(x => x.CodProductTask == c.CodProductTask);

                    if (codProduct == "")
                        codProduct = c.ProductTask != null ? c.ProductTask.CodProduct : "";

                    c.ProductPartsPrintableArticle = Context.ProductPartsPrintableArticles.AsNoTracking().SingleOrDefault(x => x.CodProductPartPrintableArticle == c.CodProductPartPrintableArticle);
                    
                    c.DocumentProduct = Context.DocumentProducts.AsNoTracking().SingleOrDefault(x => x.CodDocumentProduct == c.CodDocumentProduct);
                    if (c.DocumentProduct != null)
                    {
                        c.DocumentProduct.Document = Context.Documents.AsNoTracking().SingleOrDefault(x => x.CodDocument == c.DocumentProduct.CodDocument);
                        if (c.DocumentProduct.Document != null)
                        {
                        }
                        c.DocumentProduct.Costs = Context.Costs.AsNoTracking().Where(x => x.CodDocumentProduct == c.DocumentProduct.CodDocumentProduct).ToList();
                        //                    .Include(x => x.DocumentProduct.Document.DocumentProducts)
                        //                    .Include("DocumentProduct.Document.DocumentProducts.Costs")
                    }
                }

                if (ret.ComputedBy != null)
                {
                    ret.ComputedBy = this.GetSingle(ret.ComputedBy.CodCost);
                }

                ////voglio caricare tutti i costi precedenti ed assegnarli a Previouses
                //var gra = Context.ProductGraphLinks.Where(x => x.CodProduct == codProduct && x.CodItemGraphLink == ret.TaskCost.CodItemGraph);
                //foreach (var item in gra)
                //{
                //    Console.WriteLine(item.CodItemGraphLink);
                //}

                ret.CodPartPrintingCostDetail = Context.Costs.AsNoTracking().Where(x => x.CodDocumentProduct == ret.TaskCost.CodDocumentProduct && x.CodItemGraph == "ST").Select(y => y.CodCost);
                ret.TaskexEcutorSelected = Context.taskexecutors.AsNoTracking().Where(x => x.CodTaskExecutor == ret.CodTaskExecutorSelected).FirstOrDefault();


                try
                {
                    CacheComputedBy.Add(ret.CodCost, ret);
                }
                catch (Exception)
                {

                }
            }


            return ret;

        }


        public CostDetail GetSingleSimple(string Cod)
        {
            var codProduct = String.Empty;

            var inizio = DateTime.Now;
            var ret = Context.CostDetail.Include(x => x.Computes).SingleOrDefault(x => x.CodCostDetail == Cod);

            if (ret != null)
            {

                ret.GainPrintingOnBuyings = Context.ProductPartPrintingGain.Where(x => x.CodProductPartPrintingGainBuying == ret.CodCost).Include("Makereadies").ToList();
                //ret.ComputedBy = Context.CostDetail.SingleOrDefault(x => x.CodCost == ret.CodComputedBy);

                //ret.ProductPart = Context.ProductParts
                //    .SingleOrDefault(x => x.CodProductPart == ret.CodProductPart);

                //codProduct = ret.ProductPart != null ? ret.ProductPart.CodProduct : "";

                ret.ProductPartPrinting = Context.ProductPartPrinting.SingleOrDefault(x => x.CodProductPartPrinting == ret.CodCostDetail);

                if (ret.ProductPartPrinting != null)
                {
                    ret.ProductPartPrinting.Part = Context.ProductParts.SingleOrDefault(x => x.CodProductPart == ret.ProductPartPrinting.CodProductPart);
                    ret.ProductPartPrinting.GainPartOnPrintings = Context.ProductPartPrintingGain.Where(x => x.CodProductPartPrinting == ret.ProductPartPrinting.CodProductPartPrinting).Include("Makereadies").ToList();
                }

                //ret.TaskCost = Context.Costs        //.SingleOrDefault(x => x.CodCost == ret.CodCostDetail)
                //    .SingleOrDefault(x => x.CodCost == ret.CodCostDetail);

                //if (ret.TaskCost != null)
                //{
                //    var c = ret.TaskCost;
                //    c.ProductPartTask = Context.ProductPartTasks.Include("OptionTypeOfTask").SingleOrDefault(x => x.CodProductPartTask == c.CodProductPartTask);
                //    if (c.ProductPartTask != null)
                //    {
                //        c.ProductPartTask.ProductPart = Context.ProductParts.SingleOrDefault(x => x.CodProductPart == c.ProductPartTask.CodProductPart);
                //        if (c.ProductPartTask.ProductPart != null)
                //        {
                //            c.ProductPartTask.ProductPart.ProductPartPrintableArticles = Context.ProductPartsPrintableArticles.Include("Costs").Where(x => x.CodProductPart == c.ProductPartTask.ProductPart.CodProductPart).ToList();
                //        }
                //    }
                //    c.ProductTask = Context.ProductTasks.Include("OptionTypeOfTask").SingleOrDefault(x => x.CodProductTask == c.CodProductTask);

                //    if (codProduct == "")
                //        codProduct = c.ProductTask != null ? c.ProductTask.CodProduct : "";

                //    c.ProductPartsPrintableArticle = Context.ProductPartsPrintableArticles.SingleOrDefault(x => x.CodProductPartPrintableArticle == c.CodProductPartPrintableArticle);
                //    c.DocumentProduct = Context.DocumentProducts.SingleOrDefault(x => x.CodDocumentProduct == c.CodDocumentProduct);
                //    if (c.DocumentProduct != null)
                //    {
                //        c.DocumentProduct.Document = Context.Documents.SingleOrDefault(x => x.CodDocument == c.DocumentProduct.CodDocument);
                //        if (c.DocumentProduct.Document != null)
                //        {
                //        }
                //        c.DocumentProduct.Costs = Context.Costs.Where(x => x.CodDocumentProduct == c.DocumentProduct.CodDocumentProduct).ToList();
                //        //                    .Include(x => x.DocumentProduct.Document.DocumentProducts)
                //        //                    .Include("DocumentProduct.Document.DocumentProducts.Costs")
                //    }
                //}

                //if (ret.ComputedBy != null)
                //{
                //    ret.ComputedBy = this.GetSingle(ret.ComputedBy.CodCost);
                //}

                //////voglio caricare tutti i costi precedenti ed assegnarli a Previouses
                ////var gra = Context.ProductGraphLinks.Where(x => x.CodProduct == codProduct && x.CodItemGraphLink == ret.TaskCost.CodItemGraph);
                ////foreach (var item in gra)
                ////{
                ////    Console.WriteLine(item.CodItemGraphLink);
                ////}

                ret.CodPartPrintingCostDetail = Context.Costs.Where(x => x.CodDocumentProduct == ret.TaskCost.CodDocumentProduct && x.CodItemGraph == "ST").Select(y => y.CodCost);

            }

            return ret;
        }

    }
}