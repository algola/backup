using System;
using System.Linq;
using System.Web.Mvc;
using PapiroMVC.Models;
using Mvc.HtmlHelpers;
using PapiroMVC.Validation;

namespace PapiroMVC.Areas.Working.Controllers
{
    public partial class DocumentController : PapiroMVC.Controllers.ControllerAlgolaBase
    {
        public ActionResult Costs(GridSettings gridSettings, String codDocumentProduct)
        {
            var q = documentRepository.GetCostsByCodDocumentProduct(codDocumentProduct).Where(x=>!(x.Hidden??false));

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
                            a.Description,
                            a.UnitCost,
                            a.Quantity.ToString(),
                            a.TotalCost
                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        private IQueryable<Document> DocumentList(GridSettings gridSettings)
        {
            string codDocumentFilter = string.Empty;
            string documentNameFilter = string.Empty;

            if (gridSettings.isSearch)
            {
                codDocumentFilter = gridSettings.where.rules.Any(r => r.field == "CodDocument") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "CodDocument").data : string.Empty;

                documentNameFilter = gridSettings.where.rules.Any(r => r.field == "DocumentName") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "DocumentName").data : string.Empty;

            }

            var q = documentRepository.GetAll();

            if (!string.IsNullOrEmpty(codDocumentFilter))
            {
                q = q.Where(c => c.CodDocument.ToLower().Contains(codDocumentFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(documentNameFilter))
            {
                q = q.Where(c => c.DocumentName.ToLower().Contains(documentNameFilter.ToLower()));
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
                            a.EstimateNumber.ToString(),
                            a.DocumentName,
                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);


        }

        public ActionResult DocumentProductsList(string CodDocument, GridSettings gridSettings)
        {

            if (CodDocument != null)
            {
                var q = documentRepository.GetSingle(CodDocument).DocumentProducts.Select((p) =>
                            new { CodProduct = p.CodProduct, ProductName = p.ProductName }).Distinct();

                //var q2 = q.ToList();
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
                            id = a.CodProduct,
                            cell = new string[] 
                            {                       
                                a.CodProduct,
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

    }
}
