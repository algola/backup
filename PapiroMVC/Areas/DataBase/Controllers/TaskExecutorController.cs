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
using PapiroMVC.DbCodeManagement;
using PapiroMVC.Validation;

namespace PapiroMVC.Areas.DataBase.Controllers
{
    [AuthorizeUser]
    public partial class TaskExecutorController : PapiroMVC.Controllers.ControllerAlgolaBase
    {

        private readonly ITaskExecutorRepository taskExecutorRepository;
        private readonly ITypeOfTaskRepository typeOfTaskRepository;

        protected dbEntities db;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            taskExecutorRepository.SetDbName(CurrentDatabase);
            typeOfTaskRepository.SetDbName(CurrentDatabase);
        }

        public TaskExecutorController(ITaskExecutorRepository _tskExDataRep, ITypeOfTaskRepository _typeOfTask)
        {
            taskExecutorRepository = _tskExDataRep;
            typeOfTaskRepository = _typeOfTask;

            this.Disposables.Add(taskExecutorRepository);
            this.Disposables.Add(typeOfTaskRepository);
        }

        public ActionResult IndexLithoSheetAndRoll()
        {
            //deprecated
            TempData["TaskExecutorIndex"] = "IndexLithoSheetAndRoll";
            return View();
        }

        public ActionResult IndexPrePostPress()
        {
            //deprecated
            TempData["TaskExecutorIndex"] = "IndexPrePostPress";
            return View();
        }


        public ActionResult IndexFlexo()
        {
            //deprecated
            TempData["TaskExecutorIndex"] = "IndexFlexo";
            return View();
        }

        
        public ActionResult IndexSemiRoll()
        {
            //deprecated
            TempData["TaskExecutorIndex"] = "IndexSemiRoll";
            return View();
        }

        public ActionResult IndexPlotter()
        {
            //deprecated
            TempData["TaskExecutorIndex"] = "IndexPlotter";
            return View();
        }

        public ActionResult IndexDigitalSheetAndRoll()
        {
            //deprecated
            TempData["TaskExecutorIndex"] = "IndexDigitalSheetAndRoll";
            return View();
        }

        /// <summary>
        /// List of taskExecutor filtered by codTypeOfTask
        /// </summary>
        /// <param name="codTypeOfTask"></param>
        /// <returns></returns>
        public ActionResult IndexTaskExecutors(string codTypeOfTask)
        {
            return View("IndexTaskExecutors", model:codTypeOfTask);
        }

