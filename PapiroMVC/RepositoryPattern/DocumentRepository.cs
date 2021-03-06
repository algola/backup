﻿using System;
using System.Linq;
using PapiroMVC.Models;
using PapiroMVC.DbCodeManagement;
using System.Threading;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Data.Common;
using System.Data.Entity;

namespace Services
{
    public class DocumentRepository : GenericRepository<dbEntities, Document>, IDocumentRepository
    {
        private Dictionary<string, Document> cacheDoc;


        public dbEntities GetContext()
        {
            return this.Context;
        }

        public Document GetEstimateEcommerce(string codCustomerSupplier)
        {
            var estEcom = Context.Documents.OfType<EstimateEcommerce>().SingleOrDefault(x => x.CodCustomer == codCustomerSupplier);
            if (estEcom == null)
            {
                estEcom = new EstimateEcommerce();
                estEcom.CodCustomer = codCustomerSupplier;
                estEcom.CodDocument = GetNewCode(estEcom);
                estEcom.TimeStampTable = DateTime.Now;
                Add(estEcom);
                Save();
            }


            return estEcom;
        }

        public void EditCost(Cost entity)
        {

            var fromBD = Context.Costs.SingleOrDefault(p => p.CodCost == entity.CodCost);
            if (fromBD != null)
            {
                Context.Entry(fromBD).CurrentValues.SetValues(entity);
                Context.Entry(fromBD).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                Context.Entry(entity).State = System.Data.Entity.EntityState.Added;
                //                Context.Set<Cost>().Add(entity);
            }
        }

        public Cost GetCost(string codCost)
        {
            var inizio = DateTime.Now;
            var cost = Context.Costs.SingleOrDefault(x => x.CodCost == codCost);
            cost.DocumentProduct = Context.DocumentProducts.SingleOrDefault(x => x.CodDocumentProduct == cost.CodDocumentProduct);

            if (cost != null)
            {

                cost.ProductTask = Context.ProductTasks.SingleOrDefault(x => x.CodProductTask == cost.CodProductTask);
                if (cost.ProductTask != null)
                {
                    cost.ProductTask.OptionTypeOfTask = Context.OptionTypeOfTasks.SingleOrDefault(y => y.CodOptionTypeOfTask == cost.ProductTask.CodOptionTypeOfTask);
                }

                cost.ProductPartTask = Context.ProductPartTasks.SingleOrDefault(x => x.CodProductPartTask == cost.CodProductPartTask);
                if (cost.ProductPartTask != null)
                {
                    cost.ProductPartTask.OptionTypeOfTask = Context.OptionTypeOfTasks.SingleOrDefault(y => y.CodOptionTypeOfTask == cost.ProductPartTask.CodOptionTypeOfTask);

                    //Option of serigraphy / hotprinting
                    cost.ProductPartTask.ProductPartTaskOptions = Context.ProductPartTaskOptions.AsNoTracking().Where(x => x.CodProductPartTask == cost.ProductPartTask.CodProductPartTask).ToList();

                    var ppTask = cost.ProductPartTask;
                    ppTask.ProductPart = Context.ProductParts.SingleOrDefault(h => h.CodProductPart == ppTask.CodProductPart);

                    ppTask.ProductPart.ProductPartPrintableArticles = Context.ProductPartsPrintableArticles.Where(pp => pp.CodProductPart == ppTask.ProductPart.CodProductPart).ToList();

                    foreach (var item in ppTask.ProductPart.ProductPartPrintableArticles)
                    {
                        item.Costs = Context.Costs.Where(nn => nn.CodProductPartPrintableArticle == item.CodProductPartPrintableArticle).ToList();
                    }
                }

                cost.ProductPartImplantTask = Context.ProductPartTasks.SingleOrDefault(x => x.CodProductPartTask == cost.CodProductPartImplantTask);
                if (cost.ProductPartImplantTask != null)
                {
                    cost.ProductPartImplantTask.OptionTypeOfTask = Context.OptionTypeOfTasks.SingleOrDefault(y => y.CodOptionTypeOfTask == cost.ProductPartImplantTask.CodOptionTypeOfTask);

                    var ppTask = cost.ProductPartImplantTask;
                    ppTask.ProductPart = Context.ProductParts.SingleOrDefault(h => h.CodProductPart == ppTask.CodProductPart);

                    ppTask.ProductPart.ProductPartTasks = Context.ProductPartTasks.Where(pp => pp.CodProductPart == ppTask.ProductPart.CodProductPart).ToList();

                    foreach (var item in ppTask.ProductPart.ProductPartTasks)
                    {
                        item.Costs = Context.Costs.Where(nn => nn.CodProductPartTask == item.CodProductPartTask).ToList();
                    }
                }


            }

            cost.ProductPartsPrintableArticle = Context.ProductPartsPrintableArticles.FirstOrDefault(x => x.CodProductPartPrintableArticle == cost.CodProductPartPrintableArticle);

            if (cost.ProductPartsPrintableArticle != null)
            {
                cost.ProductPartsPrintableArticle.ProductPart = Context.ProductParts.SingleOrDefault(x => x.CodProductPart == cost.ProductPartsPrintableArticle.CodProductPart);
                var part = cost.ProductPartsPrintableArticle.ProductPart;
                if (part != null)
                {
                    part.ProductPartTasks = Context.ProductPartTasks.Where(x => x.CodProductPart == part.CodProductPart).ToList();
                    foreach (var item in part.ProductPartTasks)
                    {
                        item.OptionTypeOfTask = Context.OptionTypeOfTasks.SingleOrDefault(y => y.CodOptionTypeOfTask == item.CodOptionTypeOfTask);
                        item.Costs = Context.Costs.Where(x => x.CodProductPartTask == item.CodProductPartTask).ToList();
                    }
                }
            }


            var tempo = DateTime.Now.Subtract(inizio);

            Console.Write(tempo);

            return cost;
        }

