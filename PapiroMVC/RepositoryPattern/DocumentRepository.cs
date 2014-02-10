using System;
using System.Linq;
using PapiroMVC.Models;
using PapiroMVC.DbCodeManagement;
using System.Threading;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Data.Common;

namespace Services
{
    public class DocumentRepository : GenericRepository<dbEntities, Document>, IDocumentRepository
    {
        private Dictionary<string, Document> cacheDoc;

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


        protected Dictionary<string, Document> CacheDoc
        {
            get
            {
                if (cacheDoc == null)
                {
                    cacheDoc = new Dictionary<string, Document>();
                }
                return cacheDoc;
            }
            set
            {
                cacheDoc = value;
            }
        }

        private Dictionary<string, List<Cost>> cacheDocPro;

        protected Dictionary<string, List<Cost>> CacheDocPro
        {
            get
            {
                if (cacheDocPro == null)
                {
                    cacheDocPro = new Dictionary<string, List<Cost>>();
                }
                return cacheDocPro;
            }
            set
            {
                cacheDocPro = value;
            }
        }

        public Cost GetCost(string codCost)
        {
            var inizio = DateTime.Now;
            var cost = Context.Costs.SingleOrDefault(x => x.CodCost == codCost);

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

                    var ppTask = cost.ProductPartTask;
                    ppTask.ProductPart = Context.ProductParts.SingleOrDefault(h => h.CodProductPart == ppTask.CodProductPart);

                    ppTask.ProductPart.ProductPartPrintableArticles = Context.ProductPartsPrintableArticles.Where(pp => pp.CodProductPart == ppTask.ProductPart.CodProductPart).ToList();

                    foreach (var item in ppTask.ProductPart.ProductPartPrintableArticles)
                    {
                        item.Costs = Context.Costs.Where(nn => nn.CodProductPartPrintableArticle == item.CodProductPartPrintableArticle).ToList();
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

        public IQueryable<Cost> GetCostsByCodDocumentProduct(string codDocumentProduct)
        {
            var inizio = DateTime.Now;

            IQueryable<Cost> ret;
            bool passatoDaDb = false;

            try
            {
                ret = CacheDocPro[codDocumentProduct].AsQueryable<Cost>();
            }
            catch (Exception)
            {
                passatoDaDb = true;
                ret = Context.Costs.Include("CostDetails").Include("DocumentProduct").Include("DocumentProduct.Document").Where(x => x.CodDocumentProduct == codDocumentProduct);
            }


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
            var csCode = (from COD in this.GetAll() select COD.EstimateNumber).Max();
            return AlphaCode.GetNextIntCode(csCode ?? "0").PadLeft(6, '0');
        }

        private void DocumentProductCodeRigen(Document c)
        {
            c.EstimateNumber = c.EstimateNumber == null ? (0).ToString().PadLeft(6, '0') : c.EstimateNumber.PadLeft(6, '0');

            //prodotti in documento
            var ppart = c.DocumentProducts.OrderBy(y => y.CodDocumentProduct, new EmptyStringsAreLast()).ToList();
            foreach (var item in c.DocumentProducts.OrderBy(y => y.CodDocumentProduct, new EmptyStringsAreLast()))
            {
                item.CodDocumentProduct = c.CodDocument + "-" + ppart.IndexOf(item).ToString();
                item.CodDocument = c.CodDocument;
                item.TimeStampTable = DateTime.Now;

                var costl = item.Costs.OrderBy(x => x.CodCost).ToList();
                foreach (var itemCost in item.Costs.OrderBy(x => x.CodCost))
                {
                    itemCost.TimeStampTable = DateTime.Now;
                    itemCost.CodDocumentProduct = item.CodDocumentProduct;
                    itemCost.CodCost = item.CodDocumentProduct + "-" + costl.IndexOf(itemCost).ToString();
                }
            }

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

            //Save in cache
            CacheDoc[entity.CodDocument] = entity;

            foreach (var item in entity.DocumentProducts)
            {
                CacheDocPro[item.CodDocumentProduct] = item.Costs.ToList();
            }
            //end save in cache

        }

        public override IQueryable<Document> GetAll()
        {
            Console.WriteLine(Context.Database.Connection.ConnectionString);
            return Context.Documents.Include("DocumentProducts").Include("DocumentProducts.Costs");
        }

        public override void Save()
        {

            List<Object> modOrAdded = Context.ChangeTracker.Entries()
            .Where(x => x.State == System.Data.EntityState.Modified
            || x.State == System.Data.EntityState.Added)
            .Select(x => x.Entity).ToList();


            foreach (var item in modOrAdded.OfType<Product>())
                Context.Entry(item).State = System.Data.EntityState.Unchanged;

            foreach (var item in modOrAdded.OfType<ProductPart>())
                Context.Entry(item).State = System.Data.EntityState.Unchanged;

            foreach (var item in modOrAdded.OfType<ProductTask>())
                Context.Entry(item).State = System.Data.EntityState.Unchanged;

            foreach (var item in modOrAdded.OfType<ProductPartTask>())
                Context.Entry(item).State = System.Data.EntityState.Unchanged;

            foreach (var item in modOrAdded.OfType<ProductPartsPrintableArticle>())
                Context.Entry(item).State = System.Data.EntityState.Unchanged;

            foreach (var item in modOrAdded.OfType<CostDetail>())
                Context.Entry(item).State = System.Data.EntityState.Unchanged;

            foreach (var item in modOrAdded.OfType<TypeOfTask>())
                Context.Entry(item).State = System.Data.EntityState.Unchanged;

            foreach (var item in modOrAdded.OfType<OptionTypeOfTask>())
                Context.Entry(item).State = System.Data.EntityState.Unchanged;

            try
            {
                base.Save();
            }
            catch (OptimisticConcurrencyException)
            {
                Context.SaveChanges();
            }
        }

        public override void Edit(Document entity)
        {
            //we can have some DocumentProduct added and some just saved so...
            var dps = Context.DocumentProducts.Where(x => x.CodDocument == entity.CodDocument).ToList();
            entity.DocumentProducts = dps.Union(entity.DocumentProducts, new DocumentProductComparer()).ToList();

            DocumentProductCodeRigen(entity);

            foreach (var item in entity.DocumentProducts)            
            {
                var fromBD2 = Context.DocumentProducts.SingleOrDefault(p => p.CodDocumentProduct == item.CodDocumentProduct);
                if (fromBD2 != null)
                {
                    Context.Entry(fromBD2).CurrentValues.SetValues(item);
                }
                else
                {
                    Context.Entry(item).State = EntityState.Added;
                }

                /*                var product = item.Product;
                                var fromBD3 = Context.Products.Single(p => p.CodProduct == product.CodProduct);
                                Context.Entry(fromBD3).CurrentValues.SetValues(product);
                */

                foreach (var item2 in item.Costs)
                {
                    var cost = item2;
                    var fromBD3 = Context.Costs.SingleOrDefault(p => p.CodCost == cost.CodCost);

                    if (fromBD3 != null)
                    {
                        Context.Entry(fromBD3).CurrentValues.SetValues(cost);
                    }
                    else
                    {
                        Context.Entry(item).State = EntityState.Added;
                    }

                }

            }

            var doc = entity;
            var fromBD = Context.Documents.Single(p => p.CodDocument == doc.CodDocument);
            Context.Entry(fromBD).CurrentValues.SetValues(doc);

            //Save in cache
            CacheDoc[entity.CodDocument] = entity;

            foreach (var item in entity.DocumentProducts)
            {
                CacheDocPro[item.CodDocumentProduct] = item.Costs.ToList();
            }
            //end save in cache

        }

        public new Document GetSingle(string codDocument)
        {
            try
            {
                return CacheDoc[codDocument];
            }
            catch (Exception)
            {
                var query = Context.Documents.Include("DocumentProducts").Include("DocumentProducts.Costs").FirstOrDefault(x => x.CodDocument == codDocument);
                return query;
            }
        }

        public IQueryable<DocumentProduct> GetDocumentProductByCodProduct(string codProduct)
        {
            try
            {
                throw new Exception();
            }
            catch (Exception)
            {
                var query = Context.DocumentProducts.Include("Costs")
                    .Include("Costs.CostDetails")
                    .Include("Product").Where(x => x.CodProduct == codProduct);
                return query;
            }

        }
        public override void SetDbName(string name)
        {
            base.SetDbName(name);
        }
    }
}