        [HttpPost]
        public ActionResult TaskEstimatedOnTime(RollEstimatedOnTime c, string returnUrl)
        {
            TempData["TaskExecutorIndex"] = returnUrl;

            if (ModelState.IsValid)
            {
                try
                {
                   // c.CostPerHourRunning = "10";
                    taskExecutorRepository.AddEstimatedOn(c);
                    taskExecutorRepository.Save();

                    this.CurrentDatabase = "castello";

                    var r=taskExecutorRepository.GetSingleEstimatedOn(c.CodTaskEstimatedOn);

                    Console.Write(r);
                    return Json(new { redirectUrl = Url.Action(returnUrl) });

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "TaskEstimatedOnTime";
            return PartialView("TaskEstimatedOnTime", c);
        }


        [HttpPost]
        public ActionResult TaskEstimatedOnRun(TaskEstimatedOnRun c, string returnUrl)
        {
            TempData["TaskExecutorIndex"] = returnUrl;

            if (ModelState.IsValid)
            {
                try
                {
                    taskExecutorRepository.AddEstimatedOn(c);
                    taskExecutorRepository.Save();

                    return Json(new { redirectUrl = Url.Action(returnUrl) });

                    //deprecated
                    //return Json(new { redirectUrl = Url.Action((string)TempData["TaskExecutorIndex"])});
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "TaskEstimatedOnRun";
            return PartialView("TaskEstimatedOnRun", c);
        }

        [HttpParamAction]
        public ActionResult DigitalOnRun(DigitalOnRun c, string returnUrl)
        {
            TempData["TaskExecutorIndex"] = returnUrl;

            if (ModelState.IsValid)
            {
                try
                {
                    taskExecutorRepository.AddEstimatedOn(c);
                    taskExecutorRepository.Save();

                    return Json(new { redirectUrl = Url.Action(returnUrl) });

                    //deprecated
                    //return Json(new { redirectUrl = Url.Action((string)TempData["TaskExecutorIndex"])});
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "DigitalOnRun";
            return PartialView("DigitalOnRun", c);
        }

        [HttpParamAction]
        public ActionResult DigitalOnTime(DigitalOnTime c, string returnUrl)
        {
            TempData["TaskExecutorIndex"] = returnUrl;

            if (ModelState.IsValid)
            {
                try
                {
                    taskExecutorRepository.AddEstimatedOn(c);
                    taskExecutorRepository.Save();

                    return Json(new { redirectUrl = Url.Action(returnUrl) });

                    //deprecated
                    //return Json(new { redirectUrl = Url.Action((string)TempData["TaskExecutorIndex"])});
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "DigitalOnTime";
            return PartialView("DigitalOnTime", c);
        }


        [HttpPost]
        public ActionResult TaskEstimatedOnMq(TaskEstimatedOnMq c, string returnUrl)
        {
            TempData["TaskExecutorIndex"] = returnUrl;

            if (ModelState.IsValid)
            {
                try
                {
                    taskExecutorRepository.AddEstimatedOn(c);
                    taskExecutorRepository.Save();

                    return Json(new { redirectUrl = Url.Action(returnUrl) });

                    //deprecated
                    //return Json(new { redirectUrl = Url.Action((string)TempData["TaskExecutorIndex"])});
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "TaskEstimatedOnMq";
            return PartialView("TaskEstimatedOnMq", c);
        }

        [HttpPost]
        [HttpParamAction]
        public ActionResult PlotterOnMq(PlotterOnMq c, string returnUrl)
        {
            TempData["TaskExecutorIndex"] = returnUrl;

            if (ModelState.IsValid)
            {
                try
                {
                    taskExecutorRepository.AddEstimatedOn(c);
                    taskExecutorRepository.Save();

                    return Json(new { redirectUrl = Url.Action(returnUrl) });

                    //deprecated
                    //return Json(new { redirectUrl = Url.Action((string)TempData["TaskExecutorIndex"])});
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "PlotterOnMq";
            return PartialView("PlotterOnMq", c);
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">TaskExecutors id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TaskExecutorCost(string id)
        {
            var c = new TaskExecutorNewCostViewModel();
            //load from database taskexecutor

            var taskExecutor = taskExecutorRepository.GetSingle(id);
            c.TaskExecutorName = taskExecutor.TaskExecutorName;

            switch (taskExecutor.TypeOfExecutor)
            {
                case TaskExecutor.ExecutorType.LithoSheet:
                    TempData["TaskExecutorIndex"] = "IndexLithoSheetAndRoll";
                    break;
                case TaskExecutor.ExecutorType.LithoRoll:
                    TempData["TaskExecutorIndex"] = "IndexLithoSheetAndRoll";
                    break;
                case TaskExecutor.ExecutorType.DigitalSheet:
                    TempData["TaskExecutorIndex"] = "IndexDigitalSheetAndRoll";
                    break;
                case TaskExecutor.ExecutorType.DigitalRoll:
                    TempData["TaskExecutorIndex"] = "IndexDigitalSheetAndRoll";
                    break;
                case TaskExecutor.ExecutorType.PlotterSheet:
                    TempData["TaskExecutorIndex"] = "IndexPlotter";
                    break;
                case TaskExecutor.ExecutorType.PlotterRoll:
                    TempData["TaskExecutorIndex"] = "IndexPlotter";
                    break;
                case TaskExecutor.ExecutorType.PrePostPress:
                    TempData["TaskExecutorIndex"] = "IndexPrePostPress";
                    break;
                case TaskExecutor.ExecutorType.Binding:
                    TempData["TaskExecutorIndex"] = "IndexBinding";
                    break;
                case TaskExecutor.ExecutorType.Flexo:
                    TempData["TaskExecutorIndex"] = "IndexFlexo";
                    break;
                default:
                    break;
            }

            c.CodTaskExecutor = taskExecutor.CodTaskExecutor;

            ViewBag.TypeCost = "";

            //if cost is not defined we have to redirect to choice page
            if (taskExecutor.SetTaskExecutorEstimatedOn.Count == 0)
            {
                switch (taskExecutor.TypeOfExecutor)
                {
                    case TaskExecutor.ExecutorType.LithoSheet:
                        ViewBag.TypeCost = "RunTime";
                        break;
                    case TaskExecutor.ExecutorType.LithoRoll:
                        ViewBag.TypeCost = "RunTime";
                        break;
                    case TaskExecutor.ExecutorType.DigitalSheet:
                        ViewBag.TypeCost = "RunTime";
                        break;
                    case TaskExecutor.ExecutorType.DigitalRoll:
                        ViewBag.TypeCost = "RunTime";
                        break;
                    case TaskExecutor.ExecutorType.PlotterRoll:
                        ViewBag.TypeCost = "Mq";
                        break;
                    case TaskExecutor.ExecutorType.PlotterSheet:
                        ViewBag.TypeCost = "Mq";
                        break;
                    case TaskExecutor.ExecutorType.PrePostPress:
                        ViewBag.TypeCost = "RunTimeMq";
                        break;
                    case TaskExecutor.ExecutorType.Binding:
                        ViewBag.TypeCost = "RunTime";
                        break;
                    default:
                        ViewBag.TypeCost = "RunTimeMq";
                        break;
                }

                return View("TaskExecutorNewCost", c);
            }

            GenEmptyStep(taskExecutor);

            var tskEst = taskExecutor.SetTaskExecutorEstimatedOn.First();
            switch (tskEst.TypeOfEstimatedOn)
            {
                case TaskEstimatedOn.EstimatedOnType.OnRun:
                    ViewBag.ActionMethod = "TaskEstimatedOnRun";
                    if (taskExecutor.TypeOfExecutor == TaskExecutor.ExecutorType.Flexo ||
                        taskExecutor.TypeOfExecutor == TaskExecutor.ExecutorType.DigitalRoll)
                    {
                        ViewBag.ActionMethod = "RollEstimatedOnRun";
                    }
                    break;

                case TaskEstimatedOn.EstimatedOnType.RollEstimatedOnTime:
                    ViewBag.ActionMethod = "RollEstimatedOnTime";
                    break;

                case TaskEstimatedOn.EstimatedOnType.ControlTableRollEstimatedOnTime:
                    ViewBag.ActionMethod = "ControlTableRollEstimatedOnTime";
                    break;

                case TaskEstimatedOn.EstimatedOnType.OnTime:
                    ViewBag.ActionMethod = "TaskEstimatedOnTime";
                    if (taskExecutor.TypeOfExecutor == TaskExecutor.ExecutorType.Flexo ||
                        taskExecutor.TypeOfExecutor == TaskExecutor.ExecutorType.DigitalRoll)
                    {
                        ViewBag.ActionMethod = "RollEstimatedOnTime";
                    }
                    if (taskExecutor.TypeOfExecutor == TaskExecutor.ExecutorType.ControlTableRoll)
                    {
                        ViewBag.ActionMethod = "ControlTableRollstimatedOnTime";
                    }
                    break;

              
                case TaskEstimatedOn.EstimatedOnType.OnMq:
                    ViewBag.ActionMethod = "TaskEstimatedOnMq";
                    break;
                case TaskEstimatedOn.EstimatedOnType.DigitalOnTime:
                    ViewBag.ActionMethod = "DigitalOnTime";
                    break;
                case TaskEstimatedOn.EstimatedOnType.DigitalOnRun:
                    ViewBag.ActionMethod = "DigitalOnRun";
                    break;
                case TaskEstimatedOn.EstimatedOnType.PlotterOnMq:
                    ViewBag.ActionMethod = "PlotterOnMq";
                    break;
                case TaskEstimatedOn.EstimatedOnType.BindingOnTime:
                    break;

                case TaskEstimatedOn.EstimatedOnType.BindingOnRun:
                    break;

                default:
                    break;
            }

            //if there are more than one cost return different model and different view
            if (taskExecutor.SetTaskExecutorEstimatedOn.Count > 1)
            {
                ViewBag.ActionMethod = (string)ViewBag.ActionMethod + "s";
                return View("TaskEstimatedOn", taskExecutor.SetTaskExecutorEstimatedOn.ToList());
            }
            else
            {
                return View(ViewBag.ActionMethod, tskEst);
            }
        }

        [HttpPost]
        public ActionResult TaskExecutorNewCost(TaskExecutorNewCostViewModel c, string FieldToPreventTwoSubmission)
        {

            var taskExecutor = taskExecutorRepository.GetSingle(c.CodTaskExecutor);
            TaskEstimatedOn tskEst = null;
            String retView = String.Empty;

            switch (taskExecutor.TypeOfExecutor)
            {
                case TaskExecutor.ExecutorType.LithoSheet:
                    TempData["TaskExecutorIndex"] = "IndexLithoSheetAndRoll";
                    break;
                case TaskExecutor.ExecutorType.LithoRoll:
                    TempData["TaskExecutorIndex"] = "IndexLithoSheetAndRoll";
                    break;
                case TaskExecutor.ExecutorType.DigitalSheet:
                    TempData["TaskExecutorIndex"] = "IndexDigitalSheetAndRoll";
                    break;
                case TaskExecutor.ExecutorType.DigitalRoll:
                    TempData["TaskExecutorIndex"] = "IndexDigitalSheetAndRoll";
                    break;
                case TaskExecutor.ExecutorType.PlotterSheet:
                    TempData["TaskExecutorIndex"] = "IndexPlotter";
                    break;
                case TaskExecutor.ExecutorType.PlotterRoll:
                    TempData["TaskExecutorIndex"] = "IndexPlotter";
                    break;
                case TaskExecutor.ExecutorType.PrePostPress:
                case TaskExecutor.ExecutorType.ControlTableRoll:
                    TempData["TaskExecutorIndex"] = "IndexPrePostPress";
                    break;
                case TaskExecutor.ExecutorType.Binding:
                    TempData["TaskExecutorIndex"] = "IndexBinding";
                    break;
                default:
                    break;
            }

            //if cost is just selected (to prevent two submissions)
            if (taskExecutor.SetTaskExecutorEstimatedOn.Count() != 0)
                return RedirectToAction("TaskExecutorCost", new { id = c.CodTaskExecutor });

            if (!taskExecutor.CodTypeOfTask.StartsWith("STAMPA"))
            {
                var optionTypeOfTaskList = typeOfTaskRepository.GetSingle(taskExecutor.CodTypeOfTask).OptionTypeOfTasks;

                foreach (var item in optionTypeOfTaskList.Except(optionTypeOfTaskList.Where(x => x.CodOptionTypeOfTask == taskExecutor.CodTypeOfTask + "_NO")))
                {
                    switch (c.TypeTaskExecutorEstimatedOn)
                    {
                        case TaskEstimatedOn.EstimatedOnType.OnRun:
                            if (taskExecutor.TypeOfExecutor == TaskExecutor.ExecutorType.DigitalRoll ||
                                taskExecutor.TypeOfExecutor == TaskExecutor.ExecutorType.DigitalSheet)
                            {
                                tskEst = new DigitalOnRun();
                                retView = "DigitalOnRun";
                            }
                            else
                            {
                                tskEst = new TaskEstimatedOnRun();
                                retView = "TaskEstimatedOnRun";
                            }
                            break;
                        case TaskEstimatedOn.EstimatedOnType.OnTime:

                            if (taskExecutor.TypeOfExecutor == TaskExecutor.ExecutorType.DigitalRoll ||
                                taskExecutor.TypeOfExecutor == TaskExecutor.ExecutorType.DigitalSheet)
                            {
                                tskEst = new DigitalOnTime();
                                retView = "DigitalOnTime";
                            }
                            else
                            {
                                tskEst = new TaskEstimatedOnTime();
                                retView = "TaskEstimatedOnTime";
                            }
                            break;
                        case TaskEstimatedOn.EstimatedOnType.OnMq:

                            if (taskExecutor.TypeOfExecutor == TaskExecutor.ExecutorType.PlotterRoll ||
                                taskExecutor.TypeOfExecutor == TaskExecutor.ExecutorType.PlotterSheet)
                            {
                                tskEst = new PlotterOnMq();
                                retView = "PlotterOnMq";
                            }
                            else
                            {
                                tskEst = new TaskEstimatedOnMq();
                                retView = "TaskEstimatedOnMq";
                            }
                            break;
                        case TaskEstimatedOn.EstimatedOnType.BindingOnTime:
                            break;
                        case TaskEstimatedOn.EstimatedOnType.BindingOnRun:
                            break;
                        default:
                            break;
                    }

                    tskEst.CodOptionTypeOfTask = item.CodOptionTypeOfTask;
                    taskExecutor.SetTaskExecutorEstimatedOn.Add(tskEst);

                }
            }
            else
            {
                switch (c.TypeTaskExecutorEstimatedOn)
                {
                    case TaskEstimatedOn.EstimatedOnType.OnRun:
                        if (taskExecutor.TypeOfExecutor == TaskExecutor.ExecutorType.DigitalRoll ||
                            taskExecutor.TypeOfExecutor == TaskExecutor.ExecutorType.DigitalSheet)
                        {
                            tskEst = new DigitalOnRun();
                            retView = "DigitalOnRun";
                        }
                        else
                        {
                            tskEst = new TaskEstimatedOnRun();
                            retView = "TaskEstimatedOnRun";
                        }
                        break;
                    case TaskEstimatedOn.EstimatedOnType.OnTime:

                        if (taskExecutor.TypeOfExecutor == TaskExecutor.ExecutorType.DigitalRoll ||
                            taskExecutor.TypeOfExecutor == TaskExecutor.ExecutorType.DigitalSheet)
                        {
                            tskEst = new DigitalOnTime();
                            retView = "DigitalOnTime";
                        }
                        else
                        {
                            if (taskExecutor.TypeOfExecutor == TaskExecutor.ExecutorType.Flexo ||
                                taskExecutor.TypeOfExecutor == TaskExecutor.ExecutorType.DigitalRoll)
                            {
                                tskEst = new RollEstimatedOnTime();
                                retView = "RollEstimatedOnTime";
                            }
                            else
                                if (taskExecutor.TypeOfExecutor == TaskExecutor.ExecutorType.ControlTableRoll)
                                {
                                    tskEst = new ControlTableRollEstimatedOnTime();
                                    retView = "ControlTableRollEstimatedOnTime";
                                }
                                else
                                {
                                    tskEst = new TaskEstimatedOnTime();
                                    retView = "TaskEstimatedOnTime";
                                }
                        }
                        break;
                    case TaskEstimatedOn.EstimatedOnType.OnMq:

                        if (taskExecutor.TypeOfExecutor == TaskExecutor.ExecutorType.PlotterRoll ||
                            taskExecutor.TypeOfExecutor == TaskExecutor.ExecutorType.PlotterSheet)
                        {
                            tskEst = new PlotterOnMq();
                            retView = "PlotterOnMq";
                        }
                        else
                        {
                            tskEst = new TaskEstimatedOnMq();
                            retView = "TaskEstimatedOnMq";
                        }
                        break;
                    case TaskEstimatedOn.EstimatedOnType.BindingOnTime:
                        break;
                    case TaskEstimatedOn.EstimatedOnType.BindingOnRun:
                        break;
                    default:
                        break;
                }

                //Attenzione nella stampa manca l'optiontypeoftask
                taskExecutor.SetTaskExecutorEstimatedOn.Add(tskEst);

            }


            //se diverso da stampa allora devo creare un numero di costi pari alle opzioni di TypeOfTask

            taskExecutorRepository.Edit(taskExecutor);
            taskExecutorRepository.Save();

            GenEmptyStep(taskExecutor);

            ViewBag.ActionMethod = retView;

            //if there are more than one cost return different model and different view
            if (taskExecutor.SetTaskExecutorEstimatedOn.Count > 1)
            {
                ViewBag.ActionMethod = ViewBag.ActionMethod + "s";
                return View("TaskEstimatedOn", taskExecutor.SetTaskExecutorEstimatedOn.ToList());
            }
            else
            {
                return View(ViewBag.ActionMethod, tskEst);
            }

        }

        [HttpGet]
        public ActionResult GetTaskEstimatedOnTimePartialView(string id)
        {
            return PartialView("", taskExecutorRepository.GetSingleEstimatedOn(id));
        }

        [HttpGet]
        public ActionResult CreateLithoSheet()
        {

            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll().Where(y => y.CodCategoryOfTask == "STAMPA");

            //this feature is needed when in the view there are more than one input (submit button) form
            //Action Method speci
            ViewBag.ActionMethod = "CreateLithoSheet";

            var x = new LithoSheet();

            //            TODO: Elaborazione dell'array del tipo di lavorazione che può svolgere.
            x.CodTypeOfTask = "STAMPA";

            return View(x);
        }


        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateLithoSheet(LithoSheet c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //if code is empty then sistem has to assign one
                    //                    if (c.Article.CodArticle == null)
                    {
                        c.CodTaskExecutor = taskExecutorRepository.GetNewCode(c);

                        /*-----------------------------------
                          c.LithoSheetCuttedCost.CodArticle = c.Article.CodArticle;
                          c.LithoSheetCuttedCost.CodArticleCost = c.Article.CodArticle + "_CTC";
                          c.LithoSheetPakedCost.CodArticle = c.Article.CodArticle;
                          c.LithoSheetPakedCost.CodArticleCost = c.Article.CodArticle + "_PKC";
                          c.LithoSheetPalletCost.CodArticle = c.Article.CodArticle;
                          c.LithoSheetPalletCost.CodArticleCost = c.Article.CodArticle + "_PLC";
                        /*/
                    }

                    taskExecutorRepository.Add(c);
                    taskExecutorRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexLithoSheetAndRoll") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll().Where(y => y.CodCategoryOfTask == "STAMPA");

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreateLithoSheet";
            return PartialView("_EditAndCreateLithoSheet", c);
        }


        [HttpGet]
        public ActionResult CreateLithoRoll()
        {

            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll().Where(y => y.CodCategoryOfTask == "STAMPA");

            //this feature is needed when in the view there are more than one input (submit button) form
            //Action Method speci
            ViewBag.ActionMethod = "CreateLithoRoll";

            var x = new LithoRoll();

            //            TODO: Elaborazione dell'array del tipo di lavorazione che può svolgere.
            x.CodTypeOfTask = "STAMPA";

            return View(x);
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateLithoRoll(LithoRoll c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //if code is empty then sistem has to assign one
                    //                    if (c.Article.CodArticle == null)
                    {
                        c.CodTaskExecutor = taskExecutorRepository.GetNewCode(c);

                        /*-------------------------------------
                          c.LithoSheetCuttedCost.CodArticle = c.Article.CodArticle;
                          c.LithoSheetCuttedCost.CodArticleCost = c.Article.CodArticle + "_CTC";
                          c.LithoSheetPakedCost.CodArticle = c.Article.CodArticle;
                          c.LithoSheetPakedCost.CodArticleCost = c.Article.CodArticle + "_PKC";
                          c.LithoSheetPalletCost.CodArticle = c.Article.CodArticle;
                          c.LithoSheetPalletCost.CodArticleCost = c.Article.CodArticle + "_PLC";
                        /*/
                    }

                    taskExecutorRepository.Add(c);
                    taskExecutorRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexLithoSheetAndRoll") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll().Where(y => y.CodCategoryOfTask == "STAMPA");

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreateLithoRoll";
            return PartialView("_EditAndCreateLithoRoll", c);
        }



        [HttpGet]
        public ActionResult CreateFlexo()
        {

            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll().Where(y => y.CodCategoryOfTask == "STAMPA");

            //this feature is needed when in the view there are more than one input (submit button) form
            //Action Method speci
            ViewBag.ActionMethod = "CreateFlexo";

            var x = new Flexo();

            x.FormatMin = "0x0";
            x.FormatMax = "0x0";

            //            TODO: Elaborazione dell'array del tipo di lavorazione che può svolgere.
            x.CodTypeOfTask = "STAMPA";

            return View(x);
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateFlexo(Flexo c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //if code is empty then sistem has to assign one
                    //                    if (c.Article.CodArticle == null)
                    {
                        c.CodTaskExecutor = taskExecutorRepository.GetNewCode(c);

                        /*-------------------------------------
                          c.LithoSheetCuttedCost.CodArticle = c.Article.CodArticle;
                          c.LithoSheetCuttedCost.CodArticleCost = c.Article.CodArticle + "_CTC";
                          c.LithoSheetPakedCost.CodArticle = c.Article.CodArticle;
                          c.LithoSheetPakedCost.CodArticleCost = c.Article.CodArticle + "_PKC";
                          c.LithoSheetPalletCost.CodArticle = c.Article.CodArticle;
                          c.LithoSheetPalletCost.CodArticleCost = c.Article.CodArticle + "_PLC";
                        /*/
                    }

                    taskExecutorRepository.Add(c);
                    taskExecutorRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexFlexo") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll().Where(y => y.CodCategoryOfTask == "STAMPA");

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreateFlexo";
            return PartialView("_EditAndCreateFlexo", c);
        }


        [HttpGet]
        public ActionResult CreateControlTableRoll()
        {

            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll().Where(y => y.CodCategoryOfTask == "STAMPA");

            //this feature is needed when in the view there are more than one input (submit button) form
            //Action Method speci
            ViewBag.ActionMethod = "CreateControlTableRoll";

            var x = new ControlTableRoll();

            x.FormatMin = "0x0";
            x.FormatMax = "0x0";

            //            TODO: Elaborazione dell'array del tipo di lavorazione che può svolgere.
            x.CodTypeOfTask = "STAMPA";

            return View(x);
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateControlTableRoll(ControlTableRoll c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //if code is empty then sistem has to assign one
                    //                    if (c.Article.CodArticle == null)
                    {
                        c.CodTaskExecutor = taskExecutorRepository.GetNewCode(c);

                        /*-------------------------------------
                          c.LithoSheetCuttedCost.CodArticle = c.Article.CodArticle;
                          c.LithoSheetCuttedCost.CodArticleCost = c.Article.CodArticle + "_CTC";
                          c.LithoSheetPakedCost.CodArticle = c.Article.CodArticle;
                          c.LithoSheetPakedCost.CodArticleCost = c.Article.CodArticle + "_PKC";
                          c.LithoSheetPalletCost.CodArticle = c.Article.CodArticle;
                          c.LithoSheetPalletCost.CodArticleCost = c.Article.CodArticle + "_PLC";
                        /*/
                    }

                    taskExecutorRepository.Add(c);
                    taskExecutorRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexPrePostPress") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll().Where(y => y.CodCategoryOfTask == "STAMPA");

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreateControlTableRoll";
            return PartialView("_EditAndCreateControlTableRoll", c);
        }


        [HttpGet]
        public ActionResult CreateSemiRoll()
        {

            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll().Where(y => y.CodCategoryOfTask == "STAMPA");

            //this feature is needed when in the view there are more than one input (submit button) form
            //Action Method speci
            ViewBag.ActionMethod = "CreateSemiRoll";

            var x = new SemiRoll();

            x.FormatMin = "0x0";
            x.FormatMax = "0x0";

            //            TODO: Elaborazione dell'array del tipo di lavorazione che può svolgere.
            x.CodTypeOfTask = "STAMPA";

            return View(x);
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateSemiRoll(SemiRoll c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //if code is empty then sistem has to assign one
                    //                    if (c.Article.CodArticle == null)
                    {
                        c.CodTaskExecutor = taskExecutorRepository.GetNewCode(c);

                        /*-------------------------------------
                          c.LithoSheetCuttedCost.CodArticle = c.Article.CodArticle;
                          c.LithoSheetCuttedCost.CodArticleCost = c.Article.CodArticle + "_CTC";
                          c.LithoSheetPakedCost.CodArticle = c.Article.CodArticle;
                          c.LithoSheetPakedCost.CodArticleCost = c.Article.CodArticle + "_PKC";
                          c.LithoSheetPalletCost.CodArticle = c.Article.CodArticle;
                          c.LithoSheetPalletCost.CodArticleCost = c.Article.CodArticle + "_PLC";
                        /*/
                    }

                    taskExecutorRepository.Add(c);
                    taskExecutorRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexSemiRoll") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll().Where(y => y.CodCategoryOfTask == "STAMPA");

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreateSemiRoll";
            return PartialView("_EditAndCreateSemiRoll", c);
        }

        [HttpGet]
        public ActionResult CreatePlotterRoll()
        {
            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll().Where(y => y.CodCategoryOfTask == "STAMPA");

            //this feature is needed when in the view there are more than one input (submit button) form
            //Action Method speci
            ViewBag.ActionMethod = "CreatePlotterRoll";
            var x = new PlotterRoll();

            x.FormatMax = "0x0";
            x.FormatMin = "0x0";
            x.CodTypeOfTask = "STAMPAMORBIDO";
            return View(x);
        }

        [HttpGet]
        public ActionResult CreatePlotterSheet()
        {
            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll().Where(y => y.CodCategoryOfTask == "STAMPARIGIDO");

            //this feature is needed when in the view there are more than one input (submit button) form
            //Action Method speci
            ViewBag.ActionMethod = "CreatePlotterSheet";
            var x = new PlotterSheet();

            x.FormatMax = "0x0";
            x.CodTypeOfTask = "STAMPARIGIDO";
            return View(x);
        }

        [HttpGet]
        public ActionResult CreateDigitalSheet()
        {

            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll().Where(y => y.CodCategoryOfTask == "STAMPA");
            var x = new DigitalSheet();
            x.CodTypeOfTask = "STAMPA";

            //this feature is needed when in the view there are more than one input (submit button) form
            //Action Method speci
            ViewBag.ActionMethod = "CreateDigitalSheet";
            return View(x);
        }


        [HttpGet]

        public ActionResult CreateDigitalRoll()
        {

            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll().Where(y => y.CodCategoryOfTask == "STAMPA");
            var x = new DigitalRoll();
            x.CodTypeOfTask = "STAMPA";

            //this feature is needed when in the view there are more than one input (submit button) form
            //Action Method speci
            ViewBag.ActionMethod = "CreateDigitalRoll";
            return View(x);
        }


        [HttpParamAction]

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreatePlotterRoll(PlotterRoll c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //if code is empty then sistem has to assign one
                    //                    if (c.Article.CodArticle == null)
                    {
                        c.CodTaskExecutor = taskExecutorRepository.GetNewCode(c);
                    }

                    c.TimeStampTable = DateTime.Now;
                    taskExecutorRepository.Add(c);
                    taskExecutorRepository.Save();
                    //hooray it passed - go back to index
                    return Json(new { redirectUrl = Url.Action("IndexPlotter") });

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll().Where(y => y.CodCategoryOfTask == "STAMPA");

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreatePlotterRoll";
            return PartialView("_EditAndCreatePlotterRoll", c);
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]

        public ActionResult CreatePlotterSheet(PlotterSheet c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //if code is empty then sistem has to assign one
                    //                    if (c.Article.CodArticle == null)
                    {
                        c.CodTaskExecutor = taskExecutorRepository.GetNewCode(c);
                    }

                    c.TimeStampTable = DateTime.Now;
                    taskExecutorRepository.Add(c);
                    taskExecutorRepository.Save();
                    //hooray it passed - go back to index
                    return Json(new { redirectUrl = Url.Action("IndexPlotter") });

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll().Where(y => y.CodCategoryOfTask == "STAMPA");

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreatePlotterSheet";
            return PartialView("_EditAndCreatePlotterSheet", c);
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]

        public ActionResult CreateDigitalSheet(DigitalSheet c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //if code is empty then sistem has to assign one
                    //                    if (c.Article.CodArticle == null)
                    {
                        c.CodTaskExecutor = taskExecutorRepository.GetNewCode(c);
                    }

                    c.TimeStampTable = DateTime.Now;
                    taskExecutorRepository.Add(c);
                    taskExecutorRepository.Save();
                    //hooray it passed - go back to index
                    return Json(new { redirectUrl = Url.Action("IndexDigitalSheetAndRoll") });

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll();


            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreateDigitalSheet";
            return PartialView("_EditAndCreateDigitalSheet", c);
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]

        public ActionResult CreateDigitalRoll(DigitalRoll c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //if code is empty then sistem has to assign one
                    //                    if (c.Article.CodArticle == null)
                    {
                        c.CodTaskExecutor = taskExecutorRepository.GetNewCode(c);
                    }

                    c.TimeStampTable = DateTime.Now;
                    taskExecutorRepository.Add(c);
                    taskExecutorRepository.Save();
                    //hooray it passed - go back to index
                    return Json(new { redirectUrl = Url.Action("IndexDigitalSheetAndRoll") });

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll();


            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreateDigitalRoll";
            return PartialView("_EditAndCreateDigitalRoll", c);
        }



        [HttpGet]

        public ActionResult CreatePrePostPress()
        {
            //this feature is needed when in the view there are more than one input (submit button) form
            //Action Method speci

            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll().Where(y => y.CodCategoryOfTask == "PREPOST");


            ViewBag.ActionMethod = "CreatePrePostPress";
            return View(new PrePostPress());
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]

        public ActionResult CreatePrePostPress(PrePostPress c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //if code is empty then sistem has to assign one
                    //                    if (c.Article.CodArticle == null)
                    {
                        c.CodTaskExecutor = taskExecutorRepository.GetNewCode(c);
                    }

                    c.TimeStampTable = DateTime.Now;
                    taskExecutorRepository.Add(c);
                    taskExecutorRepository.Save();
                    //hooray it passed - go back to index
                    return Json(new { redirectUrl = Url.Action("IndexPrePostPress") });

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll().Where(y => y.CodCategoryOfTask == "PREPOST");


            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreatePrePostPress";
            return PartialView("_EditAndCreatePrePostPress", c);
        }


        #region Edit

        //
        // GET: /Article/Edit/5
        public ActionResult Edit(string id)
        {

            //base on type we have to call right method

            //load taskexecutor
            var task = taskExecutorRepository.GetSingle(id);
            ActionResult ret = null;

            //check type

            switch (task.TypeOfExecutor)
            {
                case TaskExecutor.ExecutorType.LithoSheet:
                    {
                        ret = RedirectToAction("EditLithoSheet", "TaskExecutor", new { id = id });
                        break;
                    }

                case TaskExecutor.ExecutorType.LithoRoll:
                    {
                        ret = RedirectToAction("EditLithoRoll", "TaskExecutor", new { id = id });
                        break;
                    }

                case TaskExecutor.ExecutorType.DigitalSheet:
                    {
                        ret = RedirectToAction("EditDigitalSheet", "TaskExecutor", new { id = id });
                        break;
                    }

                case TaskExecutor.ExecutorType.DigitalRoll:
                    {
                        ret = RedirectToAction("EditDigitalRoll", "TaskExecutor", new { id = id });
                        break;
                    }

                case TaskExecutor.ExecutorType.PrePostPress:
                    {
                        ret = RedirectToAction("EditPrePostPress", "TaskExecutor", new { id = id });
                        break;
                    }
                case TaskExecutor.ExecutorType.PlotterSheet:
                    {
                        ret = RedirectToAction("EditPlotterSheet", "TaskExecutor", new { id = id });
                        break;
                    }
                case TaskExecutor.ExecutorType.PlotterRoll:
                    {
                        ret = RedirectToAction("EditPlotterRoll", "TaskExecutor", new { id = id });
                        break;
                    }

                case TaskExecutor.ExecutorType.Flexo:
                    {
                        ret = RedirectToAction("EditFlexo", "TaskExecutor", new { id = id });
                        break;
                    }
                /* continuing....................*/

            }

            return ret;
        }

        [HttpGet]
        public ActionResult EditLithoSheet(string id)
        {
            LithoSheet tskEx = new LithoSheet();
            tskEx = (LithoSheet)taskExecutorRepository.GetSingle(id);

            //get producer and maker

            if (tskEx == null)
                return HttpNotFound();

            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll().Where(y => y.CodCategoryOfTask == "STAMPA");

            //this is a common point where edit function is called
            ViewBag.ActionMethod = "EditLithoSheet";
            return View("EditLithoSheet", tskEx);
        }

        [HttpGet]
        public ActionResult EditControlTableRoll(string id)
        {
            //            ControlTableRoll tskEx = new ControlTableRoll();
            var tskEx = taskExecutorRepository.GetSingle(id);

            //get producer and maker

            if (tskEx == null)
                return HttpNotFound();

            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll().Where(y => y.CodCategoryOfTask == "STAMPA");

            //this is a common point where edit function is called
            ViewBag.ActionMethod = "EditControlTableRoll";
            return View(tskEx);
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditControlTableRoll(ControlTableRoll c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    taskExecutorRepository.Edit(c);
                    taskExecutorRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexPrePostPress") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //If we come here, something went wrong. Return it back.      


            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll().Where(y => y.CodCategoryOfTask == "STAMPA");

            ViewBag.ActionMethod = "EditControlTableRoll";
            return PartialView("_EditAndCreateControlTableRoll", c);
        }



        [HttpGet]
        public ActionResult EditFlexo(string id)
        {
            //            Flexo tskEx = new Flexo();
            var tskEx = taskExecutorRepository.GetSingle(id);

            //get producer and maker

            if (tskEx == null)
                return HttpNotFound();

            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll().Where(y => y.CodCategoryOfTask == "STAMPA");

            //this is a common point where edit function is called
            ViewBag.ActionMethod = "EditFlexo";
            return View(tskEx);
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditFlexo(Flexo c)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    //controllare le lastre!!!!!!!!!!!!!!!!!!!!! se è sono articoli validi come per i fornitori di seguito
                    /*
                    CustomerSupplier[] customerSuppliers = customerSupplierRepository.GetAll().ToArray();

                    var filteredItems = customerSuppliers.Where(
                        item => !(String.IsNullOrEmpty(item.BusinessName)) && item.BusinessName.IndexOf(c.SupplierMaker, StringComparison.InvariantCultureIgnoreCase) >= 0);

                    if (filteredItems.Count() == 0) throw new Exception();

                    c.Article.CodSupplierMaker = filteredItems.Single().CodCustomerSupplier;

                     */

                    var flexoEx = taskExecutorRepository.GetSingle(c.CodTaskExecutor);

                    var maxCyl = flexoEx.TaskExecutorCylinders.Max(x => x.Z);
                    var minCyl = flexoEx.TaskExecutorCylinders.Where(y => y.Z != 0).Min(x => x.Z);

                    c.FormatMax = c.FlexoWidth + "x" + ((double)maxCyl / 8) * 2.54;
                    c.FormatMin = 0 + "x" + ((double)minCyl / 8) * 2.54;

                    taskExecutorRepository.Edit(c);
                    taskExecutorRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexFlexo") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //If we come here, something went wrong. Return it back.      


            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll().Where(y => y.CodCategoryOfTask == "STAMPA");

            ViewBag.ActionMethod = "EditFlexo";
            return PartialView("_EditAndCreateFlexo", c);
        }


        [HttpGet]
        public ActionResult EditSemiRoll(string id)
        {
            //            SemiRoll tskEx = new SemiRoll();
            var tskEx = taskExecutorRepository.GetSingle(id);

            //get producer and maker

            if (tskEx == null)
                return HttpNotFound();

            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll().Where(y => y.CodCategoryOfTask == "STAMPA");

            //this is a common point where edit function is called
            ViewBag.ActionMethod = "EditSemiRoll";
            return View(tskEx);
        }



        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSemiRoll(SemiRoll c)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    //controllare le lastre!!!!!!!!!!!!!!!!!!!!! se è sono articoli validi come per i fornitori di seguito
                    /*
                    CustomerSupplier[] customerSuppliers = customerSupplierRepository.GetAll().ToArray();

                    var filteredItems = customerSuppliers.Where(
                        item => !(String.IsNullOrEmpty(item.BusinessName)) && item.BusinessName.IndexOf(c.SupplierMaker, StringComparison.InvariantCultureIgnoreCase) >= 0);

                    if (filteredItems.Count() == 0) throw new Exception();

                    c.Article.CodSupplierMaker = filteredItems.Single().CodCustomerSupplier;

                     */

                    var flexoEx = taskExecutorRepository.GetSingle(c.CodTaskExecutor);

                    var maxCyl = flexoEx.TaskExecutorCylinders.Max(x => x.Z);
                    var minCyl = flexoEx.TaskExecutorCylinders.Where(y => y.Z != 0).Min(x => x.Z);

                    c.FormatMax = c.SemiRollWidth + "x" + ((double)maxCyl / 8) * 2.54;
                    c.FormatMin = 0 + "x" + ((double)minCyl / 8) * 2.54;

                    taskExecutorRepository.Edit(c);
                    taskExecutorRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexSemiRoll") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //If we come here, something went wrong. Return it back.      


            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll().Where(y => y.CodCategoryOfTask == "STAMPA");

            ViewBag.ActionMethod = "EditSemiRoll";
            return PartialView("_EditAndCreateSemiRoll", c);
        }

        public ActionResult EditLithoRoll(string id)
        {
            LithoRoll tskEx = new LithoRoll();
            tskEx = (LithoRoll)taskExecutorRepository.GetSingle(id);

            //get producer and maker

            if (tskEx == null)
                return HttpNotFound();

            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll().Where(y => y.CodCategoryOfTask == "STAMPA");

            //this is a common point where edit function is called
            ViewBag.ActionMethod = "EditLithoRoll";
            return View(tskEx);
        }


        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditLithoRoll(LithoRoll c)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    //controllare le lastre!!!!!!!!!!!!!!!!!!!!! se è sono articoli validi come per i fornitori di seguito
                    /*
                    CustomerSupplier[] customerSuppliers = customerSupplierRepository.GetAll().ToArray();

                    var filteredItems = customerSuppliers.Where(
                        item => !(String.IsNullOrEmpty(item.BusinessName)) && item.BusinessName.IndexOf(c.SupplierMaker, StringComparison.InvariantCultureIgnoreCase) >= 0);

                    if (filteredItems.Count() == 0) throw new Exception();

                    c.Article.CodSupplierMaker = filteredItems.Single().CodCustomerSupplier;


                     */

                    taskExecutorRepository.Edit(c);
                    taskExecutorRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexLithoSheetAndRoll") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //If we come here, something went wrong. Return it back.      

            ViewBag.ActionMethod = "EditLithoRoll";
            return PartialView("_EditAndCreateLithoRoll", c);
        }


        public ActionResult EditPlotterSheet(string id)
        {
            PlotterSheet tskEx = new PlotterSheet();
            tskEx = (PlotterSheet)taskExecutorRepository.GetSingle(id);

            if (tskEx == null)
                return HttpNotFound();

            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll().Where(y => y.CodCategoryOfTask == "STAMPA");

            //is used to know where we are from and go
            ViewBag.ActionMethod = "EditPlotterSheet";
            return View(tskEx);
        }



        public ActionResult EditPlotterRoll(string id)
        {
            PlotterRoll tskEx = new PlotterRoll();
            tskEx = (PlotterRoll)taskExecutorRepository.GetSingle(id);

            if (tskEx == null)
                return HttpNotFound();

            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll().Where(y => y.CodCategoryOfTask == "STAMPA");

            //is used to know where we are from and go
            ViewBag.ActionMethod = "EditPlotterRoll";
            return View(tskEx);
        }


        public ActionResult EditDigitalSheet(string id)
        {
            DigitalSheet tskEx = new DigitalSheet();
            tskEx = (DigitalSheet)taskExecutorRepository.GetSingle(id);

            if (tskEx == null)
                return HttpNotFound();

            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll().Where(y => y.CodCategoryOfTask == "STAMPA");

            //is used to know where we are from and go
            ViewBag.ActionMethod = "EditDigitalSheet";
            return View(tskEx);
        }


        public ActionResult EditDigitalRoll(string id)
        {
            DigitalRoll tskEx = new DigitalRoll();
            tskEx = (DigitalRoll)taskExecutorRepository.GetSingle(id);

            if (tskEx == null)
                return HttpNotFound();

            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll().Where(y => y.CodCategoryOfTask == "STAMPA");

            //is used to know where we are from and go
            ViewBag.ActionMethod = "EditDigitalRoll";
            return View(tskEx);
        }


        public ActionResult EditPrePostPress(string id)
        {
            PrePostPress tskEx = new PrePostPress();
            tskEx = (PrePostPress)taskExecutorRepository.GetSingle(id);

            if (tskEx == null)
                return HttpNotFound();

            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll();


            //is used to know where we are from and go
            ViewBag.ActionMethod = "EditPrePostPress";
            return View(tskEx);
        }
        #endregion

        //
        // POST: /Article/Edit/5
        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]

        public ActionResult EditLithoSheet(LithoSheet c)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    //controllare le lastre!!!!!!!!!!!!!!!!!!!!! se è sono articoli validi come per i fornitori di seguito
                    /*
                    CustomerSupplier[] customerSuppliers = customerSupplierRepository.GetAll().ToArray();

                    var filteredItems = customerSuppliers.Where(
                        item => !(String.IsNullOrEmpty(item.BusinessName)) && item.BusinessName.IndexOf(c.SupplierMaker, StringComparison.InvariantCultureIgnoreCase) >= 0);

                    if (filteredItems.Count() == 0) throw new Exception();

                    c.Article.CodSupplierMaker = filteredItems.Single().CodCustomerSupplier;


                     */

                    taskExecutorRepository.Edit(c);
                    taskExecutorRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexLithoSheetAndRoll") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //If we come here, something went wrong. Return it back.      

            ViewBag.ActionMethod = "EditLithoSheet";
            return PartialView("_EditAndCreateLithoSheet", c);
        }



        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]

        public ActionResult EditPlotter(Plotter c)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    taskExecutorRepository.Edit(c);
                    taskExecutorRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexPlotter") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //If we come here, something went wrong. Return it back. 

            //multi submit
            ViewBag.ActionMethod = "EditPlotter";
            return PartialView("_EditAndCreatePlotterRoll", c);
        }



        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]

        public ActionResult EditPlotterRoll(PlotterRoll c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    taskExecutorRepository.Edit(c);
                    taskExecutorRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexPlotter") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //If we come here, something went wrong. Return it back. 

            //multi submit
            ViewBag.ActionMethod = "EditPlotterRoll";
            return PartialView("_EditAndCreatePlotterRoll", c);
        }




        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]

        public ActionResult EditPlotterSheet(PlotterSheet c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    taskExecutorRepository.Edit(c);
                    taskExecutorRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexPlotter") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //If we come here, something went wrong. Return it back. 

            //multi submit
            ViewBag.ActionMethod = "EditPlotterSheet";
            return PartialView("_EditAndCreatePlotterSheet", c);
        }


        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]

        public ActionResult EditDigitalSheet(DigitalSheet c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    /* controllare le lastre
                    CustomerSupplier[] customerSuppliers = customerSupplierRepository.GetAll().ToArray();

                    var filteredItems = customerSuppliers.Where(
                        item => !(String.IsNullOrEmpty(item.BusinessName)) && item.BusinessName.IndexOf(c.SupplierMaker, StringComparison.InvariantCultureIgnoreCase) >= 0);

                    if (filteredItems.Count() == 0) throw new Exception();

                    c.Article.CodSupplierMaker = filteredItems.Single().CodCustomerSupplier;

                    */

                    taskExecutorRepository.Edit(c);
                    taskExecutorRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexDigitalSheetAndRoll") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //If we come here, something went wrong. Return it back. 

            //multi submit
            ViewBag.ActionMethod = "EditDigitalSheet";
            return PartialView("_EditAndCreateDigitalSheet", c);
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]

        public ActionResult EditDigitalRoll(DigitalRoll c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    /* controllare le lastre
                    CustomerSupplier[] customerSuppliers = customerSupplierRepository.GetAll().ToArray();

                    var filteredItems = customerSuppliers.Where(
                        item => !(String.IsNullOrEmpty(item.BusinessName)) && item.BusinessName.IndexOf(c.SupplierMaker, StringComparison.InvariantCultureIgnoreCase) >= 0);

                    if (filteredItems.Count() == 0) throw new Exception();

                    c.Article.CodSupplierMaker = filteredItems.Single().CodCustomerSupplier;

                    */

                    taskExecutorRepository.Edit(c);
                    taskExecutorRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexDigitalSheetAndRoll") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //If we come here, something went wrong. Return it back. 

            //multi submit
            ViewBag.ActionMethod = "EditDigitalRoll";
            return PartialView("_EditAndCreateDigitalRoll", c);
        }


        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]

        public ActionResult EditPrePostPress(PrePostPress c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    /* controllare le lastre
                    CustomerSupplier[] customerSuppliers = customerSupplierRepository.GetAll().ToArray();

                    var filteredItems = customerSuppliers.Where(
                        item => !(String.IsNullOrEmpty(item.BusinessName)) && item.BusinessName.IndexOf(c.SupplierMaker, StringComparison.InvariantCultureIgnoreCase) >= 0);

                    if (filteredItems.Count() == 0) throw new Exception();

                    c.Article.CodSupplierMaker = filteredItems.Single().CodCustomerSupplier;

                    */

                    taskExecutorRepository.Edit(c);
                    taskExecutorRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexPrePostPress") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }



            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll();

            //If we come here, something went wrong. Return it back. 

            //multi submit
            ViewBag.ActionMethod = "EditPrePostPress";
            return PartialView("_EditAndCreatePrePostPress", c);
        }


        //
        // GET: /Article/Delete/5

        public ActionResult Delete(string id)
        {
            return View();
        }

        [HttpPost]
        public void EditTaskExecutorCylinder(string codTaskExecutorZ, TaskExecutorCylinder c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    {
                        var taskExecutor = taskExecutorRepository.GetSingle(codTaskExecutorZ);

                        //questo è il nuovo codice... e dovrebbe essere ugual a quello contenuto in CodTaskExecutorCylinder
                        var newCode = codTaskExecutorZ + "-" + c.Z.ToString();

                        //se è diverso significa che è stato cambiato lo zeta!!! oppure si sta modificando uno zeta zero
                        if (c.CodTaskExecutorCylinder != newCode && c.CodTaskExecutorCylinder != null)
                        {
                            //cerco il codice vecchio
                            var cyl = taskExecutor.TaskExecutorCylinders.FirstOrDefault(x => x.CodTaskExecutorCylinder == c.CodTaskExecutorCylinder);
                            //devo cancellare nel db il cylindro con il nuovo e aggiungerne un altro
                            if (cyl != null)
                            {
                                taskExecutorRepository.DeleteSingleCylinder(cyl);
                                taskExecutorRepository.Save();
                            }
                        }

                        //poi inizio a ricomporre il nuovo codice
                        c.CodTaskExecutorCylinder = newCode;

                        //significa che è già salvato
                        //     cerco il codgiusto 
                        var cylNew = taskExecutor.TaskExecutorCylinders.FirstOrDefault(x => x.CodTaskExecutorCylinder == c.CodTaskExecutorCylinder);

                        c.TimeStampTable = DateTime.Now;
                        c.CodTaskExecutor = codTaskExecutorZ;

                        if (cylNew != null)
                        {
                            cylNew.Z = c.Z;
                            cylNew.Quantity = c.Quantity;

                            //new line!!!
                            taskExecutorRepository.EditSingleCylinder(cylNew);
                            taskExecutorRepository.Save();
                        }
                        else
                        {
                            c.CodTaskExecutor = codTaskExecutorZ;
                            c.CodTaskExecutorCylinder = newCode;
                            taskExecutor.TaskExecutorCylinders.Add(c);
                            taskExecutorRepository.AddSingleCylinder(c);
                            taskExecutorRepository.Save();
                        }
                    }
                }

                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "EditAvarageRunPerRunStep";
        }

        [HttpPost]
        public void EditAvarageRunPerRunStep(string codTaskExecutorOn, AvarageRunPerRunStep c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    {
                        var taskEstimatedOn = taskExecutorRepository.GetSingleEstimatedOn(codTaskExecutorOn);
                        var taskExecutor = taskExecutorRepository.GetSingle(taskEstimatedOn.CodTaskExecutor);

                        //look for to=0 from=0
                        var chkStep = taskEstimatedOn.steps.OfType<AvarageRunPerRunStep>().FirstOrDefault(x => x.IdStep == c.IdStep);

                        c.TimeStampTable = DateTime.Now;

                        if (chkStep != null)
                        {
                            chkStep.FromUnit = c.FromUnit;
                            chkStep.ToUnit = c.ToUnit;
                            chkStep.AvarageRunPerHour = c.AvarageRunPerHour;

                            //new line!!!
                            taskExecutorRepository.EditSingleStep(chkStep);
                            taskExecutorRepository.Save();
                        }
                        else
                        {
                            taskEstimatedOn.steps.Add(c);
                        }

                        GenEmptyStep(taskExecutor);

                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "EditAvarageRunPerRunStep";
            //            return View("EditAvarageRunPerRunStep", c);
        }

        [HttpPost]
        public void EditDeficitForWeightStep(string codTaskExecutorOn, DeficitForWeightStep c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    {
                        var taskEstimatedOn = taskExecutorRepository.GetSingleEstimatedOn(codTaskExecutorOn);
                        var taskExecutor = taskExecutorRepository.GetSingle(taskEstimatedOn.CodTaskExecutor);

                        //look for to=0 from=0
                        var chkStep = taskEstimatedOn.steps.OfType<DeficitForWeightStep>().FirstOrDefault(x => x.IdStep == c.IdStep);

                        c.TimeStampTable = DateTime.Now;

                        if (chkStep != null)
                        {
                            chkStep.FromUnit = c.FromUnit;
                            chkStep.ToUnit = c.ToUnit;
                            chkStep.DeficitRate = c.DeficitRate;
                            //new line!!!
                            taskExecutorRepository.EditSingleStep(chkStep);
                            taskExecutorRepository.Save();

                        }
                        else
                        {
                            taskEstimatedOn.steps.Add(c);
                        }

                        GenEmptyStep(taskExecutor);

                        taskExecutorRepository.Edit(taskExecutor);
                        taskExecutorRepository.Save();

                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "EditDeficitForWeightStep";
            //            return View("EditDeficitForWeightStep", c);
        }

        [HttpPost]
        public void EditDeficitOnCostForWeightStep(string codTaskExecutorOn, DeficitOnCostForWeightStep c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    {
                        var taskEstimatedOn = taskExecutorRepository.GetSingleEstimatedOn(codTaskExecutorOn);
                        var taskExecutor = taskExecutorRepository.GetSingle(taskEstimatedOn.CodTaskExecutor);

                        //look for to=0 from=0
                        var chkStep = taskEstimatedOn.steps.OfType<DeficitOnCostForWeightStep>().FirstOrDefault(x => x.IdStep == c.IdStep);

                        c.TimeStampTable = DateTime.Now;

                        if (chkStep != null)
                        {
                            chkStep.FromUnit = c.FromUnit;
                            chkStep.ToUnit = c.ToUnit;
                            chkStep.DeficitRate = c.DeficitRate;
                            //new line!!!
                            taskExecutorRepository.EditSingleStep(chkStep);
                            taskExecutorRepository.Save();

                        }
                        else
                        {
                            taskEstimatedOn.steps.Add(c);
                        }

                        GenEmptyStep(taskExecutor);

                        taskExecutorRepository.Edit(taskExecutor);
                        taskExecutorRepository.Save();

                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "EditDeficitOnCostForWeightStep";
            //            return View("EditDeficitOnCostForWeightStep", c);
        }

        [HttpPost]
        public void EditCostPerMqStep(string codTaskExecutorOn, CostPerMqStep c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    {
                        var taskEstimatedOn = taskExecutorRepository.GetSingleEstimatedOn(codTaskExecutorOn);
                        var taskExecutor = taskExecutorRepository.GetSingle(taskEstimatedOn.CodTaskExecutor);

                        //look for to=0 from=0
                        var chkStep = taskEstimatedOn.steps.OfType<CostPerMqStep>().FirstOrDefault(x => x.IdStep == c.IdStep);

                        c.TimeStampTable = DateTime.Now;

                        if (chkStep != null)
                        {
                            chkStep.FromUnit = c.FromUnit;
                            chkStep.ToUnit = c.ToUnit;
                            chkStep.CostPerUnit = c.CostPerUnit;
                            //new line!!!
                            taskExecutorRepository.EditSingleStep(chkStep);
                            taskExecutorRepository.Save();

                        }
                        else
                        {
                            taskEstimatedOn.steps.Add(c);
                        }

                        GenEmptyStep(taskExecutor);

                        taskExecutorRepository.Edit(taskExecutor);
                        taskExecutorRepository.Save();

                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "EditCostPerMqStep";
            //            return View("EditCostPerMqStep", c);
        }

        [HttpPost]
        public void EditCostPerRunStep(string codTaskExecutorOn, CostPerRunStep c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    {
                        var taskEstimatedOn = taskExecutorRepository.GetSingleEstimatedOn(codTaskExecutorOn);
                        var taskExecutor = taskExecutorRepository.GetSingle(taskEstimatedOn.CodTaskExecutor);

                        //look for to=0 from=0
                        var chkStep = taskEstimatedOn.steps.OfType<CostPerRunStep>().FirstOrDefault(x => x.IdStep == c.IdStep);

                        c.TimeStampTable = DateTime.Now;

                        if (chkStep != null)
                        {
                            chkStep.FromUnit = c.FromUnit;
                            chkStep.ToUnit = c.ToUnit;
                            chkStep.CostPerUnit = c.CostPerUnit;
                            //new line!!!
                            taskExecutorRepository.EditSingleStep(chkStep);
                            taskExecutorRepository.Save();

                        }
                        else
                        {
                            taskEstimatedOn.steps.Add(c);
                        }

                        GenEmptyStep(taskExecutor);

                        taskExecutorRepository.Edit(taskExecutor);
                        taskExecutorRepository.Save();

                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "EditCostPerRunStep";
            //            return View("EditCostPerRunStep", c);
        }

        [HttpPost]
        public void EditCostPerColorStep(string codTaskExecutorOn, CostPerColorStep c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    {
                        var taskEstimatedOn = taskExecutorRepository.GetSingleEstimatedOn(codTaskExecutorOn);
                        var taskExecutor = taskExecutorRepository.GetSingle(taskEstimatedOn.CodTaskExecutor);

                        //look for to=0 from=0
                        var chkStep = taskEstimatedOn.steps.OfType<CostPerColorStep>().FirstOrDefault(x => x.IdStep == c.IdStep);

                        c.TimeStampTable = DateTime.Now;

                        if (chkStep != null)
                        {
                            chkStep.FromUnit = c.FromUnit;
                            chkStep.ToUnit = c.ToUnit;
                            chkStep.CostPerUnit = c.CostPerUnit;
                            //new line!!!
                            taskExecutorRepository.EditSingleStep(chkStep);
                            taskExecutorRepository.Save();

                        }
                        else
                        {
                            taskEstimatedOn.steps.Add(c);
                        }

                        GenEmptyStep(taskExecutor);

                        taskExecutorRepository.Edit(taskExecutor);
                        taskExecutorRepository.Save();

                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "EditCostPerColorStep";
            //            return View("EditCostPerColorStep", c);
        }

        [HttpPost]
        public void EditCostPerRunStepBW(string codTaskExecutorOn, CostPerRunStepBW c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    {
                        var taskEstimatedOn = taskExecutorRepository.GetSingleEstimatedOn(codTaskExecutorOn);
                        var taskExecutor = taskExecutorRepository.GetSingle(taskEstimatedOn.CodTaskExecutor);

                        //look for to=0 from=0
                        var chkStep = taskEstimatedOn.steps.OfType<CostPerRunStepBW>().FirstOrDefault(x => x.IdStep == c.IdStep);

                        c.TimeStampTable = DateTime.Now;

                        if (chkStep != null)
                        {
                            chkStep.FromUnit = c.FromUnit;
                            chkStep.ToUnit = c.ToUnit;
                            chkStep.CostPerUnit = c.CostPerUnit;
                            //new line!!!
                            taskExecutorRepository.EditSingleStep(chkStep);
                            taskExecutorRepository.Save();

                        }
                        else
                        {
                            taskEstimatedOn.steps.Add(c);
                        }

                        GenEmptyStep(taskExecutor);

                        taskExecutorRepository.Edit(taskExecutor);
                        taskExecutorRepository.Save();

                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "EditCostPerRunStepBW";
            //            return View("EditCostPerRunStep", c);
        }

    }
}