        public Cost GetCostNoT(string codCost)
        {
            var inizio = DateTime.Now;
            var cost = Context.Costs.AsNoTracking().SingleOrDefault(x => x.CodCost == codCost);
            cost.DocumentProduct = Context.DocumentProducts.AsNoTracking().SingleOrDefault(x => x.CodDocumentProduct == cost.CodDocumentProduct);

            if (cost != null)
            {

                cost.ProductTask = Context.ProductTasks.AsNoTracking().SingleOrDefault(x => x.CodProductTask == cost.CodProductTask);
                if (cost.ProductTask != null)
                {
                    cost.ProductTask.OptionTypeOfTask = Context.OptionTypeOfTasks.AsNoTracking().SingleOrDefault(y => y.CodOptionTypeOfTask == cost.ProductTask.CodOptionTypeOfTask);
                }

                cost.ProductPartTask = Context.ProductPartTasks.AsNoTracking().SingleOrDefault(x => x.CodProductPartTask == cost.CodProductPartTask);
                if (cost.ProductPartTask != null)
                {
                    cost.ProductPartTask.OptionTypeOfTask = Context.OptionTypeOfTasks.AsNoTracking().SingleOrDefault(y => y.CodOptionTypeOfTask == cost.ProductPartTask.CodOptionTypeOfTask);

                    //Option of serigraphy / hotprinting
                    cost.ProductPartTask.ProductPartTaskOptions = Context.ProductPartTaskOptions.AsNoTracking().Where(x => x.CodProductPartTask == cost.ProductPartTask.CodProductPartTask).ToList();

                    var ppTask = cost.ProductPartTask;
                    ppTask.ProductPart = Context.ProductParts.AsNoTracking().SingleOrDefault(h => h.CodProductPart == ppTask.CodProductPart);

                    ppTask.ProductPart.ProductPartPrintableArticles = Context.ProductPartsPrintableArticles.AsNoTracking().Where(pp => pp.CodProductPart == ppTask.ProductPart.CodProductPart).ToList();

                    foreach (var item in ppTask.ProductPart.ProductPartPrintableArticles)
                    {
                        item.Costs = Context.Costs.AsNoTracking().Where(nn => nn.CodProductPartPrintableArticle == item.CodProductPartPrintableArticle).ToList();
                    }


                }

                cost.ProductPartImplantTask = Context.ProductPartTasks.AsNoTracking().SingleOrDefault(x => x.CodProductPartTask == cost.CodProductPartImplantTask);
                if (cost.ProductPartImplantTask != null)
                {
                    cost.ProductPartImplantTask.OptionTypeOfTask = Context.OptionTypeOfTasks.AsNoTracking().SingleOrDefault(y => y.CodOptionTypeOfTask == cost.ProductPartImplantTask.CodOptionTypeOfTask);

                    var ppTask = cost.ProductPartImplantTask;
                    ppTask.ProductPart = Context.ProductParts.AsNoTracking().SingleOrDefault(h => h.CodProductPart == ppTask.CodProductPart);

                    ppTask.ProductPart.ProductPartTasks = Context.ProductPartTasks.AsNoTracking().Where(pp => pp.CodProductPart == ppTask.ProductPart.CodProductPart).ToList();

                    foreach (var item in ppTask.ProductPart.ProductPartTasks)
                    {
                        item.Costs = Context.Costs.AsNoTracking().Where(nn => nn.CodProductPartTask == item.CodProductPartTask).ToList();
                    }
                }


            }

            cost.ProductPartsPrintableArticle = Context.ProductPartsPrintableArticles.AsNoTracking().FirstOrDefault(x => x.CodProductPartPrintableArticle == cost.CodProductPartPrintableArticle);

            if (cost.ProductPartsPrintableArticle != null)
            {
                cost.ProductPartsPrintableArticle.ProductPart = Context.ProductParts.AsNoTracking().SingleOrDefault(x => x.CodProductPart == cost.ProductPartsPrintableArticle.CodProductPart);
                var part = cost.ProductPartsPrintableArticle.ProductPart;
                if (part != null)
                {
                    part.ProductPartTasks = Context.ProductPartTasks.AsNoTracking().Where(x => x.CodProductPart == part.CodProductPart).ToList();
                    foreach (var item in part.ProductPartTasks)
                    {
                        item.OptionTypeOfTask = Context.OptionTypeOfTasks.AsNoTracking().SingleOrDefault(y => y.CodOptionTypeOfTask == item.CodOptionTypeOfTask);
                        item.Costs = Context.Costs.AsNoTracking().Where(x => x.CodProductPartTask == item.CodProductPartTask).ToList();
                    }
                }
            }


            var tempo = DateTime.Now.Subtract(inizio);

            Console.Write(tempo);

            return cost;
        }



