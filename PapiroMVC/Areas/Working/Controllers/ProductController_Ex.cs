using System;
using System.Linq;
using System.Web.Mvc;
using PapiroMVC.Models;
using Mvc.HtmlHelpers;
using PapiroMVC.Validation;
using System.Xml;

namespace PapiroMVC.Areas.Working.Controllers
{
    public partial class ProductController : PapiroMVC.Controllers.ControllerAlgolaBase
    {
        public ActionResult ProductList(GridSettings gridSettings)
        {
            string codProductFilter = string.Empty;
            string productNameFilter = string.Empty;
            string typeOfProductFilter = string.Empty;
            string warehouseName = string.Empty;
            //read from validation's language file

            //LANGFILE
            var resman = new System.Resources.ResourceManager(typeof(Strings).FullName, typeof(Strings).Assembly);
            string bookSheetType = resman.GetString("ProductBookSingleSheetType");
            string singleSheetType = resman.GetString("ProductSingleSheetType");
            string blockSheetType = resman.GetString("ProductBlockSheetType");

            if (gridSettings.isSearch)
            {
                codProductFilter = gridSettings.where.rules.Any(r => r.field == "CodProduct") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "CodProduct").data : string.Empty;

                productNameFilter = gridSettings.where.rules.Any(r => r.field == "ProductName") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "ProductName").data : string.Empty;

                typeOfProductFilter = gridSettings.where.rules.Any(r => r.field == "TypeOfProduct") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "TypeOfProduct").data : string.Empty;

                warehouseName = gridSettings.where.rules.Any(r => r.field == "WarehouseName") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "WarehouseName").data : string.Empty;

            }


            var fff = productRepository.GetAll().ToArray();

            var q = warehouseRepository.GetAll();

            if (!string.IsNullOrEmpty(codProductFilter))
            {
                q = q.Where(c => c.CodProduct.ToLower().Contains(codProductFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(productNameFilter))
            {
                q = q.Where(c => c.Product.ProductName.ToLower().Contains(productNameFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(warehouseName))
            {
                q = q.Where(c => c.WarehouseSpec.WarehouseName.ToLower().Contains(warehouseName.ToLower()));
            }


            switch (gridSettings.sortColumn)
            {
                case "CodProduct":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.CodProduct) : q.OrderBy(c => c.CodProduct);
                    break;
                case "ProductName":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.Product.ProductName) : q.OrderBy(c => c.Product.ProductName);
                    break;
                case "WarehouseName":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.WarehouseSpec.WarehouseName) : q.OrderBy(c => c.WarehouseSpec.WarehouseName);
                    break;
            }

            var q2 = q.ToList();

            var pp = q2.OfType<WarehouseProduct>();

            var q3 = pp.Skip((gridSettings.pageIndex - 1) * gridSettings.pageSize).Take(gridSettings.pageSize).ToList();

            int totalRecords = q.Count();

            // create json data
            int pageIndex = gridSettings.pageIndex;
            int pageSize = gridSettings.pageSize;

            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            long startRow = (pageIndex - 1) * pageSize;
            long endRow = startRow + pageSize;

            var jsonData = new
            {
                total = totalPages,
                page = pageIndex,
                records = totalRecords,
                rows =
                (
                    from a in q3
                    select new
                    {
                        id = a.CodProduct,
                        cell = new string[] 
                        {                       
                            a.CodProduct,
                            a.CodProduct,
                            a.WarehouseSpec!=null?a.WarehouseSpec.WarehouseName:"",
                            a.Product!=null?a.Product.ProductName:"",
                            ((a.QuantityOnHand??0) <= (a.MinQuantity??0))?"Sotto Scorta":"",
                            a.QuantityOnHand==null?"0":a.QuantityOnHand.ToString()
                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

     
        public ActionResult ProductNameGeneratorList(GridSettings gridSettings)
        {
            //read from validation's language file


            var q = productRepository.GetAllProductNameGenerator();
            var q3 = q.Skip((gridSettings.pageIndex - 1) * gridSettings.pageSize).Take(gridSettings.pageSize).ToList();

            int totalRecords = q.Count();

            // create json data
            int pageIndex = gridSettings.pageIndex;
            int pageSize = gridSettings.pageSize;

            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            int startRow = (pageIndex - 1) * pageSize;
            int endRow = startRow + pageSize;

            var jsonData = new
            {
                total = totalPages,
                page = pageIndex,
                records = totalRecords,
                rows =
                (
                    from a in q3
                    select new
                    {
                        id = a.CodMenuProduct,
                        cell = new string[] 
                        {                       
                            a.CodMenuProduct,
                            a.CodMenuProduct,
                            a.Generator
                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}
