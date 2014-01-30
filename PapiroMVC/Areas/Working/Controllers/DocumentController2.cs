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


namespace PapiroMVC.Areas.Working.Controllers
{
    public partial class DocumentController : PapiroMVC.Controllers.ControllerAlgolaBase
    {

        static Assembly g_assembly;


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
        public ActionResult ChangeBuyingFormatInPrintingSheetCostDetail(string buyingFormat)
        {
            PrintingSheetCostDetail cv = (PrintingSheetCostDetail)Session["CostDetail"];
            cv.BuyingFormat = buyingFormat;
            cv.PrintingFormat = buyingFormat;
            cv.Update();
            Session["CostDetail"] = cv;
            return PartialView("_PrintingSheetCostDetail", cv);
        }

        [HttpPost]
        public ActionResult ChangeProductPartFormatFormatInPrintingSheetCostDetail(string format)
        {
            PrintingSheetCostDetail cv = (PrintingSheetCostDetail)Session["CostDetail"];

            if (ModelState.IsValid)
            {
                var prod = productRepository.GetSingle(cv.ProductPart.CodProduct);
                var prodPart = prod.ProductParts.Where(x => x.CodProductPart == cv.CodProductPart).FirstOrDefault();

                prodPart.Format = format;
                if (TryValidateModel(prodPart))
                {
                    productRepository.Edit(prod);
                    productRepository.Save();

                    cv.ProductPart.Format = format;
                    cv.ProductPart.UpdateOpenedFormat();
                }

            }

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


        private void UpdateCost(string id)
        {
            CostDetail cv = costDetailRepository.GetSingle(id);
            cv.InitCostDetail(taskExecutorRepository.GetAll(), articleRepository.GetAll());

            cv.TaskCost.Update();
            foreach (var item in cv.Computes)
            {
                UpdateCost(item.CodCost);
            }

            //dopo il salvataggio del dettaglio del costo voglio aggiornare il cost!!!!

            cv.TaskCost.DocumentProduct.UpdateCost();

            documentRepository.Edit(cv.TaskCost.DocumentProduct.Document);
            documentRepository.Save();


        }

        public CostDetail EditCostAutomatically(string id)
        {
            CostDetail cv = costDetailRepository.GetSingle(id);
            Cost cost = documentRepository.GetCost(id);

            if (cv == null)
            {
                if (cost.CodProductPartPrintableArticle != null)
                {
                    var codDP = cost.CodDocumentProduct;
                    var productPartPrintableArticle = cost.ProductPartsPrintableArticle;
                    var productPart = cost.ProductPartsPrintableArticle.ProductPart;
                    var task = productPart.ProductPartTasks.FirstOrDefault(x => x.OptionTypeOfTask.CodTypeOfTask.Contains("STAMPA"));

                    cost = documentRepository.GetCost(task.Costs.FirstOrDefault(x => x.CodDocumentProduct == codDP).CodCost);
                }

                cv = cost.MakeCostDetail(taskExecutorRepository.GetAll(), articleRepository.GetAll());
                //update 
                cv.Update();
            }
            else
            {
                //se è un materiale devo aprire per ora la messa in macchina

                switch (cv.TypeOfCostDetail)
                {
                    case CostDetail.CostDetailType.PrintingSheetCostDetail:
                        break;
                    case CostDetail.CostDetailType.PrintingRollCostDetail:

                        break;
                    case CostDetail.CostDetailType.PrintedSheetArticleCostDetail:
                        id = cv.ComputedBy.CodCostDetail;
                        cv = costDetailRepository.GetSingle(id);
                        cost = documentRepository.GetCost(id);
                        break;
                    case CostDetail.CostDetailType.PrintedRigidArticleCostDetail:
                        id = cv.ComputedBy.CodCostDetail;
                        cv = costDetailRepository.GetSingle(id);
                        cost = documentRepository.GetCost(id);
                        break;
                    case CostDetail.CostDetailType.PrintedRollArticleCostDetail:
                        break;
                    default:
                        break;
                }

                Console.WriteLine("");
                cv.InitCostDetail(taskExecutorRepository.GetAll(), articleRepository.GetAll());
            }

            return cv;
        }


        [HttpGet]
        public ActionResult EditAllCost(string id)
        {
            var costsProd = documentRepository.GetCostsByCodDocumentProduct(id);
            var idRet = costsProd.FirstOrDefault().DocumentProduct.CodProduct;

            EditCost(id);

            documentRepository.SetDbName(CurrentDatabase);
            costDetailRepository.SetDbName(CurrentDatabase);

            SaveCostDetail();

            return Json(new { redirectUrl = Url.Action("EditDocumentProducts", "Document", new { id = idRet}) });

        }

        [HttpGet]
        public ActionResult EditAndSaveCost(string id)
        {
            var cv = EditCostAutomatically(id);
            cv.Update();
            SaveCostDetailAutomatically(cv);

            return Json(new { redirectUrl = Url.Action("Index", new { area = "" }) }, JsonRequestBehavior.AllowGet);
        }

        [HttpParamAction]
        [HttpGet]
        public ActionResult EditCost(string id)
        {
            var cv = EditCostAutomatically(id);
            Console.WriteLine(cv.GainForRun);

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


        public void SaveCostDetailAutomatically(CostDetail cv)
        {
            //try
            //{
            var pPart = cv.ProductPart;
            //                var prod = productRepository.GetSingle(pPart.Product.CodProduct);

            switch (cv.TypeOfCostDetail)
            {
                //if it is a printing... we have to 
                case CostDetail.CostDetailType.PrintingSheetCostDetail:

                    if (cv.Computes.Count == 0)
                    {
                        var costs = documentRepository.GetCostsByCodDocumentProduct(cv.TaskCost.CodDocumentProduct);
                        List<PrintedArticleCostDetail> x = ((PrintingCostDetail)cv).GetRelatedPrintedCostDetail(articleRepository.GetAll(), costs);
                        foreach (var item in x)
                        {
                            item.ComputedBy = cv;
                            item.InitCostDetail(null, articleRepository.GetAll());
                            //item.UpdateCost();
                            //item.GetCostFromList(articleRepository.GetAll());
                            costDetailRepository.Add(item);
                        }
                    }

                    costDetailRepository.Add(cv);
                    costDetailRepository.Save();

                    //aggiorna il costo rigenerando prima i coefficienti
                    UpdateCost(cv.CodCost);

                    break;
                case CostDetail.CostDetailType.PrintingRollCostDetail:
                    break;

                case CostDetail.CostDetailType.PrintedSheetArticleCostDetail:
                    break;
                case CostDetail.CostDetailType.PrintedRollArticleCostDetail:
                    break;
                default:
                    break;
            }

        }


        [HttpParamAction]
        [HttpGet]
        public ActionResult SaveCostDetail()
        {
            CostDetail cv = (CostDetail)Session["CostDetail"];

            SaveCostDetailAutomatically(cv);

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
