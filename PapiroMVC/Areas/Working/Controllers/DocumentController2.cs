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
using Novacode;
using System.IO;
using System.Reflection;
using PapiroMVC.ServiceLayer;
using System.Threading;
using Microsoft.Office.Interop.Word;
using DocumentFormat.OpenXml.Packaging;
using System.Threading.Tasks;


namespace PapiroMVC.Areas.Working.Controllers
{
    //[AuthorizeAlgola(Roles = "Estimate")]
    public partial class DocumentController : PapiroMVC.Controllers.ControllerAlgolaBase
    {

        static Assembly g_assembly;



        //public ActionResult PrintOrder(string codDocument)
        //{
        //    //carico l'ordine
        //    Order order = documentRepository.GetAll().OfType<Order>().FirstOrDefault(x => x.CodDocument == codDocument);

        //    //cliente dell'ordine
        //    Customer cust = (Customer)customerSupplierRepository.GetSingle(order.CodCustomer);
        //    order.CustomerSupplier = cust;

        //    //il suo prodotto con la sua quantità
        //    order.OrderProduct = documentRepository.GetDocumentProductByCodDocumentProduct(order.CodDocumentProduct);

        //    //estimate
        //    order.OrderProduct.Document = (Estimate)documentRepository.GetSingle(order.OrderProduct.CodDocument);

        //    string fileNameMain = Path.Combine(Server.MapPath(@"~/Report"), "OrderHead.docx");
        //    string fileNameCost = Path.Combine(Server.MapPath(@"~/Report"), "Cost.docx");


        //    string fileNameSaveAs = Path.Combine(Server.MapPath(@"~/Report"), order.OrderNumberSerie + "-" + order.OrderNumber + ".docx");
        //    string fileNameSaveAsAfterRepair = Path.Combine(Server.MapPath(@"~/Report"), order.OrderNumberSerie + "-" + order.OrderNumber + "AfterRepair.docx");

        //    // Store a global reference to the executing assembly.
        //    g_assembly = Assembly.GetExecutingAssembly();


        //    // Create the document in memory:
        //    var docMain = DocX.Load(fileNameMain);

        //    order.MergeField(docMain);
        //    order.OrderProduct.MergeField(docMain);
        //    order.OrderProduct.Product.MergeField(docMain);

        //    var product = productRepository.GetSingle(order.OrderProduct.Product.CodProduct);
        //    product.ProductParts.FirstOrDefault().MergeField(docMain);

        //    //    product.ProductParts.FirstOrDefault().ProductPartPrintableArticles.FirstOrDefault().MergeField(docMain);


        //    foreach (var cost in order.OrderProduct.Costs)
        //    {
        //        // Create the document in memory:
        //        //var docCost = DocX.Load(fileNameCost);
        //        //                cost.MergeField(docCost);

        //        //try
        //        //{
        //        var cd = cost.CostDetails.FirstOrDefault();
        //        if (cd != null)
        //        {
        //            cd = costDetailRepository.GetSingle(cd.CodCostDetail);
        //            cd.InitCostDetail(taskExecutorRepository.GetAll(), articleRepository.GetAll());

        //            if (cost.CostDetails.FirstOrDefault().TypeOfCostDetail.ToString() == "PrintedRollArticleCostDetail" ||
        //                cost.CostDetails.FirstOrDefault().TypeOfCostDetail.ToString() == "PrintingZRollCostDetail" ||
        //                cost.CostDetails.FirstOrDefault().TypeOfCostDetail.ToString() == "ControlTableCostDetail")
        //            {
        //                var docPrintCD = DocX.Load(Path.Combine(Server.MapPath(@"~/Report"), cost.CostDetails.FirstOrDefault().TypeOfCostDetail.ToString() + ".docx"));
        //                cd.MergeField(docPrintCD);
        //                cd.TaskCost.MergeField(docPrintCD);
        //                //il merge con il cost

        //                try
        //                {
        //                    cd.TaskCost.ProductPartTask.ProductPart.MergeField(docPrintCD);

        //                }
        //                catch
        //                { }
        //                docMain.InsertDocument(docPrintCD);
        //                docPrintCD.Dispose();
        //            }
        //            else
        //            {
        //            }
        //           // docMain.InsertDocument(docCost);

        //        }

        //        //}
        //        //catch
        //        //{ }

        //       // docCost.Dispose();
        //    }

        //    docMain.SaveAs(fileNameSaveAs);

        //    docMain.Dispose();

        //    // Open a doc file.
        //    Application application = new Application();
        //    Microsoft.Office.Interop.Word.Document document = application.Documents.Open(fileNameSaveAs, OpenAndRepair: true);


        //    Object fnsa = (Object)fileNameSaveAsAfterRepair;
        //    // Object of Missing "Null Value".
        //    Object oMissing = System.Reflection.Missing.Value;

        //    // Object of false.
        //    Object oFalse = false;
        //    // Save the document.
        //    document.SaveAs
        //    (
        //        ref fnsa, ref oMissing, ref oMissing, ref oMissing,
        //        ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
        //        ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
        //        ref oMissing, ref oMissing
        //    );            // Close word.

        //    document.Close();
        //    application.Quit();


        //    return File(fileNameSaveAsAfterRepair, "application/file", "stampa.docx");

        //}

