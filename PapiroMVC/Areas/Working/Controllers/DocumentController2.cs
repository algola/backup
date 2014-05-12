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
using Microsoft.Office.Interop.Word;
using Novacode;
using System.IO;
using System.Reflection;
using PapiroMVC.ServiceLayer;
using System.Threading;


namespace PapiroMVC.Areas.Working.Controllers
{
    public partial class DocumentController : PapiroMVC.Controllers.ControllerAlgolaBase
    {

        static Assembly g_assembly;

        /// <summary>
        /// This Action returns partial view of CosteDetail
        /// </summary>
        /// <param name="codTaskExecutor"></param>
        /// <param name="codCost"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetPartialCost(String codTaskExecutor, String codCost)
        {
            PrintingSheetCostDetail cv = (PrintingSheetCostDetail)Session["CostDetail"];
            cv.CodTaskExecutorSelected = codTaskExecutor;
            cv.Update();
            Session["CostDetail"] = cv;
            return PartialView("_" + cv.TypeOfCostDetail.ToString(), cv);
        }

        public ActionResult GetPrintingLabelHints()
        {
            PrintingCostDetail cv = (PrintingCostDetail)Session["CostDetail"];

            var prod = productRepository.GetSingle(cv.ProductPart.CodProduct);
            var prodPart = prod.ProductParts.SingleOrDefault(x => x.CodProductPart == cv.CodProductPart);

            if (cv.TaskexEcutorSelected.TypeOfExecutor == TaskExecutor.ExecutorType.Flexo)
            {
                var x = (PrintingLabelRollCostDetail)cv;
                var lst = x.BuyingFormats;

                foreach (var item in lst)
                {
                    x.BuyingFormat = item;
                    x.PrintingFormat = item;


                }

            }


            return null;
        }

        /// <summary>
        /// This Action modifies buyingFormat and Update Cost.
        /// Is needed  Session["CostDetail"]
        /// </summary>
        /// <param name="buyingFormat"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeBuyingFormat(string buyingFormat)
        {

            PrintingCostDetail cv = (PrintingCostDetail)Session["CostDetail"];

            switch (cv.TypeOfCostDetail)
            {
                case CostDetail.CostDetailType.PrintingLabelRollCostDetail:
                    ((PrintingLabelRollCostDetail)cv).BuyingFormat = buyingFormat;
                    break;
                case CostDetail.CostDetailType.PrintingSheetCostDetail:
                    ((PrintingSheetCostDetail)cv).BuyingFormat = buyingFormat;
                    break;
                case CostDetail.CostDetailType.PrintingRollCostDetail:
                    ((PrintingRollCostDetail)cv).BuyingFormat = buyingFormat;
                    break;
                default:
                    break;
            }

            cv.PrintingFormat = buyingFormat;
            cv.Update();
            Session["CostDetail"] = cv;
            return PartialView(cv.PartialViewName, cv);
        }

        [HttpGet]
        public ActionResult GetPrintingLabelRollCostDetailResult()
        {
            PrintingCostDetail cv = (PrintingCostDetail)Session["CostDetail"];
            return PartialView(cv.PartialViewName + "Result", cv);
        }

