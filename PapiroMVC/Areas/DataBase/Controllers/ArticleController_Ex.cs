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

        public class RollAutomaticallyChangesViewModel 
        {
            public string[] id { get; set; }
            public string formdata { get; set; }
        }

        //this method works on data from ajax post in _ListRollPrintableArticle 
        //data contains ids and values of parameters
        [HttpPost]
        public ActionResult RollAutomaticallyChanges(RollAutomaticallyChangesViewModel x)
        {           
            return Json(new { message = "this took multiple model..." });
        }

    
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

                var a = isRoll ? (IQueryable<Article>)q.OfType<RollPrintableArticle>() : articleRepository.FindBy(x=>x.CodArticle=="");
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

        public ActionResult SheetPrintableArticleList(GridSettings gridSettings)
        {
            //common serarch and order
            var q = ArticleList(gridSettings).OfType<SheetPrintableArticle>();

            string formatArticleFilter = string.Empty;
            string weightArticleFilter = string.Empty;
            string colorArticleFilter = string.Empty;

            if (gridSettings.isSearch)
            {
                formatArticleFilter = gridSettings.where.rules.Any(r => r.field == "Format") ?
                        gridSettings.where.rules.FirstOrDefault(r => r.field == "Format").data : string.Empty;

                weightArticleFilter = gridSettings.where.rules.Any(r => r.field == "Weight") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "Weight").data : string.Empty;
            }

            if (!string.IsNullOrEmpty(formatArticleFilter))
            {
                q = q.Where(c => c.Format.ToLower().Contains(formatArticleFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(weightArticleFilter))
            {
                try
                {
                    var weightDouble = Convert.ToDouble(weightArticleFilter);
                    q = q.Where(c => c.Weight == weightDouble);
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
                            a.Weight.ToString(),
                            a.Format,
                            a.CustomerSupplierMaker.BusinessName
                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
            
        }

        public ActionResult RollPrintableArticleList(GridSettings gridSettings)
        {
            //common serarch and order
            var q = ArticleList(gridSettings).OfType<RollPrintableArticle>();

            string widthArticleFilter = string.Empty;
            string weightArticleFilter = string.Empty;

            if (gridSettings.isSearch)
            {
                widthArticleFilter = gridSettings.where.rules.Any(r => r.field == "Width") ?
                        gridSettings.where.rules.FirstOrDefault(r => r.field == "Width").data : string.Empty;

                weightArticleFilter = gridSettings.where.rules.Any(r => r.field == "Weight") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "Weight").data : string.Empty;
            }

            if (!string.IsNullOrEmpty(widthArticleFilter))
            {
                try
                {
                    var widthtDouble = Convert.ToDouble(widthArticleFilter);
                    q = q.Where(c => c.Width == widthtDouble);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }

            if (!string.IsNullOrEmpty(weightArticleFilter))
            {
                try
                {
                    var weightDouble = Convert.ToDouble(weightArticleFilter);
                    q = q.Where(c => c.Weight == weightDouble);
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
                            a.Weight.ToString(),
                            a.Width.ToString(),
                            a.CustomerSupplierMaker.BusinessName
                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }


        private IQueryable<Printable> ArticleList(GridSettings gridSettings)
        {
            string codArticleFilter = string.Empty;
            string articleNameFilter = string.Empty;
            string typeOfMaterialFilter = string.Empty;
            string nameOfMaterialFilter = string.Empty;
            string colorFilter = string.Empty;

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

                typeOfMaterialFilter = gridSettings.where.rules.Any(r => r.field == "TypeOfMaterial") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "TypeOfMaterial").data : string.Empty;

                nameOfMaterialFilter = gridSettings.where.rules.Any(r => r.field == "NameOfMaterial") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "NameOfMaterial").data : string.Empty;

                colorFilter = gridSettings.where.rules.Any(r => r.field == "Color") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "Color").data : string.Empty;

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
                q = q.Where(c => c.Color.ToLower().Contains(supplierNameFilter.ToLower()));
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

            return q;
        }

    }
}
