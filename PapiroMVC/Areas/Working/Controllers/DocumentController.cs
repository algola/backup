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
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using PapiroMVC.Validation.Error;


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
        }

        //
        // GET: /Working/Document/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Working/Document/
        public ActionResult ListEstimate()
        {
            return View();
        }

        //
        // POST: /Working/Document/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

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


        private Estimate newEstimate()
        {
            var c = new Estimate();
            c.CodDocument = documentRepository.GetNewCode(c);
            c.EstimateNumber = documentRepository.GetNewEstimateNumber(c);
            c.DateDocument = DateTime.Now;
            documentRepository.Add(c);
            documentRepository.Save();
            Session["CodDocument"] = c.CodDocument;

            return c;
        }



        [HttpParamAction]
        [HttpGet]
        public ActionResult CreateEstimate()
        {
            var c = newEstimate();
            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "EditEstimate";
            return View("EditEstimate", c);
        }
    }
}