        public ActionResult PrintOrder(string codDocument, string reportName)
        {
            int id;
            var dbName = reportName.Replace("LabelRollHead", "");

            //carico l'ordine
            Order order = documentRepository.GetAll().OfType<Order>().FirstOrDefault(x => x.CodDocument == codDocument);

            //cliente dell'ordine
            Customer cust = (Customer)customerSupplierRepository.GetSingle(order.CodCustomer);
            order.CustomerSupplier = cust;

            //il suo prodotto con la sua quantità
            order.OrderProduct = documentRepository.GetDocumentProductByCodDocumentProduct(order.CodDocumentProduct);

            //estimate
            order.OrderProduct.Document = documentRepository.GetSingle(order.OrderProduct.CodDocument);

            string fileNameMain = Path.Combine(Server.MapPath(@"~/Report"), (reportName == "" || reportName == null) ? "OrderHead.docx" : (reportName + ".docx"));

            string fileNameCost = Path.Combine(Server.MapPath(@"~/Report"), "Cost" + dbName + ".docx");
            if (!System.IO.File.Exists(fileNameCost))
            {
                fileNameCost = Path.Combine(Server.MapPath(@"~/Report"), "Cost" + ".docx");
            }


            string retName = order.OrderNumberSerie + "-" + order.OrderNumber;
            retName = retName.PurgeFileName();

            string fileNameSaveAs = Path.Combine(Server.MapPath(@"~/Report"), retName + dbName + ".docx");
            string fileNameSaveAsAfterRepair = fileNameSaveAs;// Path.Combine(Server.MapPath(@"~/Report"), retName + "AfterRepair.docx");

            // Store a global reference to the executing assembly.
            g_assembly = Assembly.GetExecutingAssembly();

            //// Create the document in memory:
            var docMain = DocX.Load(fileNameMain);

            //questo array mi serve per il merge
            Queue<string> files = new Queue<string>();
            //questo array mi serve per il merge
            Queue<string> filesExtCost = new Queue<string>();

            //questo array mi serve per il merge
            Queue<string> RepassCost = new Queue<string>();

            //questo array mi serve per il merge
            Queue<string> filesDelivery = new Queue<string>();

            order.MergeField(docMain);
            order.OrderProduct.MergeField(docMain);
            order.OrderProduct.Product.MergeField(docMain);

            var product = productRepository.GetSingle(order.OrderProduct.Product.CodProduct);
            product.ProductParts.FirstOrDefault().MergeField(docMain);
            try
            {
                product.ProductParts.FirstOrDefault().productpartprintings.FirstOrDefault().MergeField(docMain);
            }
            catch (Exception)
            {
            }

            //    product.ProductParts.FirstOrDefault().ProductPartPrintableArticles.FirstOrDefault().MergeField(docMain);

            var costs = order.OrderProduct.Costs.OrderBy(x => x.IndexOf);
            Console.WriteLine(costs);



            //group costdetails by taskexecutor
            List<CostDetail> costDetails = new List<CostDetail>();
            foreach (var item in costs)
            {
                costDetails.AddRange(item.CostDetails);
            }

            var TaskExecutorGroups = costDetails.GroupBy(p => p.CodTaskExecutorSelected,
                p => p,
                (key, g) => new CostDetailGrouped
                {
                    CodTaskExecutorSelected = key,
                    CostDetails = g.ToList()
                });

            Console.WriteLine(TaskExecutorGroups);
            //end grouping




            foreach (var cost in costs)
            {
                // Create the document in memory:
                //var docCost = DocX.Load(fileNameCost);
                //                cost.MergeField(docCost);

                //try
                //{
                var cd = cost.CostDetails.FirstOrDefault();
                if (cd != null)
                {
                    cd = costDetailRepository.GetSingle(cd.CodCostDetail);
                    cd.InitCostDetail(taskExecutorRepository.GetAll(), articleRepository.GetAll());

                    if (cost.CostDetails.FirstOrDefault().TypeOfCostDetail.ToString() == "PrintedRollArticleCostDetail" ||
                        cost.CostDetails.FirstOrDefault().TypeOfCostDetail.ToString() == "PrintingZRollCostDetail" ||
                        cost.CostDetails.FirstOrDefault().TypeOfCostDetail.ToString() == "PrintedSheetArticleCostDetail" ||
                        cost.CostDetails.FirstOrDefault().TypeOfCostDetail.ToString() == "PrintingSheetCostDetail" ||
                        cost.CostDetails.FirstOrDefault().TypeOfCostDetail.ToString() == "ControlTableCostDetail")
                    {
                        #region SingleCostDetail
                        var cDFileName = Path.Combine(Server.MapPath(@"~/Report"), cost.CostDetails.FirstOrDefault().TypeOfCostDetail.ToString() + dbName + ".docx");
                        if (!System.IO.File.Exists(cDFileName))
                        {
                            cDFileName = Path.Combine(Server.MapPath(@"~/Report"), cost.CostDetails.FirstOrDefault().TypeOfCostDetail.ToString() + ".docx");
                        }

                        var docPrintCD = DocX.Load(cDFileName);

                        cd.MergeField(docPrintCD);
                        cd.TaskCost.MergeField(docPrintCD);

                        cd.MergeField(docMain);
                        cd.TaskCost.MergeField(docMain);

                        try
                        {
                            cd.TaskCost.ProductPartTask.ProductPart.MergeField(docPrintCD);
                            cd.TaskCost.ProductPartTask.ProductPart.MergeField(docMain);
                        }
                        catch
                        { }

                        docPrintCD.SaveAs(Path.Combine(Server.MapPath(@"~/Report"), "Cost" + cost.CodCost + ".docx"));
                        files.Enqueue(Path.Combine(Server.MapPath(@"~/Report"), "Cost" + cost.CodCost + ".docx"));

                        #region Merge con Docx     (vecchio metodo)
                        //var xxx = DocX.Load(Path.Combine(Server.MapPath(@"~/Report"), "Cost" + cost.CodCost + ".docx"));

                        //docMain.InsertDocument(xxx);
                        //docPrintCD.Dispose();
                        //xxx.Dispose();
                        #endregion
                        #endregion
                    }
                    else
                    {
                        if (!cost.CostDetails.FirstOrDefault().JustPrintedInOrder)
                        {

                            if (cost.CostDetails.FirstOrDefault().TypeOfCostDetail.ToString() == "RepassRollCostDetail")
                            {
                                //il ripasso potrebbe includere diverse lavorazioni!!!
                                //ripassi con macchine uguali ---> unisco e salvo
                                #region RepassRollCostDetail

                                //apro l'header dei costi supplementari e lo salvo
                                var nomeRepass = Path.Combine(Server.MapPath(@"~/Report"), "RepassCostHeader" + order.CodDocument + ".docx");

                                var docRepassHeaderFile = Path.Combine(Server.MapPath(@"~/Report"), "RepassRollCostHeader" + dbName + ".docx");
                                if (!System.IO.File.Exists(docRepassHeaderFile))
                                {
                                    docRepassHeaderFile = Path.Combine(Server.MapPath(@"~/Report"), "RepassRollCostHeader.docx");
                                }

                                var docRepassHeader = DocX.Load(docRepassHeaderFile);

                                //estraggo per macchina
                                var relatedCostDetails = TaskExecutorGroups.Where(x => x.CodTaskExecutorSelected == cost.CostDetails.FirstOrDefault().CodTaskExecutorSelected).FirstOrDefault().CostDetails;

                                docRepassHeader.SaveAs(nomeRepass);
                                files.Enqueue(nomeRepass);

                                foreach (var i in relatedCostDetails)
                                {
                                    var docPrePressFile = Path.Combine(Server.MapPath(@"~/Report"), "RepassRollCostDetail" + dbName + ".docx");
                                    if (!System.IO.File.Exists(docPrePressFile))
                                    {
                                        docPrePressFile = Path.Combine(Server.MapPath(@"~/Report"), "RepassRollCostDetail.docx");
                                    }

                                    var docPrePress = DocX.Load(docPrePressFile);
                                    var cv = costDetailRepository.GetSingle(i.CodCost);

                                    var print = true;
                                    if (cv.TaskCost.ProductPartTask != null)
                                    {
                                        print = !cv.TaskCost.ProductPartTask.OptionTypeOfTask.CodOptionTypeOfTask.Contains("_NO");
                                    }

                                    if (cv.TaskCost.ProductTask != null)
                                    {
                                        print = !cv.TaskCost.ProductTask.OptionTypeOfTask.CodOptionTypeOfTask.Contains("_NO");
                                    }


                                    if (print)
                                    {
                                        cv.MergeField(docPrePress);
                                        docPrePress.SaveAs(Path.Combine(Server.MapPath(@"~/Report"), "RepassRollCost" + i.CodCost + ".docx"));
                                        RepassCost.Enqueue(Path.Combine(Server.MapPath(@"~/Report"), "RepassRollCost" + i.CodCost + ".docx"));
                                    }

                                    i.JustPrintedInOrder = true;

                                }

                                id = 0;
                                foreach (var file in RepassCost.Reverse())
                                {
                                    using (WordprocessingDocument myDoc = WordprocessingDocument.Open(nomeRepass, true))
                                    {
                                        var altChunkId = "AltChunkId" + id++;
                                        Console.WriteLine(altChunkId);
                                        var mainPart = myDoc.MainDocumentPart;
                                        var chunk = mainPart.AddAlternativeFormatImportPart(DocumentFormat.OpenXml.Packaging.AlternativeFormatImportPartType.WordprocessingML, altChunkId);
                                        using (System.IO.FileStream fileStream = System.IO.File.Open(file, System.IO.FileMode.Open))
                                        {
                                            chunk.FeedData(fileStream);
                                        }
                                        var altChunk = new DocumentFormat.OpenXml.Wordprocessing.AltChunk();
                                        altChunk.Id = altChunkId;

                                        var last = mainPart.Document.Body.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().Last();

                                        mainPart.Document.Body.InsertAfter(altChunk, last);
                                        mainPart.Document.Save();
                                    }
                                }


                                #endregion

                            }



                        }

                        var res = cost.CostDetails.FirstOrDefault().TypeOfCostDetail.ToString();
                        Console.WriteLine(res);

                    }
                    // docMain.InsertDocument(docCost);

                }
            }

            #region costi supplementari

            //apro l'header dei costi supplementari e lo salvo
            var nomeExt = Path.Combine(Server.MapPath(@"~/Report"), "ExtCostHeader" + order.CodDocument + ".docx");

            var docECHeaderFile = Path.Combine(Server.MapPath(@"~/Report"), "ExternalCostHeader" + dbName + ".docx");
            if (!System.IO.File.Exists(docECHeaderFile))
            {
                docECHeaderFile = Path.Combine(Server.MapPath(@"~/Report"), "ExternalCostHeader.docx");
            }
            var docECHeader = DocX.Load(docECHeaderFile);

            docECHeader.SaveAs(nomeExt);
            files.Enqueue(nomeExt);

            //0=incluso, 1=Aux, 2=escluso
            var extCost = costs.Where(x => x.TypeOfCalcolous == 1 && (x.Quantity ?? 0) != 0);
            //prestampa
            foreach (var cost in extCost)
            {
                var docPrePressFile = Path.Combine(Server.MapPath(@"~/Report"), "ExternalCost" + dbName + ".docx");
                if (!System.IO.File.Exists(docPrePressFile))
                {
                    docPrePressFile = Path.Combine(Server.MapPath(@"~/Report"), "ExternalCost.docx");
                }
                var docPrePress = DocX.Load(docPrePressFile);

                cost.MergeField(docPrePress);
                docPrePress.SaveAs(Path.Combine(Server.MapPath(@"~/Report"), "ExtCost" + cost.CodCost + ".docx"));
                filesExtCost.Enqueue(Path.Combine(Server.MapPath(@"~/Report"), "ExtCost" + cost.CodCost + ".docx"));
            }

            id = 0;
            foreach (var file in filesExtCost.Reverse())
            {
                using (WordprocessingDocument myDoc = WordprocessingDocument.Open(nomeExt, true))
                {
                    var altChunkId = "AltChunkId" + id++;
                    Console.WriteLine(altChunkId);
                    var mainPart = myDoc.MainDocumentPart;
                    var chunk = mainPart.AddAlternativeFormatImportPart(DocumentFormat.OpenXml.Packaging.AlternativeFormatImportPartType.WordprocessingML, altChunkId);
                    using (System.IO.FileStream fileStream = System.IO.File.Open(file, System.IO.FileMode.Open))
                    {
                        chunk.FeedData(fileStream);
                    }
                    var altChunk = new DocumentFormat.OpenXml.Wordprocessing.AltChunk();
                    altChunk.Id = altChunkId;

                    var last = mainPart.Document.Body.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().Last();

                    mainPart.Document.Body.InsertAfter(altChunk, last);
                    mainPart.Document.Save();
                }
            }


            #endregion

            #region consegne

            //apro l'header dei costi supplementari e lo salvo

            var nomeDelivery = Path.Combine(Server.MapPath(@"~/Report"), "DeliveryHeader" + order.CodDocument + ".docx");

            var docDHeaderFile = Path.Combine(Server.MapPath(@"~/Report"), "DeliveryHeader" + dbName + ".docx");
            if (!System.IO.File.Exists(docDHeaderFile))
            {
                docDHeaderFile = Path.Combine(Server.MapPath(@"~/Report"), "DeliveryHeader.docx");
            }
            var docDHeader = DocX.Load(docDHeaderFile);

            docDHeader.SaveAs(nomeDelivery);
            files.Enqueue(nomeDelivery);

            #endregion

            #region Immagine
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    System.Drawing.Image myImg = System.Drawing.Image.FromFile(Path.Combine(Server.MapPath(@"~/Report"), "montaggio.png"));

            //    myImg.Save(ms, myImg.RawFormat);  // Save your picture in a memory stream.
            //    ms.Seek(0, SeekOrigin.Begin);

            //    Novacode.Image img = docMain.AddImage(ms); // Create image.

            //    // Insert an emptyParagraph into this document.
            //    Novacode.Paragraph p = docMain.InsertParagraph("", false);

            //    Picture pic1 = img.CreatePicture();     // Create picture.
            //    //   pic1.SetPictureShape(BasicShapes.cube); // Set picture shape (if needed)

            //    p.InsertPicture(pic1, 0); // Insert picture into paragraph.

            //}
            #endregion

            docMain.SaveAs(fileNameSaveAs);
            docMain.Dispose();

            id = 0;
            foreach (var file in files.Reverse())
            {
                using (WordprocessingDocument myDoc = WordprocessingDocument.Open(fileNameSaveAs, true))
                {
                    var altChunkId = "AltChunkId" + id++;
                    Console.WriteLine(altChunkId);
                    var mainPart = myDoc.MainDocumentPart;
                    var chunk = mainPart.AddAlternativeFormatImportPart(DocumentFormat.OpenXml.Packaging.AlternativeFormatImportPartType.WordprocessingML, altChunkId);
                    using (System.IO.FileStream fileStream = System.IO.File.Open(file, System.IO.FileMode.Open))
                    {
                        chunk.FeedData(fileStream);
                    }
                    var altChunk = new DocumentFormat.OpenXml.Wordprocessing.AltChunk();
                    altChunk.Id = altChunkId;

                    var last = mainPart.Document.Body.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().Last();
                    mainPart.Document.Body.InsertAfter(altChunk, last);
                    mainPart.Document.Save();

                }

            }

            //// Open a doc file.
            //Application application = new Application();
            //Microsoft.Office.Interop.Word.Document document = application.Documents.Open(fileNameSaveAs, OpenAndRepair: true);


            //Object fnsa = (Object)fileNameSaveAsAfterRepair;
            //// Object of Missing "Null Value".
            //Object oMissing = System.Reflection.Missing.Value;

            //// Object of false.
            //Object oFalse = false;
            //// Save the document.
            //document.SaveAs
            //(
            //    ref fnsa, ref oMissing, ref oMissing, ref oMissing,
            //    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            //    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
            //    ref oMissing, ref oMissing
            //);            // Close word.

            //document.Close();
            //application.Quit();


            return File(fileNameSaveAsAfterRepair, "application/file", retName + ".docx");

        }

