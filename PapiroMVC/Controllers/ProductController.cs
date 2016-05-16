using PapiroMVC.ServiceLayer;
using PapiroMVC.Models.WebApi;
using PapiroMVC.Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ModelBinding;
using PapiroMVC.Validation;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Threading;

namespace PapiroMVC.Controllers
{

    /// <summary>
    /// help a
    /// </summary>
    [EnableCors("*", "*", "*")]
    public class ProductApiController : ApiController
    {
        private IDocumentRepository documentRepository;
        private ITypeOfTaskRepository typeOfTaskRepository;
        private IProductRepository productRepository;
        protected IMenuProductRepository menu;
        private ITaskExecutorRepository taskExecutorRepository;

        private IArticleRepository articleRepository;
        private ICustomerSupplierRepository customerSupplierRepository;
        private ICostDetailRepository costDetailRepository;
        private ITaskCenterRepository taskCenterRepository;
        /// <summary>
        /// I want to track each disposable object and dispose them when controller will be dispose
        /// </summary>
        private IList<IDisposable> disposables;

        public IList<IDisposable> Disposables
        {
            get
            {
                disposables = disposables == null ? new List<IDisposable>() : disposables;
                return disposables;
            }
            set
            {
                disposables = value;
            }
        }

        /// <summary>
        /// Dispose each object that expone IDisposable Interface
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {

            if (disposables != null)
            {
                foreach (var disp in disposables)
                {
                    if (disp != null)
                    {
                        disp.Dispose();
                    }
                }
            }

            base.Dispose(disposing);
        }


        public ProductApiController(IDocumentRepository _documentRepository,
            ITypeOfTaskRepository _typeOfTaskRepository,
            IFormatsNameRepository _formatsName,
            IProductRepository _productRepository,
            ITaskExecutorRepository _taskExecutorRepository,
            IArticleRepository _articleRepository,
            ICustomerSupplierRepository _customerSupplierRepository,
            IMenuProductRepository _menuProduct,
            ICostDetailRepository _costDetailRepository,
            ITaskCenterRepository _taskCenterRepository)
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

            this.Disposables.Add(typeOfTaskRepository);
            this.Disposables.Add(documentRepository);
            this.Disposables.Add(productRepository);
            this.Disposables.Add(taskExecutorRepository);
            this.Disposables.Add(articleRepository);
            this.Disposables.Add(customerSupplierRepository);
            this.Disposables.Add(menu);
            this.Disposables.Add(costDetailRepository);
            this.Disposables.Add(taskCenterRepository);

        }


        public ProductApiController(IDocumentRepository _documentRepository, IProductRepository _productRepository, ITaskCenterRepository _taskCenterRepository)
        {
            documentRepository = _documentRepository;
            productRepository = _productRepository;
            taskCenterRepository = _taskCenterRepository;
            this.Disposables.Add(documentRepository);
            this.Disposables.Add(productRepository);
            this.Disposables.Add(taskCenterRepository);
        }

        public ProductApiController()
        {

        }

