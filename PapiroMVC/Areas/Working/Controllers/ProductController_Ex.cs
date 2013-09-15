using System;
using System.Linq;
using System.Web.Mvc;
using PapiroMVC.Models;
using Mvc.HtmlHelpers;
using PapiroMVC.Validation;

namespace PapiroMVC.Areas.Working.Controllers
{
    public partial class ProductController : PapiroMVC.Controllers.ControllerAlgolaBase
    {

        public ActionResult ProductList(GridSettings gridSettings)
        {
            string codProductFilter = string.Empty;
            string productNameFilter = string.Empty;
            string typeOfProductFilter = string.Empty;

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
            }

            var q = productRepository.GetAll();

            if (!string.IsNullOrEmpty(codProductFilter))
            {
                q = q.Where(c => c.CodProduct.ToLower().Contains(codProductFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(productNameFilter))
            {
                q = q.Where(c => c.ProductName.ToLower().Contains(productNameFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(typeOfProductFilter))
            {
                Boolean IsBook = false, isSingle = false, IsBlock = false;

                //to match with language we have to compare filter with resource
                IsBook = (bookSheetType.ToLower().Contains(typeOfProductFilter.ToLower()));
                isSingle = (singleSheetType.ToLower().Contains(typeOfProductFilter.ToLower()));
                IsBlock = (blockSheetType.ToLower().Contains(typeOfProductFilter.ToLower()));

                var a = IsBook ? (IQueryable<Product>)q.OfType<ProductBookSheet>() : productRepository.FindBy(x=>x.CodProduct=="");
                var b = isSingle ? (IQueryable<Product>)q.OfType<ProductSingleSheet>() : productRepository.FindBy(x => x.CodProduct == "");
                var c = IsBlock ? (IQueryable<Product>)q.OfType<ProductBlockSheet>() : productRepository.FindBy(x => x.CodProduct == "");

                var res = (a.Union(b).Union(c));
                q = (IQueryable<Product>)res;                            
            }

            switch (gridSettings.sortColumn)
            {
                case "CodProduct":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.CodProduct) : q.OrderBy(c => c.CodProduct);
                    break;
                case "ProductName":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.ProductName) : q.OrderBy(c => c.ProductName);
                    break;
            }

            var q2 = q.ToList();
            var q3 = q2.Skip((gridSettings.pageIndex - 1) * gridSettings.pageSize).Take(gridSettings.pageSize).ToList();

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
                        id = a.CodProduct,
                        cell = new string[] 
                        {                       
                            a.CodProduct,
                            a.CodProduct,
                            a.TypeOfProduct.ToString(),
                            a.ProductName,
                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }


    }
}