        /// <summary>
        /// This Action modifies ProductPart format and Update Cost
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangePPartFormat(string format, string dCut1, string dCut2)
        {
            PrintingCostDetail cv = (PrintingCostDetail)Session["CostDetail"];

            var inizio = DateTime.Now;

            if (ModelState.IsValid)
            {
                //catch ProductPart from repository and save it with canges
                var prod = productRepository.GetSingle(cv.ProductPart.CodProduct);

                var prodPart = prod.ProductParts.Where(x => x.CodProductPart == cv.CodProductPart).FirstOrDefault();

                try
                {
                    prodPart.DCut1 = dCut1 == "" ? 0 : Convert.ToDouble(dCut1);
                    prodPart.DCut2 = dCut2 == "" ? 0 : Convert.ToDouble(dCut2);
                    prodPart.IsDCut = true;
                }
                catch (Exception)
                {

                }

                if (prodPart.DCut1 != null && prodPart.DCut1 != 0
                    || prodPart.DCut2 != null && prodPart.DCut2 != 0)
                {
                    prodPart.IsDCut = true;
                }

                prodPart.Format = format;

                if (TryValidateModel(prodPart))
                {
                    productRepository.Edit(prod);
                    productRepository.Save();

                    cv.ProductPart.IsDCut = true;

                    cv.ProductPart.DCut1 = Convert.ToDouble(dCut1 == "" ? "0" : dCut1);
                    cv.ProductPart.DCut2 = Convert.ToDouble(dCut2 == "" ? "0" : dCut2);
                    cv.ProductPart.Format = format;
                    cv.ProductPart.UpdateOpenedFormat();
                }
                else
                {
                    var errors = ModelState
                        .Where(x => x.Value.Errors.Count > 0)
                        .Select(x => new { x.Key, x.Value.Errors })
                        .ToArray();

                    Console.WriteLine(errors);
                }

            }

            cv.Update();
            Session["CostDetail"] = cv;

            var tempo = DateTime.Now.Subtract(inizio);
            Console.WriteLine(tempo.TotalSeconds);

            return PartialView(cv.PartialViewName, cv);
        }

        /// <summary>
        /// Action modifies PrintingFormat and Update Cost
        /// </summary>
        /// <param name="PrintingFormat"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangePrintingFormatAndPPartFormat(string PrintingFormat, string format, string dCut1, string dCut2)
        {
            PrintingCostDetail cv = (PrintingCostDetail)Session["CostDetail"];
            cv.PrintingFormat = PrintingFormat;
            Session["CostDetail"] = cv;
            return ChangePPartFormat(format, dCut1, dCut2);
        }

        /// <summary>
        /// Action modifies BuyingFormat and Update Cost
        /// </summary>
        /// <param name="BuyingFormat"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeBuyingFormatAndPPartFormat(string buyingFormat, string format, string dCut1, string dCut2)
        {
            PrintingCostDetail cv = (PrintingCostDetail)Session["CostDetail"];

            switch (cv.TypeOfCostDetail)
            {
                case CostDetail.CostDetailType.PrintingLabelRollCostDetail:
                    ((PrintingLabelRollCostDetail)cv).BuyingFormat = buyingFormat;
                    break;
                case CostDetail.CostDetailType.PrintingSheetCostDetail:
                    ((PrintingSheetCostDetail)cv).BuyingFormat = buyingFormat;
                    break;
                case CostDetail.CostDetailType.PrintingRollCostDetail:
                    ((PrintingRollCostDetail)cv).BuyingFormat = buyingFormat;
                    break;
                default:
                    break;
            }

            cv.PrintingFormat = buyingFormat;
            Session["CostDetail"] = cv;
            return ChangePPartFormat(format, dCut1, dCut2);
        }

        /// <summary>
        /// Action modifies PrintingFormat and Update Cost
        /// </summary>
        /// <param name="PrintingFormat"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangePrintingFormat(string PrintingFormat)
        {
            PrintingCostDetail cv = (PrintingCostDetail)Session["CostDetail"];
            cv.PrintingFormat = PrintingFormat;
            cv.Update();
            Session["CostDetail"] = cv;
            return PartialView(cv.PartialViewName, cv);
        }

