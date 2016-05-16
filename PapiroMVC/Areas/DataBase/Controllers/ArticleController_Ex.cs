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
using System.Text.RegularExpressions;
using Newtonsoft.Json;

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
                var a = articleRepository.GetSingle(id);
                var cost = a.ArticleCosts.FirstOrDefault();

                ((RollPrintableArticleCost)cost).CostPerMq = x.CostPerMq;
                a.Tags = x.Tags != String.Empty ? x.Tags : a.Tags;
                articleRepository.Edit(a);
                articleRepository.Save();
            }

            return Json(new
            {
                message = "ok"
            });
        }

        [HttpPost]
        public ActionResult RigidAutomaticallyChanges(RigidPrintableArticleAutoChanges x)
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
                return PartialView("_RigidPrintableArticleAutoChanges", x);
            }

            foreach (var id in x.Id)
            {
                var a = articleRepository.GetSingle(id);
                var cost = a.ArticleCosts.FirstOrDefault();

                ((RigidPrintableArticleCost)cost).CostPerMq = x.CostPerMq;
                a.Tags = x.Tags != String.Empty ? x.Tags : a.Tags;
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

            foreach (var id in x.Id)
            {
                var a = articleRepository.GetSingle(id);

                if (x.TypeOfCostToModify == SheetPrintableArticleAutoChanges.ProcessCostType.AllCost
                    || x.TypeOfCostToModify == SheetPrintableArticleAutoChanges.ProcessCostType.PackedCost)
                {
                    var cost = a.ArticleCosts.FirstOrDefault(y => y.TypeOfArticleCost == ArticleCost.ArticleCostType.SheetPrintableArticlePakedCost);
                    ((SheetPrintableArticleCost)cost).CostPerKg = x.CostPerKg;
                }

                a.Tags = x.Tags != String.Empty ? x.Tags : a.Tags;
                articleRepository.Edit(a);
                articleRepository.Save();
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


        public ActionResult MeshList(GridSettings gridSettings)
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

            var q = articleRepository.GetAll().OfType<Mesh>();

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

        public ActionResult AniloxList(GridSettings gridSettings)
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

            var q = articleRepository.GetAll().OfType<Anilox>();

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

        public ActionResult InkList(GridSettings gridSettings)
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

            var q = articleRepository.GetAll().OfType<Ink>();

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

        public ActionResult FoilList(GridSettings gridSettings)
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

            var q = articleRepository.GetAll().OfType<Foil>();

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

        public ActionResult DieList(GridSettings gridSettings)
        {
            string codDieFilter = string.Empty;
            string articleDieFilter = string.Empty;
            string descriptionFilter = string.Empty;
            string printingFormatFilter = string.Empty;
            string formatTypeFilter = string.Empty;
            string zFilter = string.Empty;
            string widthFilter = string.Empty;
            string formatFilter = string.Empty;
            string formatBFilter = string.Empty;
            string dCut1Filter = string.Empty;
            string dCut2Filter = string.Empty;
            string maxGain1Filter = string.Empty;
            string maxGain2Filter = string.Empty;

            //format type
            var resman = new System.Resources.ResourceManager(typeof(Strings).FullName, typeof(Strings).Assembly);
            string quad = resman.GetString("FormatTypeName0");
            string ovale = resman.GetString("FormatTypeName1");
            string sago = resman.GetString("FormatTypeName2");
            string rett = resman.GetString("FormatTypeName3");
            string triang = resman.GetString("FormatTypeName4");
            string roto = resman.GetString("FormatTypeName5");

            if (gridSettings.isSearch)
            {
                codDieFilter = gridSettings.where.rules.Any(r => r.field == "CodDie") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "CodDie").data : string.Empty;

                articleDieFilter = gridSettings.where.rules.Any(r => r.field == "ArticleDie") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "ArticleDie").data : string.Empty;

                descriptionFilter = gridSettings.where.rules.Any(r => r.field == "Description") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "Description").data : string.Empty;

                printingFormatFilter = gridSettings.where.rules.Any(r => r.field == "PrintingFormat") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "PrintingFormat").data : string.Empty;

                formatTypeFilter = gridSettings.where.rules.Any(r => r.field == "FormatType") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "FormatType").data : string.Empty;

                zFilter = gridSettings.where.rules.Any(r => r.field == "Z") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "Z").data : string.Empty;

                widthFilter = gridSettings.where.rules.Any(r => r.field == "Width") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "Width").data : string.Empty;

                formatFilter = gridSettings.where.rules.Any(r => r.field == "Format") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "Format").data : string.Empty;

                formatBFilter = gridSettings.where.rules.Any(r => r.field == "FormatB") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "FormatB").data : string.Empty;

                dCut1Filter = gridSettings.where.rules.Any(r => r.field == "DCut1") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "DCut1").data : string.Empty;

                dCut2Filter = gridSettings.where.rules.Any(r => r.field == "DCut2") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "DCut2").data : string.Empty;

                maxGain1Filter = gridSettings.where.rules.Any(r => r.field == "MaxGain1") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "MaxGain1").data : string.Empty;

                maxGain2Filter = gridSettings.where.rules.Any(r => r.field == "MaxGain2") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "MaxGain2").data : string.Empty;
            }

            var q = (IQueryable<Die>)articleRepository.GetAll().OfType<Die>();

            if (!string.IsNullOrEmpty(codDieFilter))
            {
                q = q.Where(c => c.CodDie.ToLower().Contains(codDieFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(articleDieFilter))
            {
                q = q.Where(c => c.ArticleName.ToLower().Contains(articleDieFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(descriptionFilter))
            {
                q = q.Where(c => c.Description.ToLower().Contains(descriptionFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(printingFormatFilter))
            {
                q = q.Where(c => c.PrintingFormat.ToLower().Contains(printingFormatFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(formatTypeFilter))
            {
                Boolean isQuad = false, isOva = false, isSago = false, isRett = false, isTriang = false, isRoto = false;

                //to match with language we have to compare filter with resource
                isQuad = (quad.ToLower().Contains(formatTypeFilter.ToLower()));
                isOva = (ovale.ToLower().Contains(formatTypeFilter.ToLower()));
                isSago = (sago.ToLower().Contains(formatTypeFilter.ToLower()));
                isRett = (rett.ToLower().Contains(formatTypeFilter.ToLower()));
                isTriang = (triang.ToLower().Contains(formatTypeFilter.ToLower()));
                isRoto = (roto.ToLower().Contains(formatTypeFilter.ToLower()));


                var a = isQuad ? q.Where(x => x.FormatType == 0) : q.Where(x => x.FormatType == -1);
                var b = isOva ? q.Where(x => x.FormatType == 1) : q.Where(x => x.FormatType == -1);
                var c = isSago ? q.Where(x => x.FormatType == 2) : q.Where(x => x.FormatType == -1);
                var d = isRett ? q.Where(x => x.FormatType == 3) : q.Where(x => x.FormatType == -1);
                var e = isTriang ? q.Where(x => x.FormatType == 4) : q.Where(x => x.FormatType == -1);
                var f = isRoto ? q.Where(x => x.FormatType == 5) : q.Where(x => x.FormatType == -1);

                var res = (a.Union(b.Union(c.Union(d.Union(e.Union(f))))));
                q = res;
            }

            if (!string.IsNullOrEmpty(zFilter))
            {
                try
                {
                    var Z = Convert.ToDouble(zFilter, Thread.CurrentThread.CurrentUICulture);
                    q = q.Where(c => c.Z == Z);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            if (!string.IsNullOrEmpty(widthFilter))
            {
                try
                {
                    var Width = Convert.ToDouble(widthFilter, Thread.CurrentThread.CurrentUICulture);
                    q = q.Where(c => c.Width == Width);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            if (!string.IsNullOrEmpty(formatFilter))
            {

                //validation of format input --> ok --> search with tollerance
                string strings = "~/Views/Shared/Strings";
                var regex = (string)HttpContext.GetLocalResourceObject(strings, "FormatValidation");
                var match = Regex.Match(formatFilter, regex, RegexOptions.IgnoreCase);

                if (!match.Success)
                {
                    q = q.Where(c => c.Format.ToLower().Contains(formatFilter.ToLower()));
                }
                else
                {
                    //now I can extract side1 and side2 from valid format
                    var side1 = formatFilter.GetSide1();
                    var side2 = formatFilter.GetSide2();

                    var dies = q.ToList();
                    List<Die> toFill = new List<Die>();

                    foreach (var die in dies)
                    {

                        if (Math.Abs(die.Format.GetSide1() - side1) <= 1.5 &&
                            Math.Abs(die.Format.GetSide2() - side2) <= 1.5)
                        {
                            toFill.Add(die);
                        }
                    }

                    q = toFill.AsQueryable<Die>();

                }
            }

            if (!string.IsNullOrEmpty(formatBFilter))
            {

                //validation of format input --> ok --> search with tollerance
                string strings = "~/Views/Shared/Strings";
                var regex = (string)HttpContext.GetLocalResourceObject(strings, "FormatValidation");
                var match = Regex.Match(formatFilter, regex, RegexOptions.IgnoreCase);

                if (!match.Success)
                {
                    q = q.Where(c => c.FormatB.ToLower().Contains(formatBFilter.ToLower()));
                }
                else
                {
                    //now I can extract side1 and side2 from valid format
                    var side1 = formatBFilter.GetSide1();
                    var side2 = formatBFilter.GetSide2();

                    var dies = q.ToList();
                    List<Die> toFill = new List<Die>();

                    foreach (var die in dies)
                    {

                        if (Math.Abs(die.FormatB.GetSide1() - side1) <= 1.5 &&
                            Math.Abs(die.FormatB.GetSide2() - side2) <= 1.5)
                        {
                            toFill.Add(die);
                        }
                    }

                    q = toFill.AsQueryable<Die>();

                }
            }

            if (!string.IsNullOrEmpty(dCut1Filter))
            {
                try
                {
                    var DCut1 = Convert.ToDouble(dCut1Filter, Thread.CurrentThread.CurrentUICulture);
                    q = q.Where(c => c.DCut1 == DCut1);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            if (!string.IsNullOrEmpty(dCut2Filter))
            {
                try
                {
                    var DCut2 = Convert.ToDouble(dCut2Filter, Thread.CurrentThread.CurrentUICulture);
                    q = q.Where(c => c.DCut2 == DCut2);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            if (!string.IsNullOrEmpty(maxGain1Filter))
            {
                try
                {
                    var MaxGain1 = Convert.ToDouble(maxGain1Filter, Thread.CurrentThread.CurrentUICulture);
                    q = q.Where(c => c.MaxGain1 == MaxGain1);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            if (!string.IsNullOrEmpty(maxGain2Filter))
            {
                try
                {
                    var MaxGain2 = Convert.ToDouble(maxGain2Filter, Thread.CurrentThread.CurrentUICulture);
                    q = q.Where(c => c.MaxGain2 == MaxGain2);
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
                case "ArticleDie":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.ArticleName) : q.OrderBy(c => c.ArticleName);
                    break;
                case "Description":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.Description) : q.OrderBy(c => c.Description);
                    break;
                case "PrintingFormat":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.PrintingFormat) : q.OrderBy(c => c.PrintingFormat);
                    break;
                case "Z":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.Z) : q.OrderBy(c => c.Z);
                    break;
                case "Width":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.Width) : q.OrderBy(c => c.Width);
                    break;
                case "Format":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.Format) : q.OrderBy(c => c.Format);
                    break;
                case "DCut1":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.DCut1) : q.OrderBy(c => c.DCut1);
                    break;
                case "DCut2":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.DCut2) : q.OrderBy(c => c.DCut2);
                    break;
                case "MaxGain1":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.MaxGain1) : q.OrderBy(c => c.MaxGain1);
                    break;
                case "MaxGain2":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.MaxGain2) : q.OrderBy(c => c.MaxGain2);
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
                            a.CodDie,
                            a.Format,
                            a.FormatB,
                            a.TypeOfArticle.ToString(),
                            a.Description,
                            a.PrintingFormat,
                            a.FormatType.ToString(),
                            a.Width.ToString(),
                            a.Z.ToString(),
                           // a.DCut1.ToString(),
                           // a.DCut2.ToString(),
                            a.MaxGain1.ToString(),
                            a.MaxGain2.ToString(),                           
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

        //use to put correct typed results
        internal class ResClass : IEquatable<ResClass>
        {
            public string TypeOfMaterial { get; set; }
            public string NameOfMaterial { get; set; }
            public string Adhesive { get; set; }
            public Nullable<bool> NoUseInEstimateCalculation { get; set; }
            public string Color { get; set; }
            public double Weight { get; set; }




            public bool Equals(ResClass other)
            {
                if (TypeOfMaterial == other.TypeOfMaterial &&
                    NameOfMaterial == other.NameOfMaterial &&
                    Adhesive == other.Adhesive &&
                    Color == other.Color &&
                    Weight == other.Weight)
                    return true;

                return false;
            }

            public override int GetHashCode()
            {
                int hashTypeOfMaterial = TypeOfMaterial == null ? 0 : TypeOfMaterial.GetHashCode();
                int hashNameOfMaterial = NameOfMaterial == null ? 0 : NameOfMaterial.GetHashCode();
                int hashAdhesive = Adhesive == null ? 0 : Adhesive.GetHashCode();
                int hashColor = Color == null ? 0 : Color.GetHashCode();
                int hashWeight = Weight == null ? 0 : Weight.GetHashCode();

                return hashTypeOfMaterial ^
                    hashNameOfMaterial ^
                    hashAdhesive ^
                    hashNameOfMaterial ^
                    hashColor ^
                    hashWeight;
            }


        }


        public ActionResult RollPrintableArticleListPerProduct(GridSettings gridSettings)
        {
            List<ResClass> res = PrintableList(gridSettings).OfType<RollPrintableArticle>().Select(p => new ResClass
            {
                TypeOfMaterial = p.TypeOfMaterial,
                NameOfMaterial = p.NameOfMaterial,
                Color = p.Color,
                Adhesive = p.Adhesive,
                NoUseInEstimateCalculation = p.NoUseInEstimateCalculation,
                Weight = p.Weight ?? 0
            }).Distinct().Where(x => x.NoUseInEstimateCalculation == false || x.NoUseInEstimateCalculation == null).ToList();



            var q = res.AsQueryable().OrderBy(c => c.TypeOfMaterial).ToList();

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
                        Weight = a.Weight.ToString()
                    }
                ).ToArray()
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }


        //[HttpParamAction]
        //[AuthorizeUser]
        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult CreateWarehouseArticleMov(WarehouseArticleMov c)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {

        //            c.CodArticle = articleRepository.GetNewCode(c, customerSupplierRepository, c.SupplierMaker, c.SupplierMaker);
        //            //                    c.PrintingFormat = c.Width + "x" + Math.Truncate(Convert.ToDouble((Convert.ToDouble(c.Z) / 8) * 2.54) * 100) / 100;

        //            c.PrintingFormat = c.Width + "x" + (Convert.ToDouble(c.Z) / 8) * 2.54;

        //            articleRepository.Add(c);

        //            articleRepository.Save();
        //            return Json(new { redirectUrl = Url.Action("IndexDie") });
        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
        //        }
        //    }

        //    //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
        //    ViewBag.ActionMethod = "CreateDieFlexo";
        //    return PartialView("_EditAndCreateDieFlexo", c);

        //}

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult OnlyMov()
        {
            return View();
        }

        public ActionResult WarehouseMovListProduct(string codProduct, GridSettings gridSettings)
        {
            var q = warehouseRepository.GetAllMovsProduct(codProduct);

            string warehouseName = string.Empty;

            if (gridSettings.isSearch)
            {
                warehouseName = gridSettings.where.rules.Any(r => r.field == "WarehouseName") ?
                        gridSettings.where.rules.FirstOrDefault(r => r.field == "WarehouseName").data : string.Empty;
            }

            if (!string.IsNullOrEmpty(warehouseName))
            {
                q = q.Where(c => c.WarehouseArticle.WarehouseSpec.WarehouseName.ToLower().Contains(warehouseName.ToLower()));
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
                        id = a.CodWarehouseArticleMov,
                        cell = new string[] 
                        {              
                            a.CodWarehouseArticleMov,
                            (a.Date??DateTime.Now).ToString(),
                            a.WarehouseArticle.WarehouseSpec.WarehouseName,
                            a.TypeOfMov==null?"":a.TypeOfMov.ToString(),
                            "",
                            a.Quantity==null?"":a.Quantity.ToString()
                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);


        }

        public ActionResult WarehouseMovListProducts(GridSettings gridSettings)
        {
            var q = warehouseRepository.GetAllMovsProduct(null).Where(x => x.WarehouseArticle.CodProduct != null);

            string warehouseName = string.Empty;
            string date = string.Empty;
            string type = string.Empty;

            if (gridSettings.isSearch)
            {
                warehouseName = gridSettings.where.rules.Any(r => r.field == "WarehouseName") ?
                        gridSettings.where.rules.FirstOrDefault(r => r.field == "WarehouseName").data : string.Empty;

                date = gridSettings.where.rules.Any(r => r.field == "Date") ?
                        gridSettings.where.rules.FirstOrDefault(r => r.field == "Date").data : string.Empty;

                type = gridSettings.where.rules.Any(r => r.field == "TypeOfMov") ?
                        gridSettings.where.rules.FirstOrDefault(r => r.field == "TypeOfMov").data : string.Empty;

            }

            if (!string.IsNullOrEmpty(warehouseName))
            {
                q = q.Where(c => c.WarehouseArticle.WarehouseSpec.WarehouseName.ToLower().Contains(warehouseName.ToLower()));
            }


            if (!string.IsNullOrEmpty(date))
            {
                try
                {
                    DateTime dt = Convert.ToDateTime(date);
                    q = q.Where(c => DateTime.Compare(c.Date ?? DateTime.Now, dt) == 0);
                }
                catch (Exception)
                {

                }
            }

            //read from validation's language file
            //this resource has to be the same as view's resource
            var resman = new System.Resources.ResourceManager(typeof(Strings).FullName, typeof(Strings).Assembly);
            string unloadType = resman.GetString("UnloadType");//0
            string loadType = resman.GetString("LoadType");//1
            string orderType = resman.GetString("OrderType");//2
            string reserveType = resman.GetString("ReserveType");//3

            if (!string.IsNullOrEmpty(type))
            {
                Boolean isLoad = false, isUnload = false, isOrder = false, isReserve = false;

                //to match with language we have to compare filter with resource
                isUnload = (unloadType.ToLower().StartsWith(type.ToLower()));
                isLoad = (loadType.ToLower().StartsWith(type.ToLower()));
                isOrder = (orderType.ToLower().StartsWith(type.ToLower()));
                isReserve = (reserveType.ToLower().StartsWith(type.ToLower()));

                if (isUnload)
                {
                    q = q.Where(x => x.TypeOfMov == 0);
                }

                if (isLoad)
                {
                    q = q.Where(x => x.TypeOfMov == 1);
                    var s = q.ToList();
                }

                if (isOrder)
                {
                    q = q.Where(x => x.TypeOfMov == 2);
                }

                if (isReserve)
                {
                    q = q.Where(x => x.TypeOfMov == 3);
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
                        id = a.CodWarehouseArticleMov,
                        cell = new string[] 
                        {              
                            a.CodWarehouseArticleMov,
                            (a.Date??DateTime.Now).ToString(),
                            a.WarehouseArticle.WarehouseSpec.WarehouseName,
                            a.WarehouseArticle.Product.ProductName,
                            a.TypeOfMov==null?"":a.TypeOfMov.ToString(),
                            a.Quantity==null?"":a.Quantity.ToString(),       
                            a.CodWarehouseArticleMov
                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);


        }



        public ActionResult DeleteArticleMov(string ids, string urlBack)
        {
            string[] strings = JsonConvert.DeserializeObject<string[]>(ids);
            foreach (var id in strings)
            {
                WarehouseArticleMov q = warehouseRepository.GetAllMovsArticle("").Where(x => x.CodWarehouseArticleMov == id).FirstOrDefault();

                if (q != null)
                {
                    var s = q.WarehouseArticle;
                    warehouseRepository.DeleteMov(q);
                    warehouseRepository.UpdateArticle(s);
                    warehouseRepository.Save();
                }

            }

            return null;

        }


        public ActionResult DeleteWarehouseArticle(string ids)
        {
            string[] strings = JsonConvert.DeserializeObject<string[]>(ids);
            foreach (var id in strings)
            {
                WarehouseItem q = warehouseRepository.GetAll().Where(x => x.CodWarehouseArticle == id).FirstOrDefault();
                if (q != null)
                {
                    warehouseRepository.Delete(q);
                    warehouseRepository.Save();
                }
            }

            return null;

        }


        public ActionResult FromReserveToUnload(string codWarehouseArticleMov, string codWarehouseArticle)
        {
            WarehouseArticleMov q = warehouseRepository.GetAllMovs(codWarehouseArticle).Where(x => x.CodWarehouseArticleMov == codWarehouseArticleMov).FirstOrDefault();

            if (q != null && q.TypeOfMov == 3 || q.TypeOfMov == 2)
            {
                if (q.TypeOfMov == 2) //order
                {
                    q.TypeOfMov = 1; //load
                }

                if (q.TypeOfMov == 3) //reserve
                {
                    q.TypeOfMov = 0; //unload
                }

                q.Date = DateTime.Today;
                warehouseRepository.EditMov(q);


                warehouseRepository.UpdateArticle(q.WarehouseArticle);
                warehouseRepository.Save();


            }

            return null;


        }

        public ActionResult WarehouseMovListArticle(string codArticle, GridSettings gridSettings)
        {
            IQueryable<WarehouseArticleMov> q;

            q = warehouseRepository.GetAllMovsArticle(codArticle);

            string warehouseName = string.Empty;
            string date = string.Empty;
            string type = string.Empty;
            string articleName = string.Empty;
            string codDocument = string.Empty;
            string note = string.Empty;


            if (gridSettings.isSearch)
            {
                warehouseName = gridSettings.where.rules.Any(r => r.field == "WarehouseName") ?
                        gridSettings.where.rules.FirstOrDefault(r => r.field == "WarehouseName").data : string.Empty;

                date = gridSettings.where.rules.Any(r => r.field == "Date") ?
                        gridSettings.where.rules.FirstOrDefault(r => r.field == "Date").data : string.Empty;

                type = gridSettings.where.rules.Any(r => r.field == "TypeOfMov") ?
                        gridSettings.where.rules.FirstOrDefault(r => r.field == "TypeOfMov").data : string.Empty;

                articleName = gridSettings.where.rules.Any(r => r.field == "ArticleName") ?
                        gridSettings.where.rules.FirstOrDefault(r => r.field == "ArticleName").data : string.Empty;

                codDocument = gridSettings.where.rules.Any(r => r.field == "CodDocument") ?
                        gridSettings.where.rules.FirstOrDefault(r => r.field == "CodDocument").data : string.Empty;

                note = gridSettings.where.rules.Any(r => r.field == "Note") ?
        gridSettings.where.rules.FirstOrDefault(r => r.field == "Note").data : string.Empty;

            }

            if (!string.IsNullOrEmpty(warehouseName))
            {
                q = q.Where(c => c.WarehouseArticle.WarehouseSpec.WarehouseName.ToLower().Contains(warehouseName.ToLower()));
            }

            if (!string.IsNullOrEmpty(articleName))
            {
                var names = articleName.Split(' ');

                foreach (var name in names)
                {
                    q = q.Where(c => c.WarehouseArticle.Article.ArticleName.ToLower().Contains(name.ToLower()));
                }
            }


            if (!string.IsNullOrEmpty(note))
            {
                var names = articleName.Split(' ');

                foreach (var name in names)
                {
                    q = q.Where(c => c.Note.ToLower().Contains(note.ToLower()));
                }
            }


            if (!string.IsNullOrEmpty(codDocument))
            {
                var s = q.Where(c => c.Document != null).ToList();
                var s1 = s.Where(c => ((Order)c.Document).OrderNumber.ToLower().Contains(codDocument.ToLower())).ToList();
                q = s1.AsQueryable();
            }

            if (!string.IsNullOrEmpty(date))
            {
                try
                {
                    DateTime dt = Convert.ToDateTime(date);
                    q = q.Where(c => DateTime.Compare(c.Date ?? DateTime.Now, dt) == 0);
                }
                catch (Exception)
                {

                }
            }

            //read from validation's language file
            //this resource has to be the same as view's resource
            var resman = new System.Resources.ResourceManager(typeof(Strings).FullName, typeof(Strings).Assembly);
            string unloadType = resman.GetString("UnloadType");//0
            string loadType = resman.GetString("LoadType");//1
            string orderType = resman.GetString("OrderType");//2
            string reserveType = resman.GetString("ReserveType");//3

            if (!string.IsNullOrEmpty(type))
            {
                Boolean isLoad = false, isUnload = false, isOrder = false, isReserve = false;

                //to match with language we have to compare filter with resource
                isUnload = (unloadType.ToLower().StartsWith(type.ToLower()));
                isLoad = (loadType.ToLower().StartsWith(type.ToLower()));
                isOrder = (orderType.ToLower().StartsWith(type.ToLower()));
                isReserve = (reserveType.ToLower().StartsWith(type.ToLower()));

                if (isUnload)
                {
                    q = q.Where(x => x.TypeOfMov == 0);
                }

                if (isLoad)
                {
                    q = q.Where(x => x.TypeOfMov == 1);
                    var s = q.ToList();
                }

                if (isOrder)
                {
                    q = q.Where(x => x.TypeOfMov == 2);
                }

                if (isReserve)
                {
                    q = q.Where(x => x.TypeOfMov == 3);
                }
            }


            q = q.OrderByDescending(x => x.Date);


            switch (gridSettings.sortColumn)
            {
                case "WarehouseName":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.WarehouseArticle.WarehouseSpec.WarehouseName) : q.OrderBy(c => c.WarehouseArticle.WarehouseSpec.WarehouseName);
                    break;
                case "ArticleName":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.WarehouseArticle.Article.ArticleName) : q.OrderBy(c => c.WarehouseArticle.Article.ArticleName);
                    break;
                case "Date":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.Date) : q.OrderBy(c => c.Date);
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
                        id = a.CodWarehouseArticleMov,
                        cell = new string[] 
                        {              
                            a.CodWarehouseArticleMov,
                            String.Format("{0:d}",(a.Date??DateTime.Now)),
                            a.WarehouseArticle.WarehouseSpec.WarehouseName,
                            a.WarehouseArticle.Article.ArticleName,
                            (a.TypeOfMov==null?"":a.TypeOfMov.ToString()) + "%" + a.CodWarehouseArticleMov + "%" + a.CodWarehouseArticle,
                            a.Quantity==null?"":a.Quantity.ToString(),                         
                            ((Order)(a.Document))!=null?
                            ((Order)(a.Document)).OrderNumberSerie + "/" + ((Order)(a.Document)).OrderNumber + "%" + a.CodDocument
                            :"",
                            a.Note
                        }
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
                                x.TypeOfArticleCost == ArticleCost.ArticleCostType.RollPrintableArticleStandardCost)).CostPerKg,            
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
                            a.CodArticle,
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

        public ActionResult RigidPrintableArticleListPerProduct(GridSettings gridSettings)
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
                            a.TypeOfMaterial,
                            a.NameOfMaterial,
                            a.Color,
                            a.Weight.ToString(),
                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

    }
}