        public IQueryable<Cost> GetCostsByCodDocumentProduct(string codDocumentProduct)
        {
            var inizio = DateTime.Now;

            IQueryable<Cost> ret;
            bool passatoDaDb = false;

            ret = Context.Costs.Include("CostDetails").Include("DocumentProduct").Include("DocumentProduct.Document").Where(x => x.CodDocumentProduct == codDocumentProduct);

            var tempo = DateTime.Now.Subtract(inizio);

            Console.Write(tempo + " " + passatoDaDb.ToString());
            return ret;
        }

        public IQueryable<Cost> GetCostsByCodDocumentProductNoT(string codDocumentProduct)
        {
            var inizio = DateTime.Now;

            IQueryable<Cost> ret;
            bool passatoDaDb = false;

            ret = Context.Costs.AsNoTracking().Include("CostDetails").Include("DocumentProduct").Include("DocumentProduct.Document").Where(x => x.CodDocumentProduct == codDocumentProduct);

            var tempo = DateTime.Now.Subtract(inizio);

            Console.Write(tempo + " " + passatoDaDb.ToString());
            return ret;
        }
        public string GetNewCode(Document a)
        {
            //il trucco è di avere un pad left per poter utilizzare il Max per ottenere il maggiore nell'insieme
            //con un colpo solo!!!
            // var csCode = (from COD in this.GetAll() select COD.CodDocument).Max();

            var csCode = Context.Database.SqlQuery<string>("SELECT MAX(CodDocument) AS CodDocument FROM Documents").FirstOrDefault<string>();
            return AlphaCode.GetNextCode(csCode ?? "0").PadLeft(6, '0');


            //string csCode = "";
            //string newCode = "";

            //var lastCode = Context.LastCodes.SingleOrDefault(p => p.CodLastCode == "CodDocument");
            //if (lastCode != null)
            //{
            //    csCode = lastCode.Value;
            //    newCode = AlphaCode.GetNextCode(csCode ?? "0").PadLeft(6, '0');
            //    lastCode.Value = newCode;
            //    Context.Entry(lastCode).CurrentValues.SetValues(lastCode);
            //}
            //else
            //{
            //    csCode = (from COD in this.GetAll() select COD.CodDocument).Max();
            //    newCode = AlphaCode.GetNextCode(csCode ?? "0").PadLeft(6, '0');
            //    lastCode = new LastCode() { TimeStampTable = DateTime.Now, CodLastCode = "CodDocument", Value = csCode };
            //    Context.Entry(lastCode).State = EntityState.Added;
            //    Context.SaveChanges();
            //}
        }

