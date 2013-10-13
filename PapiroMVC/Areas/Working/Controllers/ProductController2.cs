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
    public partial class ProductController : PapiroMVC.Controllers.ControllerAlgolaBase
    {

        //initialize only posiible part task like... plastificatura etc...
        public List<ProductPartTask> GetInitalizedPartTask()
        {
            IQueryable<TypeOfTask> taskList = typeOfTaskRepository.GetAll();
            var tsksInPart = new List<ProductPartTask>();

            ProductPartTask pt;

            String[] codTypeOfTasks = { "STAMPA", "PLASTIFICATURA" };

            foreach (var item in codTypeOfTasks)
            {
                pt = new ProductPartTask();
                //default selection
                pt.OptionTypeOfTask = taskList.FirstOrDefault(x => x.CodTypeOfTask == item).OptionTypeOfTasks.FirstOrDefault(y => y.CodOptionTypeOfTask == item + "_NO");
                pt.CodOptionTypeOfTask = pt.OptionTypeOfTask.CodOptionTypeOfTask;
                pt.Hidden = true;
                tsksInPart.Add(pt);
            }

            return tsksInPart;

        }

        [HttpParamAction]
        [HttpGet]
        public ActionResult EditProduct(string id)
        {
            var prod=productRepository.GetSingle(id);
            prod.FormatsName= formatsRepository.GetAllById(prod.CodMenuProduct);
            prod.SystemTaskList = typeOfTaskRepository.GetAll();
            prod.InitPageTask();
            
            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "EditProduct";
            return View(prod);
        }

        [HttpGet]
        public ActionResult DisplayProduct(string id)
        {
            var prod = productRepository.GetSingle(id);
            prod.FormatsName = formatsRepository.GetAllById(prod.CodMenuProduct);
            prod.SystemTaskList = typeOfTaskRepository.GetAll();
            prod.InitPageTask();
            return View(prod);
        }


        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditProduct(Product c)
        {

            c.FormatsName = formatsRepository.GetAllById(c.CodMenuProduct);
            c.SystemTaskList = typeOfTaskRepository.GetAll();
            c.InitPageTask();

            var taskList = this.typeOfTaskRepository.GetAll();
            foreach (var item in c.ProductTasks)
            {
                item.OptionTypeOfTask = typeOfTaskRepository.GetSingleOptionTypeOfTask(item.CodOptionTypeOfTask);
            }

            if (ModelState.IsValid)
            {
                //try
                //{
                    productRepository.Edit(c);
                    //rigeneration name of article
                    //c.ProductSingleSheet.ProductName = c.ProductSingleSheet.ToString();
                    productRepository.Save();
                    return Json(new { redirectUrl = Url.Action("Index") });
                //}
                //catch (Exception ex)
                //{
                //    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                //}
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "EditProduct";
            return PartialView("_EditAndCreateProduct", c);

        }

        [HttpParamAction]     
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateProduct(ProductViewModel b)
        {
            var qts = b.Quantities;
            var c = b.Product;
            foreach (var item in c.ProductParts)
            {
                if (item.Format == "0x0" && item.FormatPersonalized != String.Empty)
                {
                    item.Format = item.FormatPersonalized;
                }
                if (item.Format == "0x0")
                {
                    item.Format = String.Empty;
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    c.CodProduct = productRepository.GetNewCode(c);
                    productRepository.Add(c);
                    productRepository.Save();

                    //se ho un documento attivo in sessione salvo anche la relazione
                    //oppure se non c'è un domcumento attivo lo creo e salvo la relazione
                    //dopodiche devo saltare al documento oppure alla view model del prodottodocumento
                    if (Session["CodDocument"] == null || documentRepository.GetSingle((string)Session["CodDocument"])== null)
                    {
                        var d = new Document();
                        d.CodDocument = documentRepository.GetNewCode(d);
                        Session["CodDocument"] = d.CodDocument;
                        documentRepository.Add(d);
                        documentRepository.Save();
                    }

                    var document = documentRepository.GetSingle((string)Session["CodDocument"]);

                    var documentProduct = new DocumentProduct();
                    documentProduct.CodProduct = c.CodProduct;
                    documentProduct.ProductName = c.ProductName;
                    document.DocumentProducts.Add(documentProduct);

                    documentRepository.Edit(document);
                    documentRepository.Save();

                    return Json(new { redirectUrl = Url.Action("Index") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //Carico i nomi dei formati perchè se la validazione non va a buon fine devo ripresentarli
            c.FormatsName = formatsRepository.GetAllById(c.CodMenuProduct);
            c.SystemTaskList = typeOfTaskRepository.GetAll();

            //reload option object for productTask and productPartTask
            var taskList = this.typeOfTaskRepository.GetAll();
            foreach (var item in c.ProductTasks)
            {
                item.OptionTypeOfTask = typeOfTaskRepository.GetSingleOptionTypeOfTask(item.CodOptionTypeOfTask);
            }


            foreach (var item in c.ProductParts)
            {
                foreach (var item2 in item.ProductPartTasks)
                {
                    item2.OptionTypeOfTask = typeOfTaskRepository.GetSingleOptionTypeOfTask(item2.CodOptionTypeOfTask);                    
                }
                
            }
            //-----end reloding
            //????


            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreateProduct";
            return PartialView("_EditAndCreateProduct", c);
        }

        private Product InitProduct(string id)
        {
            Product product;

            if (id == "Buste" ||
                id == "BigliettiVisita" ||
                id == "EtichetteCartellini" ||
                id == "CartolineInviti" ||
                id == "Volantini" ||
                id == "Pieghevoli" ||
                id == "CartaIntestata" ||
                id == "Locandine" ||
                id == "CartolinePostali" ||
                id == "FogliMacchina" ||
                id == "AltriFormati")
            {
                product = new ProductSingleSheet();
            }
            else
                if (id == "PuntoMetallico" ||
                    id == "SpiraleMetallica" ||
                    id == "BrossuraFresata" ||
                    id == "BrossuraCucitaFilo" ||
                    id == "RivistePostalizzazione" ||
                    id == "SchedeNonRilegate")
                {
                    product = new ProductBookSheet();
                }
                else
                    if (id == "PVC" ||
                        id == "Manifesti" ||
                        id == "Fotoquadri" ||
                        id == "Striscioni" ||
                        id == "SuppRigidi" ||
                        id == "Poster") product = new ProductRigid();
                    else{
                        product = new ProductSingleSheet();
                        }

            product.CodMenuProduct = id;
            product.ProductTaskName = prodTskNameRepository.GetAllById(id);
            product.FormatsName = formatsRepository.GetAllById(id);

            product.SystemTaskList = typeOfTaskRepository.GetAll();
            product.InitProduct();

            return product;
        
        }



    }
}
