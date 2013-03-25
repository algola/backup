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

            TaskExecutor[] formats = taskExecutorRepository.GetAll().OfType<Litho>().ToArray();

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


        private IQueryable<TaskExecutor> TaskExecutorList(GridSettings gridSettings)
        {
            //use it in filter
            string codTaskExecutorFilter = string.Empty;
            string taskExecutorNameFilter = string.Empty;
            string formatMaxFilter = string.Empty;

            //if gridsetting is a search option
            if (gridSettings.isSearch)
            {
                //pull search field
                codTaskExecutorFilter = gridSettings.where.rules.Any(r => r.field == "CodExecutor") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "CodExecutor").data : string.Empty;

                taskExecutorNameFilter = gridSettings.where.rules.Any(r => r.field == "TaskExecutorName") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "TaskExecutorName").data : string.Empty;

                taskExecutorNameFilter = gridSettings.where.rules.Any(r => r.field == "FormatMax") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "FormatMax").data : string.Empty;

            }


            //read all
            var q = taskExecutorRepository.GetAll();

            //execute filter
            if (!string.IsNullOrEmpty(codTaskExecutorFilter))
            {
                q = q.Where(c => c.CodTaskExecutor.ToLower().Contains(codTaskExecutorFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(taskExecutorNameFilter))
            {
                q = q.Where(c => c.TaskExecutorName.ToLower().Contains(taskExecutorNameFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(formatMaxFilter))
            {
                q = q.Where(c => c.FormatMax.ToLower().Contains(formatMaxFilter.ToLower()));
            }

            /*
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

             */

            //if is sorting
            switch (gridSettings.sortColumn)
            {
                case "CodTaskExecutor":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.CodTaskExecutor) : q.OrderBy(c => c.CodTaskExecutor);
                    break;
                case "TaskExecutorName":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.TaskExecutorName) : q.OrderBy(c => c.TaskExecutorName);
                    break;
                case "FormatMax":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.FormatMax) : q.OrderBy(c => c.FormatMax);
                    break;
            }

            return q;
        }

        private IQueryable<Litho> LithoList(GridSettings gridSettings)
        {
            //common serarch and order
            var q = this.TaskExecutorList(gridSettings).OfType<Litho>();

            //use it in filter
            string printingUnitFilter = string.Empty;
            string sheetwiseAfterPrintingUnitFilter = string.Empty;

            //if gridsetting is a search option
            if (gridSettings.isSearch)
            {
                //pull search field
                printingUnitFilter = gridSettings.where.rules.Any(r => r.field == "PrintingUnit") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "PrintingUnit").data : string.Empty;

                sheetwiseAfterPrintingUnitFilter = gridSettings.where.rules.Any(r => r.field == "SheetwiseAfterPrintingUnit") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "SheetwiseAfterPrintingUnit").data : string.Empty;

            }

            //execute filter
            if (!string.IsNullOrEmpty(printingUnitFilter))
            {
                try
                {
                    var printingUnitShort = Convert.ToInt16(printingUnitFilter);
                    q = q.Where(c => c.PrintingUnit == printingUnitShort);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }

            if (!string.IsNullOrEmpty(sheetwiseAfterPrintingUnitFilter))
            {
                try
                {
                    var sheetwiseAfterPrintingUnitLong = Convert.ToInt64(sheetwiseAfterPrintingUnitFilter);
                    q = q.Where(c => c.SheetwiseAfterPrintingUnit == sheetwiseAfterPrintingUnitLong);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            //if is sorting
            switch (gridSettings.sortColumn)
            {
                case "PrintingUnit":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.PrintingUnit) : q.OrderBy(c => c.PrintingUnit);
                    break;
                case "SheetwiseAfterPrintingUnit":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.SheetwiseAfterPrintingUnit) : q.OrderBy(c => c.SheetwiseAfterPrintingUnit);
                    break;
            }

            return q;
        }

        private IQueryable<Digital> DigitalList(GridSettings gridSettings)
        {
            //common serarch and order
            var q = this.TaskExecutorList(gridSettings).OfType<Digital>();

            //use it in filter
            string bwSide1Filter = string.Empty;
            string bwSide2Filter = string.Empty;
            string colorSide1Filter = string.Empty;
            string colorSide2Filter = string.Empty;

            //if gridsetting is a search option
            if (gridSettings.isSearch)
            {
                //pull search field
                bwSide1Filter = gridSettings.where.rules.Any(r => r.field == "BWSide1") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "BWSide1").data : string.Empty;

                bwSide2Filter = gridSettings.where.rules.Any(r => r.field == "BWSide2") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "BWSide2").data : string.Empty;

                colorSide1Filter = gridSettings.where.rules.Any(r => r.field == "ColorSide1") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "ColorSide1").data : string.Empty;

                colorSide2Filter = gridSettings.where.rules.Any(r => r.field == "ColorSide2") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "ColorSide2").data : string.Empty;

            }

            //execute filter
            if (!string.IsNullOrEmpty(bwSide1Filter))
            {
                try
                {
                    var bwSide1Bool = Convert.ToBoolean(bwSide1Filter);
                    q = q.Where(c => c.BWSide1 == bwSide1Bool);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            if (!string.IsNullOrEmpty(bwSide2Filter))
            {
                try
                {
                    var bwSide2Bool = Convert.ToBoolean(bwSide2Filter);
                    q = q.Where(c => c.BWSide2 == bwSide2Bool);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            if (!string.IsNullOrEmpty(colorSide1Filter))
            {
                try
                {
                    var colorSide1Bool = Convert.ToBoolean(colorSide1Filter);
                    q = q.Where(c => c.ColorSide1 == colorSide1Bool);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            if (!string.IsNullOrEmpty(colorSide2Filter))
            {
                try
                {
                    var colorSide2Bool = Convert.ToBoolean(colorSide2Filter);
                    q = q.Where(c => c.ColorSide2 == colorSide2Bool);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            //if is sorting
            switch (gridSettings.sortColumn)
            {
                case "BWSide1":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.BWSide1) : q.OrderBy(c => c.BWSide1);
                    break;
                case "BWSide2":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.BWSide2) : q.OrderBy(c => c.BWSide2);
                    break;
                case "ColorSide1":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.ColorSide1) : q.OrderBy(c => c.ColorSide1);
                    break;
                case "ColorSide2":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.ColorSide2) : q.OrderBy(c => c.ColorSide2);
                    break;

            }

            return q;
        }

        public ActionResult LithoSheetList(GridSettings gridSettings)
        {
            //common serarch and order
            var q = this.LithoList(gridSettings).OfType<LithoSheet>();

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
                        id = a.CodTaskExecutor,
                        cell = new string[] 
                        {                       
                            a.CodTaskExecutor,
                            a.TaskExecutorName,
                            a.FormatMax,
                            a.PrintingUnit.ToString(),
                            a.SheetwiseAfterPrintingUnit.ToString()

/*                            a.TypeOfMaterial,
                            a.NameOfMaterial,
                            a.Color,
                            a.Weight.ToString(),
                            a.Width.ToString(),
                            a.CustomerSupplierMaker.BusinessName,
                            ((RollPrintableArticleStandardCost)a.ArticleCosts.First(x => 
                                x.TypeOfArticleCost == ArticleCost.ArticleCostType.RollPrintableArticleStandardCost)).CostPerMq,
                            ((RollPrintableArticleStandardCost)a.ArticleCosts.First(x => 
                                x.TypeOfArticleCost == ArticleCost.ArticleCostType.RollPrintableArticleStandardCost)).CostPerMl,            
*/                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        public ActionResult DigitalSheetList(GridSettings gridSettings)
        {
            //common serarch and order
            var q = this.DigitalList(gridSettings).OfType<DigitalSheet>();

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
                        id = a.CodTaskExecutor,
                        cell = new string[] 
                        {                       
                            a.CodTaskExecutor,
                            a.TaskExecutorName,
                            a.FormatMax,
                            a.BWSide1.ToString(),
                            a.BWSide2.ToString(),
                            a.ColorSide1.ToString(),
                            a.ColorSide2.ToString()
/*                            a.TypeOfMaterial,
                            a.NameOfMaterial,
                            a.Color,
                            a.Weight.ToString(),
                            a.Width.ToString(),
                            a.CustomerSupplierMaker.BusinessName,
                            ((RollPrintableArticleStandardCost)a.ArticleCosts.First(x => 
                                x.TypeOfArticleCost == ArticleCost.ArticleCostType.RollPrintableArticleStandardCost)).CostPerMq,
                            ((RollPrintableArticleStandardCost)a.ArticleCosts.First(x => 
                                x.TypeOfArticleCost == ArticleCost.ArticleCostType.RollPrintableArticleStandardCost)).CostPerMl,            
*/                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

    }
}