        /// <summary>
        /// Uptate cost in Cost from CostDetail
        /// </summary>
        /// <param name="id"></param>
        private CostDetail UpdateCost(string id)
        {
            //  var inizio = DateTime.Now;

            CostDetail cv = costDetailRepository.GetSingle(id);
            cv.InitCostDetail(taskExecutorRepository.GetAll(), articleRepository.GetAll());

            cv.TaskCost.Update();

            var computes = cv.Computes.ToList();
            for (int i = 0; i < computes.Count; i++)
            {
                UpdateCost(computes[i].CodCost);
            }

            //dopo il salvataggio del dettaglio del costo voglio aggiornare il cost!!!!
            costDetailRepository.Edit(cv);
            costDetailRepository.Save();

            //var tempo = DateTime.Now.Subtract(inizio);
            //Console.WriteLine(tempo);

            return cv;
        }


        ///// <summary>
        ///// Action load Cost and generates related CosteDetail if it doesn't exist
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="guid"></param>
        ///// <returns></returns>
        //public CostDetail EditCostAutomatically(string id, Guid guid)
        //{
        //    CostDetail cv = costDetailRepository.GetSingle(id);
        //    Cost cost = documentRepository.GetCost(id);

        //    //spostare questa logica nella classe 
        //    if (cv == null)
        //    {
        //        if (cost.CodProductPartPrintableArticle != null)
        //        {
        //            var codDP = cost.CodDocumentProduct;
        //            var productPartPrintableArticle = cost.ProductPartsPrintableArticle;
        //            var productPart = cost.ProductPartsPrintableArticle.ProductPart;
        //            var task = productPart.ProductPartTasks.FirstOrDefault(x => x.OptionTypeOfTask.CodTypeOfTask.Contains("STAMPA"));

        //            cost = documentRepository.GetCost(task.Costs.FirstOrDefault(x => x.CodDocumentProduct == codDP).CodCost);
        //        }

        //        cv = cost.MakeCostDetail(taskExecutorRepository.GetAll(), articleRepository.GetAll());
        //        //guid ensures that costdetail is handled only one time when cost are all processed sistematically
        //        cv.Guid = guid.ToString("N");
        //        //update 
        //        cv.Update();
        //    }
        //    else
        //    {
        //        //se è un materiale devo aprire per ora la messa in macchina
        //        cv.Guid = guid.ToString("N");
        //        switch (cv.TypeOfCostDetail)
        //        {
        //            case CostDetail.CostDetailType.PrintingSheetCostDetail:
        //                break;
        //            case CostDetail.CostDetailType.PrintingRollCostDetail:

        //                break;
        //            case CostDetail.CostDetailType.PrintedSheetArticleCostDetail:
        //                id = cv.ComputedBy.CodCostDetail;
        //                cv = costDetailRepository.GetSingle(id);
        //                cost = documentRepository.GetCost(id);
        //                break;
        //            case CostDetail.CostDetailType.PrintedRigidArticleCostDetail:
        //                id = cv.ComputedBy.CodCostDetail;
        //                cv = costDetailRepository.GetSingle(id);
        //                cost = documentRepository.GetCost(id);
        //                break;
        //            case CostDetail.CostDetailType.PrintedRollArticleCostDetail:
        //                break;
        //            default:
        //                break;
        //        }

        //        Console.WriteLine("");
        //        cv.InitCostDetail(taskExecutorRepository.GetAll(), articleRepository.GetAll());
        //    }

        //    return cv;
        //}

        /// <summary>
        /// Action EditOrCreate all Cost in DocumentProduct
        /// </summary>
        /// <param name="id">CodDocumntProduct</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditAndCreateAllCost(string id)
        {
            var inizio = DateTime.Now;

            var idRet = documentRepository.GetCostsByCodDocumentProduct(id).FirstOrDefault().DocumentProduct.CodProduct;

            PapiroService p = new PapiroService();
            p.DocumentRepository = documentRepository;//new DocumentRepository();
            p.CostDetailRepository = costDetailRepository;
            p.TaskExecutorRepository = taskExecutorRepository;
            p.ArticleRepository = articleRepository;
            p.CurrentDatabase = CurrentDatabase;

            p.EditOrCreateAllCost(id);

            var fine = DateTime.Now;

            var tempo = fine.Subtract(inizio);
            Console.WriteLine(tempo.TotalSeconds);

            return RedirectToAction("EditDocumentProducts", "Document", new { id = idRet });
            //return Json(new { redirectUrl = Url.Action("EditDocumentProducts", "Document", new { id = idRet }) });

        }