        public ActionResult PrintOffer(string codProduct)
        {

            var reportName = "";
            var dbName = "";
            int id = 0;

            //carico i documentProduct in preventivo
            var docsProduct = documentRepository.GetDocumentProductsByCodProduct(codProduct);

            //carico l'ordine
            var estimate = documentRepository.GetAll().OfType<Estimate>().FirstOrDefault(x => x.CodDocument == docsProduct.FirstOrDefault().CodDocument);

            //cliente dell'ordine
            Customer cust = (Customer)customerSupplierRepository.GetSingle(estimate.CodCustomer);
            estimate.CustomerSupplier = cust;

            string fileNameMain = Path.Combine(Server.MapPath(@"~/Report"), (reportName == "" || reportName == null) ? "OfferHead.docx" : (reportName + ".docx"));

            string retName = estimate.EstimateNumberSerie + "-" + estimate.EstimateNumber;
            retName = retName.PurgeFileName();

            string fileNameSaveAs = Path.Combine(Server.MapPath(@"~/Report"), retName + dbName + ".docx");
            string fileNameSaveAsAfterRepair = fileNameSaveAs;// Path.Combine(Server.MapPath(@"~/Report"), retName + "AfterRepair.docx");

            // Store a global reference to the executing assembly.
            g_assembly = Assembly.GetExecutingAssembly();

            //// Create the document in memory:
            var docMain = DocX.Load(fileNameMain);

            //questo array mi serve per il merge
            Queue<string> files = new Queue<string>();

            estimate.MergeField(docMain);

            var product = productRepository.GetSingle(codProduct);
            product.MergeField(docMain);

            //    product.ProductParts.FirstOrDefault().ProductPartPrintableArticles.FirstOrDefault().MergeField(docMain);

            var documentProducts = estimate.DocumentProducts.Where(x => x.CodProduct == codProduct).OrderBy(x => x.Quantity);

            String hashControl = "";
            String hashControlCalculated = "";
            String nomeExtHeaderDest = "";
            String costiFileDest = "";

            foreach (var docProd in documentProducts)
            {
                var dProdFileNameSource = Path.Combine(Server.MapPath(@"~/Report"), "DocumentProduct" + dbName + ".docx");
                if (!System.IO.File.Exists(dProdFileNameSource))
                {
                    dProdFileNameSource = Path.Combine(Server.MapPath(@"~/Report"), "DocumentProduct" + ".docx");
                }

                docProd.Product = product;
                var docPrint = DocX.Load(dProdFileNameSource);

                docProd.MergeField(docPrint);

                costiFileDest = Path.Combine(Server.MapPath(@"~/Report"), "DocPro" + docProd.CodDocumentProduct + ".docx");
                docPrint.SaveAs(costiFileDest);

                #region costi supplementari

                //questo array mi serve per il merge
                Queue<string> filesExtCost = new Queue<string>();

                var costs = docProd.Costs;

                //0=incluso, 1=Aux, 2=escluso
                var extCost = costs.Where(x => x.TypeOfCalcolous == 1 && (x.Quantity ?? 0) != 0);
                //prestampa
                foreach (var cost in extCost)
                {
                    var docPrePressFile = Path.Combine(Server.MapPath(@"~/Report"), "ExternalCost" + dbName + ".docx");
                    if (!System.IO.File.Exists(docPrePressFile))
                    {
                        docPrePressFile = Path.Combine(Server.MapPath(@"~/Report"), "ExternalCost.docx");
                    }
                    var docPrePress = DocX.Load(docPrePressFile);

                    cost.MergeField(docPrePress);
                    docPrePress.SaveAs(Path.Combine(Server.MapPath(@"~/Report"), "ExtCost" + cost.CodCost + ".docx"));
                    filesExtCost.Enqueue(Path.Combine(Server.MapPath(@"~/Report"), "ExtCost" + cost.CodCost + ".docx"));

                    hashControlCalculated = cost.Quantity.Value.ToString() + cost.TotalCost;
                }

                //fusione dei file COSTI SUPPLEMENTARI 

                #region external cost header
                //apro l'header dei costi supplementari e lo salvo
                nomeExtHeaderDest = Path.Combine(Server.MapPath(@"~/Report"), "ExtCostHeader" + estimate.CodDocument + ".docx");

                var docECHeaderFile = Path.Combine(Server.MapPath(@"~/Report"), "ExternalCostHeader" + dbName + ".docx");
                if (!System.IO.File.Exists(docECHeaderFile))
                {
                    docECHeaderFile = Path.Combine(Server.MapPath(@"~/Report"), "ExternalCostHeader.docx");
                }
                var docECHeader = DocX.Load(docECHeaderFile);

                docECHeader.SaveAs(nomeExtHeaderDest);




                foreach (var file in filesExtCost.Reverse())
                {
                    using (WordprocessingDocument myDoc = WordprocessingDocument.Open(nomeExtHeaderDest, true))
                    {
                        var altChunkId = "AltChunkId" + id++;
                        Console.WriteLine(altChunkId);
                        var mainPart = myDoc.MainDocumentPart;
                        var chunk = mainPart.AddAlternativeFormatImportPart(DocumentFormat.OpenXml.Packaging.AlternativeFormatImportPartType.WordprocessingML, altChunkId);
                        using (System.IO.FileStream fileStream = System.IO.File.Open(file, System.IO.FileMode.Open))
                        {
                            chunk.FeedData(fileStream);
                        }
                        var altChunk = new DocumentFormat.OpenXml.Wordprocessing.AltChunk();
                        altChunk.Id = altChunkId;

                        var last = mainPart.Document.Body.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().Last();

                        mainPart.Document.Body.InsertAfter(altChunk, last);
                        mainPart.Document.Save();
                    }
                }


                #endregion
                #endregion

                if (hashControl != "")
                {
                    if (hashControl != hashControlCalculated)
                    {
                        //metto in coda l'header
                        files.Enqueue(nomeExtHeaderDest);                        
                    }
                }

                hashControl = hashControlCalculated;
                

                files.Enqueue(costiFileDest);

            }

            //metto in coda l'header
            files.Enqueue(nomeExtHeaderDest);


            docMain.SaveAs(fileNameSaveAs);
            docMain.Dispose();

            foreach (var file in files.Reverse())
            {
                using (WordprocessingDocument myDoc = WordprocessingDocument.Open(fileNameSaveAs, true))
                {
                    var altChunkId = "AltChunkId" + id++;
                    Console.WriteLine(altChunkId);
                    var mainPart = myDoc.MainDocumentPart;
                    var chunk = mainPart.AddAlternativeFormatImportPart(DocumentFormat.OpenXml.Packaging.AlternativeFormatImportPartType.WordprocessingML, altChunkId);
                    using (System.IO.FileStream fileStream = System.IO.File.Open(file, System.IO.FileMode.Open))
                    {
                        chunk.FeedData(fileStream);
                    }
                    var altChunk = new DocumentFormat.OpenXml.Wordprocessing.AltChunk();
                    altChunk.Id = altChunkId;

                    var last = mainPart.Document.Body.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().Last();

                    mainPart.Document.Body.InsertAfter(altChunk, last);
                    mainPart.Document.Save();
                }
            }


            return File(fileNameSaveAsAfterRepair, "application/file", retName + ".docx");

        }


