using System;
using System.Linq;
using System.Web.Mvc;
using PapiroMVC.Models;
using Mvc.HtmlHelpers;
using PapiroMVC.Validation;
using Resources;
using System.Resources;

namespace PapiroMVC.Areas.Working.Controllers
{
    public partial class DocumentController : PapiroMVC.Controllers.ControllerAlgolaBase
    {
        public ActionResult Costs(GridSettings gridSettings, String codDocumentProduct)
        {
            var q = documentRepository.GetCostsByCodDocumentProduct(codDocumentProduct).Where(x => !(x.Hidden ?? false));

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
                        id = a.CodCost,
                        cell = new string[] 
                        {                       
                            a.CodCost,
                            a.CodDocumentProduct,
                            (a.Locked??false).ToString(),
                            (a.TypeOfCalcolous??0).ToString(), //0=incluso, 1=Aux, 2=escluso
                            a.Description,
                            (a.CostDetails.FirstOrDefault()!=null?a.CostDetails.FirstOrDefault().TypeOfQuantity??0:5).ToString(),
                            (a.Quantity??0).ToString(),                            
                            a.UnitCost,
                            a.TotalCost,
                            (a.Markup??0).ToString(),
                            a.GranTotalCost
                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        private IQueryable<Document> DocumentList(GridSettings gridSettings)
        {
            string customerFilter = string.Empty;
            string codDocumentFilter = string.Empty;
            string documentNameFilter = string.Empty;

            if (gridSettings.isSearch)
            {
                customerFilter = gridSettings.where.rules.Any(r => r.field == "Customer") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "Customer").data : string.Empty;

                codDocumentFilter = gridSettings.where.rules.Any(r => r.field == "CodDocument") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "CodDocument").data : string.Empty;

                documentNameFilter = gridSettings.where.rules.Any(r => r.field == "DocumentName") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "DocumentName").data : string.Empty;

            }

            var q = documentRepository.GetAll();

            if (!string.IsNullOrEmpty(customerFilter))
            {
                q = q.Where(c => c.Customer != null && c.Customer.ToLower().Contains(customerFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(codDocumentFilter))
            {
                q = q.Where(c => c.CodDocument.ToLower().Contains(codDocumentFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(documentNameFilter))
            {
                q = q.Where(c => c.DocumentName != null && c.DocumentName.ToLower().Contains(documentNameFilter.ToLower()));
            }

            switch (gridSettings.sortColumn)
            {
                case "Customer":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.Customer) : q.OrderBy(c => c.Customer);
                    break;
                case "CodDocument":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.CodDocument) : q.OrderBy(c => c.CodDocument);
                    break;
                case "DocumentName":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.DocumentName) : q.OrderBy(c => c.DocumentName);
                    break;
                default:
                    q = q.OrderByDescending(c => c.CodDocument);
                    break;
            }
            return q;
        }

        public ActionResult EstimateList(GridSettings gridSettings)
        {

            var q = this.DocumentList(gridSettings).OfType<Estimate>();
            var q3 = q.Skip((gridSettings.pageIndex - 1) * gridSettings.pageSize).Take(gridSettings.pageSize);

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
                    from a in q3.ToList()
                    select new
                    {
                        id = a.CodDocument,
                        cell = new string[] 
                        {                       
                            a.CodDocument,
                            a.Customer,
                            a.EstimateNumber,
                            a.DocumentName,
                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);


        }

        /// <summary>
        /// List of DocumentProduct in a Document ie. Estimate
        /// </summary>
        /// <param name="CodDocument"></param>
        /// <param name="gridSettings"></param>
        /// <returns></returns>
        public ActionResult DocumentProductList(string CodDocument, GridSettings gridSettings)
        {

            if (CodDocument != null)
            {
                var q = documentRepository.GetSingle(CodDocument).DocumentProducts;

                foreach (var i in q)
                {
                    i.Product = productRepository.GetSingle(i.CodProduct);
                }

                var q2 = q.Select((p) => new { CodProduct = p.CodProduct, ProductName = p.ProductName, ProductRefName = p.Product.ProductRefName }).Distinct();

                //var q2 = q.ToList();
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
                                a.ProductRefName,
                                a.ProductName   //attributo derivato
                            }
                        }
                    ).ToArray()
                };


                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// List of all DocumentProducts
        /// </summary>
        /// <param name="gridSettings"></param>
        /// <returns></returns>
        public ActionResult DocumentProductsList(GridSettings gridSettings)
        {
            string codProductFilter = string.Empty;
            string productNameFilter = string.Empty;
            string quantityFilter = string.Empty;
            string unitPriceFilter = string.Empty;
            string totalAmountFilter = string.Empty;

            if (gridSettings.isSearch)
            {
                codProductFilter = gridSettings.where.rules.Any(r => r.field == "CodProduct") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "CodProduct").data : string.Empty;

                productNameFilter = gridSettings.where.rules.Any(r => r.field == "ProductName") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "ProductName").data : string.Empty;

