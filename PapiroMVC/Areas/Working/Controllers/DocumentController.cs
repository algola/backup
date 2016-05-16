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
using Newtonsoft.Json;
using PapiroMVC.Validation.Error;
using PapiroMVC.ServiceLayer;


namespace PapiroMVC.Areas.Working.Controllers
{

    //  [CustomHandleErrorAttribute(ExceptionType = typeof(NotImplementedException), View = "Error2")]
    //[AuthorizeAlgola(Roles = "Estimate")]
    public partial class DocumentController : PapiroMVC.Controllers.ControllerAlgolaBase
    {
        private readonly IDocumentRepository documentRepository;
        private readonly ITypeOfTaskRepository typeOfTaskRepository;
        private readonly IProductRepository productRepository;
        protected IMenuProductRepository menu;
        private readonly ITaskExecutorRepository taskExecutorRepository;

        private readonly IArticleRepository articleRepository;
        private readonly ICustomerSupplierRepository customerSupplierRepository;
        private readonly ICostDetailRepository costDetailRepository;
        private readonly ITaskCenterRepository taskCenterRepository;

        private readonly IWarehouseRepository warehouseRepository;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            documentRepository.SetDbName(CurrentDatabase);
            warehouseRepository.SetDbName(CurrentDatabase);
            productRepository.SetDbName(CurrentDatabase);

            typeOfTaskRepository.SetDbName(CurrentDatabase);
            taskExecutorRepository.SetDbName(CurrentDatabase);

            articleRepository.SetDbName(CurrentDatabase);
            customerSupplierRepository.SetDbName(CurrentDatabase);
            costDetailRepository.SetDbName(CurrentDatabase);

            taskCenterRepository.SetDbName(CurrentDatabase);

            //nel view bag voglio il CodDocument corrente!!! questo serve per avere nel menu l'accesso al documento corrente 
            //oppure per crearne uno nuovo vuoto
            if (Session["CodDocument"] == null || documentRepository.GetSingle((string)Session["CodDocument"]) == null)
            {
                ViewBag.CodDocument = null;
            }
            else
            {
                ViewBag.CodDocument = Session["CodDocument"];
            }
        }

        public DocumentController(IDocumentRepository _documentRepository,
            ITypeOfTaskRepository _typeOfTaskRepository,
            IFormatsNameRepository _formatsName,
            IProductRepository _productRepository,
            ITaskExecutorRepository _taskExecutorRepository,
            IArticleRepository _articleRepository,
            ICustomerSupplierRepository _customerSupplierRepository,
            IMenuProductRepository _menuProduct,
            ICostDetailRepository _costDetailRepository,
            ITaskCenterRepository _taskCenterRepository,
            IWarehouseRepository _warehouseDataRep)
        {
            typeOfTaskRepository = _typeOfTaskRepository;
            documentRepository = _documentRepository;
            productRepository = _productRepository;
            taskExecutorRepository = _taskExecutorRepository;
            articleRepository = _articleRepository;
            customerSupplierRepository = _customerSupplierRepository;
            menu = _menuProduct;
            costDetailRepository = _costDetailRepository;
            taskCenterRepository = _taskCenterRepository;
            warehouseRepository = _warehouseDataRep;


            this.Disposables.Add(typeOfTaskRepository);
            this.Disposables.Add(documentRepository);
            this.Disposables.Add(productRepository);
            this.Disposables.Add(taskExecutorRepository);
            this.Disposables.Add(articleRepository);
            this.Disposables.Add(customerSupplierRepository);
            this.Disposables.Add(menu);
            this.Disposables.Add(costDetailRepository);
            this.Disposables.Add(taskCenterRepository);
            this.Disposables.Add(warehouseRepository);


        }


