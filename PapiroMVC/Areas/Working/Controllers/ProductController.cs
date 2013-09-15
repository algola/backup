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
        protected IProductTaskNameRepository prodTskNameRepository;
        protected IFormatsNameRepository formatsRepository;
        protected IMenuProductRepository menu;
        private readonly IProductRepository productRepository;
        private readonly IDocumentRepository documentRepository;
        private readonly ITypeOfTaskRepository typeOfTaskRepository;

        protected dbEntities db;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            productRepository.SetDbName(CurrentDatabase);
            ViewBag.MenuProd = menu.GetAll().OrderBy(x => x.IndexOf).ToList();
        }


        public ProductController(IProductRepository _productRepository,
            ITypeOfTaskRepository _typeOfTaskRepository,
            IMenuProductRepository _menuProduct,
            IProductTaskNameRepository _productTaskName,
            IFormatsNameRepository _formatsName,
            IDocumentRepository _documentRepository
            )
        {
            formatsRepository = _formatsName;
            prodTskNameRepository = _productTaskName;
            menu = _menuProduct;
            typeOfTaskRepository = _typeOfTaskRepository;
            productRepository = _productRepository;
            documentRepository = _documentRepository;
        }


        //
        // GET: /Working/Product/
        public ActionResult Index()
        {
            return View();
        }


        //
        // POST: /Working/Product/Create
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
        public ActionResult CreateProduct(string id)
        {
            var c = InitProduct(id);

            var d = new ProductViewModel();
            d.Product = c;

            d.Quantities.Add(0);
            d.Quantities.Add(0);
            d.Quantities.Add(0);
            d.Quantities.Add(0);

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreateProduct";
            return View("CreateProduct", d);
        }




    }
}
