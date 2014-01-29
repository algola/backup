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
        private readonly IArticleRepository articleRepository;


        protected dbEntities db;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            documentRepository.SetDbName(CurrentDatabase);
            productRepository.SetDbName(CurrentDatabase);
            typeOfTaskRepository.SetDbName(CurrentDatabase);
            articleRepository.SetDbName(CurrentDatabase);

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


        public ProductController(IProductRepository _productRepository,
            ITypeOfTaskRepository _typeOfTaskRepository,
            IMenuProductRepository _menuProduct,
            IProductTaskNameRepository _productTaskName,
            IFormatsNameRepository _formatsName,
            IDocumentRepository _documentRepository,
                     IArticleRepository _articleRepository

            )
        {
            formatsRepository = _formatsName;
            prodTskNameRepository = _productTaskName;
            menu = _menuProduct;
            typeOfTaskRepository = _typeOfTaskRepository;
            productRepository = _productRepository;
            documentRepository = _documentRepository;
            articleRepository = _articleRepository;

        }

        public ActionResult LoadMenuProduct()
        {
            ViewBag.MenuProd = menu.GetAll().OrderBy(x => x.IndexOf).ToList();
            TempData["MenuProd"] = menu.GetAll().OrderBy(x => x.IndexOf).ToList();
            return null;
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

        /// <summary>
        /// Create new product and put it into current document in session or create new document and put it into
        /// </summary>
        /// <param name="id">rapresents the id of type of product</param>
        /// <returns></returns>
        [HttpParamAction]
        [HttpGet]

        public ActionResult CreateProduct(string id)
        {
            var c = InitProduct(id);

            var d = new ProductViewModel();
            d.Product = c;

            d.Quantities.Add(0);

            //              d.Quantities.Add(0);
            //            d.Quantities.Add(0);
            //            d.Quantities.Add(0);

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreateProduct";
            return View("CreateProduct", d);
        }

    }
}