        //public ActionResult PrintOrder(string codDocument, string reportName)
        //{
        //    //carico l'ordine
        //    Order order = documentRepository.GetAll().OfType<Order>().FirstOrDefault(x => x.CodDocument == codDocument);

        //    //cliente dell'ordine
        //    Customer cust = (Customer)customerSupplierRepository.GetSingle(order.CodCustomer);
        //    order.CustomerSupplier = cust;

        //    //il suo prodotto con la sua quantità
        //    order.OrderProduct = documentRepository.GetDocumentProductByCodDocumentProduct(order.CodDocumentProduct);

        //    //estimate
        //    order.OrderProduct.Document = (Estimate)documentRepository.GetSingle(order.OrderProduct.CodDocument);

        //    string fileNameMain = Path.Combine(Server.MapPath(@"~/Report"), (reportName == "" || reportName == null) ? "OrderHead.docx" : (reportName + ".docx"));
        //    string fileNameCost = Path.Combine(Server.MapPath(@"~/Report"), "Cost.docx");

        //    string retName = order.OrderNumberSerie + "-" + order.OrderNumber;
        //    retName = retName.PurgeFileName();

        //    string fileNameSaveAs = Path.Combine(Server.MapPath(@"~/Report"), retName + ".docx");
        //    string fileNameSaveAsAfterRepair = fileNameSaveAs;// Path.Combine(Server.MapPath(@"~/Report"), retName + "AfterRepair.docx");

        //    // Store a global reference to the executing assembly.
        //    g_assembly = Assembly.GetExecutingAssembly();

        //    //// Create the document in memory:
        //    var docMain = DocX.Load(fileNameMain);

        //    //questo array mi serve per il merge
        //    Queue<string> files = new Queue<string>();
        //    //questo array mi serve per il merge
        //    Queue<string> filesExtCost = new Queue<string>();
        //    //questo array mi serve per il merge
        //    Queue<string> filesDelivery = new Queue<string>();

        //    order.MergeField(docMain);
        //    order.OrderProduct.MergeField(docMain);
        //    order.OrderProduct.Product.MergeField(docMain);

        //    var product = productRepository.GetSingle(order.OrderProduct.Product.CodProduct);
        //    product.ProductParts.FirstOrDefault().MergeField(docMain);

        //    //    product.ProductParts.FirstOrDefault().ProductPartPrintableArticles.FirstOrDefault().MergeField(docMain);

        //    var costs = order.OrderProduct.Costs.OrderBy(x => x.IndexOf);

        //    Console.WriteLine(costs);

        //    foreach (var cost in costs)
        //    {
        //        // Create the document in memory:
        //        //var docCost = DocX.Load(fileNameCost);
        //        //                cost.MergeField(docCost);

        //        //try
        //        //{
        //        var cd = cost.CostDetails.FirstOrDefault();
        //        if (cd != null)
        //        {
        //            cd = costDetailRepository.GetSingle(cd.CodCostDetail);
        //            cd.InitCostDetail(taskExecutorRepository.GetAll(), articleRepository.GetAll());

        //            if (cost.CostDetails.FirstOrDefault().TypeOfCostDetail.ToString() == "PrintedRollArticleCostDetail" ||
        //                cost.CostDetails.FirstOrDefault().TypeOfCostDetail.ToString() == "PrintingZRollCostDetail" ||
        //                cost.CostDetails.FirstOrDefault().TypeOfCostDetail.ToString() == "ControlTableCostDetail")
        //            {
        //                var docPrintCD = DocX.Load(Path.Combine(Server.MapPath(@"~/Report"), cost.CostDetails.FirstOrDefault().TypeOfCostDetail.ToString() + ".docx"));
        //                cd.MergeField(docPrintCD);
        //                cd.TaskCost.MergeField(docPrintCD);
        //                //il merge con il cost

        //                try
        //                {
        //                    cd.TaskCost.ProductPartTask.ProductPart.MergeField(docPrintCD);
        //                }
        //                catch
        //                { }

        //                docPrintCD.SaveAs(Path.Combine(Server.MapPath(@"~/Report"), "Cost" + cost.CodCost + ".docx"));
        //                files.Enqueue(Path.Combine(Server.MapPath(@"~/Report"), "Cost" + cost.CodCost + ".docx"));

        //                #region Merge con Docx     (vecchio metodo)
        //                //var xxx = DocX.Load(Path.Combine(Server.MapPath(@"~/Report"), "Cost" + cost.CodCost + ".docx"));

        //                //docMain.InsertDocument(xxx);
        //                //docPrintCD.Dispose();
        //                //xxx.Dispose();
        //                #endregion

        //            }
        //            else
        //            {
        //                var res = cost.CostDetails.FirstOrDefault().TypeOfCostDetail.ToString();
        //                Console.WriteLine(res);

        //            }
        //            // docMain.InsertDocument(docCost);

        //        }
        //    }

        //    #region costi supplementari

        //    //apro l'header dei costi supplementari e lo salvo

        //    var nomeExt = Path.Combine(Server.MapPath(@"~/Report"), "ExtCostHeader" + order.CodDocument + ".docx");
        //    var docECHeader = DocX.Load(Path.Combine(Server.MapPath(@"~/Report"), "ExternalCostHeader.docx"));
        //    docECHeader.SaveAs(nomeExt);
        //    files.Enqueue(nomeExt);

        //    //0=incluso, 1=Aux, 2=escluso
        //    var extCost = costs.Where(x => x.TypeOfCalcolous == 1 && (x.Quantity ?? 0) != 0);
        //    //prestampa
        //    foreach (var cost in extCost)
        //    {
        //        var docPrePress = DocX.Load(Path.Combine(Server.MapPath(@"~/Report"), "ExternalCost.docx"));
        //        cost.MergeField(docPrePress);
        //        docPrePress.SaveAs(Path.Combine(Server.MapPath(@"~/Report"), "ExtCost" + cost.CodCost + ".docx"));
        //        filesExtCost.Enqueue(Path.Combine(Server.MapPath(@"~/Report"), "ExtCost" + cost.CodCost + ".docx"));
        //    }

        //    int id = 0;
        //    foreach (var file in filesExtCost.Reverse())
        //    {
        //        using (WordprocessingDocument myDoc = WordprocessingDocument.Open(nomeExt, true))
        //        {
        //            var altChunkId = "AltChunkId" + id++;
        //            Console.WriteLine(altChunkId);
        //            var mainPart = myDoc.MainDocumentPart;
        //            var chunk = mainPart.AddAlternativeFormatImportPart(DocumentFormat.OpenXml.Packaging.AlternativeFormatImportPartType.WordprocessingML, altChunkId);
        //            using (System.IO.FileStream fileStream = System.IO.File.Open(file, System.IO.FileMode.Open))
        //            {
        //                chunk.FeedData(fileStream);
        //            }
        //            var altChunk = new DocumentFormat.OpenXml.Wordprocessing.AltChunk();
        //            altChunk.Id = altChunkId;

        //            var last = mainPart.Document.Body.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().Last();

        //            mainPart.Document.Body.InsertAfter(altChunk, last);
        //            mainPart.Document.Save();
        //        }
        //    }


        //    #endregion

        //    #region consegne

        //    //apro l'header dei costi supplementari e lo salvo

        //    var nomeDelivery = Path.Combine(Server.MapPath(@"~/Report"), "DeliveryHeader" + order.CodDocument + ".docx");
        //    var docDHeader = DocX.Load(Path.Combine(Server.MapPath(@"~/Report"), "DeliveryHeader.docx"));
        //    docDHeader.SaveAs(nomeDelivery);
        //    files.Enqueue(nomeDelivery);

