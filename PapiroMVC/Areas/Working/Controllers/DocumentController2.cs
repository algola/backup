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

        [HttpPost]
        public ActionResult GetPartialCost(String codTaskExecutor, String codCost)
        {
            PrintingSheetCostDetail cv = (PrintingSheetCostDetail)Session["CostDetail"];
            cv.CodTaskExecutorSelected = codTaskExecutor;
            cv.Update();
            Session["CostDetail"] = cv;
            return PartialView("_" + cv.TypeOfCostDetail.ToString(), cv);
        }

        [HttpPost]
        public ActionResult ChangeBuyingFormatInPrintingSheetCostDetail(string BuyingFormat)
        {
            PrintingSheetCostDetail cv = (PrintingSheetCostDetail)Session["CostDetail"];
            cv.BuyingFormat = BuyingFormat;
            cv.PrintingFormat = BuyingFormat;
            cv.Update();
            Session["CostDetail"] = cv;
            return PartialView("_PrintingSheetCostDetail", cv);
        }

        [HttpPost]
        public ActionResult ChangePrintingFormatInPrintingSheetCostDetail(string PrintingFormat)
        {
            PrintingSheetCostDetail cv = (PrintingSheetCostDetail)Session["CostDetail"];
            cv.PrintingFormat = PrintingFormat;
            cv.Update();
            Session["CostDetail"] = cv;
            return PartialView("_PrintingSheetCostDetail", cv);
        }

        [HttpParamAction]
        [HttpGet]
        public ActionResult EditCost(string id)
        {
            CostDetail cv = costDetailRepository.GetSingle(id);
            Cost cost = documentRepository.GetCost(id);

            if (cv == null)
            {
                cv = cost.MakeCostDetail(taskExecutorRepository.GetAll(), articleRepository.GetAll());
                //update 
                cv.Update();
            }
            else
            {
                Console.WriteLine("");
                cv.InitCostDetail2(taskExecutorRepository.GetAll(), articleRepository.GetAll());

            }

            Session["CodCost"] = id;
            Session["CostDetail"] = cv;

            var viewName = String.Empty;

            switch (cv.TypeOfCostDetail)
            {
                case CostDetail.CostDetailType.PrintingSheetCostDetail:
                    viewName = "PrintingCostDetail";
                    break;
                case CostDetail.CostDetailType.PrintingRollCostDetail:
                    viewName = "PrintingCostDetail";
                    break;
                case CostDetail.CostDetailType.PrintingPlotterCostDetail:
                    viewName = "PrintingCostDetail";
                    break;
                case CostDetail.CostDetailType.PrintedSheetArticleCostDetail:
                    viewName = "PrintedCostDetail";
                    break;
                case CostDetail.CostDetailType.PrintedRollArticleCostDetail:
                    viewName = "PrintedCostDetail";
                    break;
                default:
                    break;
            }

            return View(viewName, cv);
        }

        [HttpParamAction]
        [HttpGet]
        public ActionResult SaveCostDetail()
        {
            CostDetail cv = (CostDetail)Session["CostDetail"];
            //try
            {
                var pPart = cv.ProductPart;
                //                var prod = productRepository.GetSingle(pPart.Product.CodProduct);

                switch (cv.TypeOfCostDetail)
                {
                    case CostDetail.CostDetailType.PrintingSheetCostDetail:

                        costDetailRepository.Add(cv);
                        costDetailRepository.Save();

                        break;
                    case CostDetail.CostDetailType.PrintingRollCostDetail:
                        break;
                    case CostDetail.CostDetailType.PrintingPlotterCostDetail:
                        break;
                    case CostDetail.CostDetailType.PrintedSheetArticleCostDetail:
                        break;
                    case CostDetail.CostDetailType.PrintedRollArticleCostDetail:
                        break;
                    default:
                        break;
                }
            }

            var idRet = (string)Session["idProduct"];

            if (idRet != null)
            {
                return Json(new { redirectUrl = Url.Action("EditDocumentProducts", new { id = idRet }) }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { redirectUrl = Url.Action("Index", new { area = "" }) }, JsonRequestBehavior.AllowGet);
            }

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

            //id is needed for return after edit
            Session["idProduct"] = id;

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


    }
}