                quantityFilter = gridSettings.where.rules.Any(r => r.field == "Quantity") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "Quantity").data : string.Empty;

                unitPriceFilter = gridSettings.where.rules.Any(r => r.field == "UnitPrice") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "UnitPrice").data : string.Empty;

                totalAmountFilter = gridSettings.where.rules.Any(r => r.field == "TotalAmount") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "TotalAmount").data : string.Empty;
            }

            var q = documentRepository.GetAllDocumentProducts();

            if (!string.IsNullOrEmpty(codProductFilter))
            {
                q = q.Where(c => c.CodProduct != null && c.CodProduct.ToLower().Contains(codProductFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(productNameFilter))
            {
                q = q.Where(c => c.ProductName != null && c.ProductName.ToLower().Contains(productNameFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(quantityFilter))
            {
                q = q.Where(c => c.Quantity != null && (c.Quantity??0).ToString().ToLower().Contains(quantityFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(unitPriceFilter))
            {
                q = q.Where(c => c.UnitPrice != null && c.UnitPrice.ToLower().Contains(unitPriceFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(totalAmountFilter))
            {
                q = q.Where(c => c.TotalAmount != null && c.TotalAmount.ToLower().Contains(totalAmountFilter.ToLower()));
            }

            switch (gridSettings.sortColumn)
            {
                case "CodProduct":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.CodProduct) : q.OrderBy(c => c.CodProduct);
                    break;
                case "ProductName":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.ProductName) : q.OrderBy(c => c.ProductName);
                    break;
                case "Quantity":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.Quantity) : q.OrderBy(c => c.Quantity);
                    break;
                case "UnitPrice":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.UnitPrice) : q.OrderBy(c => c.UnitPrice);
                    break;
                case "TotalAmount":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.TotalAmount) : q.OrderBy(c => c.TotalAmount);
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
                                a.CodDocumentProduct,
                                a.CodProduct,
                                a.ProductName,  //attributo derivato
                                (a.Quantity??0).ToString(),
                                a.UnitPrice,
                                a.TotalAmount
                            }
                    }
                ).ToArray()
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DocumentProductQuantitiesList(string CodDocument, string CodProduct, GridSettings gridSettings)
        {
            var q = documentRepository.GetSingle(CodDocument).DocumentProducts.Where(x => x.CodProduct == CodProduct);

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
                        id = a.CodDocumentProduct,
                        cell = new string[] 
                        {                       
                            a.CodDocumentProduct,
                            a.CodProduct,      
                            a.Quantity.ToString(),
                            a.UnitPrice,
                            a.TotalAmount
                        }
                    }
                ).Distinct().ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// AutoComplete
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult NewProductAutoComplete(string term)
        {
            MenuProduct[] menuProd = menu.GetAll().ToArray();

            string strings = "~/Views/Shared/Strings";

            foreach (var item in menuProd)
            {
                var name = (string)HttpContext.GetLocalResourceObject(strings, "CodMenuProduct" + item.CodMenuProduct);
                item.Name = name;

                if (name == "")
                {
                    Console.WriteLine();
                }
            }

            var filteredItems = menuProd.Where(
            item => item.Name.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var projection = from art in filteredItems
                             select new
                             {
                                 id = art.Name,
                                 label = art.Name,
                                 value = art.Name
                             };
            return Json(projection.Distinct().ToList(), JsonRequestBehavior.AllowGet);
        }

    }
}
