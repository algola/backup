using System;
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

        /// <summary>
        /// get all product description generator
        /// </summary>
        /// <returns></returns>
        public IQueryable<ProductNameGenerator> GetAllProductNameGenerator()
        {

            SincroAllProductNameGenerator();
            var l = Context.ProductNameGenerators.OrderBy(x=>x.CodMenuProduct);
            return l;

        }



        public void SaveProductNameGenerator(ProductNameGenerator a)
        {
            var res = Context.ProductNameGenerators.FirstOrDefault(x => x.CodMenuProduct == a.CodMenuProduct);
            //SE NON ESISTE LO AGGIUNGO E SALVO
            if (res == null)
            {
                //add and save
                a.TimeStampTable = DateTime.Now;
                this.Context.Set<ProductNameGenerator>().Add(a);
                this.Context.SaveChanges();
            }
            else 
            {
                a.TimeStampTable = DateTime.Now;

                Context.Entry(res).CurrentValues.SetValues(a);
                Context.Entry(res).State = System.Data.Entity.EntityState.Modified;
                this.Context.SaveChanges();
            }
        
        
        }
        
        private void SincroAllProductNameGenerator()
        {

            //GENERO L'ELECO RUNTIME
            var runTime = new List<ProductNameGenerator>();
            
            runTime.Add(new ProductNameGenerator { CodMenuProduct = "Vuoto", Generator = "" });
            runTime.Add(new ProductNameGenerator { CodMenuProduct = "Buste", Generator = "" });
            runTime.Add(new ProductNameGenerator { CodMenuProduct = "EtichetteCartellini", Generator = "" });
            runTime.Add(new ProductNameGenerator { CodMenuProduct = "BigliettiVisita", Generator = "" });
            runTime.Add(new ProductNameGenerator { CodMenuProduct = "CartolineInviti", Generator = "" });
            runTime.Add(new ProductNameGenerator { CodMenuProduct = "Volantini", Generator = "" });
            runTime.Add(new ProductNameGenerator { CodMenuProduct = "Pieghevoli", Generator = "" });
            runTime.Add(new ProductNameGenerator { CodMenuProduct = "CartaIntestata", Generator = "" });
            runTime.Add(new ProductNameGenerator { CodMenuProduct = "Locandine", Generator = "" });
            runTime.Add(new ProductNameGenerator { CodMenuProduct = "CartolinePostali", Generator = "" });
            runTime.Add(new ProductNameGenerator { CodMenuProduct = "FogliMacchina", Generator = "" });
            runTime.Add(new ProductNameGenerator { CodMenuProduct = "AltriFormati", Generator = "" });
            runTime.Add(new ProductNameGenerator { CodMenuProduct = "Manifesti", Generator = "" });
            runTime.Add(new ProductNameGenerator { CodMenuProduct = "PVC", Generator = "" });
            runTime.Add(new ProductNameGenerator { CodMenuProduct = "Fotoquadri", Generator = "" });
            runTime.Add(new ProductNameGenerator { CodMenuProduct = "Striscioni", Generator = "" });
            runTime.Add(new ProductNameGenerator { CodMenuProduct = "SuppRigidi", Generator = "Supporti Rigidi '%PRODNAME' @%PRINTPARTTASK @%TYPEMATERIAL %NAMEMATERIAL @F.to cm %PARTFORMATOPEN @%PARTTASKS" });
            runTime.Add(new ProductNameGenerator { CodMenuProduct = "Poster", Generator = "" });
            runTime.Add(new ProductNameGenerator { CodMenuProduct = "PuntoMetallico", Generator = "" });
            runTime.Add(new ProductNameGenerator { CodMenuProduct = "SpiraleMetallica", Generator = "" });
            runTime.Add(new ProductNameGenerator { CodMenuProduct = "BrossuraFresata", Generator = "" });
            runTime.Add(new ProductNameGenerator { CodMenuProduct = "BrossuraCucitaFilo", Generator = "" });
            runTime.Add(new ProductNameGenerator { CodMenuProduct = "RivistePostalizzazione", Generator = "" });
            runTime.Add(new ProductNameGenerator { CodMenuProduct = "SchedeNonRilegate", Generator = "" });
            runTime.Add(new ProductNameGenerator { CodMenuProduct = "EtichetteRotolo", Generator = "Etichette a bobina 1 pista '%PRODNAME' @%PRINTPARTTASK @%TYPEMATERIAL %NAMEMATERIAL Adesivo: %ADESHIVEMATERIAL @F.to mm %PARTFORMATOPENMM o similare in ns possesso previo Vs. conferma @Uscita lato %PARTFORMATOPENMMSIDE1 @%PARTTASKS" });
            runTime.Add(new ProductNameGenerator { CodMenuProduct = "EtichetteSagRotolo", Generator = "Etichette sagomate a bobina 1 pista '%PRODNAME' @%PRINTPARTTASK @%TYPEMATERIAL %NAMEMATERIAL Adesivo: %ADESHIVEMATERIAL @F.to mm sagomato %PARTFORMATOPENMM @Uscita lato %PARTFORMATOPENMMSIDE1 @%PARTTASKS" });
            runTime.Add(new ProductNameGenerator { CodMenuProduct = "FasceGommateRotolo", Generator = "" });
            runTime.Add(new ProductNameGenerator { CodMenuProduct = "Inciso", Generator = "" });
            runTime.Add(new ProductNameGenerator { CodMenuProduct = "Fotopolimero", Generator = "" });
            //CARICO I DATI DAL DATABASE
            var db = Context.ProductNameGenerators;          
            //PER OGNI ELEMENTO A RUNTIME CONTROLLO SE ESISTE NEL DATABASE

            foreach (var item in runTime)
            {
                var res = db.FirstOrDefault(x => x.CodMenuProduct == item.CodMenuProduct);
                //SE NON ESISTE LO AGGIUNGO E SALVO
                if (res == null) 
                {
                    //add and save
                    item.TimeStampTable = DateTime.Now;
                    this.Context.Set<ProductNameGenerator>().Add(item);
                    this.Context.SaveChanges();                
                }
            }


        }

        public ProductNameGenerator GetProductNameGenerator(string id)
        {
            SincroAllProductNameGenerator();
            var l = GetAllProductNameGenerator();
            return l.FirstOrDefault(x => x.CodMenuProduct == id);
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
            var numWare = Context.warehouseSpec.Count();

            //update with new warehouse
            var lstProd = Context.Products.Include("WarehouseArticles").Where(x => x.WarehouseArticles.Count != numWare).ToList();

            foreach (var product in lstProd)
            {
                if (numWare != product.WarehouseArticles.Count())
                {
                    foreach (var warehouse in Context.warehouseSpec)
                    {
                        //Does article esist in Warehouse?
                        var res = product.WarehouseArticles.Where(x => x.CodWarehouse == warehouse.CodWarehouse);
                        //no warehouse
                        if (res.Count() == 0)
                        {
                            var newP = new WarehouseProduct
                            {
                                CodWarehouse = warehouse.CodWarehouse,
                                CodProduct = product.CodProduct
                            };

                            product.WarehouseArticles.Add(newP);
                            this.Context.Set<WarehouseProduct>().Add(newP);
                        }
                    }
                }
                product.ProductCodeRigen();
                Save();
            }

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
               .Except(modOrAdded.OfType<ProductGraphLink>())
               .Except(modOrAdded.OfType<WarehouseProduct>());

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

            var query = Context.Products.Include("ProductParts").Include("ProductTasks.OptionTypeOfTask").Include("ProductParts.ProductPartPrintableArticles").Include("ProductTasks.OptionTypeOfTask.TypeOfTask").Include("ProductTasks").Include("ProductGraphLinks").Include("warehousearticles").FirstOrDefault(x => x.CodProduct == codProduct);


            if (query != null)
            {
                //Including ProductPartTask creates a problem with autogenerated SQL statement... 
                //so it's necessary inject single ProductPartTask list e to each ProductPart manually
                foreach (var item in query.ProductParts)
                {
                    List<ProductPartTask> q = Context.ProductPartTasks.Include("OptionTypeOfTask").Where(x => x.CodProductPart == item.CodProductPart).ToList();
                    item.ProductPartTasks = q.ToList();
                }

                var numWare = Context.warehouseSpec.Count();

                if (numWare != query.WarehouseArticles.Count())
                {
                    foreach (var item in Context.warehouseSpec)
                    {
                        //Does article esist in Warehouse?
                        var res = query.WarehouseArticles.Where(x => x.CodWarehouse == item.CodWarehouse);
                        //no warehouse
                        if (res == null)
                        {
                            query.WarehouseArticles.Add(new WarehouseProduct
                            {
                                CodWarehouse = item.CodWarehouse,
                                CodProduct = codProduct
                            });
                        }
                    }


                }

                //                    query.WarehouseArticles.Add(new WarehouseProduct());

            }

            return query;
        }

        public override void SetDbName(string name)
        {
            base.SetDbName(name);
        }
    }
}