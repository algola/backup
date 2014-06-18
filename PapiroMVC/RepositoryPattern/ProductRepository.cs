﻿using System;
using System.Linq;
using PapiroMVC.Models;
using PapiroMVC.DbCodeManagement;
using System.Threading;
using System.Data;
using System.Collections.Generic;

namespace Services
{
    public class ProductRepository : GenericRepository<dbEntities, Product>, IProductRepository
    {
        public String GetProductNameGenerator(string id)
        {
            switch (id)
            {
                case "EtichetteRotolo":
                    return "Etichette a bobina 1 pista '%PRODNAME' @%PRINTPARTTASK @%TYPEMATERIAL %NAMEMATERIAL Adesivo: %ADESHIVEMATERIAL @F.to mm %PARTFORMATOPENMM o similare in ns possesso previo Vs. conferma @Uscita lato %PARTFORMATOPENMMSIDE1 @%PARTTASKS";
                case "EtichetteSagRotolo":
                    return "Etichette sagomate a bobina 1 pista '%PRODNAME' @%PRINTPARTTASK @%TYPEMATERIAL %NAMEMATERIAL Adesivo: %ADESHIVEMATERIAL @F.to mm sagomato %PARTFORMATOPENMM @Uscita lato %PARTFORMATOPENMMSIDE1 @%PARTTASKS";
                case "FasceGommateRotolo":
                    return "prova descrizione";
                default:
                    return "";
            }
        }

        public string GetNewCode(Product a)
        {
            // var csCode = (from COD in this.GetAll() select COD.CodProduct).Max();

            var csCode = Context.Database.SqlQuery<string>("SELECT MAX(CodProduct) AS CodProduct FROM Products").FirstOrDefault<string>();
            return AlphaCode.GetNextCode(csCode ?? "0").PadLeft(6, '0');
        }

        private void ProductPartCodeRigen(Product c)
        {
            //polimorfismo
            c.ProductCodeRigen();
        }

        public override void Add(Product entity)
        {
            ProductPartCodeRigen(entity);
            base.Add(entity);
        }

        public override IQueryable<Product> GetAll()
        {
            Console.WriteLine(Context.Database.Connection.ConnectionString);
            return Context.Products.Include("ProductParts").Include("ProductTasks").Include("ProductGraphLinks");
        }

        public override void Save()
        {
            //we want to save anly product and related-closed class
            List<Object> modOrAdded = Context.ChangeTracker.Entries()
                .Where(x => x.State == System.Data.Entity.EntityState.Modified
                || x.State == System.Data.Entity.EntityState.Added)
                .Select(x => x.Entity).ToList();

            var notAll = modOrAdded.Except(modOrAdded.OfType<Product>())
               .Except(modOrAdded.OfType<ProductPart>())
               .Except(modOrAdded.OfType<ProductPartTask>())
               .Except(modOrAdded.OfType<ProductTask>())
               .Except(modOrAdded.OfType<ProductPartsPrintableArticle>())
               .Except(modOrAdded.OfType<ProductGraphLink>());

            foreach (var item in notAll)
            {
                Context.Entry(item).State = System.Data.Entity.EntityState.Unchanged;
            }

            base.Save();
        }


        public override void Edit(Product entity)
        {
            //we can have some DocumentProduct added and some just saved so...
            var part = Context.ProductParts.Where(x => x.CodProduct == entity.CodProduct).ToList();
            entity.ProductParts = part.Union(entity.ProductParts, new ProductPartComparer()).ToList();

            ProductPartCodeRigen(entity);

            foreach (var item in entity.ProductParts)
            {
                Context.Entry(item).State = System.Data.Entity.EntityState.Modified;

                foreach (var item2 in item.ProductPartPrintableArticles)
                {
                    Context.Entry(item2).State = System.Data.Entity.EntityState.Modified;
                }

                foreach (var item2 in item.ProductPartTasks)
                {
                    Context.Entry(item2).State = System.Data.Entity.EntityState.Modified;
                }
            }

            foreach (var item in entity.ProductTasks)
            {
                Context.Entry(item).State = System.Data.Entity.EntityState.Modified;
            }

            foreach (var item in entity.ProductGraphLinks)
            {
                Context.Entry(item).State = System.Data.Entity.EntityState.Modified;
            }

            base.Edit(entity);
        }

        public Product GetSingle(string codProduct)
        {

            //            var query = Context.Products.Include("ProductParts").Include("ProductParts.ProductPartTasks").Include("ProductTasks.OptionTypeOfTask").Include("ProductParts.ProductPartPrintableArticles").Include("ProductTasks.OptionTypeOfTask.TypeOfTask").Include("ProductTasks").FirstOrDefault(x => x.CodProduct == codProduct);

            var query = Context.Products.Include("ProductParts").Include("ProductTasks.OptionTypeOfTask").Include("ProductParts.ProductPartPrintableArticles").Include("ProductTasks.OptionTypeOfTask.TypeOfTask").Include("ProductTasks").Include("ProductGraphLinks").FirstOrDefault(x => x.CodProduct == codProduct);

            //Including ProductPartTask creates a problem with autogenerated SQL statement... 
            //so it's necessary inject single ProductPartTask list e to each ProductPart manually
            foreach (var item in query.ProductParts)
            {
                List<ProductPartTask> q = Context.ProductPartTasks.Include("OptionTypeOfTask").Where(x => x.CodProductPart == item.CodProductPart).ToList();
                item.ProductPartTasks = q.ToList();
            }

            return query;
        }

        public override void SetDbName(string name)
        {
            base.SetDbName(name);
        }
    }
}