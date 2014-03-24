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
using System.Globalization;

namespace PapiroMVC.Areas.DataBase.Controllers
{
    public partial class TaskExecutorController : PapiroMVC.Controllers.ControllerAlgolaBase
    {

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

                formatMaxFilter = gridSettings.where.rules.Any(r => r.field == "FormatMax") ?
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
            var qqqq = this.TaskExecutorList(gridSettings).OfType<LithoSheet>();
            var qq = this.TaskExecutorList(gridSettings).OfType<LithoRoll>();

            var q = qq.Union((IQueryable<Litho>)qqqq);

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

        public ActionResult LithoSheetAndRollList(GridSettings gridSettings)
        {
            //common serarch and order
            //            var q = this.LithoList(gridSettings).OfType<LithoSheet>();
            var q = this.LithoList(gridSettings);


            string typeOfTaskExecutorFilter = string.Empty;
            //if gridsetting is a search option

            //read from validation's language file
            //this resource has to be the same as view's resource
            var resman = new System.Resources.ResourceManager(typeof(Strings).FullName, typeof(Strings).Assembly);
            string sheetType = resman.GetString("SheetType");
            string rollType = resman.GetString("RollType");


            if (gridSettings.isSearch)
            {
                typeOfTaskExecutorFilter = gridSettings.where.rules.Any(r => r.field == "TypeOfTaskExecutor") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "TypeOfTaskExecutor").data : string.Empty;
            }

