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


namespace PapiroMVC.Areas.Working.Controllers
{
    public partial class DocumentController : PapiroMVC.Controllers.ControllerAlgolaBase
    {
        private CostDetail GetCostDetail(String codTaskExecutor, String codCost)
        {
            //ITA carico il costo
            var cost = documentRepository.GetCost(codCost);
            ProductPart productPart = null;
            ICollection<ProductPartsPrintableArticle> productPartPrintabelArticles = new List<ProductPartsPrintableArticle>();

            String codTypeOfTask = String.Empty;
            if (cost.CodProductPartTask != null)
            {
                codTypeOfTask = cost.ProductPartTask.OptionTypeOfTask.CodTypeOfTask;
                productPart = cost.ProductPartTask.ProductPart;
                productPartPrintabelArticles = productPart.ProductPartPrintableArticles;
            }

            if (cost.CodProductTask != null)
            {
                codTypeOfTask = cost.ProductTask.OptionTypeOfTask.CodTypeOfTask;
            }

            var tskExec = taskExecutorRepository.GetAll().Where(x => x.CodTypeOfTask == codTypeOfTask);

            CostDetail cv = null;

            if (codTypeOfTask == "STAMPA")
            {
                String codParte = String.Empty;

                /* se è una STAMPA 
                 * dovrò selezionare il tipo di macchina anche a seconda del tipo di lavoro
                 * etichette in rotolo, manifesti etc...
                 * per ora carico.
                 */

                #region Format
                List<string> formats = new List<string>();
                //
                //voglio sapere quali sono i formati degli articoli
                foreach (var item in productPartPrintabelArticles)
                {
                    var formatList = articleRepository.GetAll().OfType<SheetPrintableArticle>()
                                .Where(x => x.TypeOfMaterial == item.TypeOfMaterial &&
                                    x.Color == item.Color &&
                                    x.NameOfMaterial == item.NameOfMaterial)
                                        .Select(x => x.Format);

                    formats = formats.Union(formatList.ToList()).ToList();
                }

                #endregion

                if (tskExec.Count() > 0)
                {
                    //se non ho una tskExecutor selezionata alloa
                    //assegno il primo executor al view

                    TaskExecutor currentTskEx;
                    if (codTaskExecutor == null || codTaskExecutor == String.Empty)
                    {
                        currentTskEx = tskExec.FirstOrDefault();
                    }
                    else
                    {
                        currentTskEx = tskExec.Where(x => x.CodTaskExecutor == codTaskExecutor).FirstOrDefault();
                    }

                    switch (currentTskEx.TypeOfExecutor)
                    {
                        case TaskExecutor.ExecutorType.LithoSheet:
                            cv = new PrintingSheetCostDetail();

                            ((PrintingSheetCostDetail)cv).BuyingFormats = formats;
                            ((PrintingSheetCostDetail)cv).BuyingFormat = (formats != null) && (formats.Count > 0) ? formats.FirstOrDefault() : null;

                            //TODO: E' da calcolare il formato di stampa a seconda del formato macchina
                            ((PrintingSheetCostDetail)cv).PrintingFormat = ((PrintingSheetCostDetail)cv).BuyingFormat;

                            cv.TaskExecutors = tskExec.ToList();
                            cv.TaskCost = cost;

                            break;
                        case TaskExecutor.ExecutorType.LithoWeb:
                            break;
                        case TaskExecutor.ExecutorType.DigitalSheet:
                            cv = new PrintingSheetCostDetail();
                            cv.TaskExecutors = tskExec.ToList();
                            cv.TaskCost = cost;
                            break;
                        case TaskExecutor.ExecutorType.DigitalWeb:
                            break;
                        case TaskExecutor.ExecutorType.Plotter:
                            break;
                        case TaskExecutor.ExecutorType.PrePostPress:
                            break;
                        case TaskExecutor.ExecutorType.Binding:
                            break;
                        default:
                            break;
                    }
                }
            }

            if (cv != null)
            {
                cv.ProductPart = productPart;
                //TODO: nella macchine a foglio devo leggere il massimo formato di stampa e impostare il PrintingFormat = al massimo'??

                cv.CodTaskExecutorSelected = codTaskExecutor;
            }

            Session["CostDetail"] = cv;
            return (cv);
        }

        [HttpPost]
        public ActionResult GetPartialCost(String codTaskExecutor, String codCost)
        {
            var x = GetCostDetail(codTaskExecutor, codCost);
            x.Update();
            return PartialView("_" + x.TypeOfCostDetail.ToString(), x);
        }

        [HttpPost]
        public ActionResult ChangeBuyingFormatInPrintingSheetCostDetail(string BuyingFormat)
        {
            PrintingSheetCostDetail cv = (PrintingSheetCostDetail)Session["CostDetail"];
            cv.BuyingFormat = BuyingFormat;
            cv.PrintingFormat = BuyingFormat;
            cv.Update();

            return PartialView("_PrintingSheetCostDetail", cv);
        }

        [HttpPost]
        public ActionResult ChangePrintingFormatInPrintingSheetCostDetail(string PrintingFormat)
        {
            PrintingSheetCostDetail cv = (PrintingSheetCostDetail)Session["CostDetail"];
            cv.PrintingFormat = PrintingFormat;
            cv.Update();
            return PartialView("_PrintingSheetCostDetail", cv);
        }

        [HttpParamAction]
        [HttpGet]
        public ActionResult EditCost(string id)
        {
            Session["CodCost"] = id;
            var vm = GetCostDetail(null, id);
            vm.Update();
            return View(vm);
        }

        [HttpParamAction]
        [HttpGet]
        public ActionResult SaveCostDetail(CostDetail c)
        {
            try
            {
                //uso passare solo l'id (con la session) e non l'intero costo nella view.                
                string id = c.TaskCost.CodCost;
                Cost cost = documentRepository.GetCost(id);

            }
            catch (Exception)
            {

                throw;
            }

            return View();
        }

        [HttpParamAction]
        [HttpGet]
        public ActionResult EditDocument(string id)
        {
            Session["CodDocument"] = id;

            var prod = documentRepository.GetSingle(id);

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "EditDocument";
            return View(prod);
        }

        [HttpParamAction]
        [HttpGet]
        public ActionResult EditDocumentProducts(string id)
        {
            var docProd = documentRepository.GetDocumentProductByCodProduct(id);

            var prod = productRepository.GetSingle(id);

            foreach (var item in docProd)
            {
                item.Product = prod;
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "EditDocumentProducts";
            return View(docProd.ToList());
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditDocument(Document c)
        {
            var taskList = this.typeOfTaskRepository.GetAll();

            if (ModelState.IsValid)
            {
                try
                {
                    documentRepository.Edit(c);
                    //rigeneration name of article
                    //c.DocumentSingleSheet.DocumentName = c.DocumentSingleSheet.ToString();
                    documentRepository.Save();
                    return Json(new { redirectUrl = Url.Action("Index") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "EditDocument";
            return PartialView("_EditAndCreateDocument", c);
        }

        //[HttpParamAction]
        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult CreateDocument(Document c)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            c.CodDocument = documentRepository.GetNewCode(c);
        //            documentRepository.Add(c);
        //            documentRepository.Save();
        //            Session["CodDocument"] = c.CodDocument; 
        //            return Json(new { redirectUrl = Url.Action("Index") });
        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
        //        }
        //    }

        //    //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
        //    ViewBag.ActionMethod = "CreateDocument";
        //    return PartialView("_EditAndCreateDocument", c);
        //}

    }
}