        [HttpParamAction]
        [HttpGet]
        public ActionResult EditCost(string id)
        {
            PapiroService p = new PapiroService();
            p.DocumentRepository = documentRepository;//new DocumentRepository();
            p.CostDetailRepository = costDetailRepository;
            p.TaskExecutorRepository = taskExecutorRepository;
            p.ArticleRepository = articleRepository;
            p.CurrentDatabase = CurrentDatabase;

            var cv = p.EditCostAutomatically(id, new Guid());
            //Console.WriteLine(cv.GainForRun);

            if (cv==null)
            {
                return View("NotImplementedCostDetail");
            }

            Session["CodCost"] = id;
            Session["CostDetail"] = cv;

            Console.Write(cv.Error);

            var viewName = String.Empty;

            switch (cv.TypeOfCostDetail)
            {
                case CostDetail.CostDetailType.PrintingLabelRollCostDetail:
                    ((PrintingLabelRollCostDetail)cv).FuzzyAlgo();
                    viewName = "PrintingCostDetail";
                    break;
                case CostDetail.CostDetailType.PrintingRollCostDetail:
                    ((PrintingRollCostDetail)cv).FuzzyAlgo();
                    viewName = "PrintingCostDetail";
                    break;
                case CostDetail.CostDetailType.PrintingSheetCostDetail:
                    viewName = "PrintingCostDetail";
                    break;
                case CostDetail.CostDetailType.PrintedSheetArticleCostDetail:
                    viewName = "PrintingCostDetail";
                    break;
                case CostDetail.CostDetailType.PrintedRollArticleCostDetail:
                    viewName = "PrintingCostDetail";
                    break;

                default:
                    break;
            }

            return View(viewName, cv);
        }


        [HttpGet]
        public ActionResult EditCostTroggleLock(string id)
        {

            var cost = documentRepository.GetCost(id);
            cost.Locked = !(cost.Locked??false);

            documentRepository.EditCost(cost);
            documentRepository.Save();

            Console.Write(id);
            return null;
        }

        [HttpPost]
        public ActionResult EditCostManual(string id, string quantity, string unitCost)
        {

            var qta = Convert.ToDouble(quantity);
            var uCost = Convert.ToDouble(unitCost, Thread.CurrentThread.CurrentUICulture);

            var cost = documentRepository.GetCost(id);
            cost.Quantity = qta;
            cost.UnitCost = uCost.ToString("#,0.000", Thread.CurrentThread.CurrentUICulture); ;

            var tot = uCost * qta;

            cost.TotalCost = (tot).ToString("#,0.00", Thread.CurrentThread.CurrentUICulture);

            //blocco il costo
            cost.Locked = true;

            documentRepository.EditCost(cost);
            documentRepository.Save();

            Console.Write(id);
            return null;
            //PapiroService p = new PapiroService();
            //p.DocumentRepository = documentRepository;//new DocumentRepository();
            //p.CostDetailRepository = costDetailRepository;
            //p.TaskExecutorRepository = taskExecutorRepository;
            //p.ArticleRepository = articleRepository;
            //p.CurrentDatabase = CurrentDatabase;


            //var cv = p.EditCostAutomatically(id, new Guid());
            //Console.WriteLine(cv.GainForRun);

            //Session["CodCost"] = id;
            //Session["CostDetail"] = cv;


            //Console.Write(cv.Error);

            //var viewName = String.Empty;

            //switch (cv.TypeOfCostDetail)
            //{

            //    case CostDetail.CostDetailType.PrintingLabelRollCostDetail:
            //        ((PrintingLabelRollCostDetail)cv).FuzzyAlgo();
            //        viewName = "PrintingCostDetail";
            //        break;
            //    case CostDetail.CostDetailType.PrintingRollCostDetail:
            //    case CostDetail.CostDetailType.PrintingSheetCostDetail:
            //        viewName = "PrintingCostDetail";
            //        break;
            //    case CostDetail.CostDetailType.PrintedSheetArticleCostDetail:
            //        viewName = "PrintingCostDetail";
            //        break;
            //    case CostDetail.CostDetailType.PrintedRollArticleCostDetail:
            //        viewName = "PrintingCostDetail";
            //        break;

            //    default:
            //        break;
            //}

            //return View(viewName, cv);
        }

