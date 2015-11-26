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



        /// <summary>
        /// List of all DocumentProducts
        /// </summary>
        /// <param name="gridSettings"></param>
        /// <returns></returns>
        public ActionResult ProductsList(GridSettings gridSettings)
        {
            string codProductFilter = string.Empty;
            string productRefNameFilter = string.Empty;
            string productNameFilter = string.Empty;

            if (gridSettings.isSearch)
            {
                codProductFilter = gridSettings.where.rules.Any(r => r.field == "CodProduct") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "CodProduct").data : string.Empty;

                productNameFilter = gridSettings.where.rules.Any(r => r.field == "ProductName") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "ProductName").data : string.Empty;

                productRefNameFilter = gridSettings.where.rules.Any(r => r.field == "ProductRefName") ?
                   gridSettings.where.rules.FirstOrDefault(r => r.field == "ProductRefName").data : string.Empty;

            }

            var q = documentRepository.GetAllProducts().OrderByDescending(x => x.TimeStampTable).AsQueryable();

            if (!string.IsNullOrEmpty(codProductFilter))
            {
                q = q.Where(c => c.CodProduct != null && c.CodProduct.ToLower().Contains(codProductFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(productNameFilter))
            {
                q = q.Where(c => c.ProductName != null && c.ProductName.ToLower().Contains(productNameFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(productRefNameFilter))
            {
                q = q.Where(c => (c.ProductRefName != null && c.ProductRefName.ToLower().Contains(productRefNameFilter.ToLower())) ||
                    (c.ProductRefName != null && c.ProductRefName.ToLower().Contains(productRefNameFilter.ToLower())));
            }



            switch (gridSettings.sortColumn)
            {
                case "CodProduct":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.CodProduct) : q.OrderBy(c => c.CodProduct);
                    break;
                case "ProductName":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.ProductName) : q.OrderBy(c => c.ProductName);
                    break;
                case "ProductRefName":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.ProductRefName) : q.OrderBy(c => c.ProductRefName);
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
                                a.ProductName,
                                a.ProductRefName  //attributo derivato
                            }
                    }
                ).ToArray()
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }


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
        

        public ActionResult ProductPartPrintRollOptionList(GridSettings gridSettings, string codProductPartTask)
        {


//            var q = productRepository.GetProductPartTaskOptions(id).OfType<ProductPartPrintRollOption>().OrderBy(x=>x.CodProductPartTaskOption);

            var q2 = productRepository.GetProductPartTaskOptions(codProductPartTask).OrderBy(x => x.CodProductPartTaskOption);

            var q = q2.OfType<ProductPartPrintRollOption>();
            
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
                        id = a.CodProductPartTaskOption,
                        cell = new string[] 
                        {                       
                            a.CodProductPartTaskOption,
                            a.TypeOfTaskPrint,
                            a.Ink,
                            a.Overlay.ToString()
                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);


        }

        public ActionResult ProductPartHotPrintingOptionList(GridSettings gridSettings, string codProductPartTask)
        {

            var q2 = productRepository.GetProductPartTaskOptions(codProductPartTask).OrderBy(x => x.CodProductPartTaskOption);
            var q = q2.OfType<ProductPartHotPrintingOption>();
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
                        id = a.CodProductPartTaskOption,
                        cell = new string[] 
                        {                       
                            a.CodProductPartTaskOption,
                            a.Foil,
                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);


        }

        public ActionResult ProductPartSerigraphyOptionList(GridSettings gridSettings, string codProductPartTask)
        {

            var q2 = productRepository.GetProductPartTaskOptions(codProductPartTask).OrderBy(x => x.CodProductPartTaskOption);
            var q = q2.OfType<ProductPartSerigraphyOption>();
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
                        id = a.CodProductPartTaskOption,
                        cell = new string[] 
                        {                       
                            a.CodProductPartTaskOption,
                            a.TypeOfTaskSerigraphy,
                            a.InkSerigraphy,
                            a.Overlay.ToString()
                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }


        private IQueryable<Document> DocumentList(GridSettings gridSettings)
        {
            string customerFilter = string.Empty;
            string serialFilter = string.Empty;
            string codDocumentFilter = string.Empty;
            string documentNameFilter = string.Empty;
            string dateDocumentFilter = string.Empty;
            

            string categoryFilter = string.Empty;

            if (gridSettings.isSearch)
            {

                categoryFilter = gridSettings.where.rules.Any(r => r.field == "Category") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "Category").data : string.Empty;


                customerFilter = gridSettings.where.rules.Any(r => r.field == "Customer") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "Customer").data : string.Empty;

                serialFilter = gridSettings.where.rules.Any(r => r.field == "EstimateNumberSerie") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "EstimateNumberSerie").data : string.Empty;

                codDocumentFilter = gridSettings.where.rules.Any(r => r.field == "CodDocument") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "CodDocument").data : string.Empty;

                dateDocumentFilter = gridSettings.where.rules.Any(r => r.field == "DateDocument") ?
                   gridSettings.where.rules.FirstOrDefault(r => r.field == "DateDocument").data : string.Empty;

                documentNameFilter = gridSettings.where.rules.Any(r => r.field == "DocumentName") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "DocumentName").data : string.Empty;

               
            }

            var q = documentRepository.GetAll();

            if (!string.IsNullOrEmpty(categoryFilter))
            {
                q = q.Where(c => c.DocumentStates.Where(y=>y.StateName == categoryFilter && y.Selected).Count() > 0);
            }


            if (!string.IsNullOrEmpty(customerFilter))
            {
                q = q.Where(c => c.Customer != null && c.Customer.ToLower().Contains(customerFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(serialFilter))
            {
                q = q.Where(c => (c.EstimateNumberSerie != null && c.EstimateNumberSerie.ToLower().Contains(serialFilter.ToLower())) ||
                    (c.EstimateNumber != null && c.EstimateNumber.ToLower().Contains(serialFilter.ToLower())) ||
                    (c.CodDocument != null && c.EstimateNumber != null && (c.EstimateNumberSerie + "/" + c.EstimateNumber).ToLower().Contains(serialFilter.ToLower())));
            }

            if (!string.IsNullOrEmpty(codDocumentFilter))
            {
                q = q.Where(c => c.CodDocument.ToLower().Contains(codDocumentFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(dateDocumentFilter))
            {
                try
                {
                    DateTime dt = Convert.ToDateTime(dateDocumentFilter);
                    q = q.Where(c => (c.DateDocument ?? DateTime.Now).Year == dt.Year && (c.DateDocument ?? DateTime.Now).Month == dt.Month && (c.DateDocument ?? DateTime.Now).Day == dt.Day);

                }
                catch (Exception)
                {

                }
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

                case "DateDocument":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.DateDocument) : q.OrderBy(c => c.DateDocument);
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
                            a.EstimateNumberSerie + "/" + a.EstimateNumber + "%" + a.CodDocument,
                            (a.DateDocument??DateTime.Now).ToString("d"),
                            a.DocumentName,
                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);


        }

        public ActionResult OrderList(GridSettings gridSettings)
        {

            var q = this.DocumentList(gridSettings).OfType<Order>();
            var q3 = q.Skip((gridSettings.pageIndex - 1) * gridSettings.pageSize).Take(gridSettings.pageSize);

            int totalRecords = q.Count();
            string orderNumberFilter = string.Empty;

            if (gridSettings.isSearch)
            {
                orderNumberFilter = gridSettings.where.rules.Any(r => r.field == "OrderNumberSerie") ?
                      gridSettings.where.rules.FirstOrDefault(r => r.field == "OrderNumberSerie").data : string.Empty;

            }

            if (!string.IsNullOrEmpty(orderNumberFilter))
            {
                q = q.Where(c => c.DocumentName != null && c.DocumentName.ToLower().Contains(orderNumberFilter.ToLower()));
            }
            switch (gridSettings.sortColumn)
            {
            case "OrderNumberSerie":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.OrderNumberSerie) : q.OrderBy(c => c.OrderNumberSerie);
                    break;
                default:
                    q = q.OrderByDescending(c => c.CodDocument);
                    break;
            }
           
        

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
                            a.OrderNumberSerie + "/" + a.OrderNumber,
                            a.Customer,
                            a.OrderProduct.Document.EstimateNumberSerie + "/" + a.OrderProduct.Document.EstimateNumber + "%" + a.OrderProduct.CodDocument,
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
        public ActionResult DocumentProductListInDocument(string CodDocument, GridSettings gridSettings)
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
                                a.CodProduct,
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
            string serialFilter = string.Empty;
            string documentAndProductRefNameFilter = string.Empty;
            string productNameFilter = string.Empty;
            string quantityFilter = string.Empty;
            string unitPriceFilter = string.Empty;
            string totalAmountFilter = string.Empty;
            string markupFilter = string.Empty;
                

            if (gridSettings.isSearch)
            {
                codProductFilter = gridSettings.where.rules.Any(r => r.field == "CodProduct") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "CodProduct").data : string.Empty;

                serialFilter = gridSettings.where.rules.Any(r => r.field == "EstimateNumberSerie") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "EstimateNumberSerie").data : string.Empty;

                productNameFilter = gridSettings.where.rules.Any(r => r.field == "ProductName") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "ProductName").data : string.Empty;

                documentAndProductRefNameFilter = gridSettings.where.rules.Any(r => r.field == "DocumentAndProductRefName") ?
                   gridSettings.where.rules.FirstOrDefault(r => r.field == "DocumentAndProductRefName").data : string.Empty;

                quantityFilter = gridSettings.where.rules.Any(r => r.field == "Quantity") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "Quantity").data : string.Empty;

                unitPriceFilter = gridSettings.where.rules.Any(r => r.field == "UnitPrice") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "UnitPrice").data : string.Empty;

                totalAmountFilter = gridSettings.where.rules.Any(r => r.field == "TotalAmount") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "TotalAmount").data : string.Empty;

                markupFilter = gridSettings.where.rules.Any(r => r.field == "Markup") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "Markup").data : string.Empty;
            }

            var q = documentRepository.GetAllDocumentProducts().OrderByDescending(x => x.TimeStampTable).AsQueryable();

            if (!string.IsNullOrEmpty(codProductFilter))
            {
                q = q.Where(c => c.CodProduct != null && c.CodProduct.ToLower().Contains(codProductFilter.ToLower()));
            }

             if (!string.IsNullOrEmpty(serialFilter))
            {
                q = q.Where(c => (c.Document.EstimateNumberSerie != null && c.Document.EstimateNumberSerie.ToLower().Contains(serialFilter.ToLower())) ||
                    (c.Document.EstimateNumber != null && c.Document.EstimateNumber.ToLower().Contains(serialFilter.ToLower())) ||
                    (c.Document.CodDocument != null && c.Document.EstimateNumber != null && (c.Document.EstimateNumberSerie + "/" + c.Document.EstimateNumber).ToLower().Contains(serialFilter.ToLower())));
            }

            if (!string.IsNullOrEmpty(productNameFilter))
            {
                q = q.Where(c => c.ProductName != null && c.ProductName.ToLower().Contains(productNameFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(documentAndProductRefNameFilter))
            {
                q = q.Where(c => (c.Document.DocumentName != null && c.Document.DocumentName.ToLower().Contains(documentAndProductRefNameFilter.ToLower())) || 
                    (c.Product.ProductRefName != null && c.Product.ProductRefName.ToLower().Contains(documentAndProductRefNameFilter.ToLower())));
            }

            if (!string.IsNullOrEmpty(quantityFilter))
            {
                q = q.Where(c => c.Quantity != null && (c.Quantity ?? 0).ToString().ToLower().Contains(quantityFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(unitPriceFilter))
            {
                q = q.Where(c => c.UnitPrice != null && c.UnitPrice.ToLower().Contains(unitPriceFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(totalAmountFilter))
            {
                q = q.Where(c => c.TotalAmount != null && c.TotalAmount.ToLower().Contains(totalAmountFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(markupFilter))
            {
                q = q.Where(c => c.Markup != null && (c.Markup??0).ToString().ToLower().Contains(markupFilter.ToLower()));
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
                case "Markup":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.Markup) : q.OrderBy(c => c.Markup);
                    break;
            }

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
                                a.CodDocumentProduct,
                                a.CodProduct,
                                a.Document.EstimateNumberSerie + "/" + a.Document.EstimateNumber + "%" + a.CodDocument,
                                a.Document.DocumentName + " - " + a.Product.ProductRefName,
                                a.ProductName,  //attributo derivato
                                (a.Quantity??0).ToString(),
                                (a.Markup??0).ToString(),
                                a.UnitPrice??"0",
                                a.TotalAmount??"0"
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
                            (a.Markup??0).ToString(),
                            a.UnitPrice??"0",
                            a.TotalAmount??"0"
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