        public string GetNewEstimateNumber(Document a)
        {
            //il trucco è di avere un pad left per poter utilizzare il Max per ottenere il maggiore nell'insieme
            //con un colpo solo!!!
            var csCode = (from COD in this.GetAll() where COD.EstimateNumberSerie == a.EstimateNumberSerie select COD.EstimateNumber).Max();
            return AlphaCode.GetNextIntCode(csCode ?? "0").PadLeft(6, '0');
        }

        public string GetNewOrderNumber(Document a)
        {
            //il trucco è di avere un pad left per poter utilizzare il Max per ottenere il maggiore nell'insieme
            //con un colpo solo!!!
            var csCode = (from COD in this.GetAll().OfType<Order>() where COD.OrderNumberSerie == ((Order)a).OrderNumberSerie select COD.OrderNumber).Max();
            return AlphaCode.GetNextIntCode(csCode ?? "0").PadLeft(6, '0');
        }

        private void DocumentProductCodeRigen(Document c)
        {
            c.DocumentProductsCodeRigen();
            //c.EstimateNumber = c.EstimateNumber == null ? (0).ToString().PadLeft(6, '0') : c.EstimateNumber.PadLeft(6, '0');

            ////prodotti in documento
            //var ppart = c.DocumentProducts.OrderBy(y => y.CodDocumentProduct, new EmptyStringsAreLast()).ToList();
            //foreach (var item in c.DocumentProducts.OrderBy(y => y.CodDocumentProduct, new EmptyStringsAreLast()))
            //{
            //    item.CodDocumentProduct = c.CodDocument + "-" + ppart.IndexOf(item).ToString();
            //    item.CodDocument = c.CodDocument;
            //    item.TimeStampTable = DateTime.Now;

            //    var costl = item.Costs.OrderBy(x => x.CodCost).ToList();
            //    foreach (var itemCost in item.Costs.OrderBy(x => x.CodCost))
            //    {
            //        itemCost.TimeStampTable = DateTime.Now;
            //        itemCost.CodDocumentProduct = item.CodDocumentProduct;
            //        itemCost.CodCost = item.CodDocumentProduct + "-" + costl.IndexOf(itemCost).ToString();
            //    }
            //}

            //c.TimeStampTable = DateTime.Now;

        }

        public void SaveOnSession(Document entity)
        {
            entity.CodDocument = "session";
            DocumentProductCodeRigen(entity);
            System.Web.HttpContext.Current.Session["document"] = entity;
        }

        public Document GetFromSession()
        {
            return (Document)System.Web.HttpContext.Current.Session["document"];
        }

        public override void Add(Document entity)
        {
            DocumentProductCodeRigen(entity);
            base.Add(entity);


        }