        //public void SaveCostDetailAutomaticallyOLD(CostDetail cv)
        //{
        //    //try
        //    //{
        //    var pPart = cv.ProductPart;
        //    //                var prod = productRepository.GetSingle(pPart.Product.CodProduct);

        //    switch (cv.TypeOfCostDetail)
        //    {
        //        //if it is a printing... we have to 
        //        case CostDetail.CostDetailType.PrintingSheetCostDetail:

        //            //if (cv.Computes.Count == 0)
        //            {
        //                var costs = documentRepository.GetCostsByCodDocumentProduct(cv.TaskCost.CodDocumentProduct);
        //                List<CostDetail> x = ((PrintingCostDetail)cv).GetRelatedPrintedCostDetail(articleRepository.GetAll(), costs);

        //                foreach (var item in x)
        //                {
        //                    //only if it is not just in list
        //                    if (!(cv.Computes.Select(y => y.CodCost).ToList().Contains(item.ComputedBy.CodCost)))
        //                    {
        //                        item.ComputedBy = cv;
        //                        item.InitCostDetail(null, articleRepository.GetAll());
        //                        //item.UpdateCost();
        //                        //item.GetCostFromList(articleRepository.GetAll());
        //                        costDetailRepository.Add(item);
        //                    }
        //                }
        //            }

        //            costDetailRepository.Add(cv);
        //            costDetailRepository.Save();

        //            //aggiorna il costo rigenerando prima i coefficienti
        //            UpdateCost(cv.CodCost);

        //            break;
        //        case CostDetail.CostDetailType.PrintingRollCostDetail:
        //            break;

        //        case CostDetail.CostDetailType.PrintedSheetArticleCostDetail:
        //            break;
        //        case CostDetail.CostDetailType.PrintedRollArticleCostDetail:
        //            break;
        //        default:
        //            break;
        //    }

        //    //reload saved cost
        //    var dp = documentRepository.GetDocumentProductByCodProduct(cv.TaskCost.DocumentProduct.CodProduct).Where(x => x.CodDocumentProduct == cv.TaskCost.CodDocumentProduct).FirstOrDefault();

        //    if (dp != null)
        //    {
        //        dp.UpdateCost();
        //    }

        //    documentRepository.Edit(dp.Document);
        //    documentRepository.Save();

        //}


