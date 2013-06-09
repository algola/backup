using System;
using System.Linq;
using PapiroMVC.Models;
using PapiroMVC.DbCodeManagement;
using System.Threading;

namespace Services
{
    public class ProductRepository : GenericRepository<dbEntities, Product>, IProductRepository
    {
        public string GetNewCode(Product a, ICustomerSupplierRepository customerSupplierRepository)
        {
            var codes = (from COD in this.GetAll() select COD.CodProduct).ToArray().OrderBy(x => x, new SemiNumericComparer());
            var csCode = codes.Count() != 0 ? codes.Last() : "0";

            return AlphaCode.GetNextCode(csCode);
        }

        private void ProductPartCodeRigen(Product c)
        {
            c.TimeStampTable = DateTime.Now;

            foreach (var item in c.ProductParts)
            {
                item.TimeStampTable = DateTime.Now;
            }

            //switch (c.TypeOfProduct)
            //{
                   
            //    case Product.ProductType.SheetPrintableProduct:

            //        /*
            //        #region Cutted
            //        try
            //        {
            //            var cuttedPart = ((SheetPrintableProductCuttedPart)c.ProductParts.First(x =>
            //                x.TypeOfProductPart == ProductPart.ProductPartType.SheetPrintableProductCuttedPart));

            //            cuttedPart.PartPerKg = cuttedPart.PartPerKg == null ?
            //                null : Convert.ToDouble(cuttedPart.PartPerKg,
            //                Thread.CurrentThread.CurrentUICulture).ToString("#,0.000", Thread.CurrentThread.CurrentUICulture);

            //            cuttedPart.PartPerSheet = cuttedPart.PartPerSheet == null ?
            //            null :
            //            Convert.ToDouble(cuttedPart.PartPerSheet, Thread.CurrentThread.CurrentUICulture).ToString("#,0.000", Thread.CurrentThread.CurrentUICulture);

            //            cuttedPart.CodProduct = c.CodProduct;
            //            cuttedPart.CodProductPart = c.CodProduct + "_CTD";

            //        }
            //        catch (Exception e)
            //        {
            //            Console.WriteLine(e.Message);
            //        }

            //        #endregion
            //        */

            //        #region Paked
            //        try
            //        {
            //            var pakedPart = ((SheetPrintableProductPakedPart)c.ProductParts.First(x =>
            //                x.TypeOfProductPart == ProductPart.ProductPartType.SheetPrintableProductPakedPart));

            //            pakedPart.PartPerKg = pakedPart.PartPerKg == null ?
            //                null : Convert.ToDouble(pakedPart.PartPerKg,
            //                Thread.CurrentThread.CurrentUICulture).ToString("#,0.000", Thread.CurrentThread.CurrentUICulture);

            //            pakedPart.PartPerSheet = pakedPart.PartPerSheet == null ?
            //            null :
            //            Convert.ToDouble(pakedPart.PartPerSheet, Thread.CurrentThread.CurrentUICulture).ToString("#,0.000", Thread.CurrentThread.CurrentUICulture);

            //            pakedPart.CodProduct = c.CodProduct;
            //            pakedPart.CodProductPart = c.CodProduct + "_PKC";
            //        }
            //        catch (Exception e)
            //        {
            //            Console.WriteLine(e.Message);
            //        }
            //        #endregion

            //        #region Pallet
            //        try
            //        {
            //            var palletPart = ((SheetPrintableProductPalletPart)c.ProductParts.First(x =>
            //            x.TypeOfProductPart == ProductPart.ProductPartType.SheetPrintableProductPalletPart));

            //            palletPart.PartPerKg = palletPart.PartPerKg == null ?
            //                null : Convert.ToDouble(palletPart.PartPerKg,
            //                Thread.CurrentThread.CurrentUICulture).ToString("#,0.000", Thread.CurrentThread.CurrentUICulture);

            //            palletPart.PartPerSheet = palletPart.PartPerSheet == null ?
            //            null :
            //            Convert.ToDouble(palletPart.PartPerSheet, Thread.CurrentThread.CurrentUICulture).ToString("#,0.0000", Thread.CurrentThread.CurrentUICulture);

            //            palletPart.CodProduct = c.CodProduct;
            //            palletPart.CodProductPart = c.CodProduct + "_PLC";
            //        }
            //        catch (Exception e)
            //        {
            //            Console.WriteLine(e.Message);
            //        }

            //        #endregion

            //        break;
            //    case Product.ProductType.RollPrintableProduct:
            //        #region Standard
            //        try
            //        {
            //            var standardPart = ((RollPrintableProductStandardPart)c.ProductParts.First(x =>
            //            x.TypeOfProductPart == ProductPart.ProductPartType.RollPrintableProductStandardPart));

            //            standardPart.PartPerMq = standardPart.PartPerMq == null ?
            //                null : Convert.ToDouble(standardPart.PartPerMq,
            //                Thread.CurrentThread.CurrentUICulture).ToString("#,0.000", Thread.CurrentThread.CurrentUICulture);

            //            standardPart.PartPerMl = standardPart.PartPerMl == null ?
            //            null :
            //            Convert.ToDouble(standardPart.PartPerMl, Thread.CurrentThread.CurrentUICulture).ToString("#,0.0000", Thread.CurrentThread.CurrentUICulture);

            //            standardPart.CodProduct = c.CodProduct;
            //            standardPart.CodProductPart = c.CodProduct + "_STC";
            //        }
            //        catch (Exception e)
            //        {
            //            Console.WriteLine(e.Message);
            //        }

            //        #endregion
            //        break;
            //    case Product.ProductType.RigidPrintableProduct:
            //        break;
            //    case Product.ProductType.ObjectPrintableProduct:
            //        break;
            //    default:
            //        break;
            //}
        }

        public override void Add(Product entity)
        {
            ProductPartCodeRigen(entity);
            base.Add(entity);
        }

        public override IQueryable<Product> GetAll()
        {
            Console.WriteLine(Context.Database.Connection.ConnectionString);
            return Context.Products.Include("ProductParts").Include("ProductTasks");
        }

        public override void Edit(Product entity)
        {
            ProductPartCodeRigen(entity);

            foreach (var item in entity.ProductParts)
            {
                Context.Entry(item).State = System.Data.EntityState.Modified;
            }
            foreach (var item in entity.ProductTasks)
            {
                Context.Entry(item).State = System.Data.EntityState.Modified;
            }
            base.Edit(entity);
        }

        public Product GetSingle(string codProduct)
        {
            var query = Context.Products.Include("ProductParts").Include("ProductTasks").FirstOrDefault(x => x.CodProduct == codProduct);
            return query;
        }

        public override void SetDbName(string name)
        {
            base.SetDbName(name);
        }
    }
}