        //    #endregion

        //    #region Immagine
        //    //using (MemoryStream ms = new MemoryStream())
        //    //{
        //    //    System.Drawing.Image myImg = System.Drawing.Image.FromFile(Path.Combine(Server.MapPath(@"~/Report"), "montaggio.png"));

        //    //    myImg.Save(ms, myImg.RawFormat);  // Save your picture in a memory stream.
        //    //    ms.Seek(0, SeekOrigin.Begin);

        //    //    Novacode.Image img = docMain.AddImage(ms); // Create image.

        //    //    // Insert an emptyParagraph into this document.
        //    //    Novacode.Paragraph p = docMain.InsertParagraph("", false);

        //    //    Picture pic1 = img.CreatePicture();     // Create picture.
        //    //    //   pic1.SetPictureShape(BasicShapes.cube); // Set picture shape (if needed)

        //    //    p.InsertPicture(pic1, 0); // Insert picture into paragraph.

        //    //}
        //    #endregion

        //    docMain.SaveAs(fileNameSaveAs);
        //    docMain.Dispose();

        //    id = 0;
        //    foreach (var file in files.Reverse())
        //    {
        //        using (WordprocessingDocument myDoc = WordprocessingDocument.Open(fileNameSaveAs, true))
        //        {
        //            var altChunkId = "AltChunkId" + id++;
        //            Console.WriteLine(altChunkId);
        //            var mainPart = myDoc.MainDocumentPart;
        //            var chunk = mainPart.AddAlternativeFormatImportPart(DocumentFormat.OpenXml.Packaging.AlternativeFormatImportPartType.WordprocessingML, altChunkId);
        //            using (System.IO.FileStream fileStream = System.IO.File.Open(file, System.IO.FileMode.Open))
        //            {
        //                chunk.FeedData(fileStream);
        //            }
        //            var altChunk = new DocumentFormat.OpenXml.Wordprocessing.AltChunk();
        //            altChunk.Id = altChunkId;

        //            var last = mainPart.Document.Body.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().Last();

        //            mainPart.Document.Body.InsertAfter(altChunk, last);
        //            mainPart.Document.Save();
        //        }
        //    }

        //    //// Open a doc file.
        //    //Application application = new Application();
        //    //Microsoft.Office.Interop.Word.Document document = application.Documents.Open(fileNameSaveAs, OpenAndRepair: true);


        //    //Object fnsa = (Object)fileNameSaveAsAfterRepair;
        //    //// Object of Missing "Null Value".
        //    //Object oMissing = System.Reflection.Missing.Value;

        //    //// Object of false.
        //    //Object oFalse = false;
        //    //// Save the document.
        //    //document.SaveAs
        //    //(
        //    //    ref fnsa, ref oMissing, ref oMissing, ref oMissing,
        //    //    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
        //    //    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
        //    //    ref oMissing, ref oMissing
        //    //);            // Close word.

        //    //document.Close();
        //    //application.Quit();


        //    return File(fileNameSaveAsAfterRepair, "application/file", retName + ".docx");

        //}

        /// <summary>
        /// This Action returns partial view of CosteDetail
        /// </summary>
        /// <param name="codTaskExecutor"></param>
        /// <param name="codCost"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetPrintingZRollCostDetailPartial(String codTaskExecutor, String codCost)
        {

            PrintingCostDetail cv = (PrintingCostDetail)Session["CostDetail"];
            cv.CodTaskExecutorSelected = codTaskExecutor;
            cv.TaskexEcutorSelected = taskExecutorRepository.GetSingle(codTaskExecutor);
            cv.InitCostDetail(taskExecutorRepository.GetAll(), articleRepository.GetAll());

            ((PrintingZRollCostDetail)cv).BuyingFormat = null;
            ((PrintingZRollCostDetail)cv).PrintingFormat = null;

            ((PrintingZRollCostDetail)(cv)).FuzzyAlgo();


            ((PrintingZRollCostDetail)cv).BuyingFormat =
             (((PrintingZRollCostDetail)cv).BuyingFormat == "" || ((PrintingZRollCostDetail)cv).BuyingFormat == null) ?
             (((PrintingZRollCostDetail)cv).BuyingFormats != null) && (((PrintingZRollCostDetail)cv).BuyingFormats.Count > 0) ? ((PrintingZRollCostDetail)cv).BuyingFormats.FirstOrDefault() : null
             : ((PrintingZRollCostDetail)cv).BuyingFormat;

            //TODO: E' da calcolare il formato di stampa a seconda del formato macchina
            ((PrintingZRollCostDetail)cv).PrintingFormat =
            (((PrintingZRollCostDetail)cv).PrintingFormat == "" || ((PrintingZRollCostDetail)cv).PrintingFormat == null) ?
            ((PrintingZRollCostDetail)cv).BuyingFormat
            : ((PrintingZRollCostDetail)cv).PrintingFormat;


            cv.Update();
            Session["CostDetail"] = cv;
            //var myFirstTask = System.Threading.Tasks.Task.Factory.StartNew(() => SaveCostDetail());

            SaveCostDetail();
            return PartialView("_" + cv.TypeOfCostDetail.ToString(), cv);
        }

        [HttpPost]
        public ActionResult ChangeDocumentProductMarkup(Double markup, String codDocumentProduct)
        {
            var docProduct = documentRepository.GetDocumentProductByCodDocumentProduct(codDocumentProduct);

            var doc = documentRepository.GetSingle(docProduct.CodDocument);
            docProduct = doc.DocumentProducts.FirstOrDefault(x => x.CodDocumentProduct == codDocumentProduct);

            docProduct.Markup = markup;
            docProduct.UpdateTotal();

            documentRepository.Edit(doc);
            documentRepository.Save();

            return PartialView("_PricePartial", docProduct);
        }


        [HttpPost]
        public ActionResult ChangeDocumentProductUnitPrice(string unitPrice, String codDocumentProduct)
        {
            var docProduct = documentRepository.GetDocumentProductByCodDocumentProduct(codDocumentProduct);

            var doc = documentRepository.GetSingle(docProduct.CodDocument);
            docProduct = doc.DocumentProducts.FirstOrDefault(x => x.CodDocumentProduct == codDocumentProduct);

            docProduct.UnitPriceLocked = !(unitPrice == "" || unitPrice == null);

            docProduct.UnitPrice = unitPrice;
            docProduct.UpdateTotal();

            documentRepository.Edit(doc);
            documentRepository.Save();

            return PartialView("_PricePartial", docProduct);
        }




        [HttpPost]
        public ActionResult GetPartialCost(String codTaskExecutor, String codCost)
        {
            CostDetail cv = (CostDetail)Session["CostDetail"];

            cv.CodTaskExecutorSelected = codTaskExecutor;
            cv.Update();

            costDetailRepository.Edit(cv);
            costDetailRepository.Save();

            Session["CostDetail"] = cv;
            return PartialView("_" + cv.TypeOfCostDetail.ToString(), cv);
        }

        /// <summary>
        /// returns DocumentProduct
        /// </summary>
        /// <param name="codDocumentProduct"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult GetDocumentProduct(String codDocument, String codDocumentProduct)
        {
            var docs = documentRepository.GetSingle(codDocument);
            var docProduct = docs.DocumentProducts.Where(z => z.CodDocumentProduct == codDocumentProduct).FirstOrDefault();

            docProduct.UpdateTotal();
            documentRepository.Edit(docs);
            documentRepository.Save();

            return PartialView("_DocumentProduct", docProduct);
        }


        public ActionResult GetPrintingLabelHints()
        {
            PrintingCostDetail cv = (PrintingCostDetail)Session["CostDetail"];

            var prod = productRepository.GetSingle(cv.ProductPart.CodProduct);
            var prodPart = prod.ProductParts.SingleOrDefault(x => x.CodProductPart == cv.CodProductPart);

            if (cv.TaskexEcutorSelected.TypeOfExecutor == TaskExecutor.ExecutorType.Flexo)
            {
                var x = (PrintingZRollCostDetail)cv;
                var lst = x.BuyingFormats;

                foreach (var item in lst)
                {
                    x.BuyingFormat = item;
                    x.PrintingFormat = item;

                }

            }


            return null;
        }


