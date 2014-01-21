﻿using System;
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
                            Context.Entry(mkr).State = System.Data.EntityState.Added;
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

        }

        public override void SetDbName(string name)
        {
            base.SetDbName(name);
        }


        public override void Save()
        {

            List<Object> modOrAdded = Context.ChangeTracker.Entries()
                .Where(x => x.State == System.Data.EntityState.Modified
                || x.State == System.Data.EntityState.Added)
                .Select(x => x.Entity).ToList();

            var notAll = modOrAdded.Except(modOrAdded.OfType<CostDetail>())
               .Except(modOrAdded.OfType<ProductPartPrinting>())
               .Except(modOrAdded.OfType<ProductPartPrintingGain>())
               .Except(modOrAdded.OfType<Makeready>());

            foreach (var item in notAll)
            {
                Context.Entry(item).State = System.Data.EntityState.Modified;
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


        public override CostDetail GetSingle(string Cod)
        {
            var ret = Context.CostDetail
                .Include(x => x.GainPrintingOnBuyings)
                .Include("GainPrintingOnBuyings.Makereadies")
                .Include(x => x.Computes)
                .Include(x => x.ComputedBy)
                .SingleOrDefault(x => x.CodCostDetail == Cod);

            if (ret != null)
            {
                ret.ProductPart = Context.ProductParts
                    .SingleOrDefault(x => x.CodProductPart == ret.CodProductPart);

                ret.ProductPartPrinting = Context.ProductPartPrinting
                    .Include(x => x.Part)
                    .Include(x => x.GainPartOnPrintings)
                    .Include("GainPartOnPrintings.Makereadies")
                    .SingleOrDefault(x => x.CodProductPartPrinting == ret.CodCostDetail);


                ret.TaskCost = Context.Costs
                    .Include(x => x.ProductPartTask.OptionTypeOfTask)
                    .Include(x => x.ProductPartTask)
                    .Include(x => x.ProductTask.OptionTypeOfTask)
                    .Include(x => x.ProductPartTask.ProductPart)
                    .Include(x => x.ProductPartTask.ProductPart.ProductPartPrintableArticles)
                    .Include("ProductPartTask.ProductPart.ProductPartPrintableArticles.Costs")
                    .Include(x => x.ProductPartsPrintableArticle)
                    .Include(x => x.ProductTask)
                    .Include(x => x.DocumentProduct)
                    .Include(x => x.DocumentProduct.Document)
                    .Include(x => x.DocumentProduct.Document.DocumentProducts)
                    .Include("DocumentProduct.Document.DocumentProducts.Costs")
                    .SingleOrDefault(x => x.CodCost == ret.CodCostDetail);

                if (ret.ComputedBy != null)
                {
                    ret.ComputedBy = this.GetSingle(ret.ComputedBy.CodCost);
                }

            }

            return ret;
        }

    }
}