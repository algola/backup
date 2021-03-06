﻿using System;
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
    public partial class ArticleController : PapiroMVC.Controllers.ControllerAlgolaBase
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

        public ActionResult TaskExecutorNameAutoComplete(string term)
        {
            Die[] taskExecutorNames = articleRepository.GetAll().OfType<Die>().ToArray();

            var filteredItems = taskExecutorNames.Where(
            item => item.TaskExecutorName.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var projection = from art in filteredItems
                             select new
                             {
                                 id = art.TaskExecutorName,
                                 label = art.TaskExecutorName,
                                 value = art.TaskExecutorName
                             };
            return Json(projection.Distinct().ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// AutoComplete
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult AdhesiveAutoComplete(string term)
        {
            Printable[] adhesives = articleRepository.GetAll().OfType<Printable>().ToArray();

            if (adhesives != null)
            {
                var filteredItems = adhesives.Where(
                item => item.Adhesive != null && item.Adhesive.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
                );

                var projection = from art in filteredItems
                                 select new
                                 {
                                     id = art.Adhesive,
                                     label = art.Adhesive,
                                     value = art.Adhesive
                                 };
                return Json(projection.Distinct().ToList(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
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


        public ActionResult FromMinFormatAutoComplete(string term)
        {
            RigidPrintableArticle[] formats = articleRepository.GetAll().OfType<RigidPrintableArticle>().ToArray();

            var filteredItems = formats.Where(
            item => item.FromMinFormat.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var projection = from art in filteredItems
                             select new
                             {
                                 id = art.FromMinFormat,
                                 label = art.FromMinFormat,
                                 value = art.FromMinFormat
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
                                 id = art.SheetPerPallet,
                                 label = art.SheetPerPallet,
                                 value = art.SheetPerPallet
                             };
            return Json(projection.Distinct().ToList(), JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// AutoComplete
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult MqForafaitAutoComplete(string term)
        {
            RollPrintableArticle[] sheetPerPallet = articleRepository.GetAll().OfType<RollPrintableArticle>().ToArray();

            var filteredItems = sheetPerPallet.Where(
            item => item.MqForafait.ToString().IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var projection = from art in filteredItems
                             select new
                             {
                                 id = art.MqForafait,
                                 label = art.MqForafait,
                                 value = art.MqForafait
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
            Printable[] colors = articleRepository.GetAll().OfType<Printable>().ToArray();

            var notNull = colors.Except(colors.Where(item => string.IsNullOrEmpty(item.Color)));

            var filteredItems = notNull.Where(
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
            Printable[] weight = articleRepository.GetAll().OfType<Printable>().ToArray();

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

        public ActionResult WidthAutoComplete(string term)
        {
            RollPrintableArticle[] width = articleRepository.GetAll().OfType<RollPrintableArticle>().ToArray();

            var filteredItems = width.Where(
            item => item.Width.ToString().IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var projection = from art in filteredItems
                             select new
                             {
                                 id = art.Width,
                                 label = art.Width,
                                 value = art.Width
                             };
            return Json(projection.Distinct().ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult UnitOfMeasureStoreAutoComplete(string term)
        {
            Article[] um = articleRepository.GetAll().OfType<Article>().Where(x => x.WarehouseArticles.Count > 0 && x.WarehouseArticles.FirstOrDefault().UnitOfMeasureStore!=null).ToArray();

            var filteredItems = um.Where(
                item => item.WarehouseArticles.FirstOrDefault().UnitOfMeasureStore.ToString().IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var projection = from art in filteredItems
                             select new
                             {
                                 id = art.WarehouseArticles.FirstOrDefault().UnitOfMeasureStore,
                                 label = art.WarehouseArticles.FirstOrDefault().UnitOfMeasureStore,
                                 value = art.WarehouseArticles.FirstOrDefault().UnitOfMeasureStore
                             };
            return Json(projection.Distinct().ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Autocomplete Unit Of Measure
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult UnitOfMeasureMovAutoComplete(string term)
        {
            Article[] um = articleRepository.GetAll().OfType<Article>().Where(x => x.WarehouseArticles.Count > 0 && x.WarehouseArticles.FirstOrDefault().UnitOfMeasureMov != null).ToArray();

            var filteredItems = um.Where(
                item => item.WarehouseArticles.FirstOrDefault().UnitOfMeasureMov.ToString().IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var projection = from art in filteredItems
                             select new
                             {
                                 id = art.WarehouseArticles.FirstOrDefault().UnitOfMeasureMov,
                                 label = art.WarehouseArticles.FirstOrDefault().UnitOfMeasureMov,
                                 value = art.WarehouseArticles.FirstOrDefault().UnitOfMeasureMov
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
                                 id = art.SheetPerPacked,
                                 label = art.SheetPerPacked,
                                 value = art.SheetPerPacked
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

        public ActionResult FoilAutoComplete(string term)
        {
            Article[] typeOfSerigraphy = articleRepository.GetAll().OfType<Foil>().ToArray();

            var resman = new System.Resources.ResourceManager(typeof(Strings).FullName, typeof(Strings).Assembly);

            var filteredItems = typeOfSerigraphy.Where(
            item => item.ArticleName.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var projection = from art in filteredItems
                             select new
                             {
                                 id = art.ArticleName,
                                 label = art.ArticleName,
                                 value = art.ArticleName
                             };
            return Json(projection.Distinct().ToList(), JsonRequestBehavior.AllowGet);
        }

        
        ///// <summary>
        ///// AutoComplete
        ///// </summary>
        ///// <param name="term"></param>
        ///// <returns></returns>
        //public ActionResult UnitOfMeasureAutoComplete(string term)
        //{
        //    Article[] unitOfMeasures = articleRepository.GetAll().OfType<Article>().ToArray();

        //    var filteredItems = unitOfMeasures.Where(
        //    item => item.UnitOfMeasure.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
        //    );

        //    var projection = from art in filteredItems
        //                     select new
        //                     {
        //                         id = art.UnitOfMeasure,
        //                         label = art.UnitOfMeasure,
        //                         value = art.UnitOfMeasure
        //                     };
        //    return Json(projection.Distinct().ToList(), JsonRequestBehavior.AllowGet);
        //}

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

    }
}
