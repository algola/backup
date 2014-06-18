using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PapiroMVC.Models;
using Services;
using Ninject.Planning.Bindings;
using System.Web.Security;
using Mvc.HtmlHelpers;
using PapiroMVC.Validation;
using System.Threading;

namespace PapiroMVC.Areas.DataBase.Controllers
{
    public partial class ArticleController : PapiroMVC.Controllers.ControllerAlgolaBase
    {
        //sperimentale
        public ActionResult DataProcessedCorrectly()
        {
            return PartialView("_DataProcessedCorrecly");
        }

        /// <summary>
        /// RollAutomaticallyChangesValidation is used to validate parameter and its responses are used in a ajax beginform
        /// results depend on HttpContext.Response.StatusCode = 500
        /// 500 shows back that there is an error so ajax beginform runs OnFaliure code (reload form with validation message)
        /// else ajax beginform runs OnSuccess code (run javacript that collects data and pass to RollAutomaticallyChanges)       
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult RollAutomaticallyChangesValidation(RollPrintableArticleAutoChanges x)
        {
            if (ModelState.IsValid)
            {
                return PartialView("_RollPrintableArticleAutoChanges");
            }

            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    Console.WriteLine(error);
                }
            }

            HttpContext.Response.StatusCode = 500;
            HttpContext.Response.Clear();
            return PartialView("_RollPrintableArticleAutoChanges", x);
        }

        //this method works on data from ajax post in _ListRollPrintableArticle 
        //data contains ids and values of parameters
        [HttpPost]
        public ActionResult RollAutomaticallyChanges(RollPrintableArticleAutoChanges x)
        {
            //Console.WriteLine(HttpContext.Request.UrlReferrer.OriginalString);
            ////model contsins data that will be processed
            try
            {
                if (x.SupplierMaker == "error")
                    throw new Exception();
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                HttpContext.Response.StatusCode = 500;
                HttpContext.Response.Clear();
                return PartialView("_RollPrintableArticleAutoChanges", x);
            }

            foreach (var id in x.Id)
            {
                var a=articleRepository.GetSingle(id);
                var cost = a.ArticleCosts.FirstOrDefault();

                ((RollPrintableArticleCost)cost).CostPerMq = x.CostPerMq;

                articleRepository.Edit(a);
                articleRepository.Save();
            }

            return Json(new
            {
                
                                message = "ok"
            });
        }


        /// <summary>
        /// SheetAutomaticallyChangesValidation is used to validate parameter and its responses are used in a ajax beginform
        /// results depend on HttpContext.Response.StatusCode = 500
        /// 500 shows back that there is an error so ajax beginform runs OnFaliure code (reload form with validation message)
        /// else ajax beginform runs OnSuccess code (run javacript that collects data and pass to SheetAutomaticallyChanges)       
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult SheetAutomaticallyChangesValidation(SheetPrintableArticleAutoChanges x)
        {
            if (ModelState.IsValid)
            {
                return PartialView("_SheetPrintableArticleAutoChanges");
            }

            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    Console.WriteLine(error);
                }
            }

            HttpContext.Response.StatusCode = 500;
            HttpContext.Response.Clear();
            return PartialView("_SheetPrintableArticleAutoChanges", x);
        }

        //this method works on data from ajax post in _ListSheetPrintableArticle 
        //data contains ids and values of parameters
        [HttpPost]
        public ActionResult SheetAutomaticallyChanges(SheetPrintableArticleAutoChanges x)
        {
            //Console.WriteLine(HttpContext.Request.UrlReferrer.OriginalString);
            ////model contsins data that will be processed
            try
            {
                if (x.SupplierMaker == "error")
                    throw new Exception();
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                HttpContext.Response.StatusCode = 500;
                HttpContext.Response.Clear();
                return PartialView("_SheetPrintableArticleAutoChanges", x);
            }

            return Json(new
            {
                message = "ok"
            });
        }

        public ActionResult ArticleListOld(GridSettings gridSettings)
        {
            string codArticleFilter = string.Empty;
            string articleNameFilter = string.Empty;
            string supplierNameFilter = string.Empty;
            string typeOfArticleFilter = string.Empty;

            //read from validation's language file
            var resman = new System.Resources.ResourceManager(typeof(Strings).FullName, typeof(Strings).Assembly);
            string rollType = resman.GetString("RollPrintableArticleType");
            string sheetType = resman.GetString("SheetPrintableArticleType");
            string objectType = resman.GetString("ObjectPrintableArticleType");
            string rigidType = resman.GetString("RigidPrintableArticleType");

            if (gridSettings.isSearch)
            {
                codArticleFilter = gridSettings.where.rules.Any(r => r.field == "CodArticle") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "CodArticle").data : string.Empty;

                articleNameFilter = gridSettings.where.rules.Any(r => r.field == "ArticleName") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "ArticleName").data : string.Empty;

                supplierNameFilter = gridSettings.where.rules.Any(r => r.field == "SupplierName") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "SupplierName").data : string.Empty;

                typeOfArticleFilter = gridSettings.where.rules.Any(r => r.field == "TypeOfArticle") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "TypeOfArticle").data : string.Empty;
            }

            var q = articleRepository.GetAll();

            if (!string.IsNullOrEmpty(codArticleFilter))
            {
                q = q.Where(c => c.CodArticle.ToLower().Contains(codArticleFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(articleNameFilter))
            {
                q = q.Where(c => c.ArticleName.ToLower().Contains(articleNameFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(supplierNameFilter))
            {
                q = q.Where(c => c.CustomerSupplierMaker.BusinessName.ToLower().Contains(supplierNameFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(typeOfArticleFilter))
            {
                Boolean isRoll = false, isSheet = false, isObject = false, isRigid = false;

                //to match with language we have to compare filter with resource
                isRoll = (rollType.ToLower().Contains(typeOfArticleFilter.ToLower()));
                isSheet = (sheetType.ToLower().Contains(typeOfArticleFilter.ToLower()));
                isObject = (objectType.ToLower().Contains(typeOfArticleFilter.ToLower()));
                isRigid = (rigidType.ToLower().Contains(typeOfArticleFilter.ToLower()));

                var a = isRoll ? (IQueryable<Article>)q.OfType<RollPrintableArticle>() : articleRepository.FindBy(x => x.CodArticle == "");
                var b = isSheet ? (IQueryable<Article>)q.OfType<SheetPrintableArticle>() : articleRepository.FindBy(x => x.CodArticle == "");
                var c = isObject ? (IQueryable<Article>)q.OfType<ObjectPrintableArticle>() : articleRepository.FindBy(x => x.CodArticle == "");
                var d = isRigid ? (IQueryable<Article>)q.OfType<RigidPrintableArticle>() : articleRepository.FindBy(x => x.CodArticle == "");

                var res = (a.Union(b).Union(c).Union(d));
                q = (IQueryable<Article>)res;
            }

            switch (gridSettings.sortColumn)
            {
                case "CodArticle":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.CodArticle) : q.OrderBy(c => c.CodArticle);
                    break;
                case "ArticleName":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.ArticleName) : q.OrderBy(c => c.ArticleName);
                    break;
                case "SupplierName":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.CustomerSupplierMaker.BusinessName) : q.OrderBy(c => c.CustomerSupplierMaker.BusinessName);
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
                        id = a.CodArticle,
                        cell = new string[] 
                        {                       
                            a.CodArticle,
                            a.CodArticle,
                            a.TypeOfArticle.ToString(),
                            a.ArticleName,
                            a.CustomerSupplierMaker.BusinessName
                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }


        private IQueryable<Printable> PrintableList(GridSettings gridSettings)
        {
            string codArticleFilter = string.Empty;
            string articleNameFilter = string.Empty;
            string typeOfMaterialFilter = string.Empty;
            string nameOfMaterialFilter = string.Empty;
            string colorFilter = string.Empty;
            string adhesiveFilter = string.Empty;

            string supplierNameFilter = string.Empty;
            string typeOfArticleFilter = string.Empty;

            string weightArticleFilter = string.Empty;

            //read from validation's language file
            var resman = new System.Resources.ResourceManager(typeof(Strings).FullName, typeof(Strings).Assembly);
            string rollType = resman.GetString("RollPrintableArticleType");
            string sheetType = resman.GetString("SheetPrintableArticleType");
            string objectType = resman.GetString("ObjectPrintableArticleType");
            string rigidType = resman.GetString("RigidPrintableArticleType");

            if (gridSettings.isSearch)
            {
                codArticleFilter = gridSettings.where.rules.Any(r => r.field == "CodArticle") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "CodArticle").data : string.Empty;

                articleNameFilter = gridSettings.where.rules.Any(r => r.field == "ArticleName") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "ArticleName").data : string.Empty;

                supplierNameFilter = gridSettings.where.rules.Any(r => r.field == "SupplierName") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "SupplierName").data : string.Empty;

                typeOfArticleFilter = gridSettings.where.rules.Any(r => r.field == "TypeOfArticle") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "TypeOfArticle").data : string.Empty;

                typeOfMaterialFilter = gridSettings.where.rules.Any(r => r.field == "TypeOfMaterial") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "TypeOfMaterial").data : string.Empty;

                nameOfMaterialFilter = gridSettings.where.rules.Any(r => r.field == "NameOfMaterial") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "NameOfMaterial").data : string.Empty;

                colorFilter = gridSettings.where.rules.Any(r => r.field == "Color") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "Color").data : string.Empty;

                adhesiveFilter = gridSettings.where.rules.Any(r => r.field == "Adhesive") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "Adhesive").data : string.Empty;

                weightArticleFilter = gridSettings.where.rules.Any(r => r.field == "Weight") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "Weight").data : string.Empty;

            }

            var q = articleRepository.GetAll().OfType<Printable>();

            if (!string.IsNullOrEmpty(codArticleFilter))
            {
                q = q.Where(c => c.CodArticle.ToLower().Contains(codArticleFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(articleNameFilter))
            {
                q = q.Where(c => c.ArticleName.ToLower().Contains(articleNameFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(supplierNameFilter))
            {
                q = q.Where(c => c.CustomerSupplierMaker.BusinessName.ToLower().Contains(supplierNameFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(nameOfMaterialFilter))
            {
                q = q.Where(c => c.NameOfMaterial.ToLower().Contains(nameOfMaterialFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(typeOfMaterialFilter))
            {
                q = q.Where(c => c.TypeOfMaterial.ToLower().Contains(typeOfMaterialFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(colorFilter))
            {
                q = q.Where(c => c.Color.ToLower().Contains(colorFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(adhesiveFilter))
            {
                q = q.Where(c => c.Adhesive.ToLower().Contains(adhesiveFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(typeOfArticleFilter))
            {
                Boolean isRoll = false, isSheet = false, isObject = false, isRigid = false;

                //to match with language we have to compare filter with resource
                isRoll = (rollType.ToLower().Contains(typeOfArticleFilter.ToLower()));
                isSheet = (sheetType.ToLower().Contains(typeOfArticleFilter.ToLower()));
                isObject = (objectType.ToLower().Contains(typeOfArticleFilter.ToLower()));
                isRigid = (rigidType.ToLower().Contains(typeOfArticleFilter.ToLower()));

                var a = isRoll ? (IQueryable<Article>)q.OfType<RollPrintableArticle>() : articleRepository.FindBy(x => x.CodArticle == "");
                var b = isSheet ? (IQueryable<Article>)q.OfType<SheetPrintableArticle>() : articleRepository.FindBy(x => x.CodArticle == "");
                var c = isObject ? (IQueryable<Article>)q.OfType<ObjectPrintableArticle>() : articleRepository.FindBy(x => x.CodArticle == "");
                var d = isRigid ? (IQueryable<Article>)q.OfType<RigidPrintableArticle>() : articleRepository.FindBy(x => x.CodArticle == "");

                var res = (a.Union(b).Union(c).Union(d));
                q = (IQueryable<Printable>)res;
            }

            if (!string.IsNullOrEmpty(weightArticleFilter))
            {
                try
                {
                    var weightDouble = Convert.ToDouble(weightArticleFilter, Thread.CurrentThread.CurrentUICulture);
                    q = q.Where(c => c.Weight == weightDouble);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            switch (gridSettings.sortColumn)
            {
                case "CodArticle":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.CodArticle) : q.OrderBy(c => c.CodArticle);
                    break;
                case "ArticleName":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.ArticleName) : q.OrderBy(c => c.ArticleName);
                    break;
                case "SupplierName":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.CustomerSupplierMaker.BusinessName) : q.OrderBy(c => c.CustomerSupplierMaker.BusinessName);
                    break;
                case "Weight":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.Weight) : q.OrderBy(c => c.Weight);
                    break;
                default:
                    q = q.OrderBy(c => c.ArticleName);
                    break;
            }

            return q;
        }

        public ActionResult NoPrintableList(GridSettings gridSettings)
        {
            string codArticleFilter = string.Empty;
            string articleNameFilter = string.Empty;

            string supplierNameFilter = string.Empty;

            if (gridSettings.isSearch)
            {
                codArticleFilter = gridSettings.where.rules.Any(r => r.field == "CodArticle") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "CodArticle").data : string.Empty;

                articleNameFilter = gridSettings.where.rules.Any(r => r.field == "ArticleName") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "ArticleName").data : string.Empty;

                supplierNameFilter = gridSettings.where.rules.Any(r => r.field == "SupplierName") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "SupplierName").data : string.Empty;

            }

            var q = articleRepository.GetAll().OfType<NoPrintable>();

            if (!string.IsNullOrEmpty(codArticleFilter))
            {
                q = q.Where(c => c.CodArticle.ToLower().Contains(codArticleFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(articleNameFilter))
            {
                q = q.Where(c => c.ArticleName.ToLower().Contains(articleNameFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(supplierNameFilter))
            {
                q = q.Where(c => c.CustomerSupplierMaker.BusinessName.ToLower().Contains(supplierNameFilter.ToLower()));
            }


            switch (gridSettings.sortColumn)
            {
                case "CodArticle":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.CodArticle) : q.OrderBy(c => c.CodArticle);
                    break;
                case "ArticleName":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.ArticleName) : q.OrderBy(c => c.ArticleName);
                    break;
                case "SupplierName":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.CustomerSupplierMaker.BusinessName) : q.OrderBy(c => c.CustomerSupplierMaker.BusinessName);
                    break;
                default:
                    q = q.OrderBy(c => c.ArticleName);
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
                        id = a.CodArticle,
                        cell = new string[] 
                        {                       
                            a.CodArticle,
                            a.CodArticle,
                            a.ArticleName,
                            a.CustomerSupplierMaker.BusinessName,
                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        public ActionResult SheetPrintableArticleList(GridSettings gridSettings)
        {
            IQueryable<SheetPrintableArticle> q;

            //common serarch and order
            q = PrintableList(gridSettings).OfType<SheetPrintableArticle>();

            string formatArticleFilter = string.Empty;
            string sheetPerPackedFilter = string.Empty;
            string sheetPerPalletFilter = string.Empty;
            string colorArticleFilter = string.Empty;
            string adhesiveArticleFilter = string.Empty;

            if (gridSettings.isSearch)
            {
                formatArticleFilter = gridSettings.where.rules.Any(r => r.field == "Format") ?
                        gridSettings.where.rules.FirstOrDefault(r => r.field == "Format").data : string.Empty;

                sheetPerPackedFilter = gridSettings.where.rules.Any(r => r.field == "SheetPerPacked") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "SheetPerPacked").data : string.Empty;

                sheetPerPalletFilter = gridSettings.where.rules.Any(r => r.field == "SheetPerPallet") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "SheetPerPallet").data : string.Empty;
            }

            if (!string.IsNullOrEmpty(formatArticleFilter))
            {
                q = q.Where(c => c.Format.ToLower().Contains(formatArticleFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(sheetPerPackedFilter))
            {
                try
                {
                    var sheetPerPackedDouble = Convert.ToDouble(sheetPerPackedFilter, Thread.CurrentThread.CurrentUICulture);
                    q = q.Where(c => c.SheetPerPacked == sheetPerPackedDouble);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            if (!string.IsNullOrEmpty(sheetPerPalletFilter))
            {
                try
                {
                    var sheetPerPalletDouble = Convert.ToDouble(sheetPerPalletFilter, Thread.CurrentThread.CurrentUICulture);
                    q = q.Where(c => c.SheetPerPallet == sheetPerPalletDouble);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }


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
                        id = a.CodArticle,
                        cell = new string[] 
                        {                       
                            a.CodArticle,
                            a.CodArticle,
                            a.TypeOfMaterial,
                            a.NameOfMaterial,
                            a.Color,
                            a.Adhesive,
                            a.Weight.ToString(),
                            a.Format,
                            (a.NoUseInEstimateCalculation??false).ToString(),
                            a.SheetPerPacked.ToString(),
                            a.SheetPerPallet.ToString(),
                            a.CustomerSupplierMaker.BusinessName,
                            ((SheetPrintableArticlePakedCost)a.ArticleCosts.First(x => 
                                x.TypeOfArticleCost == ArticleCost.ArticleCostType.SheetPrintableArticlePakedCost)).CostPerKg,
                            ((SheetPrintableArticlePakedCost)a.ArticleCosts.First(x => 
                                x.TypeOfArticleCost == ArticleCost.ArticleCostType.SheetPrintableArticlePakedCost)).CostPerSheet,            
                        }
                    }
                ).ToArray()
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);


        }


        public ActionResult RollPrintableArticleListPerProduct(GridSettings gridSettings)
        {
            var q = PrintableList(gridSettings).OfType<RollPrintableArticle>().Select(p => new
            {
                TypeOfMaterial = p.TypeOfMaterial,
                NameOfMaterial = p.NameOfMaterial,
                Color = p.Color,
                Adhesive = p.Adhesive,
                NoUseInEstimateCalculation = p.NoUseInEstimateCalculation,
                Weight = p.Weight,
            }).Where(x => x.NoUseInEstimateCalculation == false || x.NoUseInEstimateCalculation == null).Distinct();

            q = q.OrderBy(c => c.TypeOfMaterial);

            var q3 = q.Skip((gridSettings.pageIndex - 1) * gridSettings.pageSize).Take(gridSettings.pageSize);

            int totalRecords = q.ToList().Count();

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
                        a.TypeOfMaterial,
                        a.NameOfMaterial,
                        a.Color,
                        a.Adhesive,
                        a.Weight,
                    }
                ).ToArray()
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        public ActionResult SheetPrintableArticleListPerProduct(GridSettings gridSettings)
        {
            var q = PrintableList(gridSettings).OfType<SheetPrintableArticle>().Select(p => new
            {
                TypeOfMaterial = p.TypeOfMaterial,
                NameOfMaterial = p.NameOfMaterial,
                Color = p.Color,
                Adhesive = p.Adhesive,
                NoUseInEstimateCalculation = p.NoUseInEstimateCalculation,
                Weight = p.Weight,
            }).Where(x => x.NoUseInEstimateCalculation != true).Distinct();

            q = q.OrderBy(c => c.TypeOfMaterial);

            var q3 = q.Skip((gridSettings.pageIndex - 1) * gridSettings.pageSize).Take(gridSettings.pageSize);

            int totalRecords = q.ToList().Count();

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
                        a.TypeOfMaterial,
                        a.NameOfMaterial,
                        a.Color,
                        a.Adhesive,
                        a.Weight,
                    }
                ).ToArray()
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }


        public ActionResult RollPrintableArticleList(GridSettings gridSettings)
        {
            //common serarch and order
            var q = PrintableList(gridSettings).OfType<RollPrintableArticle>();

            string widthArticleFilter = string.Empty;

            if (gridSettings.isSearch)
            {
                widthArticleFilter = gridSettings.where.rules.Any(r => r.field == "Width") ?
                        gridSettings.where.rules.FirstOrDefault(r => r.field == "Width").data : string.Empty;

            }

            if (!string.IsNullOrEmpty(widthArticleFilter))
            {
                try
                {
                    var widthtDouble = Convert.ToDouble(widthArticleFilter, Thread.CurrentThread.CurrentUICulture);
                    q = q.Where(c => c.Width == widthtDouble);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
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
                        id = a.CodArticle,
                        cell = new string[] 
                        {                       
                            a.CodArticle,
                            a.CodArticle,
                            a.TypeOfMaterial,
                            a.NameOfMaterial,
                            a.Color,
                            a.Adhesive,
                            a.Weight.ToString(),
                            a.Width.ToString(),
                            (a.NoUseInEstimateCalculation??false).ToString(),
                            a.CustomerSupplierMaker.BusinessName,
                            ((RollPrintableArticleStandardCost)a.ArticleCosts.First(x => 
                                x.TypeOfArticleCost == ArticleCost.ArticleCostType.RollPrintableArticleStandardCost)).CostPerMq,
                            ((RollPrintableArticleStandardCost)a.ArticleCosts.First(x => 
                                x.TypeOfArticleCost == ArticleCost.ArticleCostType.RollPrintableArticleStandardCost)).CostPerMl,            
                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        public ActionResult RigidPrintableArticleList(GridSettings gridSettings)
        {
            //common serarch and order
            var q = PrintableList(gridSettings).OfType<RigidPrintableArticle>();

            string formatArticleFilter = string.Empty;

            if (gridSettings.isSearch)
            {
                formatArticleFilter = gridSettings.where.rules.Any(r => r.field == "Format") ?
                        gridSettings.where.rules.FirstOrDefault(r => r.field == "Format").data : string.Empty;

            }

            if (!string.IsNullOrEmpty(formatArticleFilter))
            {
                q = q.Where(c => c.Format.ToLower().Contains(formatArticleFilter.ToLower()));
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
                        id = a.CodArticle,
                        cell = new string[] 
                        {                       
                            a.CodArticle,
                          //  a.CodArticle,
                            a.TypeOfMaterial,
                            a.NameOfMaterial,
                            a.Color,
                            a.Weight.ToString(),
                            a.Format.ToString(),
                            a.CustomerSupplierMaker.BusinessName,
                            ((RigidPrintableArticleStandardCost)a.ArticleCosts.First(x => 
                                x.TypeOfArticleCost == ArticleCost.ArticleCostType.RigidPrintableArticleStandardCost)).CostPerMq,
                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

    }
}
