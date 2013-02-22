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

namespace PapiroMVC.Areas.DataBase.Controllers
{
    public partial class ArticleController : PapiroMVC.Controllers.ControllerBase
    {
    
        /// <summary>
        /// AutoComplete
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult TypeOfMaterialAutoComplete(string term)
        {
            Printable[] typesOfMaterial = articleRepository.GetAll().OfType<Printable>().ToArray();

            var filteredItems = typesOfMaterial.Where(
            item => item.TypeOfMaterial.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var projection = from art in filteredItems
                             select new
                             {
                                 id = art.TypeOfMaterial,
                                 label = art.TypeOfMaterial,
                                 value = art.TypeOfMaterial
                             };
            return Json(projection.Distinct().ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// AutoComplete
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult FormatAutoComplete(string term)
        {
            SheetPrintableArticle[] formats = articleRepository.GetAll().OfType<SheetPrintableArticle>().ToArray();

            var filteredItems = formats.Where(
            item => item.Format.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var projection = from art in filteredItems
                             select new
                             {
                                 id = art.Format,
                                 label = art.Format,
                                 value = art.Format
                             };
            return Json(projection.Distinct().ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// AutoComplete
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult SheetPerPalletComplete(string term)
        {
            SheetPrintableArticle[] sheetPerPallet = articleRepository.GetAll().OfType<SheetPrintableArticle>().ToArray();

            var filteredItems = sheetPerPallet.Where(
            item => item.SheetPerPallet.ToString().IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var projection = from art in filteredItems
                             select new
                             {
                                 id = art.Format,
                                 label = art.Format,
                                 value = art.Format
                             };
            return Json(projection.Distinct().ToList(), JsonRequestBehavior.AllowGet);
        }

                /// <summary>
        /// AutoComplete
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult ColorAutoComplete(string term)
        {
            SheetPrintableArticle[] colors = articleRepository.GetAll().OfType<SheetPrintableArticle>().ToArray();

            var filteredItems = colors.Where(
            item => item.Color.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var projection = from art in filteredItems                             
                             select new
                             {
                                 id = art.Color,
                                 label = art.Color,
                                 value = art.Color
                             };
            return Json(projection.Distinct().ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult WeightAutoComplete(string term)
        {
            SheetPrintableArticle[] weight = articleRepository.GetAll().OfType<SheetPrintableArticle>().ToArray();

            var filteredItems = weight.Where(
            item => item.Weight.ToString().IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var projection = from art in filteredItems
                             select new
                             {
                                 id = art.Weight,
                                 label = art.Weight,
                                 value = art.Weight
                             };
            return Json(projection.Distinct().ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult HandAutoComplete(string term)
        {
            SheetPrintableArticle[] hand = articleRepository.GetAll().OfType<SheetPrintableArticle>().ToArray();

            var filteredItems = hand.Where(
            item => item.Hand.ToString().IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var projection = from art in filteredItems
                             select new
                             {
                                 id = art.Hand,
                                 label = art.Hand,
                                 value = art.Hand
                             };
            return Json(projection.Distinct().ToList(), JsonRequestBehavior.AllowGet);
        }
                /// <summary>
        /// AutoComplete
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult SheetPerPackedComplete(string term)
        {
            SheetPrintableArticle[] sheetPerPacked = articleRepository.GetAll().OfType<SheetPrintableArticle>().ToArray();

            var filteredItems = sheetPerPacked.Where(
            item => item.SheetPerPacked.ToString().IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var projection = from art in filteredItems
                             select new
                             {
                                 id = art.Format,
                                 label = art.Format,
                                 value = art.Format
                             };
            return Json(projection.Distinct().ToList(), JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// AutoComplete
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult NameOfMaterialAutoComplete(string term)
        {
            Printable[] namesOfMaterial = articleRepository.GetAll().OfType<Printable>().ToArray();

            var filteredItems = namesOfMaterial.Where(
            item => item.NameOfMaterial.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var projection = from art in filteredItems
                             select new
                             {
                                 id = art.NameOfMaterial,
                                 label = art.NameOfMaterial,
                                 value = art.NameOfMaterial
                             };
            return Json(projection.Distinct().ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// AutoComplete
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult UnitOfMeasureAutoComplete(string term)
        {
            Article[] unitOfMeasures = articleRepository.GetAll().OfType<Article>().ToArray();

            var filteredItems = unitOfMeasures.Where(
            item => item.UnitOfMeasure.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var projection = from art in filteredItems
                             select new
                             {
                                 id = art.UnitOfMeasure,
                                 label = art.UnitOfMeasure,
                                 value = art.UnitOfMeasure
                             };
            return Json(projection.Distinct().ToList(), JsonRequestBehavior.AllowGet);
        }

        /*
        /// <summary>
        /// AutoComplete
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult CustomerSupplierCityAutoComplete(string term)
        {
            string[] allCities = customerSupplierBaseRepository.GetAll().Select(x => x.City).ToArray();

            var cities = from d in allCities.Distinct()
                         where (d.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0)
                         select new
                         {
                             label = d,
                             value = d
                         };

            return Json(cities.ToList(), JsonRequestBehavior.AllowGet);
        }

         */

        public ActionResult ArticleList(GridSettings gridSettings)
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
                //to match with language we have to compare filter with resource
                if (rollType.ToLower().Contains(typeOfArticleFilter.ToLower()))
                    q = q.Where(c => c.TypeOfArticle == Article.ArticleType.RollPrintableArticle);

                if (rollType.ToLower().Contains(typeOfArticleFilter.ToLower()))
                    q = q.Where(c => c.TypeOfArticle == Article.ArticleType.SheetPrintableArticle);

                if (rollType.ToLower().Contains(typeOfArticleFilter.ToLower()))
                    q = q.Where(c => c.TypeOfArticle == Article.ArticleType.ObjectPrintableArticle);

                if (rollType.ToLower().Contains(typeOfArticleFilter.ToLower()))
                    q = q.Where(c => c.TypeOfArticle == Article.ArticleType.RigidPrintableArticle);
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

    }
}
