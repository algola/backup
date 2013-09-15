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
    public partial class TaskExecutorController : PapiroMVC.Controllers.ControllerAlgolaBase
    {
       
        //example of autocpmplete
        public ActionResult FormatMaxAutoComplete(string term)
        {
            TaskExecutor[] formats = taskExecutorRepository.GetAll().OfType<TaskExecutor>().ToArray();

            var notNull = formats.Except(formats.Where(item => string.IsNullOrEmpty(item.FormatMax)));

            var filteredItems = notNull.Where(
            item => item.FormatMax.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var projection = from art in filteredItems
                             select new
                             {
                                 id = art.FormatMax,
                                 label = art.FormatMax,
                                 value = art.FormatMax
                             };
            return Json(projection.Distinct().ToList(), JsonRequestBehavior.AllowGet);
        }


        public ActionResult FormatMinAutoComplete(string term)
        {
            TaskExecutor[] formats = taskExecutorRepository.GetAll().OfType<TaskExecutor>().ToArray();

            var notNull = formats.Except(formats.Where(item => string.IsNullOrEmpty(item.FormatMin)));

            var filteredItems = notNull.Where(
            item => item.FormatMin.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var projection = from art in filteredItems
                             select new
                             {
                                 id = art.FormatMin,
                                 label = art.FormatMin,
                                 value = art.FormatMin
                             };
            return Json(projection.Distinct().ToList(), JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult ProofSheetFirstStartAutoComplete(string term)
        {
            DigitalSheet[] proofSheetFirstStart = taskExecutorRepository.GetAll().OfType<DigitalSheet>().ToArray();
            LithoSheet[] proofSheetFirstStart2 = taskExecutorRepository.GetAll().OfType<LithoSheet>().ToArray();

            var notNull = proofSheetFirstStart.Except(proofSheetFirstStart.Where(item => string.IsNullOrEmpty(item.ProofSheetFirstStart.ToString())));
            var notNull2 = proofSheetFirstStart2.Except(proofSheetFirstStart2.Where(item => string.IsNullOrEmpty(item.ProofSheetFirstStart.ToString())));

            var filteredItems = notNull.Where(
            item => item.ProofSheetFirstStart.ToString().IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var filteredItems2 = notNull2.Where(
            item => item.ProofSheetFirstStart.ToString().IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var projection = from art in filteredItems
                             select new
                             {
                                 id = art.ProofSheetFirstStart,
                                 label = art.ProofSheetFirstStart,
                                 value = art.ProofSheetFirstStart
                             };


            var projection2 = from art in filteredItems2
                             select new
                             {
                                 id = art.ProofSheetFirstStart,
                                 label = art.ProofSheetFirstStart,
                                 value = art.ProofSheetFirstStart
                             };
            var p = projection.Union(projection2);

            return Json(p.Distinct().ToList(), JsonRequestBehavior.AllowGet);

        }

        public ActionResult ProofSheetSecondsStartAutoComplete(string term)
        {
            DigitalSheet[] proofSheetSecondsStart = taskExecutorRepository.GetAll().OfType<DigitalSheet>().ToArray();
            LithoSheet[] proofSheetSecondsStart2 = taskExecutorRepository.GetAll().OfType<LithoSheet>().ToArray();

            var notNull = proofSheetSecondsStart.Except(proofSheetSecondsStart.Where(item => string.IsNullOrEmpty(item.ProofSheetSecondsStart.ToString())));
            var notNull2 = proofSheetSecondsStart2.Except(proofSheetSecondsStart2.Where(item => string.IsNullOrEmpty(item.ProofSheetSecondsStart.ToString())));

            var filteredItems = notNull.Where(
            item => item.ProofSheetSecondsStart.ToString().IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var filteredItems2 = notNull2.Where(
            item => item.ProofSheetSecondsStart.ToString().IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var projection = from art in filteredItems
                             select new
                             {
                                 id = art.ProofSheetSecondsStart,
                                 label = art.ProofSheetSecondsStart,
                                 value = art.ProofSheetSecondsStart
                             };


            var projection2 = from art in filteredItems2
                              select new
                              {
                                  id = art.ProofSheetSecondsStart,
                                  label = art.ProofSheetSecondsStart,
                                  value = art.ProofSheetSecondsStart
                              };
            var p = projection.Union(projection2);

            return Json(p.Distinct().ToList(), JsonRequestBehavior.AllowGet);

        }


        public ActionResult ProductionWasteAutoComplete(string term)
        {
            DigitalSheet[] productionWaste = taskExecutorRepository.GetAll().OfType<DigitalSheet>().ToArray();
            LithoSheet[] productionWaste2 = taskExecutorRepository.GetAll().OfType<LithoSheet>().ToArray();

            var notNull = productionWaste.Except(productionWaste.Where(item => string.IsNullOrEmpty(item.ProductionWaste.ToString())));
            var notNull2 = productionWaste2.Except(productionWaste2.Where(item => string.IsNullOrEmpty(item.ProductionWaste.ToString())));

            var filteredItems = notNull.Where(
            item => item.ProductionWaste.ToString().IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var filteredItems2 = notNull2.Where(
            item => item.ProductionWaste.ToString().IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var projection = from art in filteredItems
                             select new
                             {
                                 id = art.ProductionWaste,
                                 label = art.ProductionWaste,
                                 value = art.ProductionWaste
                             };


            var projection2 = from art in filteredItems2
                              select new
                              {
                                  id = art.ProductionWaste,
                                  label = art.ProductionWaste,
                                  value = art.ProductionWaste
                              };
            var p = projection.Union(projection2);

            return Json(p.Distinct().ToList(), JsonRequestBehavior.AllowGet);

        }





        public ActionResult WidthAutoComplete(string term)
        {
            Plotter[] formats = taskExecutorRepository.GetAll().OfType<Plotter>().ToArray();

            var notNull = formats.Except(formats.Where(item => string.IsNullOrEmpty(item.Width.ToString())));

            var filteredItems = notNull.Where(
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

        
        
    }




}