        [HttpParamAction]
        [HttpGet]
        public ActionResult SaveCostDetail()
        {
            CostDetail cv = (CostDetail)Session["CostDetail"];

            PapiroService p = new PapiroService();
            p.DocumentRepository = documentRepository;//new DocumentRepository();
            p.CostDetailRepository = costDetailRepository;
            p.TaskExecutorRepository = taskExecutorRepository;
            p.ArticleRepository = articleRepository;
            p.CurrentDatabase = CurrentDatabase;
            p.SaveCostDetailAutomatically(cv);

            var idRet = (string)Session["codProduct"];

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
            Session["codProduct"] = id;

            var docProd = documentRepository.GetDocumentProductByCodProduct(id);
            var prod = productRepository.GetSingle(id);

            foreach (var item in docProd)
            {
                item.Product = prod;
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "EditDocumentProducts";

            return View(docProd.OrderBy(x => x.Quantity).ToList());
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditDocument(PapiroMVC.Models.Document c)
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


        [HttpGet]
        public ActionResult EditEstimate(string id)
        {
            Session["CodDocument"] = id;
            var prod = documentRepository.GetSingle(id);

            if (prod == null)
            {
                throw new NotFoundResException();
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "EditEstimate";
            return View(prod);

        }


        [HttpGet]
        public ActionResult EditLastEstimate()
        {

            var id = (from cod in documentRepository.GetAll().OfType<Estimate>() select cod.CodDocument).Max();
            if (id != null)
            {
                Session["CodDocument"] = id;
                var prod = documentRepository.GetSingle(id);

                //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
                ViewBag.ActionMethod = "EditEstimate";
                return View("EditEstimate", prod);
            }

            return new HttpNotFoundResult("****Errore non trovato");
        }


        public ActionResult PrintEstimate(string id)
        {

            //load document (estimate and collect data)
            var d = documentRepository.GetSingle(id);

            //estimate
            if (d == null)
            {
                throw new NullReferenceException();
            }

            //template
            // Modify to suit your machine:
            string fileName = Server.MapPath("~/Views/Home/EstimateTemplate.docx");
            DocX g_document;

            // Try to load the template 'InvoiceTemplate.docx'.
            try
            {
                // Store a global reference to the executing assembly.
                g_assembly = Assembly.GetExecutingAssembly();

                // Store a global reference to the loaded document.
                g_document = DocX.Load(fileName);

                /*
                 * The template 'EstimateTemplate.docx' exists, 
                 * so lets use it to create an invoice for a factitious company
                 * called "The Happy Builder" and store a global reference it.
                 */
                g_document = CompileFromTemplate(DocX.Load(fileName), d);

                // Save all changes made to this template as Invoice_The_Happy_Builder.docx (We don't want to replace InvoiceTemplate.docx).
                g_document.SaveAs(Server.MapPath("~/Views/Home/estimate.docx"));


            }

            // The template 'InvoiceTemplate.docx' does not exist, so create it.
            catch (FileNotFoundException)
            {
                //// Create a store a global reference to the template 'InvoiceTemplate.docx'.
                //g_document = CreateInvoiceTemplate();
                //// Save the template 'InvoiceTemplate.docx'.
                //g_document.Save();
            }

            return File(Server.MapPath("~/Views/Home/estimate.docx"), "application/file", "PapiroStar-Estimate-" + d.Number.ToString() + ".docx");

        }

        // Create an invoice for a factitious company called "The Happy Builder".
        private DocX CompileFromTemplate(DocX template, PapiroMVC.Models.Document d)
        {

            //customer
            var customer = customerSupplierRepository.GetSingle(d.CodCustomer);
            customer = customer ?? new Customer();

            var cbase = customer.CustomerSupplierBases.Where(x => x.TypeOfBase.CodTypeOfBase == "0001").FirstOrDefault();

            //se non c'è una sede
            cbase = cbase ?? new CustomerSupplierBase();

            #region Set CustomProperty values
            // Set the value of the custom property 'company_name'.
            template.AddCustomProperty(new Novacode.CustomProperty("company_name", customer.BusinessName));

            // Set the value of the custom property 'company_slogan'.
            template.AddCustomProperty(new Novacode.CustomProperty("company_slogan", ""));

            // Set the value of the custom properties 'hired_company_address_line_one', 'hired_company_address_line_two' and 'hired_company_address_line_three'.
            template.AddCustomProperty(new Novacode.CustomProperty("hired_company_address_line_one", cbase.Address ?? ""));
            template.AddCustomProperty(new Novacode.CustomProperty("hired_company_address_line_two", (cbase.PostalCode ?? "") + " " + (cbase.City ?? "") + " " + (cbase.Province ?? "")));
            template.AddCustomProperty(new Novacode.CustomProperty("hired_company_address_line_three", (cbase.Phone ?? "")));

            // Set the value of the custom property 'invoice_date'.
            template.AddCustomProperty(new Novacode.CustomProperty("invoice_date", d.DateDocument.Value.ToString("d")));

            // Set the value of the custom property 'invoice_number'.
            template.AddCustomProperty(new Novacode.CustomProperty("invoice_number", d.Number.ToString()));

            // Set the value of the custom property 'hired_company_details_line_one' and 'hired_company_details_line_two'.
            template.AddCustomProperty(new Novacode.CustomProperty("hired_company_details_line_one", "Business Street, Dublin, 12345"));
            template.AddCustomProperty(new Novacode.CustomProperty("hired_company_details_line_two", "Phone: 012-345-6789, Fax: 012-345-6789, e-mail: support@thehappybuilder.com"));
            #endregion


            /* 
             * InvoiceTemplate.docx contains a blank Table, 
             * we want to replace this with a new Table that
             * contains all of our invoice data.
             */
            Novacode.Table t = template.Tables[1];
            Novacode.Table invoice_table = CreateAndInsertInvoiceTableAfter(t, ref template, d);
            t.Remove();

            // Return the template now that it has been modified to hold all of our custom data.
            return template;
        }


        private static Novacode.Table CreateAndInsertInvoiceTableAfter(Novacode.Table t, ref DocX document, PapiroMVC.Models.Document d)
        {

            var docProd = d.DocumentProducts.Select(x => x.CodProduct).Distinct();

            /* 
             * The trick to replacing one Table with another,
             * is to insert the new Table after the old one, 
             * and then remove the old one.
             */
            Novacode.Table invoice_table = t.InsertTableAfterSelf(d.DocumentProducts.Count + 1, 4);
            invoice_table.Design = TableDesign.LightShadingAccent1;

            #region Table title
            Formatting table_title = new Formatting();
            table_title.Bold = true;


            Type res = typeof(PapiroMVC.Models.Resources.Document.ResDocumentProduct);


            invoice_table.Rows[0].Cells[0].Paragraphs[0].InsertText(
                (string)res.GetProperty("Product").GetValue(null, null), false, table_title);
            invoice_table.Rows[0].Cells[0].Paragraphs[0].Alignment = Alignment.left;

            invoice_table.Rows[0].Cells[1].Paragraphs[0].InsertText(
                (string)res.GetProperty("Quantity").GetValue(null, null), false, table_title);
            invoice_table.Rows[0].Cells[1].Paragraphs[0].Alignment = Alignment.right;

            invoice_table.Rows[0].Cells[2].Paragraphs[0].InsertText(
                (string)res.GetProperty("UnitPrice").GetValue(null, null), false, table_title);
            invoice_table.Rows[0].Cells[2].Paragraphs[0].Alignment = Alignment.right;

            invoice_table.Rows[0].Cells[3].Paragraphs[0].InsertText(
                (string)res.GetProperty("TotalAmount").GetValue(null, null), false, table_title);
            invoice_table.Rows[0].Cells[3].Paragraphs[0].Alignment = Alignment.right;

            #endregion

            // Loop through the rows in the Table and insert data from the data source.
            for (int row = 1; row < invoice_table.RowCount; )
            {
                Novacode.Paragraph cell_paragraph;

                foreach (var dp in docProd)
                {
                    var sel = d.DocumentProducts.Where(y => y.CodProduct == dp).OrderBy(z => z.Quantity);
                    var k = 0;
                    foreach (var item in sel)
                    {
                        cell_paragraph = invoice_table.Rows[row].Cells[0].Paragraphs[0];
                        cell_paragraph.InsertText(k++ == 0 ? item.ProductName : "", false);
                        invoice_table.Rows[row].Cells[0].Paragraphs[0].Alignment = Alignment.left;

                        cell_paragraph = invoice_table.Rows[row].Cells[1].Paragraphs[0];
                        cell_paragraph.InsertText(item.Quantity.ToString(), false);
                        invoice_table.Rows[row].Cells[1].Paragraphs[0].Alignment = Alignment.right;

                        cell_paragraph = invoice_table.Rows[row].Cells[2].Paragraphs[0];
                        cell_paragraph.InsertText(item.UnitPrice, false);
                        invoice_table.Rows[row].Cells[2].Paragraphs[0].Alignment = Alignment.right;

                        cell_paragraph = invoice_table.Rows[row].Cells[3].Paragraphs[0];
                        cell_paragraph.InsertText(item.TotalAmount, false);
                        invoice_table.Rows[row].Cells[3].Paragraphs[0].Alignment = Alignment.right;

                        row++;
                    }
                }

            }


            invoice_table.InsertRow();

            // Let the tables coloumns expand to fit its contents.
            invoice_table.AutoFit = AutoFit.Contents;

            // Center the Table
            invoice_table.Alignment = Alignment.center;

            //Return the invloce table now that it has been created.
            return invoice_table;
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditEstimate(Estimate c)
        {
            var taskList = this.typeOfTaskRepository.GetAll();

            if (ModelState.IsValid)
            {
                try
                {
                    documentRepository.Edit(c);
                    //rigeneration name of article
                    documentRepository.Save();
                    return Json(new { redirectUrl = Url.Action("ListEstimate") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }
            Session["CodDocument"] = c.CodDocument;

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "EditEstimate";
            return PartialView("_EditAndCreateDocument", c);
        }



        public ActionResult MenuNewProduct()
        {
            if (Session["CodDocument"] != null)
                return PartialView("_MenuNewProduct");
            else
                return null;
        }


        [HttpParamAction]
        public ActionResult NewProductCurrentEstimate(Estimate c)
        {
            MenuProduct[] menuProd = menu.GetAll().ToArray();
            string strings = "~/Views/Shared/Strings";

            foreach (var item in menuProd)
            {
                var name = (string)HttpContext.GetLocalResourceObject(strings, "CodMenuProduct" + item.CodMenuProduct);
                item.Name = name;

                if (name == "")
                {
                    Console.WriteLine();
                }
            }

            var filteredItems = menuProd.Where(
                item => item.Name.IndexOf(c.NewProductCommand.NewProduct, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var sel = filteredItems.FirstOrDefault();

            if (sel != null)
            {
                var est = Session["CodDocument"] != null ? Session["CodDocument"] : newEstimate();
                return Json(new { redirectUrl = Url.Action("CreateProduct", "Product", new { id = sel.CodMenuProduct }) });
            }
            else
            {
                return Json(new { error = true });
            }

        }

        [HttpParamAction]
        public ActionResult NewProductNewEstimate(NewProductCommand c)
        {
            MenuProduct[] menuProd = menu.GetAll().ToArray();
            string strings = "~/Views/Shared/Strings";

            foreach (var item in menuProd)
            {
                var name = (string)HttpContext.GetLocalResourceObject(strings, "CodMenuProduct" + item.CodMenuProduct);
                item.Name = name;

                if (name == "")
                {
                    Console.WriteLine();
                }
            }


            var filteredItems = menuProd.Where(
                item => item.Name.IndexOf(c.NewProduct, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var sel = filteredItems.FirstOrDefault();

            if (sel != null)
            {
                var est = newEstimate();

                return Json(new { redirectUrl = Url.Action("CreateProduct", "Product", new { id = sel.CodMenuProduct }) });
            }
            else
            {
                return PartialView("_NewProductCommand", c);
            }

        }

        public ActionResult MenuNewEstimate()
        {
            if (Session["CodDocument"] != null)
                return PartialView("_MenuCurrentEstimate", Session["CodDocument"]);
            else
                return PartialView("_MenuNewEstimate");
        }

    }
}