        public override IQueryable<Document> GetAll()
        {

            Console.WriteLine(Context.Database.Connection.ConnectionString);
            return Context.Documents.Include("DocumentProducts").Include("DocumentProducts.Costs").Include("OrderProduct").Include("OrderProduct.Document");
        }

        private void ResetStatus()
        {

            List<Object> modOrAdded = Context.ChangeTracker.Entries()
    .Where(x => x.State == System.Data.Entity.EntityState.Modified
    || x.State == System.Data.Entity.EntityState.Added)
    .Select(x => x.Entity).ToList();


            foreach (var item in modOrAdded.OfType<CustomerSupplier>())
                Context.Entry(item).State = System.Data.Entity.EntityState.Unchanged;

            foreach (var item in modOrAdded.OfType<Product>())
                Context.Entry(item).State = System.Data.Entity.EntityState.Unchanged;

            foreach (var item in modOrAdded.OfType<ProductPart>())
                Context.Entry(item).State = System.Data.Entity.EntityState.Unchanged;

            foreach (var item in modOrAdded.OfType<ProductTask>())
                Context.Entry(item).State = System.Data.Entity.EntityState.Unchanged;

            foreach (var item in modOrAdded.OfType<ProductPartTask>())
                Context.Entry(item).State = System.Data.Entity.EntityState.Unchanged;

            foreach (var item in modOrAdded.OfType<ProductPartTaskOption>())
                Context.Entry(item).State = System.Data.Entity.EntityState.Unchanged;

            foreach (var item in modOrAdded.OfType<ProductPartsPrintableArticle>())
                Context.Entry(item).State = System.Data.Entity.EntityState.Unchanged;

            foreach (var item in modOrAdded.OfType<CostDetail>())
                Context.Entry(item).State = System.Data.Entity.EntityState.Unchanged;

            foreach (var item in modOrAdded.OfType<TypeOfTask>())
                Context.Entry(item).State = System.Data.Entity.EntityState.Unchanged;

            foreach (var item in modOrAdded.OfType<OptionTypeOfTask>())
                Context.Entry(item).State = System.Data.Entity.EntityState.Unchanged;

            foreach (var item in modOrAdded.OfType<ProductGraphLink>())
                Context.Entry(item).State = System.Data.Entity.EntityState.Unchanged;

        }

        public override void Save()
        {

            ResetStatus();
            base.Save();

        }

        public override void Edit(Document entity)
        {

            //we can have some DocumentProduct added and some Just Stored so...
            var dpJustStored = Context.DocumentProducts.Where(x => x.CodDocument == entity.CodDocument).ToList();
            entity.DocumentProducts = entity.DocumentProducts.Union(dpJustStored, new DocumentProductComparer()).ToList();

            #region DocumentProducts
            //we can have some Cost added and dome Just Stored so...
            foreach (var item in entity.DocumentProducts)
            {
                //perform only if CodDocumentProduct is not null
                if (item.CodDocumentProduct != null && item.CodDocumentProduct != "")
                {
                    var costJustStored = Context.Costs.Where(x => x.CodDocumentProduct == item.CodDocumentProduct).ToList();
                    item.Costs = item.Costs.Union(costJustStored, new CostComparer()).ToList();

                }

            }
            #endregion

            DocumentProductCodeRigen(entity);

            #region States

            foreach (var item in entity.DocumentStates)
            {
                var fromBD2 = Context.DocumentStates.SingleOrDefault(p => p.CodDocumentState == item.CodDocumentState);
                if (fromBD2 != null)
                {
                    Context.Entry(fromBD2).CurrentValues.SetValues(item);
                    Context.Entry(fromBD2).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    Context.Entry(item).State = System.Data.Entity.EntityState.Added;
                }
            }

            #endregion



            foreach (var item in entity.DocumentProducts)
            {
                var fromBD2 = Context.DocumentProducts.SingleOrDefault(p => p.CodDocumentProduct == item.CodDocumentProduct);
                if (fromBD2 != null)
                {
                    Context.Entry(fromBD2).CurrentValues.SetValues(item);
                    Context.Entry(fromBD2).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    Context.Entry(item).State = System.Data.Entity.EntityState.Added;
                }

                foreach (var item2 in item.Costs)
                {
                    var cost = item2;
                    var fromBD3 = Context.Costs.SingleOrDefault(p => p.CodCost == cost.CodCost);

                    if (fromBD3 != null)
                    {
                        Context.Entry(fromBD3).CurrentValues.SetValues(cost);
                        Context.Entry(fromBD3).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        List<Object> added = Context.ChangeTracker.Entries().Where(x => x.State == System.Data.Entity.EntityState.Added).Select(x => x.Entity).ToList();
                        Console.Write(added);

                        item2.DocumentProduct = null;
                        Context.Entry(item2).State = System.Data.Entity.EntityState.Added;

                        List<Object> added2 = Context.ChangeTracker.Entries().Where(x => x.State == System.Data.Entity.EntityState.Added).Select(x => x.Entity).ToList();
                        Console.Write(added2);

                    }


                    if (item2.CostDetails.FirstOrDefault() != null)
                        EditAdd(item2.CostDetails.FirstOrDefault());

                }
            }

            var doc = entity;
            var fromBD = Context.Documents.Single(p => p.CodDocument == doc.CodDocument);
            Context.Entry(fromBD).CurrentValues.SetValues(doc);
            Context.Entry(fromBD).State = System.Data.Entity.EntityState.Modified;




        }


