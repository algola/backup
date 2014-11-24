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
    [AuthorizeUser]
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

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            documentRepository.SetDbName(CurrentDatabase);
            productRepository.SetDbName(CurrentDatabase);

            typeOfTaskRepository.SetDbName(CurrentDatabase);
            taskExecutorRepository.SetDbName(CurrentDatabase);

            articleRepository.SetDbName(CurrentDatabase);
            customerSupplierRepository.SetDbName(CurrentDatabase);
            costDetailRepository.SetDbName(CurrentDatabase);

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
            ICostDetailRepository _costDetailRepository)
        {
            typeOfTaskRepository = _typeOfTaskRepository;
            documentRepository = _documentRepository;
            productRepository = _productRepository;
            taskExecutorRepository = _taskExecutorRepository;
            articleRepository = _articleRepository;
            customerSupplierRepository = _customerSupplierRepository;
            menu = _menuProduct;
            costDetailRepository = _costDetailRepository;

            this.Disposables.Add(typeOfTaskRepository);
            this.Disposables.Add(documentRepository);
            this.Disposables.Add(productRepository);
            this.Disposables.Add(taskExecutorRepository);
            this.Disposables.Add(articleRepository);
            this.Disposables.Add(customerSupplierRepository);
            this.Disposables.Add(menu);
            this.Disposables.Add(costDetailRepository);

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

                //reset contentx
                p.CostDetailRepository.SetDbName(CurrentDatabase);

                foreach (var c in newProdDoc.Costs)
                {
                    var cost = p.CostDetailRepository.GetSingle(c.CodCost);

                    if (cost != null)
                    {

                        //TEMPORANEOOOOOOOOO
                        //another important link to fix is the printer link
                        //this for PrePressCost
                        if (cost.TypeOfCostDetail == CostDetail.CostDetailType.ControlTableCostDetail ||
                            cost.TypeOfCostDetail == CostDetail.CostDetailType.PrePostPressCostDetail)
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
                                    cost.Printeres.Add(cv2);
                                    cv2.InitCostDetail(taskExecutorRepository.GetAll(), articleRepository.GetAll());
                                }
                            }
                        }

                        //regen cost initialization
                        cost.InitCostDetail(taskExecutorRepository.GetAll(), articleRepository.GetAll());
                        //and regen cost
                        cost.Update();
                        cost.TaskCost.Update();

                        p.CostDetailRepository.Add(cost);
                    }

                }

                //save all
                p.CostDetailRepository.Save();

                doc = documentRepository.GetSingle(codDocument);
                newProdDoc = doc.DocumentProducts.FirstOrDefault(x => x.CodDocumentProduct == newCodDocumentProduct);
                newProdDoc.UpdateCost();

                p.DocumentRepository.Edit(doc);
                p.DocumentRepository.Save();
                // end regen costs
            }

            return Json(new { redirectUrl = Url.Action("EditDocumentProducts", "Document", new { area = "Working", id = prod.CodProduct }) });

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
            ViewBag.States = documentRepository.GetAllStates().Where(x => x.UseInEstimate??false).OrderBy(x=>x.StateNumber);
            return View();
        }

        public ActionResult ListOrder()
        {
            //passo questo elenco alla view per poter implementare una ricerca mediante dropdown nella jqgrid
            ViewBag.States = documentRepository.GetAllStates().Where(x => x.UseInOrder ?? false).OrderBy(x => x.StateNumber);
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

           var allStates = documentRepository.GetAllStates().Where(x => (x.UseInEstimate??false));

           foreach (var s in allStates)
           {
               c.DocumentStates.Add(new DocumentState
               {
                   CodDocument = c.CodDocument,
                   StateNumber = s.StateNumber,
                   StateName = s.StateName,
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

            var allStates = documentRepository.GetAllStates().Where(x => (x.UseInOrder ?? false)).OrderBy(x=>x.StateNumber);

            foreach (var s in allStates)
            {
                c.DocumentStates.Add(new DocumentState
                {
                    CodDocument = c.CodDocument,
                    StateNumber = s.StateNumber,
                    StateName = s.StateName,
                    ResetLinkedStates = s.ResetLinkedStates,
                    Selected = false
                });
            }

            documentRepository.Add(c);
            documentRepository.Save();

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
            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "EditOrder";
            if (Request.IsAjaxRequest())
            {
                return Json(new { redirectUrl = Url.Action("EditOrder", new { id = c.CodDocument})}, JsonRequestBehavior.AllowGet);
            }
            return View("EditOrder", c);
        }

    }
}