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
    public partial class TaskExecutorController : PapiroMVC.Controllers.ControllerAlgolaBase
    {

        private readonly ITaskExecutorRepository taskExecutorRepository;
        private readonly ITypeOfTaskRepository typeOfTaskRepository;

        protected dbEntities db;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            taskExecutorRepository.SetDbName(CurrentDatabase);
        }

        public TaskExecutorController(ITaskExecutorRepository _tskExDataRep, ITypeOfTaskRepository _typeOfTask)
        {
            taskExecutorRepository = _tskExDataRep;
            typeOfTaskRepository = _typeOfTask;
        }

        public ActionResult IndexLithoSheet()
        {
            //deprecated
            TempData["TaskExecutorIndex"] = "IndexLithoSheet";
            return View();
        }

        public ActionResult IndexPlotter()
        {
            //deprecated
            TempData["TaskExecutorIndex"] = "IndexPlotter";
            return View();
        }

        public ActionResult IndexDigitalSheet()
        {
            //deprecated
            TempData["TaskExecutorIndex"] = "IndexDigitalSheet";
            return View();
        }

        public ActionResult IndexPrePostPress()
        {
            //deprecated
            TempData["TaskExecutorIndex"] = "IndexPrePostPress";
            return View();
        }

        [HttpPost]
        public ActionResult TaskEstimatedOnTime(TaskEstimatedOnTime c, string returnUrl)
        {
            TempData["TaskExecutorIndex"] = returnUrl;

            if (ModelState.IsValid)
            {
                try
                {
                    var r = taskExecutorRepository.GetSingleEstimatedOn(c.CodTaskExecutorOn);
                    c.Copy(r);
                    taskExecutorRepository.Edit(r.taskexecutors);
                    taskExecutorRepository.Save();

                    //                    deprecated  
                    //                    return Json(new { redirectUrl = Url.Action((string)TempData["TaskExecutorIndex"]) });
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
                    var r = taskExecutorRepository.GetSingleEstimatedOn(c.CodTaskExecutorOn);
                    c.Copy(r);
                    taskExecutorRepository.Edit(r.taskexecutors);
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

        [HttpPost]
        public ActionResult TaskEstimatedOnMq(TaskEstimatedOnMq c, string returnUrl)
        {
            TempData["TaskExecutorIndex"] = returnUrl;

            if (ModelState.IsValid)
            {
                try
                {
                    var r = taskExecutorRepository.GetSingleEstimatedOn(c.CodTaskExecutorOn);
                    c.Copy(r);
                    taskExecutorRepository.Edit(r.taskexecutors);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">TaskExecutors id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TaskExecutorCost(string id)
        {
            var c = new TaskExecutorNewCostDetail();
            //load from database taskexecutor

            var taskExecutor = taskExecutorRepository.GetSingle(id);

            switch (taskExecutor.TypeOfExecutor)
            {
                case TaskExecutor.ExecutorType.LithoSheet:
                    TempData["TaskExecutorIndex"] = "IndexLithoSheet";
                    break;
                case TaskExecutor.ExecutorType.LithoWeb:
                    TempData["TaskExecutorIndex"] = "IndexLithoWeb";
                    break;
                case TaskExecutor.ExecutorType.DigitalSheet:
                    TempData["TaskExecutorIndex"] = "IndexDigitalSheet";
                    break;
                case TaskExecutor.ExecutorType.DigitalWeb:
                    TempData["TaskExecutorIndex"] = "IndexDigitalWeb";
                    break;
                case TaskExecutor.ExecutorType.Plotter:
                    TempData["TaskExecutorIndex"] = "IndexPlotter";
                    break;
                case TaskExecutor.ExecutorType.PrePostPress:
                    TempData["TaskExecutorIndex"] = "IndexPrePostPress";
                    break;
                case TaskExecutor.ExecutorType.Binding:
                    TempData["TaskExecutorIndex"] = "IndexBinding";
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
                    case TaskExecutor.ExecutorType.LithoWeb:
                        ViewBag.TypeCost = "RunTime";
                        break;
                    case TaskExecutor.ExecutorType.DigitalSheet:
                        ViewBag.TypeCost = "RunTime";
                        break;
                    case TaskExecutor.ExecutorType.DigitalWeb:
                        ViewBag.TypeCost = "RunTime";
                        break;
                    case TaskExecutor.ExecutorType.Plotter:
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
                    break;
                case TaskEstimatedOn.EstimatedOnType.OnTime:
                    ViewBag.ActionMethod = "TaskEstimatedOnTime";
                    break;
                case TaskEstimatedOn.EstimatedOnType.OnMq:
                    ViewBag.ActionMethod = "TaskEstimatedOnMq";
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
        public ActionResult TaskExecutorNewCost(TaskExecutorNewCostDetail c, string FieldToPreventTowSubmission)
        {

            var taskExecutor = taskExecutorRepository.GetSingle(c.CodTaskExecutor);
            TaskEstimatedOn tskEst = null;
            String retView = String.Empty;

            switch (taskExecutor.TypeOfExecutor)
            {
                case TaskExecutor.ExecutorType.LithoSheet:
                    TempData["TaskExecutorIndex"] = "IndexLithoSheet";
                    break;
                case TaskExecutor.ExecutorType.LithoWeb:
                    TempData["TaskExecutorIndex"] = "IndexLithoWeb";
                    break;
                case TaskExecutor.ExecutorType.DigitalSheet:
                    TempData["TaskExecutorIndex"] = "IndexDigitalSheet";
                    break;
                case TaskExecutor.ExecutorType.DigitalWeb:
                    TempData["TaskExecutorIndex"] = "IndexDigitalWeb";
                    break;
                case TaskExecutor.ExecutorType.Plotter:
                    TempData["TaskExecutorIndex"] = "IndexPlotter";
                    break;
                case TaskExecutor.ExecutorType.PrePostPress:
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

            if (taskExecutor.CodTypeOfTask != "STAMPA")
            {
                var optionTypeOfTaskList = typeOfTaskRepository.GetSingle(taskExecutor.CodTypeOfTask).OptionTypeOfTasks;

                foreach (var item in optionTypeOfTaskList.Except(optionTypeOfTaskList.Where(x => x.CodOptionTypeOfTask == taskExecutor.CodTypeOfTask + "_NO")))
                {
                    switch (c.TypeTaskExecutorEstimatedOn)
                    {
                        case TaskEstimatedOn.EstimatedOnType.OnRun:
                            tskEst = new TaskEstimatedOnRun();
                            retView = "TaskEstimatedOnRun";
                            break;
                        case TaskEstimatedOn.EstimatedOnType.OnTime:
                            tskEst = new TaskEstimatedOnTime();
                            retView = "TaskEstimatedOnTime";
                            break;
                        case TaskEstimatedOn.EstimatedOnType.OnMq:
                            tskEst = new TaskEstimatedOnMq();
                            retView = "TaskEstimatedOnMq";
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
                        tskEst = new TaskEstimatedOnRun();
                        retView = "TaskEstimatedOnRun";
                        break;
                    case TaskEstimatedOn.EstimatedOnType.OnTime:
                        tskEst = new TaskEstimatedOnTime();
                        retView = "TaskEstimatedOnTime";
                        break;
                    case TaskEstimatedOn.EstimatedOnType.OnMq:
                        tskEst = new TaskEstimatedOnMq();
                        retView = "TaskEstimatedOnMq";
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

                        /***********************************
                          c.LithoSheetCuttedCost.CodArticle = c.Article.CodArticle;
                          c.LithoSheetCuttedCost.CodArticleCost = c.Article.CodArticle + "_CTC";
                          c.LithoSheetPakedCost.CodArticle = c.Article.CodArticle;
                          c.LithoSheetPakedCost.CodArticleCost = c.Article.CodArticle + "_PKC";
                          c.LithoSheetPalletCost.CodArticle = c.Article.CodArticle;
                          c.LithoSheetPalletCost.CodArticleCost = c.Article.CodArticle + "_PLC";
                        /************************************/
                    }

                    taskExecutorRepository.Add(c);
                    taskExecutorRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexLithoSheet") });
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
        public ActionResult CreatePlotter()
        {
            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll().Where(y => y.CodCategoryOfTask == "STAMPA");

            //this feature is needed when in the view there are more than one input (submit button) form
            //Action Method speci
            ViewBag.ActionMethod = "CreatePlotter";
            var x = new Plotter();

            x.FormatMax = "0x0";
            x.FormatMin = "0x0";
            x.CodTypeOfTask = "STAMPA";
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


        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreatePlotter(Plotter c)
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
            ViewBag.ActionMethod = "CreatePlotter";
            return PartialView("_EditAndCreatePlotter", c);
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
                    return Json(new { redirectUrl = Url.Action("IndexDigitalSheet") });

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

                case TaskExecutor.ExecutorType.DigitalSheet:
                    {
                        ret = RedirectToAction("EditDigitalSheet", "TaskExecutor", new { id = id });
                        break;
                    }

                case TaskExecutor.ExecutorType.PrePostPress:
                    {
                        ret = RedirectToAction("EditPrePostPress", "TaskExecutor", new { id = id });
                        break;
                    }
                case TaskExecutor.ExecutorType.Plotter:
                    {
                        ret = RedirectToAction("EditPlotter", "TaskExecutor", new { id = id });
                        break;
                    }

                /* continuing....................*/

            }

            return ret;
        }


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
            return View(tskEx);
        }


        public ActionResult EditPlotter(string id)
        {
            Plotter tskEx = new Plotter();
            tskEx = (Plotter)taskExecutorRepository.GetSingle(id);

            if (tskEx == null)
                return HttpNotFound();

            //Load each type of base
            ViewBag.TypeOfTaskList = typeOfTaskRepository.GetAll().Where(y => y.CodCategoryOfTask == "STAMPA");

            //is used to know where we are from and go
            ViewBag.ActionMethod = "EditPlotter";
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
                        item => item.BusinessName.IndexOf(c.SupplierMaker, StringComparison.InvariantCultureIgnoreCase) >= 0);

                    if (filteredItems.Count() == 0) throw new Exception();

                    c.Article.CodSupplierMaker = filteredItems.Single().CodCustomerSupplier;


                     */

                    taskExecutorRepository.Edit(c);
                    taskExecutorRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexLithoSheet") });
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
            return PartialView("_EditAndCreatePlotter", c);
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
                        item => item.BusinessName.IndexOf(c.SupplierMaker, StringComparison.InvariantCultureIgnoreCase) >= 0);

                    if (filteredItems.Count() == 0) throw new Exception();

                    c.Article.CodSupplierMaker = filteredItems.Single().CodCustomerSupplier;

                    */

                    taskExecutorRepository.Edit(c);
                    taskExecutorRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexDigitalSheet") });
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
        public ActionResult EditPrePostPress(PrePostPress c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    /* controllare le lastre
                    CustomerSupplier[] customerSuppliers = customerSupplierRepository.GetAll().ToArray();

                    var filteredItems = customerSuppliers.Where(
                        item => item.BusinessName.IndexOf(c.SupplierMaker, StringComparison.InvariantCultureIgnoreCase) >= 0);

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
        public void EditAvarageRunPerRunStep(string codTaskExecutorOn, AvarageRunPerRunStep c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //if code is empty then sistem has to assign one
                    //                    if (c.Article.CodArticle == null)
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

    }
}