        public void Edit(Document entity, bool deep)
        {

        }


        public void DeleteDocumentProduct(DocumentProduct documentProduct)
        {
            this.Context.Set<DocumentProduct>().Remove(documentProduct);
        }


        public new Document GetSingle(string codDocument)
        {
            var query = Context.Documents.Include("DocumentProducts").Include("DocumentProducts.Costs").FirstOrDefault(x => x.CodDocument == codDocument);
            return query;
        }

        public IQueryable<DocumentProduct> GetAllDocumentProductsSimply()
        {
            var query = Context.DocumentProducts.Include("Document");
            return query;
        }

        public IQueryable<DocumentProduct> GetAllDocumentProducts()
        {
            try
            {
                throw new Exception();
            }
            catch (Exception)
            {
                var query = Context.DocumentProducts.Include("Costs")
                    .Include("Costs.CostDetails")
                    .Include("Product")
                    .Include("Document");
                return query;
            }
        }

        public IQueryable<Product> GetAllProducts()
        {
            try
            {
                throw new Exception();
            }
            catch (Exception)
            {
                var query = Context.Products.Include("ProductParts");
                return query;
            }
        }

        public IQueryable<DocumentProduct> GetDocumentProductsByCodProduct(string codProduct)
        {
            try
            {
                throw new Exception();
            }
            catch (Exception)
            {
                var query = Context.DocumentProducts.Include("Costs")
                    .Include("Costs.CostDetails")
                    .Include("Product")
                    .Include("Document").Where(x => x.CodProduct == codProduct);
                return query;
            }

        }

        public DocumentProduct GetDocumentProductByCodDocumentProduct(string codDocumentProduct)
        {
            try
            {
                throw new Exception();
            }
            catch (Exception)
            {
                var query = Context.DocumentProducts.Include("Costs")
                    .Include("Costs.CostDetails")
                    .Include("Product")
                    .Include("Document").Where(x => x.CodDocumentProduct == codDocumentProduct);
                return query.SingleOrDefault();
            }

        }

        public override void SetDbName(string name)
        {
            base.SetDbName(name);
        }

