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
                base.Add(entity);
            }

            Cache[entity.CodCostDetail] = entity;
        }


        public override void Edit(CostDetail entity)
        {
            var fromBD = Context.CostDetail.Single(p => p.CodCostDetail == entity.CodCostDetail);
            Context.Entry(fromBD).CurrentValues.SetValues(entity);

            foreach (var item in entity.Computes)
            {
                item.CodComputedBy = entity.CodCostDetail;
                var fromBDComp = Context.CostDetail.Single(p => p.CodCostDetail == item.CodCostDetail);
                Context.Entry(fromBDComp).CurrentValues.SetValues(item);
            }

            if (entity.ProductPartPrinting != null)
            {
                var prdPartPrt = entity.ProductPartPrinting;
                var fromBD2 = Context.ProductPartPrinting.Single(p => p.CodProductPartPrinting == prdPartPrt.CodProductPartPrinting);
                Context.Entry(fromBD2).CurrentValues.SetValues(prdPartPrt);

                if (prdPartPrt.GainPartOnPrinting != null)
                {
                    var prdPartPrtGain = prdPartPrt.GainPartOnPrinting;
                    var fromBD3 = Context.ProductPartPrintingGain.Single(p => p.CodProductPartPrintingGain == prdPartPrtGain.CodProductPartPrintingGain);
                    Context.Entry(fromBD3).CurrentValues.SetValues(prdPartPrtGain);


                    //devo comportarmi in 2 diversi modi a seconda del fatto che le makereadies siano maggiori nel db o in memoria dopo le modifiche

                    //caso in cui le makereadies in memoria sono > di quelle del db
                    foreach (var mkr in prdPartPrtGain.Makereadies)
                    {
                        try
                        {
                            var fromBD4 = Context.Makereadies.Single(p => p.CodMakeready == mkr.CodMakeready);
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
                var fromBD3 = Context.ProductPartPrintingGain.Single(p => p.CodProductPartPrintingGainBuying == prtGain.CodProductPartPrintingGainBuying);
                Context.Entry(fromBD3).CurrentValues.SetValues(prtGain);

                foreach (var mkr in prtGain.Makereadies)
                {
                    var fromBD4 = Context.Makereadies.Single(p => p.CodMakeready == mkr.CodMakeready);
                    Context.Entry(fromBD4).CurrentValues.SetValues(mkr);
                }
            }

            Cache[entity.CodCostDetail] = entity;

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

            var notAll = modOrAdded.Except(modOrAdded.OfType<CostDetail>())
               .Except(modOrAdded.OfType<ProductPartPrinting>())
               .Except(modOrAdded.OfType<ProductPartPrintingGain>())
               .Except(modOrAdded.OfType<Makeready>());

            foreach (var item in notAll)
            {
                Context.Entry(item).State = System.Data.Entity.EntityState.Modified;
            }

            base.Save();
        }

        //public override CostDetail GetSingle(string Cod)
        //{
        //    var ret = Context.CostDetail.Include("ProductPart")
        //        .Include("ProductPartPrinting")
        //        .Include("ProductPartPrinting.Part")
        //        .Include("ProductPartPrinting.GainPartOnPrintings")
        //        .Include("ProductPartPrinting.GainPartOnPrintings.Makereadies")

        //        .Include("GainPrintingOnBuyings")
        //        .Include("GainPrintingOnBuyings.Makereadies")

        //        .Include("TaskCost")
        //        .Include("TaskCost.ProductPartTask.OptionTypeOfTask")
        //        .Include("TaskCost.ProductPartTask")
        //        .Include("TaskCost.ProductTask.OptionTypeOfTask")
        //        .Include("TaskCost.ProductPartTask.ProductPart")
        //        .Include("TaskCost.ProductPartTask.ProductPart.ProductPartPrintableArticles")
        //        .Include("TaskCost.ProductPartTask.ProductPart.ProductPartPrintableArticles.Costs")
        //        .Include("TaskCost.ProductPartsPrintableArticle")
        //        .Include("TaskCost.ProductTask")
        //        .Include("TaskCost.DocumentProduct")
        //        .Include("TaskCost.DocumentProduct.Document")
        //        .Include("TaskCost.DocumentProduct.Document.DocumentProducts")
        //        .Include("TaskCost.DocumentProduct.Document.DocumentProducts.Costs")

        //        .Include("Computes")
        //        .Include("ComputedBy")

        //        .Where(x => x.CodCostDetail == Cod).FirstOrDefault();

        //    return ret;
        //}


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

        public override CostDetail GetSingle(string Cod)
        {

            try
            {
                return Cache[Cod];

            }
            catch (Exception)
            {

                var inizio = DateTime.Now;
                var ret = Context.CostDetail.Include(x => x.Computes).SingleOrDefault(x => x.CodCostDetail == Cod);

                if (ret != null)
                {
                    ret.GainPrintingOnBuyings = Context.ProductPartPrintingGain.Where(x => x.CodProductPartPrintingGainBuying == ret.CodCost).Include("Makereadies").ToList();
                    ret.ComputedBy = Context.CostDetail.SingleOrDefault(x => x.CodCost == ret.CodComputedBy);

                    ret.ProductPart = Context.ProductParts
                        .SingleOrDefault(x => x.CodProductPart == ret.CodProductPart);

                    ret.ProductPartPrinting = Context.ProductPartPrinting.SingleOrDefault(x => x.CodProductPartPrinting == ret.CodCostDetail);

                    if (ret.ProductPartPrinting != null)
                    {
                        ret.ProductPartPrinting.Part = Context.ProductParts.SingleOrDefault(x => x.CodProductPart == ret.ProductPartPrinting.CodProductPart);
                        ret.ProductPartPrinting.GainPartOnPrintings = Context.ProductPartPrintingGain.Where(x => x.CodProductPartPrinting == ret.ProductPartPrinting.CodProductPartPrinting).Include("Makereadies").ToList();
                    }

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
                        ret.ComputedBy = this.GetSingle(ret.ComputedBy.CodCost);
                    }

                }

                var tempo = DateTime.Now.Subtract(inizio);

                Console.Write(tempo);

                return ret;
            }
        }

    }
}