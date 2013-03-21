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

        protected dbEntities db;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            taskExecutorRepository.SetDbName(CurrentDatabase);
        }

        public TaskExecutorController(ITaskExecutorRepository _tskExDataRep)
        {
            taskExecutorRepository = _tskExDataRep;
        }

        public ActionResult Index()
        {
            return View ();
        }

        [HttpGet]
        public ActionResult CreateLithoSheet()
        {
            //this feature is needed when in the view there are more than one input (submit button) form
            //Action Method speci
            ViewBag.ActionMethod = "CreateLithoSheet";
            return View(new LithoSheet());
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
                                                                                        
                    c.TimeStampTable = DateTime.Now;
                    taskExecutorRepository.Add(c);
                    taskExecutorRepository.Save();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreateLithoSheet";
            return View("CreateLithoSheet", c);
        }

        #region Edit

        //
        // GET: /Article/Edit/5
        public ActionResult Edit(string id)
        {
            //this is a common point where edit function is called
            //base on type we have to call right method

            //load article
            var task = taskExecutorRepository.GetSingle(id);
            ActionResult ret = null;

            //check type
            
            switch (task.TypeOfExecutor)
            {
                case TaskExecutor.ExecutorType.LithoSheet:
                    {
                        ret = RedirectToAction("EditLithoSheet", "Article", new { id = id }); 
                        break;
                    }
                         
                case TaskExecutor.ExecutorType.DigitalSheet:
                    {
                        ret = RedirectToAction("EditDigitalSheet", "Article", new { id = id }); 
                        break;
                    }
    
                    /* continuing....................*/

            }

            return ret;
        }


        public ActionResult EditLithoSheet(string id)
        {
            LithoSheet tskEx=new LithoSheet();
            tskEx= (LithoSheet) taskExecutorRepository.GetSingle(id);

            //get producer and maker

            if (tskEx == null)
                return HttpNotFound();

            ViewBag.ActionMethod = "EditLithoSheet";
            return View(tskEx);
        }


        public ActionResult EditDigitalSheet(string id)
        {
            DigitalSheet tskEx = new DigitalSheet();
            tskEx= (DigitalSheet)taskExecutorRepository.GetSingle(id);

            if (tskEx == null)
                return HttpNotFound();

            //is used to know where we are from and go
            ViewBag.ActionMethod = "EditDigitalSheet";
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
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //If we come here, something went wrong. Return it back.      

            ViewBag.ActionMethod = "EditLithoSheet";
            return View("EditLithoSheet", c);       
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
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //If we come here, something went wrong. Return it back. 

            //multi submit
            ViewBag.ActionMethod = "EditDigitalSheet";
            return View("EditDigitalSheet", c);
        }


        //
        // GET: /Article/Delete/5

        public ActionResult Delete(string id)
        {
            return View();
        }

    }
}