        protected void EditAdd(CostDetail entity)
        {

            var fromBD = Context.CostDetail.SingleOrDefault(p => p.CodCostDetail == entity.CodCostDetail);
            if (fromBD != null)
            {
                Context.Entry(fromBD).CurrentValues.SetValues(entity);
                Context.Entry(fromBD).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                Context.Entry(entity).State = System.Data.Entity.EntityState.Added;
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


        /// <summary>
        /// returns all system states
        /// </summary>
        /// <returns></returns>
        public IQueryable<State> GetAllStates()
        {
            Console.WriteLine(Context.Database.Connection.ConnectionString);

            var states = Context.States;

            var tbCode = new State[14];

            tbCode[0] = new State { CodState = "AttInformazioni", StateName = "Attendo Informazioni", StateNumber = 10, UseInEstimate = true };
            tbCode[1] = new State { CodState = "InElaborazione", StateName = "In Elaborazione", StateNumber = 30, UseInEstimate = true };
            tbCode[2] = new State { CodState = "Inviato", StateName = "Preventivo Inviato", StateNumber = 40, UseInEstimate = true, ResetLinkedStates = "10+30" };
            tbCode[3] = new State { CodState = "Commessa", StateName = "E' Commessa", StateNumber = 50, UseInEstimate = true, ResetLinkedStates = "10+30" };
            tbCode[4] = new State { CodState = "Annullato", StateName = "Annullato", StateNumber = 60, UseInEstimate = true };


            tbCode[5] = new State { CodState = "AttConfermaDordine", StateName = "Attesa Conferma D'ordine", StateNumber = 110, UseInOrder = true };
            tbCode[6] = new State { CodState = "AttAnticipo", StateName = "Attesa Anticipo", StateNumber = 120, UseInOrder = true };
            tbCode[7] = new State { CodState = "AttMateriali", StateName = "Attesa Materiali", StateNumber = 130, UseInOrder = true };
            tbCode[8] = new State { CodState = "AttImpianti", StateName = "Attesa Impianti", StateNumber = 140, UseInOrder = true };
            tbCode[9] = new State { CodState = "InStampa", StateName = "In Stampa", StateNumber = 150, UseInOrder = true };
            tbCode[10] = new State { CodState = "LavEsterna", StateName = "Da Lavorante", StateNumber = 160, UseInOrder = true };
            tbCode[11] = new State { CodState = "InConfezione", StateName = "In Confezione", StateNumber = 170, UseInOrder = true };
            tbCode[12] = new State { CodState = "InSpedizione", StateName = "In Spedizione", StateNumber = 180, UseInOrder = true };
            tbCode[13] = new State { CodState = "Concluso", StateName = "Ordine Concluso", StateNumber = 190, UseInOrder = true };

            foreach (var item in tbCode)
            {

                item.TimeStampTable = DateTime.Now;
                var fromBD2 = Context.States.SingleOrDefault(p => p.CodState == item.CodState);
                if (fromBD2 != null)
                {
                    Context.Entry(fromBD2).CurrentValues.SetValues(item);
                    Context.Entry(fromBD2).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    Context.Entry(item).State = System.Data.Entity.EntityState.Added;
                }
            }

            this.Save();




            return Context.States;
        }

        /// <summary>
        /// return the document states
        /// </summary>
        /// <param name="codDocument"></param>
        /// <returns></returns>
        public IQueryable<DocumentState> GetAllDocumentStates(string codDocument)
        {
            return Context.DocumentStates.Where(x => x.CodDocument == codDocument).OrderBy(x => x.StateNumber);
        }

        /// <summary>
        /// LIst of report installed for orders
        /// </summary>
        /// <returns></returns>

        public IQueryable<ReportOrderName> GetAllReportOrderName(string databaseName)
        {
            var ret = new List<ReportOrderName>();


            if (databaseName.ToLower() == "canepa")
            {
                var p = new ReportOrderName { Name = "LabelRollHeadCanepa", Description = "Etichette in rotolo" };
                ret.Add(p);
            }
            else
            {
                var p = new ReportOrderName { Name = "LabelRollHead", Description = "Etichette in rotolo" };
                ret.Add(p);
            }

            var g = new ReportOrderName { Name = "LabelHead", Description = "Etichette in piano" };
            ret.Add(g);

            return ret.AsQueryable();
        }
    }
}