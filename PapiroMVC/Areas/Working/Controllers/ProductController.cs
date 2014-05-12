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
using PapiroMVC.ServiceLayer;
using System.Threading.Tasks;


namespace PapiroMVC.Areas.Working.Controllers
{
    [AuthorizeUser]
    public partial class ProductController : PapiroMVC.Controllers.ControllerAlgolaBase
    {
        protected IProductTaskNameRepository prodTskNameRepository;
        protected IFormatsNameRepository formatsRepository;
        protected IMenuProductRepository menu;
        private readonly IProductRepository productRepository;
        private readonly IDocumentRepository documentRepository;
        private readonly ITypeOfTaskRepository typeOfTaskRepository;
        private readonly IArticleRepository articleRepository;
        private readonly ICostDetailRepository costDetailRepository;
        private readonly ITaskExecutorRepository taskExecuteRepository;

        protected dbEntities db;


        #region Module Property of Validation and Control
        /// <summary>
        /// 
        /// </summary>
        public IMenuProductRepository MenuProductRepository
        {
            get { return menu; }
        }
        public MembershipUser MembershipUser
        {
            get { return Membership.GetUser(CurrentUser); }
        }
        #endregion


        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            documentRepository.SetDbName(CurrentDatabase);
            productRepository.SetDbName(CurrentDatabase);
            typeOfTaskRepository.SetDbName(CurrentDatabase);
            articleRepository.SetDbName(CurrentDatabase);
            costDetailRepository.SetDbName(CurrentDatabase);
            taskExecuteRepository.SetDbName(CurrentDatabase);

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
            IArticleRepository _articleRepository,
            ICostDetailRepository _costDetailRepository,
            ITaskExecutorRepository _taskExecuteRepository

            )
        {
            formatsRepository = _formatsName;
            prodTskNameRepository = _productTaskName;
            menu = _menuProduct;
            typeOfTaskRepository = _typeOfTaskRepository;
            productRepository = _productRepository;
            documentRepository = _documentRepository;
            articleRepository = _articleRepository;
            costDetailRepository = _costDetailRepository;
            taskExecuteRepository = _taskExecuteRepository;

            this.Disposables.Add(typeOfTaskRepository);
            this.Disposables.Add(documentRepository);
            this.Disposables.Add(productRepository);
            this.Disposables.Add(formatsRepository);
            this.Disposables.Add(articleRepository);
            this.Disposables.Add(prodTskNameRepository);
            this.Disposables.Add(costDetailRepository);
            this.Disposables.Add(taskExecuteRepository);

            this.Disposables.Add(menu);
        }

        public ActionResult LoadMenuProduct()
        {
            ViewBag.MenuProd = menu.GetAll().OrderBy(x => x.IndexOf).ToList();
            TempData["MenuProd"] = menu.GetAll().OrderBy(x => x.IndexOf).ToList();
            return null;
        }


        public ActionResult WarmUp()
        {

            //var inizio = DateTime.Now;

            //IDocumentRepository docRep = documentRepository;
            //IProductRepository prodRep = productRepository;

            //PapiroService p = new PapiroService();
            //p.DocumentRepository = docRep;
            //p.CostDetailRepository = costDetailRepository;
            //p.TaskExecutorRepository = taskExecuteRepository;
            //p.ArticleRepository = articleRepository;

            //Document doc = docRep.GetEstimateEcommerce("000001");
            //doc.EstimateNumber = "0";

            ////work with product
            //Product prod = p.InitProduct("EtichetteRotolo", new ProductTaskNameRepository(), new FormatsNameRepository(), new TypeOfTaskRepository());

            ////------passaggio del prodotto inizializzato all'ecommerce o alla view
            //prod.CodProduct = prodRep.GetNewCode(prod);
            //prod.ProductParts.FirstOrDefault().Format = "5x5";
            //prod.ProductParts.FirstOrDefault().SubjectNumber = 1;

            //var art = prod.ProductParts.FirstOrDefault().ProductPartPrintableArticles.FirstOrDefault();

            //#region Printable Article

            //IArticleRepository artRep = new ArticleRepository();
            //var artFormList = artRep.GetAll().OfType<RigidPrintableArticle>().FirstOrDefault();

            //art.TypeOfMaterial = artFormList.TypeOfMaterial;
            //art.NameOfMaterial = artFormList.NameOfMaterial;
            //art.Weight = artFormList.Weight;
            //art.Color = artFormList.Color;
            //#endregion

            ////------ritorno del prodotto modificato!!!!

            ////rigenero
            //prodRep.Add(prod);
            //prodRep.Save();

            //#region ViewModel
            //ProductViewModel pv = new ProductViewModel();
            //pv.Product = prod;
            ////            prod.ProductCodeRigen();

            //pv.Quantities.Add(1000);
            //#endregion

            //DocumentProduct dp = new DocumentProduct();
            //dp.Document = null;
            //dp.CodProduct = pv.Product.CodProduct;
            //dp.Product = pv.Product;
            //dp.Quantity = pv.Quantities.FirstOrDefault();

            //dp.InitCost();

            //doc.DocumentProducts.Add(dp);

            //docRep.Edit(doc);
            //docRep.Save();

            //var step = DateTime.Now;

            //p.EditOrCreateAllCost(dp.CodDocumentProduct);

            //var fine = DateTime.Now.Subtract(inizio).TotalSeconds;


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
        [AuthorizeModule]
        [AsyncTimeout(2000)]
        [HandleError(ExceptionType = typeof(TimeoutException), View = "TimedOut")]

        public async Task<ActionResult> CreateProduct(string id)
        {

            var p = new PapiroService();
            var inizio = DateTime.Now;
            var c = p.InitProduct(id, prodTskNameRepository, formatsRepository, typeOfTaskRepository);

            var d = new ProductViewModel();
            d.Product = c;

            d.Quantities.Add(0);

            //              d.Quantities.Add(0);
            //            d.Quantities.Add(0);
            //            d.Quantities.Add(0);

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreateProduct";


            var tempo = DateTime.Now.Subtract(inizio);
            Console.WriteLine(tempo);
            return View("CreateProduct", d);
        }

    }
}
