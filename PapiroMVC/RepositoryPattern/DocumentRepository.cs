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
        public string GetNewCode(Document a)
        {
            var codes = (from COD in this.GetAll() select COD.CodDocument).ToArray().OrderBy(x => x, new SemiNumericComparer());
            var csCode = codes.Count() != 0 ? codes.Last() : "0";

            return AlphaCode.GetNextCode(csCode);
        }

        private void DocumentProductCodeRigen(Document c)
        {
            c.TimeStampTable = DateTime.Now;

            //prodotti in documento
            var ppart = c.DocumentProducts.ToList();
            foreach (var item in c.DocumentProducts)
            {             
                item.CodDocumentProduct = c.CodDocument + "-" +  ppart.IndexOf(item).ToString();
                item.CodDocument = c.CodDocument;
                item.TimeStampTable = DateTime.Now;

                //task della parte del prodotto
                var pptask = item.Costs.ToList();
                foreach (var item2 in item.Costs)
                {
                    item2.CodDocumentProduct = item.CodDocumentProduct;
                    item2.TimeStampTable = DateTime.Now;
                    item2.CodCost = item.CodDocumentProduct + "-" + pptask.IndexOf(item2).ToString();                    
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
            return (Document) System.Web.HttpContext.Current.Session["document"];  
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
                    Context.Entry(item).State = System.Data.EntityState.Modified;
                }
                foreach (var item2 in item.Costs)
                {
                    Context.Entry(item2).State = System.Data.EntityState.Modified;
                }

            }

            base.Edit(entity);
        }

        public Document GetSingle(string codDocument)
        {
            var query = Context.Documents.Include("DocumentProducts").Include("DocumentProducts.Costs").FirstOrDefault(x => x.CodDocument == codDocument);           
            return query;
        }

        public IQueryable<DocumentProduct> GetDocumentProductByCodProduct(string codProduct)
        {
            var query = Context.DocumentProducts.Include("Costs").Include("Product").Where(x => x.CodProduct == codProduct);
            return query;
        }
        public override void SetDbName(string name)
        {
            base.SetDbName(name);
        }
    }
}