        [HttpPost]
        public ActionResult AddProductPartPrintRollOption(ProductPartPrintRollOption op)
        {

            var status = "err";

            if (ModelState.IsValid)
            {
                status = "ok";
                var obj = new
                {
                    textStatus = status,
                };

                productRepository.AddProductPartTaskOption(op);
                productRepository.Save();


                PrintingCostDetail cv = (PrintingCostDetail)Session["CostDetail"];
                cv.ProductPart.ProductPartTasks.FirstOrDefault(z => z.CodProductPartTask == op.CodProductPartTask).ProductPartTaskOptions = productRepository.GetProductPartTaskOptions(op.CodProductPartTask).ToList();

                cv.Update();
                Session["CostDetail"] = cv;

                SaveCostDetail();

                return Json(obj, JsonRequestBehavior.AllowGet);

            }
            else
            {
                //this is error
                status = "err";
                var retPW = PartialView(this, "_ProductPartPrintRollOptionError", op);

                var obj = new
                {
                    textStatus = status,
                    view = retPW
                };

                return Json(obj, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult DeleteProductPartTaskOption(string ids)
        {

            string[] strings = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(ids);

            var status = "err";

            try
            {
                status = "ok";
                var obj = new
                {
                    textStatus = status,
                };

                CostDetail cv = (CostDetail)Session["CostDetail"];

                string codProductPartTask = "";

                foreach (var id in strings.ToList())
                {
                    codProductPartTask = productRepository.DeleteProductPartTaskOption(id);
                    productRepository.Save();
                }

                var x = productRepository.GetProductPartTaskOptions(codProductPartTask).ToList();
                var productPartTask = cv.ProductPart.ProductPartTasks.FirstOrDefault(z => z.CodProductPartTask == codProductPartTask);


                productPartTask.ProductPartTaskOptions = x;

                cv.Update();
                Session["CostDetail"] = cv;

                SaveCostDetail();

                return Json(obj, JsonRequestBehavior.AllowGet);


            }
            catch (Exception)
            {

                //this is error
                status = "err";
                var obj = new
                {
                    textStatus = status,
                };
                return Json(obj, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult AddProductPartHotPrintingOption(ProductPartHotPrintingOption op)
        {

            var status = "err";

            if (ModelState.IsValid)
            {
                status = "ok";
                var obj = new
                {
                    textStatus = status,
                };

                productRepository.AddProductPartTaskOption(op);
                productRepository.Save();

                RepassRollCostDetail cv = (RepassRollCostDetail)Session["CostDetail"];
                cv.ProductPart.ProductPartTasks.FirstOrDefault(z => z.CodProductPartTask == op.CodProductPartTask).ProductPartTaskOptions = productRepository.GetProductPartTaskOptions(op.CodProductPartTask).ToList();

                cv.Update();
                Session["CostDetail"] = cv;

                SaveCostDetail();

                return Json(obj, JsonRequestBehavior.AllowGet);

            }
            else
            {
                //this is error
                status = "err";

                var retPW = PartialView(this, "_ProductPartHotPrintingOptionError", op);

                var obj = new
                {
                    textStatus = status,
                    view = retPW
                };

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AddProductPartSerigraphyOption(ProductPartSerigraphyOption op)
        {

            var status = "err";

            if (ModelState.IsValid)
            {
                status = "ok";
                var obj = new
                {
                    textStatus = status,
                };

                productRepository.AddProductPartTaskOption(op);
                productRepository.Save();

                RepassRollCostDetail cv = (RepassRollCostDetail)Session["CostDetail"];
                cv.ProductPart.ProductPartTasks.FirstOrDefault(z => z.CodProductPartTask == op.CodProductPartTask).ProductPartTaskOptions = productRepository.GetProductPartTaskOptions(op.CodProductPartTask).ToList();

                cv.Update();
                Session["CostDetail"] = cv;

                SaveCostDetail();

                return Json(obj, JsonRequestBehavior.AllowGet);

            }
            else
            {
                //this is error
                status = "err";
                var retPW = PartialView(this, "_ProductPartSerigraphyOptionError", op);

                var obj = new
                {
                    textStatus = status,
                    view = retPW
                };

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
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
                case CostDetail.CostDetailType.PrintingZRollCostDetail:
                    ((PrintingZRollCostDetail)cv).BuyingFormat = buyingFormat;
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

            //var myFirstTask = System.Threading.Tasks.Task.Factory.StartNew(() => SaveCostDetail());
            SaveCostDetail();
            return PartialView(cv.PartialViewName, cv);
        }

        /// <summary>
        /// This Action modifies buyingFormat and Update Cost.
        /// Is needed  Session["CostDetail"]
        /// </summary>
        /// <param name="buyingFormat"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangePerfecting(bool perfecting)
        {
            PrintingCostDetail cv = (PrintingCostDetail)Session["CostDetail"];

            cv.ProductPartPrinting.Perfecting = perfecting;
            cv.Update();

            Session["CostDetail"] = cv;

            //var myFirstTask = System.Threading.Tasks.Task.Factory.StartNew(() => SaveCostDetail());
            SaveCostDetail();
            return PartialView(cv.PartialViewName, cv);
        }



        [HttpGet]
        public ActionResult GetPrintingZRollCostDetailResult()
        {
            PrintingCostDetail cv = (PrintingCostDetail)Session["CostDetail"];
            return PartialView(cv.PartialViewName + "Result", cv);
        }


        /// <summary>
        /// Change the max gain and update cost
        /// </summary>
        /// <param name="maxGain1"></param>
        /// <param name="maxGain2"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeMaxGain(string maxGain1, string maxGain2, string forceSide)
        {
            PrintingCostDetail cv = (PrintingCostDetail)Session["CostDetail"];
            var inizio = DateTime.Now;

            if (ModelState.IsValid)
            {
                cv.ProductPartPrinting.MaxGain1 = Convert.ToInt32(maxGain1);
                cv.ProductPartPrinting.MaxGain2 = Convert.ToInt32(maxGain2);

                cv.ProductPartPrinting.ForceSide = Convert.ToInt32(forceSide);
            }

            cv.Update();
            Session["CostDetail"] = cv;

            //var myFirstTask = System.Threading.Tasks.Task.Factory.StartNew(() => SaveCostDetail());
            SaveCostDetail();
            return PartialView(cv.PartialViewName, cv);
        }


        /// <summary>
        /// Change the max gain and update cost
        /// </summary>
        /// <param name="maxGain1"></param>
        /// <param name="maxGain2"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeGain(string maxGain1, string maxGain2, string forceSide)
        {
            PrintingCostDetail cv = (PrintingCostDetail)Session["CostDetail"];
            var inizio = DateTime.Now;

            if (ModelState.IsValid)
            {
                cv.ProductPartPrinting.MaxGain1 = Convert.ToInt32(maxGain1);
                cv.ProductPartPrinting.MaxGain2 = Convert.ToInt32(maxGain2);

                cv.ProductPartPrinting.ForceGain = true;
                cv.ProductPartPrinting.ForceSide = Convert.ToInt32(forceSide);
            }

            cv.Update();
            Session["CostDetail"] = cv;

            //var myFirstTask = System.Threading.Tasks.Task.Factory.StartNew(() => SaveCostDetail());
            SaveCostDetail();

            return PartialView(cv.PartialViewName, cv);
        }


        [HttpPost]
        public ActionResult ChangeLateralAndFuzzy(string lateral)
        {
            PrintingCostDetail cv = (PrintingCostDetail)Session["CostDetail"];

            cv.ForceLateral = true;
            cv.Lateral = Convert.ToDouble(lateral);
            
            cv.Update();
            Session["CostDetail"] = cv;

            SaveCostDetail();

            return GetPrintingZRollCostDetailPartial(cv.CodTaskExecutorSelected, cv.CodCostDetail);
            //return PartialView(cv.PartialViewName, cv);
        }



        [HttpPost]
        public ActionResult RemoveLateralAndFuzzy()
        {
            PrintingCostDetail cv = (PrintingCostDetail)Session["CostDetail"];

            cv.ForceLateral = false;

            cv.Update();
            Session["CostDetail"] = cv;

            SaveCostDetail();

            return GetPrintingZRollCostDetailPartial(cv.CodTaskExecutorSelected, cv.CodCostDetail);
            //return PartialView(cv.PartialViewName, cv);
        }



        [HttpPost]
        public ActionResult ChangePPartFormatAndFuzzy(string format, string dCut1, string dCut2, string minDCut, string maxDCut, string maxGain1 = "", string maxGain2 = "")
        {
            var x = ChangePPartFormat(format, dCut1, dCut2, minDCut, maxDCut, maxGain1, maxGain2);
            PrintingCostDetail cv = (PrintingCostDetail)Session["CostDetail"];
            return GetPrintingZRollCostDetailPartial(cv.CodTaskExecutorSelected, cv.CodCostDetail);
        }
        /// <summary>
        /// This Action modifies ProductPart format and Update Cost
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangePPartFormat(string format, string dCut1, string dCut2, string minDCut, string maxDCut, string maxGain1 = "", string maxGain2 = "")
        {

            string[] formats = new string[2];
            formats = format.Split('+');

            for (int i = 0; i < formats.Length; i++)
            {
                formats[i] = formats[i].Trim();
            }

            PrintingCostDetail cv = (PrintingCostDetail)Session["CostDetail"];

            var inizio1 = DateTime.Now;

            if (ModelState.IsValid)
            {
                //catch ProductPart from repository and save it with canges
                var prod = productRepository.GetSingle(cv.ProductPart.CodProduct);
                var prodPart = prod.ProductParts.Where(x => x.CodProductPart == cv.CodProductPart).FirstOrDefault();

                try
                {
                    prodPart.DCut1 = dCut1 == "" ? 0 : Convert.ToDouble(dCut1);
                    prodPart.DCut2 = dCut2 == "" ? 0 : Convert.ToDouble(dCut2);

                    prodPart.MinDCut = minDCut == "" ? prodPart.MinDCut : Convert.ToDouble(minDCut);
                    prodPart.MaxDCut = maxDCut == "" ? prodPart.MaxDCut : Convert.ToDouble(maxDCut);

                    ////NUOVODCUT
                    //var max = Math.Max(prodPart.DCut1??0, prodPart.DCut2??0);
                    //var min = Math.Min(prodPart.DCut1 ?? 0, prodPart.DCut2 ?? 0);

                    //max = Math.Max(max, prodPart.MaxDCut??0);
                    //min = Math.Min(min, prodPart.MinDCut??0);

                    //if ((prodPart.MinDCut??0)>min)
                    //{
                    //    prodPart.MinDCut = min;                
                    //}

                    //if ((prodPart.MaxDCut ?? 0) < max)
                    //{
                    //    prodPart.MaxDCut = max;
                    //}

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

                prodPart.Format = formats[0];

                if (prodPart.TypeOfProductPart == ProductPart.ProductPartType.ProductPartDoubleLabelRoll)
                {
                    ((ProductPartDoubleLabelRoll)prodPart).FormatA = formats[0];
                    ((ProductPartDoubleLabelRoll)prodPart).FormatB = formats[1];
                }


                if (TryValidateModel(prodPart))
                {

                    cv.ProductPart.IsDCut = true;
                    try
                    {
                        cv.ProductPart.DCut1 = Convert.ToDouble(dCut1.Trim() == "" ? "0" : dCut1);
                    }
                    catch (Exception)
                    {
                        cv.ProductPart.DCut1 = 0;
                    }

                    try
                    {
                        cv.ProductPart.DCut2 = Convert.ToDouble(dCut2.Trim() == "" ? "0" : dCut2);
                    }
                    catch (Exception)
                    {
                        cv.ProductPart.DCut2 = 0;
                    }

                    cv.ProductPart.MinDCut = minDCut == "" ? cv.ProductPart.MinDCut : Convert.ToDouble(minDCut);
                    cv.ProductPart.MaxDCut = maxDCut == "" ? cv.ProductPart.MaxDCut : Convert.ToDouble(maxDCut);

                    //var max = Math.Max(cv.ProductPart.DCut1 ?? 0, cv.ProductPart.DCut2 ?? 0);
                    //var min = Math.Min(cv.ProductPart.DCut1 ?? 0, cv.ProductPart.DCut2 ?? 0);

                    //max = Math.Max(max, cv.ProductPart.MaxDCut??0);
                    //min = Math.Min(min, cv.ProductPart.MinDCut??0);

                    //if ((cv.ProductPart.MinDCut ?? 0) > min)
                    //{
                    //    cv.ProductPart.MinDCut = min;
                    //}

                    //if ((cv.ProductPart.MaxDCut ?? 0) < max)
                    //{
                    //    cv.ProductPart.MaxDCut = max;
                    //}

                    cv.ProductPart.Format = formats[0];

                    if (cv.ProductPart.TypeOfProductPart == ProductPart.ProductPartType.ProductPartDoubleLabelRoll)
                    {
                        ((ProductPartDoubleLabelRoll)cv.ProductPart).FormatA = formats[0];
                        ((ProductPartDoubleLabelRoll)cv.ProductPart).FormatB = formats[1];
                    }

                    cv.ProductPart.UpdateOpenedFormat();

                    productRepository.Edit(prod);
                    productRepository.Save();

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

            if (ModelState.IsValid)
            {
                maxGain1 = maxGain1 == "" ? "0" : maxGain1;
                maxGain2 = maxGain2 == "" ? "0" : maxGain2;

                cv.ProductPartPrinting.MaxGain1 = Convert.ToInt32(maxGain1);
                cv.ProductPartPrinting.MaxGain2 = Convert.ToInt32(maxGain2);
            }

            cv.Update();
            Session["CostDetail"] = cv;

            var inizio2 = DateTime.Now;

            //var myFirstTask = System.Threading.Tasks.Task.Factory.StartNew(() => SaveCostDetail());
            SaveCostDetail();

            var tempo1 = DateTime.Now.Subtract(inizio1);
            var tempo2 = DateTime.Now.Subtract(inizio2);


            Console.WriteLine(tempo1.TotalSeconds);
            Console.WriteLine(tempo2.TotalSeconds);

            return PartialView(cv.PartialViewName, cv);

        }





        /// <summary>
        /// Action modifies PrintingFormat and Update Cost
        /// </summary>
        /// <param name="PrintingFormat"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangePrintingFormatAndPPartFormat(string PrintingFormat, string format, string dCut1, string dCut2, string minDCut, string maxDCut)
        {
            PrintingCostDetail cv = (PrintingCostDetail)Session["CostDetail"];
            cv.PrintingFormat = PrintingFormat;
            Session["CostDetail"] = cv;

            var myFirstTask = System.Threading.Tasks.Task.Factory.StartNew(() => SaveCostDetail());

            return ChangePPartFormat(format, dCut1, dCut2, minDCut, maxDCut);
        }

        /// <summary>
        /// Action modifies BuyingFormat and Update Cost
        /// </summary>
        /// <param name="BuyingFormat"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeBuyingFormatAndPPartFormat(string buyingFormat, string format, string dCut1, string dCut2, string maxGain1 = "", string maxGain2 = "")
        {
            PrintingCostDetail cv = (PrintingCostDetail)Session["CostDetail"];


            switch (cv.TypeOfCostDetail)
            {
                case CostDetail.CostDetailType.PrintingZRollCostDetail:
                    ((PrintingZRollCostDetail)cv).BuyingFormat = buyingFormat;
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

            //   var myFirstTask = System.Threading.Tasks.Task.Factory.StartNew(() => SaveCostDetail());

            return ChangePPartFormat(format, dCut1, dCut2, "", "", maxGain1, maxGain2);
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

            //var myFirstTask = System.Threading.Tasks.Task.Factory.StartNew(() => SaveCostDetail());
            SaveCostDetail();
            return PartialView(cv.PartialViewName, cv);
        }

        [HttpPost]
        public ActionResult ChangePrintingFormatRepass(string PrintingFormat)
        {
            PrePostPressCostDetail cv = (PrePostPressCostDetail)Session["CostDetail"];
            PapiroService p = (PapiroService)Session["PapiroService"];

            cv.WorkingFormat = PrintingFormat;
            cv.Update();
            Session["CostDetail"] = cv;

            SaveCostDetail();

            var lst = p.CostDetailsDic.Select(x => x.Value).OfType<PrePostPressCostDetail>().Where(y => y.CodTaskExecutorSelected == cv.CodTaskExecutorSelected);

            foreach (var item in lst.Where(x => x.CodCostDetail != cv.CodCostDetail))
            {
                item.WorkingFormat = PrintingFormat;
                item.Update();
                Session["CostDetail" + item.CodCostDetail] = item;

                SaveCostDetail(item.CodCostDetail);

                //System.Threading.Tasks.Task.Factory.StartNew(() => {
                ////var myFirstTask = System.Threading.Tasks.Task.Factory.StartNew(() => SaveCostDetail(item.CodCostDetail));
                //});

            }

            //            

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

            var idRet = documentRepository.GetCostsByCodDocumentProduct(id).FirstOrDefault().DocumentProduct.CodProduct;

            PapiroService p = new PapiroService();
            p.DocumentRepository = documentRepository;//new DocumentRepository();
            p.CostDetailRepository = costDetailRepository;
            p.TaskExecutorRepository = taskExecutorRepository;
            p.ArticleRepository = articleRepository;
            p.TypeOfTaskRepository = typeOfTaskRepository;

            p.CurrentDatabase = CurrentDatabase;

            p.EditOrCreateAllCost(id);

            return RedirectToAction("EditDocumentProducts", "Document", new { id = idRet });

        }

        [HttpParamAction]
        [HttpGet]
        public ActionResult EditCost(string id)
        {


            var cd = costDetailRepository.GetSingle(id);

            if (cd == null)
            {
                var cost = documentRepository.GetCost(id);
                EditAndCreateAllCost(cost.CodDocumentProduct);
                cd = costDetailRepository.GetSingle(id);
            }

            PapiroService p = new PapiroService();
            p.DocumentRepository = documentRepository;//new DocumentRepository();
            p.CostDetailRepository = costDetailRepository;
            p.TaskExecutorRepository = taskExecutorRepository;
            p.ArticleRepository = articleRepository;
            p.TypeOfTaskRepository = typeOfTaskRepository;
            p.CurrentDatabase = CurrentDatabase;

            p.InitCostDocumentProduct(cd.TaskCost.CodDocumentProduct);

            var cv = p.EditCostAutomatically(id, new Guid());
            //Console.WriteLine(cv.GainForRun);

            if (cv == null)
            {
                return View("NotImplementedCostDetail");
            }

            Session["CostDetail"] = cv;
            Session["PapiroService"] = p;

            Console.Write(cv.Error);

            var viewName = String.Empty;

            switch (cv.TypeOfCostDetail)
            {
                case CostDetail.CostDetailType.PrintingZRollCostDetail:
                case CostDetail.CostDetailType.PrintingFlatRollCostDetail:

                    ((PrintingZRollCostDetail)cv).DieTollerance = 0.5;
                    ((PrintingZRollCostDetail)cv).Dies = articleRepository.GetAll().OfType<Die>().ToList();
                    ((PrintingZRollCostDetail)cv).FuzzyAlgo();
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
                case CostDetail.CostDetailType.ControlTableCostDetail:
                    viewName = "ControlTableCostDetail";
                    break;

                case CostDetail.CostDetailType.PrePostPressCostDetail:
                case CostDetail.CostDetailType.ImplantMeshCostDetail:
                case CostDetail.CostDetailType.ImplantHotPrintingCostDetail:
                case CostDetail.CostDetailType.RepassRollCostDetail:

                    //get ST codCost
                    cv.CodPartPrintingCostDetail = p.DocumentRepository.GetCostsByCodDocumentProduct(cv.TaskCost.CodDocumentProduct).Where(y1 => y1.CodItemGraph == "ST").Select(z => z.CodCost);

                    //if there is a ST
                    if (cv.CodPartPrintingCostDetail != null)
                    {
                        //fix all links
                        foreach (var item in cv.CodPartPrintingCostDetail)
                        {
                            var cv2 = p.CostDetailRepository.GetSingle(item);
                            if (!cv.Printers.Select(x => x.CodCostDetail).Contains(cv2.CodCostDetail))
                            {
                                cv.Printers.Add(cv2);
                                cv2.InitCostDetail(taskExecutorRepository.GetAll(), articleRepository.GetAll());
                            }
                        }
                    }

                    viewName = "PrintingCostDetail";
                    break;

                default:
                    viewName = "ControlTableCostDetail";

                    break;
            }

            Session["CostDetail"] = cv;
            Session["PapiroService"] = p;

            return View(viewName, cv);
        }

        [HttpGet]
        public ActionResult EditCostTroggleLock(string id)
        {

            var cost = documentRepository.GetCost(id);
            cost.Locked = !(cost.Locked ?? false);

            documentRepository.EditCost(cost);
            documentRepository.Save();

            Console.Write(id);
            return null;
        }

        [HttpGet]
        public ActionResult EditCostTroggleInclusion(string id)
        {
            var cost = documentRepository.GetCost(id);
            cost.TypeOfCalcolous = ((cost.TypeOfCalcolous ?? 0) + 1) % 3;

            //dopo il salvataggio del dettaglio del costo voglio aggiornare il cost!!!!

            documentRepository.EditCost(cost);
            documentRepository.Save();

            var doc = documentRepository.GetSingle(cost.DocumentProduct.CodDocument);
            doc.DocumentProducts.FirstOrDefault(x => x.CodDocumentProduct == cost.DocumentProduct.CodDocumentProduct).UpdateTotal();
            documentRepository.Edit(doc);
            documentRepository.Save();


            Console.Write(id);
            return null;
        }

        [HttpPost]
        public ActionResult EditCostManual(string id, string quantity, string unitCost, double markup)
        {

            bool doLock;

            var qta = Convert.ToDouble(quantity);
            var uCost = Convert.ToDouble(unitCost == "" ? "0" : unitCost, Thread.CurrentThread.CurrentUICulture);

            var cost = documentRepository.GetCost(id);

            doLock = !(cost.Quantity == qta);

            cost.Quantity = qta;
            cost.UnitCost = uCost.ToString("#,0.000", Thread.CurrentThread.CurrentUICulture); ;
            cost.Markup = markup;

            var tot = uCost * qta;
            cost.TotalCost = (tot).ToString("#,0.00", Thread.CurrentThread.CurrentUICulture);

            cost.GranTotalCost = (Convert.ToDouble(cost.TotalCost, Thread.CurrentThread.CurrentUICulture) +
                (Convert.ToDouble(cost.TotalCost, Thread.CurrentThread.CurrentUICulture) *
                ((cost.Markup ?? 1) / 100))).ToString("#,0.00", Thread.CurrentThread.CurrentUICulture);


            //se cambio solo il markup non blocco!!!!!
            //se cambio solo il prezzo unitario non blocco
            if (doLock)
            {
                //blocco il costo
                cost.Locked = doLock;
            }

            //dopo il salvataggio del dettaglio del costo voglio aggiornare il cost!!!!
            cost.DocumentProduct.UpdateTotal();

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

            //    case CostDetail.CostDetailType.PrintingZRollCostDetail:
            //        ((PrintingZRollCostDetail)cv).FuzzyAlgo();
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
        //                List<CostDetail> x = ((PrintingCostDetail)cv).CreateRelatedPrintedCostDetail(articleRepository.GetAll(), costs);

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
        //    var dp = documentRepository.GetDocumentProductsByCodProduct(cv.TaskCost.DocumentProduct.CodProduct).Where(x => x.CodDocumentProduct == cv.TaskCost.CodDocumentProduct).FirstOrDefault();

        //    if (dp != null)
        //    {
        //        dp.UpdateCost();
        //    }

        //    documentRepository.Edit(dp.Document);
        //    documentRepository.Save();

        //}


        [HttpParamAction]
        [HttpGet]
        public ActionResult SaveCostDetail(string optCod = "")
        {
            disposable = false;

            CostDetail cv = (CostDetail)Session["CostDetail" + optCod];

            PapiroService p = new PapiroService();
            p.DocumentRepository = documentRepository;//new DocumentRepository();
            p.CostDetailRepository = costDetailRepository;
            p.TaskExecutorRepository = taskExecutorRepository;
            p.ArticleRepository = articleRepository;
            p.TypeOfTaskRepository = typeOfTaskRepository;
            p.CurrentDatabase = CurrentDatabase;

            p.InitCostDocumentProduct(costDetailRepository.GetSingle(cv.CodCost).TaskCost.CodDocumentProduct);

            var cvRet = p.SaveCostDetailFromController(cv);

            var idRet = (string)Session["codProduct"];
            Session["CostDetail" + optCod] = cvRet;

            disposable = true;

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

            var docProd = documentRepository.GetDocumentProductsByCodProduct(id);
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
            prod.DocumentStates = documentRepository.GetAllDocumentStates(id).ToList();

            if (prod == null)
            {
                throw new NotFoundResException();
            }


            var state = documentRepository.GetAllStates().ToArray();

            foreach (var item in prod.DocumentStates)
            {
                if (item.CodState == null && !item.StateName.Contains(" "))
                {
                    item.CodState = item.StateName;
                }

                item.StateName = state.FirstOrDefault(z => z.CodState == item.CodState).StateName;
            }


            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "EditEstimate";
            return View(prod);

        }

        [HttpGet]
        public ActionResult EditOrder(string id)
        {
            //   Session["CodDocument"] = id;
            var prod = documentRepository.GetSingle(id);


            if (prod == null)
            {
                throw new NotFoundResException();
            }

            ((Order)prod).ReportOrderNames = documentRepository.GetAllReportOrderName(CurrentDatabase);
            prod.DocumentStates = documentRepository.GetAllDocumentStates(id).ToList();
            //aggiorno il nome dello stato

            var state = documentRepository.GetAllStates().ToArray();

            foreach (var item in prod.DocumentStates)
            {
                if (item.CodState == null && !item.StateName.Contains(" "))
                {
                    item.CodState = item.StateName;
                }

                item.StateName = state.FirstOrDefault(z => z.CodState == item.CodState).StateName;
            }

            prod.OrderProduct = documentRepository.GetDocumentProductByCodDocumentProduct(prod.CodDocumentProduct);

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "EditOrder";
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
                var est = Session["CodDocument"] != null ? Session["CodDocument"] : NewEstimate();
                return Json(new { redirectUrl = Url.Action("CreateProduct", "Product", new { id = sel.CodMenuProduct }) });
            }

            else
            {
                return Json(new { error = true });
            }

        }


        public ActionResult NewProductNewEstimateById(string id)
        {
            var est = NewEstimate();
            return RedirectToAction("CreateProduct", "Product", new { id = id });
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


            var filteredItems = menuProd.Where(x => x.Name != null).Where(
                item => item.Name.IndexOf(c.NewProduct, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var sel = filteredItems.FirstOrDefault();

            if (sel != null)
            {
                var est = NewEstimate();
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

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditOrder(PapiroMVC.Models.Order c)
        {
            var taskList = this.typeOfTaskRepository.GetAll();
            ((Order)c).ReportOrderNames = documentRepository.GetAllReportOrderName(CurrentDatabase);

            if (ModelState.IsValid)
            {
                try
                {

                    Console.Write(c.DocumentStates);

                    documentRepository.Edit(c);
                    //rigeneration name of article
                    //c.OrderSingleSheet.OrderName = c.OrderSingleSheet.ToString();
                    documentRepository.Save();
                    return Json(new { redirectUrl = Url.Action("ListOrder") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "EditOrder";
            return PartialView("_EditAndCreateOrder", c);
        }

    }


}
