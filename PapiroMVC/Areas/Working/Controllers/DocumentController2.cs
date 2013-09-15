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