            if (!string.IsNullOrEmpty(typeOfTaskExecutorFilter))
            {
                Boolean isCust = false, isSupp = false;

                //to match with language we have to compare filter with resource
                isCust = (sheetType.ToLower().Contains(typeOfTaskExecutorFilter.ToLower()));
                isSupp = (rollType.ToLower().Contains(typeOfTaskExecutorFilter.ToLower()));

                var a = isCust ? (IQueryable<Litho>)q.OfType<LithoSheet>() : q.Where(x => x.CodTaskExecutor == "");
                var b = isSupp ? (IQueryable<Litho>)q.OfType<LithoRoll>() : q.Where(x => x.CodTaskExecutor == "");

                var res = (a.Union(b));
                q = (IQueryable<PapiroMVC.Models.Litho>)res;
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
                        id = a.CodTaskExecutor,
                        cell = new string[] 
                        {                                
                            a.SetTaskExecutorEstimatedOn.Count()==0?"CostError":
                                a.SetTaskExecutorEstimatedOn.FirstOrDefault().TypeOfEstimatedOn==TaskEstimatedOn.EstimatedOnType.OnMq?"CostMq":
                                    a.SetTaskExecutorEstimatedOn.FirstOrDefault().TypeOfEstimatedOn==TaskEstimatedOn.EstimatedOnType.OnTime?"CostTime":
                                        a.SetTaskExecutorEstimatedOn.FirstOrDefault().TypeOfEstimatedOn==TaskEstimatedOn.EstimatedOnType.OnRun?"CostRun":"",                                           
                            a.CodTaskExecutor,
                            a.CodTaskExecutor,
                            a.TypeOfExecutor.ToString(),
                            a.TaskExecutorName,
                            a.FormatMax,
                            a.PrintingUnit.ToString(),
                            a.SheetwiseAfterPrintingUnit.ToString()

                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        public ActionResult FlexoList(GridSettings gridSettings)
        {
            //common serarch and order
            //            var q = this.LithoList(gridSettings).OfType<LithoSheet>();

            var q = this.TaskExecutorList(gridSettings).OfType<Flexo>();

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
                            a.SetTaskExecutorEstimatedOn.Count()==0?"CostError":
                                a.SetTaskExecutorEstimatedOn.FirstOrDefault().TypeOfEstimatedOn==TaskEstimatedOn.EstimatedOnType.OnMq?"CostMq":
                                    a.SetTaskExecutorEstimatedOn.FirstOrDefault().TypeOfEstimatedOn==TaskEstimatedOn.EstimatedOnType.OnTime?"CostTime":
                                        a.SetTaskExecutorEstimatedOn.FirstOrDefault().TypeOfEstimatedOn==TaskEstimatedOn.EstimatedOnType.OnRun?"CostRun":"",                                           
                            a.CodTaskExecutor,
                            a.CodTaskExecutor,
                            a.TypeOfExecutor.ToString(),
                            a.TaskExecutorName,
                            a.PrintingUnit.ToString(),
//                            (a.Width??Convert.ToDouble(0)).ToString(CultureInfo.InvariantCulture.NumberFormat)
                        }

                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        public ActionResult DigitalSheetListAndRoll(GridSettings gridSettings)
        {
            //common serarch and order
            // var q = this.DigitalList(gridSettings).OfType<DigitalSheet>();
            var q = this.DigitalList(gridSettings);

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
                            a.SetTaskExecutorEstimatedOn.Count()==0?"CostError":
                                a.SetTaskExecutorEstimatedOn.FirstOrDefault().TypeOfEstimatedOn==TaskEstimatedOn.EstimatedOnType.OnMq?"CostMq":
                                    a.SetTaskExecutorEstimatedOn.FirstOrDefault().TypeOfEstimatedOn==TaskEstimatedOn.EstimatedOnType.OnTime?"CostTime":
                                        a.SetTaskExecutorEstimatedOn.FirstOrDefault().TypeOfEstimatedOn==TaskEstimatedOn.EstimatedOnType.OnRun?"CostRun":                                           
                                            a.SetTaskExecutorEstimatedOn.FirstOrDefault().TypeOfEstimatedOn==TaskEstimatedOn.EstimatedOnType.DigitalOnTime?"CostTime":                                           
                                                a.SetTaskExecutorEstimatedOn.FirstOrDefault().TypeOfEstimatedOn==TaskEstimatedOn.EstimatedOnType.DigitalOnRun?"CostRun":"",                                           
                            a.CodTaskExecutor,
                            a.CodTaskExecutor,
                            a.TypeOfExecutor.ToString(),
                            a.TaskExecutorName,
                            a.FormatMax,
                            a.BWSide1.ToString(),
                            a.BWSide2.ToString(),
                            a.ColorSide1.ToString(),
                            a.ColorSide2.ToString(),
                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        public IQueryable<Plotter> PlotterList(GridSettings gridSettings)
        {
            //common serarch and order
            var q = this.TaskExecutorList(gridSettings).OfType<Plotter>();
            return q;
        }

        public ActionResult PlotterListSheetAndRoll(GridSettings gridSettings)
        {

            //common serarch and order
            var q = this.PlotterList(gridSettings);

            string typeOfTaskExecutorFilter = string.Empty;
            //if gridsetting is a search option

            //read from validation's language file
            //this resource has to be the same as view's resource
            var resman = new System.Resources.ResourceManager(typeof(Strings).FullName, typeof(Strings).Assembly);
            string sheetType = resman.GetString("PlotterSheetType");
            string rollType = resman.GetString("PlotterRollType");


            if (gridSettings.isSearch)
            {
                typeOfTaskExecutorFilter = gridSettings.where.rules.Any(r => r.field == "TypeOfTaskExecutor") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "TypeOfTaskExecutor").data : string.Empty;
            }

            if (!string.IsNullOrEmpty(typeOfTaskExecutorFilter))
            {
                Boolean isCust = false, isSupp = false;

                //to match with language we have to compare filter with resource
                isCust = (sheetType.ToLower().Contains(typeOfTaskExecutorFilter.ToLower()));
                isSupp = (rollType.ToLower().Contains(typeOfTaskExecutorFilter.ToLower()));

                var a = isCust ? (IQueryable<Plotter>)q.OfType<PlotterSheet>() : q.Where(x => x.CodTaskExecutor == "");
                var b = isSupp ? (IQueryable<Plotter>)q.OfType<PlotterRoll>() : q.Where(x => x.CodTaskExecutor == "");

                var res = (a.Union(b));
                q = (IQueryable<Plotter>)res;
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
                        id = a.CodTaskExecutor,
                        cell = new string[] 
                        {     
                            a.SetTaskExecutorEstimatedOn.Count()==0?"CostError":
                                a.SetTaskExecutorEstimatedOn.FirstOrDefault().TypeOfEstimatedOn==TaskEstimatedOn.EstimatedOnType.OnMq?"CostMq":
                                    a.SetTaskExecutorEstimatedOn.FirstOrDefault().TypeOfEstimatedOn==TaskEstimatedOn.EstimatedOnType.OnTime?"CostTime":
                                        a.SetTaskExecutorEstimatedOn.FirstOrDefault().TypeOfEstimatedOn==TaskEstimatedOn.EstimatedOnType.OnRun?"CostRun":
                                            a.SetTaskExecutorEstimatedOn.FirstOrDefault().TypeOfEstimatedOn==TaskEstimatedOn.EstimatedOnType.PlotterOnMq?"PlotterOnMq":"",                                           
  
                            a.CodTaskExecutor,
                            a.CodTaskExecutor,
                            a.TypeOfExecutor.ToString(),
                            a.TaskExecutorName,
                            a.FormatMax,
                            //a.BWSide1.ToString(),
                            //a.BWSide2.ToString(),
                            //a.ColorSide1.ToString(),
                            //a.ColorSide2.ToString(),
                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }





        public ActionResult PrePostPressList(GridSettings gridSettings)
        {
            //common serarch and order
            var q = this.TaskExecutorList(gridSettings).OfType<PrePostPress>();

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
                            a.SetTaskExecutorEstimatedOn.Count()==0?"CostError":
                                a.SetTaskExecutorEstimatedOn.FirstOrDefault().TypeOfEstimatedOn==TaskEstimatedOn.EstimatedOnType.OnMq?"CostMq":
                                    a.SetTaskExecutorEstimatedOn.FirstOrDefault().TypeOfEstimatedOn==TaskEstimatedOn.EstimatedOnType.OnTime?"CostTime":
                                        a.SetTaskExecutorEstimatedOn.FirstOrDefault().TypeOfEstimatedOn==TaskEstimatedOn.EstimatedOnType.OnRun?"CostRun":
                                            a.SetTaskExecutorEstimatedOn.FirstOrDefault().TypeOfEstimatedOn==TaskEstimatedOn.EstimatedOnType.BindingOnTime?"CostTime":
                                                a.SetTaskExecutorEstimatedOn.FirstOrDefault().TypeOfEstimatedOn==TaskEstimatedOn.EstimatedOnType.BindingOnRun?"CostRun":"",                                           
                            a.CodTaskExecutor,
                            a.CodTaskExecutor,
//CODTASKTODO   a.CodTask,
                            a.TaskExecutorName,
                            a.FormatMax,
                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        private IQueryable<Step> StepsList(string codTaskExecutorOn, GridSettings gridSettings)
        {
            //read all
            var z = taskExecutorRepository.GetSingleEstimatedOn(codTaskExecutorOn);
            var z2 = z.steps;
            return z2.OrderBy(x => x.FromUnit).AsQueryable();
        }

        public ActionResult AvarageRunPerRunStepList(string codTaskExecutorOn, GridSettings gridSettings)
        {
            var q = StepsList(codTaskExecutorOn, gridSettings).OfType<AvarageRunPerRunStep>();

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
                        id = a.IdStep,
                        cell = new string[] 
                        {       
                            a.IdStep.ToString(),
                            a.IdStep.ToString(),
                            a.CodTaskEstimatedOn.ToString(),
                            a.FromUnit.ToString(),
                            a.ToUnit.ToString(),
                            a.AvarageRunPerHour.ToString()
                         }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TaskExecutorCylinderList(string codTaskExecutor, GridSettings gridSettings)
        {

            //read all
            var z = taskExecutorRepository.GetSingle(codTaskExecutor);

            ((Flexo)z).CheckZeroCylinder();

            var z2 = z.TaskExecutorCylinders;
            
            var q = z2.OrderBy(x => x.Z).AsQueryable();

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
                        id = a.CodTaskExecutorCylinder,
                        cell = new string[] 
                        {       
                            a.CodTaskExecutorCylinder,
                            a.CodTaskExecutorCylinder,
                            a.CodTaskExecutor,
                            a.Z.ToString(),
                            a.Quantity.ToString()
                         }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeficitForWeightStepList(string codTaskExecutorOn, GridSettings gridSettings)
        {
            var q = StepsList(codTaskExecutorOn, gridSettings).OfType<DeficitForWeightStep>();

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
                        id = a.IdStep,
                        cell = new string[] 
                        {       
                            a.IdStep.ToString(),
                            a.IdStep.ToString(),
                            a.CodTaskEstimatedOn.ToString(),
                            a.FromUnit.ToString(),
                            a.ToUnit.ToString(),
                            a.DeficitRate
                         }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeficitOnCostForWeightStepList(string codTaskExecutorOn, GridSettings gridSettings)
        {
            var q = StepsList(codTaskExecutorOn, gridSettings).OfType<DeficitOnCostForWeightStep>();

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
                        id = a.IdStep,
                        cell = new string[] 
                        {       
                            a.IdStep.ToString(),
                            a.IdStep.ToString(),
                            a.CodTaskEstimatedOn.ToString(),
                            a.FromUnit.ToString(),
                            a.ToUnit.ToString(),
                            a.DeficitRate
                         }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CostPerRunStepList(string codTaskExecutorOn, GridSettings gridSettings)
        {
            var q = StepsList(codTaskExecutorOn, gridSettings).OfType<CostPerRunStep>();

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
                        id = a.IdStep,
                        cell = new string[] 
                        {       
                            a.IdStep.ToString(),
                            a.IdStep.ToString(),
                            a.CodTaskEstimatedOn.ToString(),
                            a.FromUnit.ToString(),
                            a.ToUnit.ToString(),
                            a.CostPerUnit
                         }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CostPerRunStepListBW(string codTaskExecutorOn, GridSettings gridSettings)
        {
            var q = StepsList(codTaskExecutorOn, gridSettings).OfType<CostPerRunStepBW>();

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
                        id = a.IdStep,
                        cell = new string[] 
                        {       
                            a.IdStep.ToString(),
                            a.IdStep.ToString(),
                            a.CodTaskEstimatedOn.ToString(),
                            a.FromUnit.ToString(),
                            a.ToUnit.ToString(),
                            a.CostPerUnit
                         }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        //SAREBBE MEGLIO FARE IL POLIMORFISMO A SECONDA DEL TASKEXECUTOR
        private void GenEmptyStep(TaskExecutor taskExecutor)
        {
            taskExecutor = taskExecutorRepository.GetSingle(taskExecutor.CodTaskExecutor);

            foreach (var tskEst in taskExecutor.SetTaskExecutorEstimatedOn)
            {

                Step newStep;

                //look for to=0 from=0
                newStep = tskEst.steps.OfType<AvarageRunPerRunStep>().FirstOrDefault(x => x.FromUnit == 9999999999 && x.ToUnit == 9999999999);

                if (newStep == null)
                {
                    newStep = new AvarageRunPerRunStep();
                    newStep.FromUnit = 9999999999;
                    newStep.ToUnit = 9999999999;
                    newStep.CodTaskEstimatedOn = tskEst.CodTaskEstimatedOn;
                    newStep.TimeStampTable = DateTime.Now;
                    tskEst.steps.Add(newStep);
                }

                //look for to=0 from=0
                newStep = tskEst.steps.OfType<DeficitForWeightStep>().FirstOrDefault(x => x.FromUnit == 9999999999 && x.ToUnit == 9999999999);

                if (newStep == null)
                {
                    newStep = new DeficitForWeightStep();
                    newStep.FromUnit = 9999999999;
                    newStep.ToUnit = 9999999999;
                    newStep.CodTaskEstimatedOn = tskEst.CodTaskEstimatedOn;
                    newStep.TimeStampTable = DateTime.Now;
                    tskEst.steps.Add(newStep);
                }


                //look for to=0 from=0
                newStep = tskEst.steps.OfType<DeficitOnCostForWeightStep>().FirstOrDefault(x => x.FromUnit == 9999999999 && x.ToUnit == 9999999999);

                if (newStep == null)
                {
                    newStep = new DeficitOnCostForWeightStep();
                    newStep.FromUnit = 9999999999;
                    newStep.ToUnit = 9999999999;
                    newStep.CodTaskEstimatedOn = tskEst.CodTaskEstimatedOn;
                    newStep.TimeStampTable = DateTime.Now;
                    tskEst.steps.Add(newStep);
                }

                //look for to=0 from=0
                newStep = tskEst.steps.OfType<BindingAvarageRunPerRunStep>().FirstOrDefault(x => x.FromUnit == 9999999999 && x.ToUnit == 9999999999);

                if (newStep == null)
                {
                    newStep = new BindingAvarageRunPerRunStep();
                    newStep.FromUnit = 9999999999;
                    newStep.ToUnit = 9999999999;
                    newStep.CodTaskEstimatedOn = tskEst.CodTaskEstimatedOn;
                    newStep.TimeStampTable = DateTime.Now;
                    tskEst.steps.Add(newStep);
                }

                //look for to=0 from=0
                newStep = tskEst.steps.OfType<BindingCostPerRunStep>().FirstOrDefault(x => x.FromUnit == 9999999999 && x.ToUnit == 9999999999);

                if (newStep == null)
                {
                    newStep = new BindingCostPerRunStep();
                    newStep.FromUnit = 9999999999;
                    newStep.ToUnit = 9999999999;
                    newStep.CodTaskEstimatedOn = tskEst.CodTaskEstimatedOn;
                    newStep.TimeStampTable = DateTime.Now;
                    tskEst.steps.Add(newStep);
                }

                //look for to=0 from=0
                newStep = tskEst.steps.OfType<CostPerMqStep>().FirstOrDefault(x => x.FromUnit == 9999999999 && x.ToUnit == 9999999999);

                if (newStep == null)
                {
                    newStep = new CostPerMqStep();
                    newStep.FromUnit = 9999999999;
                    newStep.ToUnit = 9999999999;
                    newStep.CodTaskEstimatedOn = tskEst.CodTaskEstimatedOn;
                    newStep.TimeStampTable = DateTime.Now;
                    tskEst.steps.Add(newStep);
                }


                //look for to=0 from=0
                newStep = tskEst.steps.OfType<CostPerRunStep>().FirstOrDefault(x => x.FromUnit == 9999999999 && x.ToUnit == 9999999999);

                if (newStep == null)
                {
                    newStep = new CostPerRunStep();
                    newStep.FromUnit = 9999999999;
                    newStep.ToUnit = 9999999999;
                    newStep.CodTaskEstimatedOn = tskEst.CodTaskEstimatedOn;
                    newStep.TimeStampTable = DateTime.Now;
                    tskEst.steps.Add(newStep);
                }


                //look for to=0 from=0
                newStep = tskEst.steps.OfType<CostPerRunStepBW>().FirstOrDefault(x => x.FromUnit == 9999999999 && x.ToUnit == 9999999999);

                if (newStep == null)
                {
                    newStep = new CostPerRunStepBW();
                    newStep.FromUnit = 9999999999;
                    newStep.ToUnit = 9999999999;
                    newStep.CodTaskEstimatedOn = tskEst.CodTaskEstimatedOn;
                    newStep.TimeStampTable = DateTime.Now;
                    tskEst.steps.Add(newStep);
                }



                //look for to=0 from=0
                newStep = tskEst.steps.OfType<BindingAvarageRunPerRunStep>().FirstOrDefault(x => x.FromUnit == 9999999999 && x.ToUnit == 9999999999);

                if (newStep == null)
                {
                    newStep = new BindingAvarageRunPerRunStep();
                    newStep.FromUnit = 9999999999;
                    newStep.ToUnit = 9999999999;
                    newStep.CodTaskEstimatedOn = tskEst.CodTaskEstimatedOn;
                    newStep.TimeStampTable = DateTime.Now;
                    tskEst.steps.Add(newStep);
                }

            }

            taskExecutorRepository.Edit(taskExecutor);
            taskExecutorRepository.Save();

        }

        /// <summary>
        /// deleting one step and responding with error or ok message
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeletingStep(string id)
        {
            try
            {
                var step = taskExecutorRepository.GetSingleStep(Convert.ToInt32(id));
                var taskEstimatedOn = step.taskexecutorestimatedon;
                var taskExecutor = step.taskexecutorestimatedon.taskexecutors;

                taskEstimatedOn.steps.Remove(step);

                taskExecutorRepository.Edit(taskExecutor);
                taskExecutorRepository.Save();

                GenEmptyStep(taskExecutor);

            }
            catch (Exception e)
            {
                //error
                HttpContext.Response.StatusCode = 500;
                HttpContext.Response.Clear();

                return Json(new
                {
                    message = e.Message
                });
            }

            return Json(new
            {
                message = "Ok"
            });

        }


            /// <summary>
        /// deleting one step and responding with error or ok message
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeletingTaskExecutorCylinder(string id)
        {
            try
            {
                var x=taskExecutorRepository.GetSingleTaskExecutorCylindern(id);
                taskExecutorRepository.DeleteSingleCylinder(x);
                taskExecutorRepository.Save();
            }
            catch (Exception e)
            {
                //error
                HttpContext.Response.StatusCode = 500;
                HttpContext.Response.Clear();

                return Json(new
                {
                    message = e.Message
                });
            }

            return Json(new
            {
                message = "Ok"
            });

        }
    }


}