        /// <summary>
        /// Create and send by Json the DocumentProduct Cost description 
        /// </summary>
        /// <param name="CodDocumentProduct"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PrintDocumentProductCosts(string CodDocumentProduct)
        {

            DocumentProduct docPro = documentRepository.GetDocumentProductByCodDocumentProduct(CodDocumentProduct);
            Console.WriteLine(docPro.Product.ProductName);

            var res = String.Empty;

            //regen doc
            docPro.DocumentProductNameGenerator = "";

            docPro.FgDescription = "Fg.";
            docPro.MlDescription = "Ml.";
            docPro.MqDescription = "Mq.";
            docPro.NrDescription = "Nr.";
            docPro.UpDescription = "Cad €";
            docPro.UpDescription1000 = "X 1000 €";
            docPro.AmountDescription = "Totale €";
            docPro.QtyDescription = "Nr.";

            docPro.ToName();
            Console.WriteLine(docPro.DocumentProductNameGenerator);

            res = (docPro.Product.ProductName + "@@" + docPro.DocumentProductNameGenerator).Replace("@", Environment.NewLine);

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveDieFlatRoll(DieFlatRoll Die)
        {
            string status = "ok";
            if (ModelState.IsValid)
            {
                try
                {

                    Die.CodArticle = articleRepository.GetNewCode(Die, customerSupplierRepository, Die.SupplierMaker, Die.SupplierMaker);


                    articleRepository.Add(Die);

                    articleRepository.Save();
                    status = "ok";
                    var obj = new
                    {
                        textStatus = status,
                    };
                    return Json(obj, JsonRequestBehavior.AllowGet);

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            status = "err";
            var retPW = PartialView(this, "~/Areas/Working/Views/Document/_SaveDieFlatRoll.cshtml", (Die)Die);

            var obj2 = new
            {
                textStatus = status,
                view = retPW
            };

            return Json(obj2, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult SaveDieFlexo(DieFlexo die)
        {
            string status = "ok";



            try
            {
                string[] formats = new string[2];
                formats = die.Format.Split('+');

                for (int i = 0; i < formats.Length; i++)
                {
                    formats[i] = formats[i].Trim();
                }

                die.Format = formats[0];
                die.FormatB = formats[1];

            }
            catch (Exception)
            {

            }

            ModelState.Clear();
            TryValidateModel(die);

            if (ModelState.IsValid)
            {
                try
                {


                    die.CodArticle = articleRepository.GetNewCode(die, customerSupplierRepository, die.SupplierMaker, die.SupplierMaker);

                    articleRepository.Add(die);

                    articleRepository.Save();
                    status = "ok";
                    var obj = new
                    {
                        textStatus = status,
                    };
                    return Json(obj, JsonRequestBehavior.AllowGet);

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            status = "err";
            var retPW = PartialView(this, "~/Areas/Working/Views/Document/_SaveDieFlexo.cshtml", (Die)die);

            var obj2 = new
            {
                textStatus = status,
                view = retPW
            };

            return Json(obj2, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// This command invokes the cloning of DocumentProduct in the same Document with a new quantity
        /// </summary>
        /// <param name="codDocument"></param>
        /// <param name="codDocumentProduct"></param>
        /// <param name="newQuantity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CloneDocumentProduct(string codDocument, string codDocumentProduct, int newQuantity)
        {
            Document doc = documentRepository.GetSingle(codDocument);

            var codProduct = doc.DocumentProducts.FirstOrDefault(x => x.CodDocumentProduct == codDocumentProduct).CodProduct;

            var prod = documentRepository.GetDocumentProductsByCodProduct(codProduct).First();

            if (newQuantity > 0)
            {
                PapiroService p = new PapiroService();
                p.DocumentRepository = documentRepository;
                p.CostDetailRepository = costDetailRepository;

                //get product document and clone it
                DocumentProduct newProdDoc = (DocumentProduct)prod.Clone();
                //with new quantity
                newProdDoc.Quantity = newQuantity;

                newProdDoc.CodDocumentProduct = "";
                newProdDoc.Document = null;
                //the document product cloned is added to Document
                doc.DocumentProducts.Add(newProdDoc);
                //and all codes are regenerated
                doc.DocumentProductsCodeRigen(true);

                //keep new Code
                var newCodDocumentProduct = newProdDoc.CodDocumentProduct;

                p.DocumentRepository.Edit(doc);
                p.DocumentRepository.Save();

                p.DocumentRepository.SetDbName(CurrentDatabase);

                //array di sostituzione dei codici
                Dictionary<string, string> trans = new Dictionary<string, string>();

                //now we have to clone cost and add it in new DocumentProduct
                foreach (var c in prod.Costs)
                {
                    //get cost
                    var cost = p.CostDetailRepository.GetSingleSimple(c.CodCost);

                    //if cost exist
                    if (cost != null)
                    {
                        //clone it
                        var newCost = (CostDetail)cost.Clone();

                        //get new CostCode by replace code part about CodDocumentProduct
                        newCost.CodCostDetail = newCost.CodCostDetail.Replace(prod.CodDocumentProduct, newProdDoc.CodDocumentProduct);
                        newCost.CodCost = newCost.CodCostDetail;

                        newCost.CostDetailCostCodeRigen();

                        //if cost is computed by another one we have to fix the link
                        if (newCost.CodComputedBy != null)
                        {
                            newCost.CodComputedBy = newCost.CodComputedBy.Replace(prod.CodDocumentProduct, newProdDoc.CodDocumentProduct);
                        }

                        p.CostDetailRepository.Add(newCost);
                    }
                }

                //save all changes
                p.CostDetailRepository.Save();

                //Updating
                //reset contentx
                p.CostDetailRepository.SetDbName(CurrentDatabase);

                foreach (var c in newProdDoc.Costs.OrderBy(x => x.CodCost))
                {
                    var cost = p.CostDetailRepository.GetSingle(c.CodCost);

                    if (cost != null)
                    {

                        //TEMPORANEOOOOOOOOO
                        //another important link to fix is the printer link
                        //this for PrePressCost
                        if (cost.TypeOfCostDetail == CostDetail.CostDetailType.ControlTableCostDetail ||
                            cost.TypeOfCostDetail == CostDetail.CostDetailType.PrePostPressCostDetail ||
                            cost.TypeOfCostDetail == CostDetail.CostDetailType.RepassRollCostDetail
                            )
                        {
                            //get ST codCost
                            cost.CodPartPrintingCostDetail = p.DocumentRepository.GetCostsByCodDocumentProduct(cost.TaskCost.CodDocumentProduct).Where(y1 => y1.CodItemGraph == "ST").Select(z => z.CodCost);

                            //if there is a ST
                            if (cost.CodPartPrintingCostDetail != null)
                            {
                                //fix all links
                                foreach (var item in cost.CodPartPrintingCostDetail)
                                {
                                    var cv2 = p.CostDetailRepository.GetSingle(item);
                                    cost.Printers.Add(cv2);
                                    cv2.InitCostDetail(taskExecutorRepository.GetAll(), articleRepository.GetAll());
                                }
                            }
                        }

                        //regen cost initialization
                        cost.InitCostDetail(taskExecutorRepository.GetAll(), articleRepository.GetAll());
                        //and regen cost


                        //DA RIMUOVERE
                        if (cost.TaskCost.CostDetails.FirstOrDefault(x => x.CodCostDetail == cost.CodCostDetail) == null)
                        {
                            cost.TaskCost.CostDetails.Add(cost);
                        }
                        //********

                        cost.Update();
                        cost.TaskCost.Update();

                        p.CostDetailRepository.Add(cost);
                        p.CostDetailRepository.Save();
                    }

                }

                //save all
                p.CostDetailRepository.Save();

                doc = documentRepository.GetSingle(codDocument);
                newProdDoc = doc.DocumentProducts.FirstOrDefault(x => x.CodDocumentProduct == newCodDocumentProduct);
                newProdDoc.UpdateTotal();

                p.DocumentRepository.Edit(doc);
                p.DocumentRepository.Save();
                // end regen costs
            }

            return Json(new { redirectUrl = Url.Action("EditDocumentProducts", "Document", new { area = "Working", id = prod.CodProduct }) });

        }



        /// <summary>
        /// This command invokes the deleting of DocumentProduct
        /// </summary>
        /// <param name="codDocument"></param>
        /// <param name="codDocumentProduct"></param>
        /// <param name="newQuantity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteDocumentProduct(string codDocument, string codDocumentProduct)
        {

            Document doc = documentRepository.GetSingle(codDocument);

            var documentProduct = doc.DocumentProducts.FirstOrDefault(x => x.CodDocumentProduct == codDocumentProduct);
            var codProduct = documentProduct.CodProduct;

            if (doc.DocumentProducts.Count > 1)
            {

                documentRepository.DeleteDocumentProduct(documentProduct);
                documentRepository.Save();
            }

            return Json(new { redirectUrl = Url.Action("EditDocumentProducts", "Document", new { area = "Working", id = codProduct }) });

        }


        /// <summary>
        /// use it for update CodDocumentProduct to solve proble in Cloning 
        /// in old DocumentProduct before using of PadLeft with "0" in CodDocumentProduct
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UpdateAllCodDocumentProduct()
        {
            //Per tutti i DocumentProduct del Prodotto
            var docs = documentRepository.GetAll().Select(x => x.CodDocument).ToList();

            foreach (var d in docs)
            {
                UpdateCodDocumentProduct(d);
            }

            return View();
        }


        /// <summary>
        /// Add manual cost to a document product
        /// </summary>
        /// <param name="codProduct"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddManualCostToDocumentProduct(string codProduct, string description)
        {
            //Per tutti i DocumentProduct del Documento
            var docsProduct = documentRepository.GetDocumentProductsByCodProduct(codProduct).ToList();
            var doc = documentRepository.GetSingle(docsProduct.First().CodDocument);

            foreach (var dp in doc.DocumentProducts.Where(x => x.CodProduct == codProduct))
            {
                dp.NewManualCost(description);
            }

            documentRepository.Edit(doc);
            documentRepository.Save();




            return Json(new { redirectUrl = Url.Action("EditDocumentProducts", "Document", new { area = "Working", id = codProduct }) });

        }

        /// <summary>
        /// id = CodCodument
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UpdateCodDocumentProduct(string id)
        {
            //Per tutti i DocumentProduct del Documento
            var docsProduct = documentRepository.GetAllDocumentProducts().Where(x => x.CodDocument == id).ToList();
            var ctx = documentRepository.GetContext();

            foreach (var dp in docsProduct)
            {
                //ricreo il CodDocumentProduct
                var codDocument = dp.CodDocument;
                var rigth = dp.CodDocumentProduct.Substring(dp.CodDocumentProduct.IndexOf('-') + 1);
                var newCodDocumentProduct = codDocument + "-" + rigth.PadLeft(3, '0');
                var oldCodDocumentProduct = dp.CodDocumentProduct;

                //eseguo un aggiornamento
                var sql = @"UPDATE DocumentProducts SET CodDocumentProduct='" +
                    newCodDocumentProduct + "' where CodDocumentProduct ='" +
                    oldCodDocumentProduct + "';";
                try
                {
                    if (newCodDocumentProduct != oldCodDocumentProduct)
                    {
                        ctx.Database.ExecuteSqlCommand(sql);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                //devo aggiornare anche i costi ora!!!
                foreach (var c in dp.Costs)
                {
                    var rigthCostRest = c.CodCost.Replace(oldCodDocumentProduct + "-", "").PadLeft(3, '0');
                    var newCodCost = c.CodCost.Replace(oldCodDocumentProduct, newCodDocumentProduct);
                    newCodCost = newCodCost.Substring(0, newCodCost.LastIndexOf("-")) + "-" + rigthCostRest;

                    var oldCodCost = c.CodCost;

                    //eseguo un aggiornamento COST
                    var sql2 = @"UPDATE Costs SET CodCost='" +
                        newCodCost + "' where CodCost ='" +
                        oldCodCost + "';";
                    try
                    {
                        if (newCodCost != oldCodCost)
                        {
                            ctx.Database.ExecuteSqlCommand(sql2);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }



                    string oldCodCostDetail = String.Empty;

                    if (c.CostDetails.FirstOrDefault() != null)
                    {
                        oldCodCostDetail = c.CostDetails.FirstOrDefault().CodCostDetail;

                        //eseguo un aggiornamento costdetails
                        var sql3 = @"UPDATE costdetails SET CodCostDetail='" +
                            newCodCost + "' where CodCostDetail ='" +
                            oldCodCostDetail + "';";
                        try
                        {
                            if (newCodCost != oldCodCostDetail)
                            {
                                ctx.Database.ExecuteSqlCommand(sql3);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                        //eseguo un aggiornamento costdetails
                        sql3 = @"UPDATE costdetails SET CodComputedBy='" +
                            newCodCost + "' where CodComputedBy ='" +
                            oldCodCostDetail + "';";
                        try
                        {
                            if (newCodCost != oldCodCostDetail)
                            {
                                ctx.Database.ExecuteSqlCommand(sql3);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }


                        //CodProductPartPrintingGain
                        //eseguo un aggiornamento costdetails
                        sql3 = @"UPDATE ProductPartPrintingGain SET CodProductPartPrintingGain=
                        REPLACE(CodProductPartPrintingGain, '" + oldCodCostDetail + "', '" + newCodCost + "');";
                        try
                        {
                            if (newCodCost != oldCodCostDetail)
                            {
                                ctx.Database.ExecuteSqlCommand(sql3);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                        //CodProductPartPrintingGain
                        //eseguo un aggiornamento costdetails
                        sql3 = @"UPDATE makereadies SET CodMakeready=
                        REPLACE(CodMakeready, '" + oldCodCostDetail + "', '" + newCodCost + "');";
                        try
                        {
                            if (newCodCost != oldCodCostDetail)
                            {
                                ctx.Database.ExecuteSqlCommand(sql3);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                    }
                }
            }

            return View();

        }

        //
        // GET: /Working/Document/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Working/Document/
        /// <summary>
        /// return view of estimates
        /// </summary>
        /// <returns></returns>
        public ActionResult ListEstimate()
        {
            //passo questo elenco alla view per poter implementare una ricerca mediante dropdown nella jqgrid
            ViewBag.States = documentRepository.GetAllStates().Where(x => x.UseInEstimate ?? false).OrderBy(x => x.StateNumber);
            return View();
        }

        public ActionResult ListOrder()
        {
            //passo questo elenco alla view per poter implementare una ricerca mediante dropdown nella jqgrid
            ViewBag.States = documentRepository.GetAllStates().Where(x => x.UseInOrder ?? false).OrderBy(x => x.StateNumber);

            //var d = documentRepository.GetAll().ToList();

            //foreach (var item in d)
            //{
            //    item.DocumentStates = documentRepository.GetAllDocumentStates(item.CodDocument).ToList();

            //    var allStates = documentRepository.GetAllStates().Where(x => (x.UseInOrder ?? false)).ToArray();

            //    if (item.TypeOfDocument == Document.DocumentType.Estimate)
            //    {
            //        allStates = documentRepository.GetAllStates().Where(x => (x.UseInEstimate ?? false)).ToArray();
            //    }

            //    foreach (var s in allStates)
            //    {
            //        item.DocumentStates.Add(new DocumentState
            //        {
            //            CodDocument = item.CodDocument,
            //            StateNumber = s.StateNumber,
            //            CodState = s.CodState,
            //            ResetLinkedStates = s.ResetLinkedStates,
            //            Selected = false
            //        });
            //    }

            //    documentRepository.Edit(item);
            //}

            //documentRepository.Save();

            Console.Write(ViewBag.States);
            return View();
        }

        /// <summary>
        /// return view of alla DocumentProduct
        /// </summary>
        /// <returns></returns>
        public ActionResult ListDocumentProducts()
        {
            return View();
        }


        /// <summary>
        /// return view of all Product
        /// </summary>
        /// <returns></returns>
        public ActionResult ListProducts()
        {
            return View();
        }



        /// <summary>
        /// Delete DocumentProduct 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteDocumentProducts(string ids)
        {
            string[] strings = JsonConvert.DeserializeObject<string[]>(ids);
            foreach (var id in strings)
            {
                var c = documentRepository.GetDocumentProductsByCodProduct(id);

                foreach (var item in c)
                {
                    documentRepository.DeleteDocumentProduct(item);
                }
            }

            documentRepository.Save();

            return Json(new { redirectUrl = Url.Action("ListEstimate", "Document", new { area = "Working" }) });
        }



        /// <summary>
        /// Delete estimate and rediret to List
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteEstimates(string ids)
        {
            string[] strings = JsonConvert.DeserializeObject<string[]>(ids);
            foreach (var id in strings)
            {
                var c = documentRepository.GetSingle(id);
                documentRepository.Delete(c);
            }

            documentRepository.Save();

            return Json(new { redirectUrl = Url.Action("ListEstimate", "Document", new { area = "Working" }) });
        }

        [HttpPost]
        public ActionResult DeleteOrders(string ids)
        {
            string[] strings = JsonConvert.DeserializeObject<string[]>(ids);
            foreach (var id in strings)
            {
                var c = documentRepository.GetSingle(id);
                documentRepository.Delete(c);
            }

            documentRepository.Save();

            return Json(new { redirectUrl = Url.Action("ListOrder", "Document", new { area = "Working" }) });
        }


        [HttpParamAction]
        [HttpGet]
        public ActionResult CreateDocument()
        {
            var c = new Document();
            c.CodDocument = documentRepository.GetNewCode(c);
            documentRepository.Add(c);
            documentRepository.Save();
            Session["CodDocument"] = c.CodDocument;

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "EditDocument";

            return View("EditDocument", c);
        }

        /// <summary>
        /// create fisical new estimate
        /// </summary>
        /// <returns></returns>
        private Estimate NewEstimate()
        {

            var c = new Estimate();
            c.EstimateNumberSerie = DateTime.Now.Year.ToString();
            c.CodDocument = documentRepository.GetNewCode(c);
            c.EstimateNumber = documentRepository.GetNewEstimateNumber(c);
            c.DateDocument = DateTime.Now;

            var allStates = documentRepository.GetAllStates().Where(x => (x.UseInEstimate ?? false));

            foreach (var s in allStates)
            {
                c.DocumentStates.Add(new DocumentState
                {
                    CodDocument = c.CodDocument,
                    StateNumber = s.StateNumber,
                    CodState = s.CodState,
                    ResetLinkedStates = s.ResetLinkedStates,
                    Selected = false
                });
            }

            documentRepository.Add(c);
            documentRepository.Save();
            Session["CodDocument"] = c.CodDocument;

            return c;
        }

        /// <summary>
        /// Action to create new estimate
        /// </summary>
        /// <returns></returns>
        [HttpParamAction]
        [HttpGet]
        public ActionResult CreateEstimate()
        {
            var c = NewEstimate();
            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "EditEstimate";
            return View("EditEstimate", c);
        }


        /// <summary>
        /// create fisical new order
        /// </summary>
        /// <returns></returns>
        private Order NewOrder(string codDocumentProduct)
        {
            var c = new Order();
            c.CodDocument = documentRepository.GetNewCode(c);
            c.OrderNumberSerie = DateTime.Now.Year.ToString();
            c.OrderNumber = documentRepository.GetNewOrderNumber(c);

            var docProd = documentRepository.GetDocumentProductByCodDocumentProduct(codDocumentProduct);

            c.OrderProduct = docProd;
            c.CodCustomer = docProd.Document.CodCustomer;
            c.Customer = docProd.Document.Customer;

            c.DateDocument = DateTime.Now;

            var allStates = documentRepository.GetAllStates().Where(x => (x.UseInOrder ?? false)).OrderBy(x => x.StateNumber);

            foreach (var s in allStates)
            {
                c.DocumentStates.Add(new DocumentState
                {
                    CodDocument = c.CodDocument,
                    StateNumber = s.StateNumber,
                    CodState = s.CodState,
                    //                    StateName = s.StateName, //derivated!!!
                    ResetLinkedStates = s.ResetLinkedStates,
                    Selected = false
                });
            }

            documentRepository.Add(c);
            documentRepository.Save();

            //se ci sono dei TaskCenter inizio a buttare i taskcenter nel primo taskcenter (IndexOf==0)
            var taskCenter = taskCenterRepository.GetAll().Where(y => y.IndexOf == 0).FirstOrDefault();

            var prod = productRepository.GetSingle(docProd.CodProduct);

            if (taskCenter != null)
            {
                DocumentTaskCenter dtc = new DocumentTaskCenter();
                dtc.CodTaskCenter = taskCenter.CodTaskCenter;
                dtc.CodDocument = c.CodDocument;

                if (docProd.Product.ProductRefName == null)
                {
                    dtc.DocumentName = docProd.ProductName;
                }
                else
                {
                    dtc.DocumentName = docProd.Product.ProductRefName;
                }

                try
                {
                    dtc.FieldA = prod.GetColorOfPrinting();
                }
                catch (Exception)
                {

                }

                taskCenterRepository.AddNewDocumentTaskCenter(dtc);
                taskCenterRepository.Save();
            }

            foreach (var item in prod.ProductParts)
            {
                foreach (var p in item.ProductPartPrintableArticles)
                {

                    //devo caricare nel prodotto anche tutte le informazioni per il magazzino
                    var locations = warehouseRepository.GetWarehouseList();
                    ViewBag.Locations = locations;

                    //the new movment display warehouse information
                    //plus new movment in accord to warehouse article specify
                    var cost = docProd.Costs.FirstOrDefault(x => x.CodProductPartPrintableArticle == p.CodProductPartPrintableArticle);

                    // cost.
                    var articles = articleRepository.GetAll();
                    string codArticle;
                    Article art;

                    var extract = articles.GetArticlesByProductPartPrintableArticle(p);

                    var warehouse = warehouseRepository.GetWarehouseList().OrderBy(x => x.CodWarehouse).FirstOrDefault();
                    var codWarehouse = warehouse != null ? warehouse.CodWarehouse : "";

                    var costDetail = cost.CostDetails.FirstOrDefault();

                    try
                    {

                        if (extract.FirstOrDefault() != null)
                        {
                            art = extract.FirstOrDefault();

                            if (costDetail != null && costDetail.TypeOfCostDetail == CostDetail.CostDetailType.PrintedRollArticleCostDetail)
                            {
                                //carico i dati
                                EditCost(cost.CodCost);
                                CostDetail temp = (CostDetail)Session["CostDetail"];


                                var h = temp.PrintingFormat.GetSide1();
                                art = extract.OfType<RollPrintableArticle>().FirstOrDefault(x => x.Width == h);

                                if (art != null)
                                {
                                    codArticle = art.CodArticle;
                                }
                            }

                            if (art != null)
                            {

                                Console.Write(art.CodArticle);

                                var z = warehouseRepository.GetSingleArticle(art.CodArticle, codWarehouse);
                                var newMov = new NewMovViewModel();
                                newMov.ArticleOrProduct = z;
                                newMov.IsProduct = false;

                                newMov.Mov = new WarehouseArticleMov
                                {
                                    WarehouseArticle = z,
                                    CodDocument = c.CodDocument,
                                    CodProductPartPrintableArticle = p.CodProductPartPrintableArticle,
                                    CodWarehouseArticle = z == null ? "" : z.CodWarehouseArticle,
                                    Quantity = cost == null ? 0 : art.TransformQuantity(cost.Quantity ?? 0, (CostDetail.QuantityType)(cost.CostDetails.Count() != 0 ? (cost.CostDetails.FirstOrDefault().TypeOfQuantity ?? 10) : 10)),
                                    TypeOfMov = 3
                                };

                                //ilNewMov.dice del movimento lo prendo dalNewMov.dice dell'articolo
                                newMov.Mov.CodWarehouseArticle = newMov.ArticleOrProduct.CodWarehouseArticle;
                                newMov.Mov.CodWarehouseArticleMov = warehouseRepository.GetNewMovCode(newMov.Mov);
                                warehouseRepository.AddMov(newMov.Mov);
                                warehouseRepository.Save();

                                warehouseRepository.UpdateArticle(warehouseRepository.GetSingle(newMov.Mov.CodWarehouseArticle));
                                warehouseRepository.Save();


                            }
                        }
                    }

                    catch (Exception)
                    {

                    }

                }
            }
            return c;
        }
        /// <summary>
        /// Action to create new order
        /// </summary>
        /// <returns></returns>
        [HttpParamAction]
        [HttpGet]
        public ActionResult CreateOrder(string codDocumentProduct)
        {
            var c = NewOrder(codDocumentProduct);
            ((Order)c).ReportOrderNames = documentRepository.GetAllReportOrderName(CurrentDatabase);

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "EditOrder";
            if (Request.IsAjaxRequest())
            {
                return Json(new { redirectUrl = Url.Action("EditOrder", new { id = c.CodDocument }) }, JsonRequestBehavior.AllowGet);
            }

            return View("EditOrder", c);
        }
    }
}