using System;
using System.Linq;
using PapiroMVC.Models;
using PapiroMVC.DbCodeManagement;
using System.Threading;
using System.Data;
using System.Collections.Generic;
using System.Web;

namespace Services
{
    public class DocumentRepository : GenericRepository<dbEntities, Document>, IDocumentRepository
    {
        public Cost GetCost(string codCost)
        {
            return Context.Costs.Include("ProductPartTask.OptionTypeOfTask")
                .Include("ProductPartTask")
                .Include("ProductTask.OptionTypeOfTask")
                .Include("ProductPartTask.ProductPart")
                .Include("ProductPartTask.ProductPart.ProductPartPrintableArticles")
                .Include("ProductPartTask.ProductPart.ProductPartPrintableArticles.Costs")
                .Include("ProductPartsPrintableArticle")
                .Include("ProductPartsPrintableArticle.ProductPart")
                .Include("ProductPartsPrintableArticle.ProductPart.ProductPartTasks")
                .Include("ProductPartsPrintableArticle.ProductPart.ProductPartTasks.OptionTypeOfTask")
                .Include("ProductPartsPrintableArticle.ProductPart.ProductPartTasks.Costs")

                .Include("ProductTask").Where(x => x.CodCost == codCost).FirstOrDefault();
        }

        public IQueryable<Cost> GetCostsByCodDocumentProduct(string codDocumentProduct)
        {
            return Context.Costs.Include("CostDetails").Where(x => x.CodDocumentProduct == codDocumentProduct);
        }

        public string GetNewCode(Document a)
        {
            //il trucco è di avere un pad left per poter utilizzare il Max per ottenere il maggiore nell'insieme
            //con un colpo solo!!!
            var csCode = (from COD in this.GetAll() select COD.CodDocument).Max();
            return AlphaCode.GetNextCode(csCode??"0").PadLeft(6,'0');        
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

            c.EstimateNumber = c.EstimateNumber.PadLeft(6, '0');

            c.TimeStampTable = DateTime.Now;

            //prodotti in documento
            var ppart = c.DocumentProducts.ToList();
            foreach (var item in c.DocumentProducts.OrderBy(y => y.CodDocumentProduct))
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
        }

        public override IQueryable<Document> GetAll()
        {
            Console.WriteLine(Context.Database.Connection.ConnectionString);
            return Context.Documents.Include("DocumentProducts").Include("DocumentProducts.Costs");
        }

        public override void Save()
        {
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

            DocumentProductCodeRigen(entity);

            foreach (var item in entity.DocumentProducts)
            {
                if (Context.Entry(item).State != System.Data.EntityState.Added)
                {
                    var dcProduct = item;
                    var fromBD2 = Context.DocumentProducts.Single(p => p.CodDocumentProduct == dcProduct.CodDocumentProduct);
                    Context.Entry(fromBD2).CurrentValues.SetValues(dcProduct);

                    //                    Context.Entry(item).State = System.Data.EntityState.Modified;
                }
                foreach (var item2 in item.Costs)
                {
                    if (Context.Entry(item2).State != System.Data.EntityState.Added)
                    {
                        var cost = item2;
                        var fromBD2 = Context.Costs.Single(p => p.CodCost == cost.CodCost);
                        Context.Entry(fromBD2).CurrentValues.SetValues(cost);
                        //                        Context.Entry(item2).State = System.Data.EntityState.Modified;
                    }
                }

            }

            var doc = entity;
            var fromBD = Context.Documents.Single(p => p.CodDocument == doc.CodDocument);
            Context.Entry(fromBD).CurrentValues.SetValues(doc);

            //            base.Edit(entity);
        }

        public new Document GetSingle(string codDocument)
        {
            var query = Context.Documents.Include("DocumentProducts").Include("DocumentProducts.Costs").FirstOrDefault(x => x.CodDocument == codDocument);
            return query;
        }

        public IQueryable<DocumentProduct> GetDocumentProductByCodProduct(string codProduct)
        {
            var query = Context.DocumentProducts.Include("Costs")
                .Include("Costs.CostDetails")
                .Include("Product").Where(x => x.CodProduct == codProduct);
            return query;
        }
        public override void SetDbName(string name)
        {
            base.SetDbName(name);
        }
    }
}