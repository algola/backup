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
        private readonly IDocumentRepository documentRepository;
        private readonly ITypeOfTaskRepository typeOfTaskRepository;
        private readonly IProductRepository productRepository;

        protected dbEntities db;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            documentRepository.SetDbName(CurrentDatabase);
        }


        public DocumentController(IDocumentRepository _documentRepository,
            ITypeOfTaskRepository _typeOfTaskRepository,
            IFormatsNameRepository _formatsName,
            IProductRepository _productRepository
            )
        {
            typeOfTaskRepository = _typeOfTaskRepository;
            documentRepository = _documentRepository;
            productRepository = _productRepository; 
        }


        //
        // GET: /Working/Document/
        public ActionResult Index()
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


    }
}
