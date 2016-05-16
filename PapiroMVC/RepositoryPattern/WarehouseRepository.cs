using System;
using System.Linq;
using PapiroMVC.Models;
using PapiroMVC.DbCodeManagement;
using System.Threading;
using PapiroMVC.Validation;
using System.Collections.Generic;

namespace Services
{
    /// <summary>
    /// repository magazzeno
    /// </summary>
    public class WarehouseRepository : GenericRepository<dbEntities, WarehouseItem>, IWarehouseRepository
    {

        public void UpdateArticle(WarehouseItem entity)
        {
            //object and all movments
            var obj = Context.warehousearticles.Include("warehousearticlemovs").SingleOrDefault(p => p.CodWarehouseArticle == entity.CodWarehouseArticle);

            //tipo di movimento --> 0 = scarico, 1 carico, 2 ordine, 3 impegno
            var loads = obj.WarehouseArticleMovs.Where(x => x.TypeOfMov == 1).AsEnumerable().Sum(o => o.Quantity);
            var unloads = obj.WarehouseArticleMovs.Where(x => x.TypeOfMov == 0).AsEnumerable().Sum(o => o.Quantity);

            var reserves = obj.WarehouseArticleMovs.Where(x => x.TypeOfMov == 3).AsEnumerable().Sum(o => o.Quantity);
            var orders = obj.WarehouseArticleMovs.Where(x => x.TypeOfMov == 2).AsEnumerable().Sum(o => o.Quantity);

            obj.QuantityOnHand = loads - unloads;
            obj.Available = obj.QuantityOnHand - reserves;
            obj.PotentialQuantityOnHand = obj.QuantityOnHand + orders;
            obj.PotentialAvailable = obj.Available + orders;

            //we need to update status!!!



            Edit(obj);

        }

        public override IQueryable<WarehouseItem> GetAll()
        {
            return Context.warehousearticles.Include("warehousearticlemovs").Include("Article").Include("Product").Include("WarehouseSpec");
        }

        public override void Edit(WarehouseItem entity)
        {
            var fromBD = Context.warehousearticles.SingleOrDefault(p => p.CodWarehouseArticle == entity.CodWarehouseArticle);
            if (fromBD != null)
            {
                Context.Entry(fromBD).CurrentValues.SetValues(entity);
                Context.Entry(fromBD).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                Context.Entry(entity).State = System.Data.Entity.EntityState.Added;
            }
        }

        public new WarehouseItem GetSingle(string codWarehouseArticle)
        {
            return Context.warehousearticles.Include("warehousearticlemovs").Include("article").Include("Product").FirstOrDefault(x => x.CodWarehouseArticle == codWarehouseArticle);
        }

        public WarehouseItem GetSingleProduct(string codProduct, string codWarehouse)
        {
            return Context.warehousearticles.Include("warehousearticlemovs").Include("article").Include("Product").FirstOrDefault(x => x.CodProduct == codProduct && x.CodWarehouse == codWarehouse);
        }

        public WarehouseItem GetSingleArticle(string codArticle, string codWarehouse)
        {
            return Context.warehousearticles.Include("warehousearticlemovs").Include("article").Include("Product").FirstOrDefault(x => x.CodArticle == codArticle && x.CodWarehouse == codWarehouse);
        }

        public override void SetDbName(string name)
        {
            base.SetDbName(name);
        }

        /// <summary>
        /// get all movments
        /// </summary>
        /// <param name="codWarehouseWarehouseArticleMov"></param>
        /// <returns></returns>
        public IQueryable<WarehouseArticleMov> GetAllMovs(string codWarehouseArticle)
        {
            return Context.warehousearticlemovs.Include("warehousearticle").Where(x => x.CodWarehouseArticle == codWarehouseArticle);
        }

        public IQueryable<WarehouseArticleMov> GetAllMovsProduct(string codProduct)
        {
            IQueryable<WarehouseArticleMov> ret = Context.warehousearticlemovs.Include("warehousearticle").Include("WarehouseArticle.WarehouseSpec").Include("warehousearticle.Product");

            if (codProduct !=null)
            {
                ret = ret.Where(x => x.WarehouseArticle.CodProduct == codProduct);
            }

            return ret;
        }

        public IQueryable<WarehouseArticleMov> GetAllMovsArticle(string codArticle)
        {            
            IQueryable<WarehouseArticleMov> ret = Context.warehousearticlemovs
                .Include("WarehouseArticle")
                .Include("WarehouseArticle.Article")
                .Include("WarehouseArticle.WarehouseSpec")
                .Include("Document");

            if (codArticle !=null && codArticle != "")
            {
                ret = ret.Where(x => x.WarehouseArticle.CodArticle == codArticle);
            }

            return ret;
        }

        public void DeleteMov(WarehouseArticleMov entity)
        {
            Context.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
            //            throw new NotImplementedException();
        }

        public void EditMov(WarehouseArticleMov entity)
        {
            Context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
//            throw new NotImplementedException();
        }


       public  IQueryable<WarehouseSpec> GetWarehouseList()
        {
            if (Context.warehouseSpec.Count() == 0)
            {
                var entity = new WarehouseSpec { CodWarehouse = "001", TimeStampTable = DateTime.Now, WarehouseName = "Default" };               
                Context.warehouseSpec.Add(entity);
                Context.Entry(entity).State = System.Data.Entity.EntityState.Added;
                Context.SaveChanges();
            }
           
           return Context.warehouseSpec;
        }


        /// <summary>
        /// newMovCode
        /// </summary>
        public string GetNewMovCode(WarehouseArticleMov entity)
        {
            //il trucco è di avere un pad left per poter utilizzare il Max per ottenere il maggiore nell'insieme
            //con un colpo solo!!!
            // var csCode = (from COD in this.GetAll() select COD.CodDocument).Max();
            var csCode = Context.Database.SqlQuery<string>("SELECT MAX(CodWarehouseArticleMov) AS CodWarehouseArticleMov FROM WarehouseArticleMovs").FirstOrDefault<string>();
            return AlphaCode.GetNextCode(csCode ?? "0").PadLeft(6, '0');

        }

        public void AddMov(WarehouseArticleMov entity)
        {
            entity.WarehouseArticle = null;
            entity.TimeStampTable = DateTime.Now;
            entity.Date = DateTime.Now;
            Context.Entry(entity).State = System.Data.Entity.EntityState.Added;
        }

    }
}