        /// <summary>
        /// get empty product initialized by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/rigid/empty/{id}")]
        [HttpGet]
        public HttpResponseMessage GetEmptyRigid(string id)
        {
            try
            {
                PapiroService papiro = new PapiroService();
                //Initialize new product
                PapiroMVC.Models.ProductRigid prodIntero = (ProductRigid)papiro.InitProduct(id, new ProductTaskNameRepository(), new FormatsNameRepository(), new TypeOfTaskRepository());

                var prod = new ProductRigidApi();
                prod.Id = id;

                Projection.MakeProjection(prodIntero, prod);

                return Request.CreateResponse<ProductRigidApi>(HttpStatusCode.OK, prod);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }


        private void Init()
        {

            typeOfTaskRepository = new TypeOfTaskRepository();
            documentRepository = new DocumentRepository();
            productRepository = new ProductRepository();
            taskExecutorRepository = new TaskExecutorRepository();
            articleRepository = new ArticleRepository();
            customerSupplierRepository = new CustomerSupplierRepository();
            costDetailRepository = new CostDetailRepository();
            taskCenterRepository = new TaskCenterRepository();

            this.Disposables.Add(typeOfTaskRepository);
            this.Disposables.Add(documentRepository);
            this.Disposables.Add(productRepository);
            this.Disposables.Add(taskExecutorRepository);
            this.Disposables.Add(articleRepository);
            this.Disposables.Add(customerSupplierRepository);
            this.Disposables.Add(menu);
            this.Disposables.Add(costDetailRepository);
            this.Disposables.Add(taskCenterRepository);



        }

        /// <summary>
        /// get back product with its prices, discounted by date
        /// </summary>
        /// <param name="prod"></param>
        /// <returns></returns>
        [Route("api/rigid/productprice")]
        [HttpPost]
        public HttpResponseMessage GetPriceRigid([FromBody]ProductRigidApi prod)
        {
            try
            {
                PapiroService papiro = new PapiroService();
                //Initialize new product
                PapiroMVC.Models.ProductRigid prodIntero = (ProductRigid)papiro.InitProduct(prod.Id, new ProductTaskNameRepository(), new FormatsNameRepository(), new TypeOfTaskRepository());

                Projection.ResolveProjection(prod, prodIntero);

                //generazione dei prezzi a caso
                prod.Prices.Clear();
                prod.Prices.Add(new Price { Date = DateTime.Today.AddDays(2), UnitPrice = "0.087" });
                prod.Prices.Add(new Price { Date = DateTime.Today.AddDays(4), UnitPrice = "0.059" });
                prod.Prices.Add(new Price { Date = DateTime.Today.AddDays(9), UnitPrice = "0.024" });

                return Request.CreateResponse<ProductRigidApi>(HttpStatusCode.OK, prod);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [Route("api/test")]
        [HttpPost]
        public HttpResponseMessage Test(
            string username,
            string nCom,
            string nPrev,
            string desc,
            string field1,
            string field2,
            string field3,
            string quantity,
            string price)
        {

            this.Init();

            taskCenterRepository.SetDbName(username);
            productRepository.SetDbName(username);
            documentRepository.SetDbName(username);

            //cerco il prodotto ---> se c'è ok
            //altrimenti lo creo come generico!!!

            //cerco la commessa ---> se c'è ok
            //altrimenti la creo

            var prod = productRepository.GetAll().Where(x => x.PapiroPrev == nPrev).FirstOrDefault();

            if (prod == null)
            {
                prod = new ProductEmpty();
                prod.CodProduct = productRepository.GetNewCode(prod);
                prod.CodMenuProduct = "Vuoto";
                prod.PapiroPrev = nPrev;
                prod.ProductName = desc;

                productRepository.Add(prod);
                productRepository.Save();
            }

            var docProd = documentRepository.GetAllDocumentProducts().Where(x => x.CodProduct == prod.CodProduct).FirstOrDefault();

            if (docProd == null)
            {
                docProd = new DocumentProduct();
                docProd.CodProduct = prod.CodProduct;
                docProd.UnitPrice = "0";//Convert.ToDouble(price).ToString();
                docProd.Quantity = 0; // Convert.ToInt16(quantity);

                Estimate e = new Estimate();
                e.CodDocument = documentRepository.GetNewCode(e);
                e.EstimateNumberSerie = DateTime.Now.Year.ToString();
                e.EstimateNumber = documentRepository.GetNewEstimateNumber(e);

                //var docProd = documentRepository.GetDocumentProductByCodDocumentProduct(codDocumentProduct);
                e.DocumentProducts.Add(docProd);

                documentRepository.Add(e);
                documentRepository.Save();

                docProd.Product = prod;
            }


            var c = (Order)documentRepository.GetAll().Where(x => x.PapiroCom == nCom).FirstOrDefault();

            if (c == null)
            {
                c = new Order();
                c.CodDocument = documentRepository.GetNewCode(c);
                c.OrderNumberSerie = DateTime.Now.Year.ToString();
                c.OrderNumber = documentRepository.GetNewOrderNumber(c);
                c.PapiroCom = nCom;

                c.OrderProduct = docProd;
                //c.CodCustomer = docProd.Document.CodCustomer;
                //c.Customer = docProd.Document.Customer;

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

                if (taskCenter != null)
                {
                    DocumentTaskCenter dtc = new DocumentTaskCenter();
                    dtc.CodTaskCenter = taskCenter.CodTaskCenter;
                    dtc.CodDocument = c.CodDocument;

                    if (docProd.Product.ProductRefName == null)
                    {
                        dtc.DocumentName = docProd.Product.ProductName;
                    }
                    else
                    {
                        dtc.DocumentName = docProd.Product.ProductRefName;
                    }


                    dtc.FieldA = field1;
                    dtc.FieldB = field2;
                    dtc.FieldC = field3;

                    if (username.ToLower() == "lamarina")
                    {
                        try
                        {
                            dtc.DocumentName = nCom.Substring(0,nCom.IndexOf('/')) + " " + dtc.DocumentName + " " + field1;
                        }
                        catch (Exception)
                        {
                        }
                        dtc.FieldA = "";
                    }
                    else
                    {
                        dtc.DocumentName = nCom + " " + dtc.DocumentName;
                    }

                    taskCenterRepository.AddNewDocumentTaskCenter(dtc);
                    taskCenterRepository.Save();
                }

            }

            taskCenterRepository.Dispose();
            productRepository.Dispose();
            documentRepository.Dispose();

            return Request.CreateResponse<string>(HttpStatusCode.OK, username);
        }


        /// <summary>
        /// get back product with its prices, discounted by date
        /// </summary>
        /// <param name="prod"></param>
        /// <returns></returns>
        [Route("api/rigid/productprice2")]
        [HttpPost]
        public HttpResponseMessage GetPriceRigidBind([ModelBinder(typeof(ProductApiModelBinder))]ProductApi prod)
        {
            try
            {
                PapiroService papiro = new PapiroService();
                //Initialize new product
                PapiroMVC.Models.ProductRigid prodIntero = (ProductRigid)papiro.InitProduct(prod.Id, new ProductTaskNameRepository(), new FormatsNameRepository(), new TypeOfTaskRepository());

                Projection.ResolveProjection((ProductRigidApi)prod, prodIntero);

                //generazione dei prezzi a caso
                prod.Prices.Clear();
                prod.Prices.Add(new Price { Date = DateTime.Today.AddDays(2), UnitPrice = "0.087" });
                prod.Prices.Add(new Price { Date = DateTime.Today.AddDays(4), UnitPrice = "0.059" });
                prod.Prices.Add(new Price { Date = DateTime.Today.AddDays(9), UnitPrice = "0.024" });

                return Request.CreateResponse<ProductRigidApi>(HttpStatusCode.OK, (ProductRigidApi)prod);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }



        /// <summary>
        /// return Rigid Material
        /// </summary>
        /// <param name="prod"></param>
        /// <returns></returns>
        [Route("api/rigid/materials")]
        [HttpGet]
        public HttpResponseMessage GetMaterialRigid()
        {
            try
            {
                PapiroService papiro = new PapiroService();
                //Initialize new product
                var x = papiro.GetRigidList(new ArticleRepository());

                return Request.CreateResponse<List<PrintableArticleApi>>(HttpStatusCode.OK, x);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }



    }
}
