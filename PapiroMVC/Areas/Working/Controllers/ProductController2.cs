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
using System.Net;


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
            var prod = productRepository.GetSingle(id);
            prod.FormatsName = formatsRepository.GetAllById(prod.CodMenuProduct);
            prod.SystemTaskList = typeOfTaskRepository.GetAll().ToList();
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
            prod.SystemTaskList = typeOfTaskRepository.GetAll().ToList();
            prod.InitPageTask();
            return View(prod);
        }


        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditProduct(Product c)
        {
            c.FormatsName = formatsRepository.GetAllById(c.CodMenuProduct);
            c.SystemTaskList = typeOfTaskRepository.GetAll().ToList();
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
        public ActionResult CreateProduct(ProductViewModel pv)
        {
            var qts = pv.Quantities;
            var product = pv.Product;

            foreach (var item in product.ProductParts)
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

            //CHECK IF EACH PRINTABLE ARTICLE IS IN LIST
            foreach (var item in product.ProductParts)
            {
                foreach (var pArticle in item.ProductPartPrintableArticles)
                {
                    if (!pArticle.IsInList(articleRepository.GetAll()))
                    {
                        ModelState.AddModelError("PersError", "ProductPartPrintableArticleListError");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                //               try
                {
                    //il cliente
                    var cust = customerSupplierRepository.GetAll().OfType<Customer>().Where(x => x.BusinessName == pv.Customer).FirstOrDefault();
                    if (cust == null)
                    {
                        cust = new Customer();
                        cust.CodCustomerSupplier = customerSupplierRepository.GetNewCode(cust);
                        cust.BusinessName = pv.Customer;
                        cust.VatNumber = "00000000000";
                        customerSupplierRepository.Add(cust);
                        customerSupplierRepository.Save();
                    }

                    product.CodProduct = productRepository.GetNewCode(product);

                    //save the product
                    productRepository.Add(product);
                    productRepository.Save();

                    documentRepository.SetDbName("");
                    documentRepository.SetDbName(CurrentDatabase);

                    //se ho un documento attivo in sessione salvo anche la relazione
                    //oppure se non c'è un domcumento attivo lo creo e salvo la relazione
                    //dopodiche devo saltare al documento oppure alla view model del prodottodocumento
                    if (Session["CodDocument"] == null || documentRepository.GetSingle((string)Session["CodDocument"]) == null)
                    {
                        var d = new Document();
                        d.CodDocument = documentRepository.GetNewCode(d);
                        Session["CodDocument"] = d.CodDocument;
                        documentRepository.Add(d);
                        documentRepository.Save();
                    }

                    var document = documentRepository.GetSingle((string)Session["CodDocument"]);
                    document.DocumentName = pv.ProductName;
                    document.CustomerSupplier = cust;
                    document.Customer = cust.BusinessName;
                    document.CodCustomer = cust.CodCustomerSupplier;

                    documentRepository.Edit(document);
                    documentRepository.Save();


                    DocumentProduct dp;
                    DocumentProduct firstDocumentProduct = null;

                    foreach (var qtsitem in qts)
                    {
                        if (qtsitem != 0)
                        {
                            dp = new DocumentProduct();
                            //use first document product to lead each tecnical choice
                            if (firstDocumentProduct == null)
                            {
                                firstDocumentProduct = dp;
                            }
                            dp.Document = null;
                            dp.CodProduct = pv.Product.CodProduct;
                            dp.Product = pv.Product;

                            dp.Quantity = qtsitem;

                            dp.InitCost();
                            document.DocumentProducts.Add(dp);

                            documentRepository.Edit(document);
                            documentRepository.Save();

                        }
                    }

                    //OK questo funziona ma riporta alla lista dei costi
                    //TODO: Sending singlaR notification to client to reload basket product
                    //return Json(new { redirectUrl = Url.Action("EditDocumentProducts", "Document", new { id = document.DocumentProducts.LastOrDefault().CodProduct }) });

                    if (firstDocumentProduct.Costs.FirstOrDefault() != null)
                    {
                        Session["codProduct"] = document.DocumentProducts.LastOrDefault().CodProduct;
                        
                        return Json( new { redirectUrl = Url.Action("EditAndCreateAllCost", "Document", new { id = firstDocumentProduct.Costs.FirstOrDefault().CodDocumentProduct }) });
                        //   return Json(new { redirectUrl = Url.Action("EditCost", "Document", new { id = firstDocumentProduct.Costs.FirstOrDefault().CodCost }) });
                    }
                    else
                    {
                        return Json(new { redirectUrl = Url.Action("EditDocumentProducts", "Document", new { id = document.DocumentProducts.LastOrDefault().CodProduct }) });
                    }

                }
                //                catch (Exception ex)
                //{
                //    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                //}
            }

            //Carico i nomi dei formati perchè se la validazione non va a buon fine devo ripresentarli
            product.FormatsName = formatsRepository.GetAllById(product.CodMenuProduct);
            product.SystemTaskList = typeOfTaskRepository.GetAll().ToList();

            //reload option object for productTask and productPartTask
            var taskList = this.typeOfTaskRepository.GetAll();
            foreach (var item in product.ProductTasks)
            {
                item.OptionTypeOfTask = typeOfTaskRepository.GetSingleOptionTypeOfTask(item.CodOptionTypeOfTask);
            }


            foreach (var item in product.ProductParts)
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
            return PartialView("_EditAndCreateProduct", pv);
        }

    }
}
