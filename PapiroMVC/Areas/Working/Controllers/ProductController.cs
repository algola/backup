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
using System.Data.OleDb;
using System.Xml;
using System.Configuration;
using System.Data.SqlClient;
using Newtonsoft.Json;


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
        private readonly ICustomerSupplierRepository customerSupplierRepository;
        private readonly IWarehouseRepository warehouseRepository;

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
            customerSupplierRepository.SetDbName(CurrentDatabase);

            warehouseRepository.SetDbName(CurrentDatabase);

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
            ITaskExecutorRepository _taskExecuteRepository,
            ICustomerSupplierRepository _customerSupplierRepository,
            IWarehouseRepository _warehouseRepository

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
            customerSupplierRepository = _customerSupplierRepository;
            warehouseRepository = _warehouseRepository;

            this.Disposables.Add(typeOfTaskRepository);
            this.Disposables.Add(documentRepository);
            this.Disposables.Add(productRepository);
            this.Disposables.Add(formatsRepository);
            this.Disposables.Add(articleRepository);
            this.Disposables.Add(prodTskNameRepository);
            this.Disposables.Add(costDetailRepository);
            this.Disposables.Add(taskExecuteRepository);
            this.Disposables.Add(customerSupplierRepository);

            this.Disposables.Add(menu);
        }

        public ActionResult LoadMenuProduct()
        {
            ViewBag.MenuProd = menu.GetAll().OrderBy(x => x.IndexOf).ToList();
            TempData["MenuProd"] = menu.GetAll().OrderBy(x => x.IndexOf).ToList();
            return null;
        }


        public ActionResult ListProductNameGenerator()
        {

            return View();

        }

        public ActionResult TypeOfTaskSerigraphyAutoComplete(string term)
        {
            OptionTypeOfTask[] typeOfSerigraphy = typeOfTaskRepository.GetAllOptionTypeOfTask().Where(x => x.CodTypeOfTask=="SERIGRAFIASOLOTIPI").ToArray();

            var resman = new System.Resources.ResourceManager(typeof(Strings).FullName, typeof(Strings).Assembly);
            foreach(var x in typeOfSerigraphy)
            {
                var res = resman.GetString(x.CodOptionTypeOfTask);
                x.OptionName = res!=null?res:"" ;
            }

            var filteredItems = typeOfSerigraphy.Where(
            item => item.OptionName.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var projection = from art in filteredItems
                             select new
                             {
                                 id = art.OptionName,
                                 label = art.OptionName,
                                 value = art.OptionName
                             };
            return Json(projection.Distinct().ToList(), JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public ActionResult DeleteProducts(string ids)
        {
            string[] strings = JsonConvert.DeserializeObject<string[]>(ids);

            //delete product and documentproduct
            foreach (var id in strings)
            {
                var c = productRepository.GetSingle(id);
                productRepository.Delete(c);

            }

            productRepository.Save();

            return Json(new { redirectUrl = Url.Action("Index", "Product", new { area = "Working" }) });
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


        [HttpPost]
        public ActionResult Excel(HttpPostedFileBase file)
        {
            DataSet ds = new DataSet();
            if (Request.Files["file"].ContentLength > 0)
            {
                string fileExtension =
                                     System.IO.Path.GetExtension(Request.Files["file"].FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    string fileLocation = Server.MapPath("~/Content/") + Request.Files["file"].FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {

                        System.IO.File.Delete(fileLocation);
                    }
                    Request.Files["file"].SaveAs(fileLocation);
                    string excelConnectionString = string.Empty;
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                    fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    //connection String for xls file format.
                    if (fileExtension == ".xls")
                    {
                        excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    //connection String for xlsx file format.
                    else if (fileExtension == ".xlsx")
                    {
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }
                    //Create Connection to Excel work book and add oledb namespace
                    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                    excelConnection.Open();
                    DataTable dt = new DataTable();

                    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null)
                    {
                        return null;
                    }

                    String[] excelSheets = new String[dt.Rows.Count];
                    int t = 0;
                    //excel data saves in temp file here.
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row["TABLE_NAME"].ToString();
                        t++;
                    }
                    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);


                    string query = string.Format("Select * from [{0}]", excelSheets[0]);
                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }

                    excelConnection.Close();
                    excelConnection1.Close();
                }
                if (fileExtension.ToString().ToLower().Equals(".xml"))
                {
                    string fileLocation = Server.MapPath("~/Content/") + Request.Files["FileUpload"].FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                    }

                    Request.Files["FileUpload"].SaveAs(fileLocation);
                    XmlTextReader xmlreader = new XmlTextReader(fileLocation);
                    // DataSet ds = new DataSet();
                    ds.ReadXml(xmlreader);
                    xmlreader.Close();
                }

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    var a = new ProductEmpty();
                    a.CodProduct = (ds.Tables[0].Rows[i][0].ToString()).PadLeft(6, '0');
                    a.ProductName = ds.Tables[0].Rows[i][1].ToString();
                    a.CodMenuProduct = "Vuoto";

                    var b = productRepository.GetSingle(a.CodProduct);
                    if (b == null)
                    {
                        productRepository.Add(a);
                    }
                    else
                    {
                        b.ProductName = a.ProductName;
                        b.CodMenuProduct = "Vuoto";
                        productRepository.Edit(b);
                    }

                    productRepository.Save();

                    //now we have to create new warehouse article (ptoduct) and new mov of load!!!

                    var mov = new WarehouseArticleMov();
                    //il codice del movimento lo prendo dal codice dell'articolo
                    mov.CodWarehouseArticle = "001P" + b.CodProduct;
                    mov.CodWarehouseArticleMov = warehouseRepository.GetNewMovCode(mov);
                    mov.TypeOfMov = 1;
                    mov.Date = DateTime.Now;
                    mov.Quantity = Convert.ToDouble(ds.Tables[0].Rows[i][2].ToString());


                    if (mov.Quantity != 0)
                    {
                        warehouseRepository.AddMov(mov);
                        warehouseRepository.Save();

                        warehouseRepository.UpdateArticle(warehouseRepository.GetSingle(mov.CodWarehouseArticle));
                        warehouseRepository.Save();
                    }

                }

            }



            return View();
        }

        public ActionResult UpdateSerigraphy(ProductViewModel pv)
        {
            var product = pv.Product;

            var sery = product.ProductParts.FirstOrDefault().ProductPartTasks.OfType<ProductPartSerigraphy>().SingleOrDefault(x => x.TypeOfProductPartTask == ProductPartTask.ProductPartTasksType.ProductPartSerigraphy);
            int requested = 0;
           
                try
                {
                    requested = Convert.ToInt32(sery.CodOptionTypeOfTask.Replace("SERIGRAFIAROTOLO_", ""));
                }
                catch (Exception)
                { 
                    requested = 0; 
                }
            
            int actual = sery.ProductPartTaskOptions.Count;



            if (requested > actual)
            {
                int c = requested - actual;
                for (int i = 0; i < c; i++)
                {
                    sery.ProductPartTaskOptions.Add(new ProductPartSerigraphyOption());
                }
            }
            else
            {
                ProductPartTaskOption[] a = sery.ProductPartTaskOptions.ToArray();
                if (requested < actual)
                {
                    int c = actual-requested;
                    var last = a.Count() - 1;
                    for(int i=0;i<c;i++)
                    {
                        sery.ProductPartTaskOptions.Remove(a[last]);
                        last--;
                    }
                }
            }
           
            //Carico i nomi dei formati perchè se la validazione non va a buon fine devo ripresentarli
            product.FormatsName = formatsRepository.GetAllById(product.CodMenuProduct);
            product.SystemTaskList = typeOfTaskRepository.GetAll().ToList();

            //reload option object for productTask and productPartTask
            var taskList = this.typeOfTaskRepository.GetAll();
            foreach (var item in product.ProductTasks)
            {
                item.OptionTypeOfTask = typeOfTaskRepository.GetSingleOptionTypeOfTask(item.CodOptionTypeOfTask);
            }


            foreach (var item in product.ProductParts)
            {
                foreach (var item2 in item.ProductPartTasks)
                {
                    item2.OptionTypeOfTask = typeOfTaskRepository.GetSingleOptionTypeOfTask(item2.CodOptionTypeOfTask);
                }

            }
            //-----end reloding
            //????


            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreateProduct";
            return PartialView("_EditAndCreateProduct", pv);

        }

        public ActionResult UpdateHotPrinting(ProductViewModel pv)
        {
            var product = pv.Product;

            var sery = product.ProductParts.FirstOrDefault().ProductPartTasks.OfType<ProductPartHotPrinting>().SingleOrDefault(x => x.TypeOfProductPartTask == ProductPartTask.ProductPartTasksType.ProductPartHotPrinting);
            int requested = 0;

            try
            {
                requested = Convert.ToInt32(sery.CodOptionTypeOfTask.Replace("STAMPAACALDOROTOLO_", ""));
            }
            catch (Exception)
            {
                requested = 0;
            }

            int actual = sery.ProductPartTaskOptions.Count;



            if (requested > actual)
            {
                int c = requested - actual;
                for (int i = 0; i < c; i++)
                {
                    sery.ProductPartTaskOptions.Add(new ProductPartHotPrintingOption());
                }
            }
            else
            {
                ProductPartTaskOption[] a = sery.ProductPartTaskOptions.ToArray();
                if (requested < actual)
                {
                    int c = actual - requested;
                    var last = a.Count() - 1;
                    for (int i = 0; i < c; i++)
                    {
                        sery.ProductPartTaskOptions.Remove(a[last]);
                        last--;
                    }
                }
            }

            //Carico i nomi dei formati perchè se la validazione non va a buon fine devo ripresentarli
            product.FormatsName = formatsRepository.GetAllById(product.CodMenuProduct);
            product.SystemTaskList = typeOfTaskRepository.GetAll().ToList();

            //reload option object for productTask and productPartTask
            var taskList = this.typeOfTaskRepository.GetAll();
            foreach (var item in product.ProductTasks)
            {
                item.OptionTypeOfTask = typeOfTaskRepository.GetSingleOptionTypeOfTask(item.CodOptionTypeOfTask);
            }


            foreach (var item in product.ProductParts)
            {
                foreach (var item2 in item.ProductPartTasks)
                {
                    item2.OptionTypeOfTask = typeOfTaskRepository.GetSingleOptionTypeOfTask(item2.CodOptionTypeOfTask);
                }

            }
            //-----end reloding
            //????


            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreateProduct";
            return PartialView("_EditAndCreateProduct", pv);

        }



        [HttpGet]
        public ActionResult NewFromCodProduct(string id)
        {
            var p = new PapiroService();
            p.ProductRepository = productRepository;
            p.CurrentDatabase = CurrentDatabase;



            var inizio = DateTime.Now;
            var c = p.ProductRepository.GetSingle(id);// p.InitProduct(id, prodTskNameRepository, formatsRepository, typeOfTaskRepository);

            var d = new ProductViewModel();

            //controllo i dati di cliente e riferimento
            //cercando la session
            if (Session["CodDocument"] != null)
            {
                var doc = documentRepository.GetSingle((string)Session["CodDocument"]);
                d.DocumentName = doc.DocumentName;
                d.Customer = doc.Customer;
            }

            d.Product = c;


            var product = c;

            //Carico i nomi dei formati perchè se la validazione non va a buon fine devo ripresentarli
            product.FormatsName = formatsRepository.GetAllById(product.CodMenuProduct);
            product.SystemTaskList = typeOfTaskRepository.GetAll().ToList();

            product.InitPageTask();


            //reload option object for productTask and productPartTask
            var taskList = this.typeOfTaskRepository.GetAll();
            foreach (var item in product.ProductTasks)
            {
                item.OptionTypeOfTask = typeOfTaskRepository.GetSingleOptionTypeOfTask(item.CodOptionTypeOfTask);
            }


            foreach (var item in product.ProductParts)
            {
                foreach (var item2 in item.ProductPartTasks)
                {
                    item2.OptionTypeOfTask = typeOfTaskRepository.GetSingleOptionTypeOfTask(item2.CodOptionTypeOfTask);
                }

            }
            //-----end reloding
            //????


            // d.Quantities.Add(0);

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreateProduct";


            var tempo = DateTime.Now.Subtract(inizio);
            Console.WriteLine(tempo);
            return View("CreateProduct", d);
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
            p.ProductRepository = productRepository;
            p.CurrentDatabase = CurrentDatabase;

            var inizio = DateTime.Now;
            var c = p.InitProduct(id, prodTskNameRepository, formatsRepository, typeOfTaskRepository);

            var d = new ProductViewModel();

            //controllo i dati di cliente e riferimento
            //cercando la session
            if (Session["CodDocument"] != null)
            {
                var doc = documentRepository.GetSingle((string)Session["CodDocument"]);
                d.DocumentName = doc.DocumentName;
                d.Customer = doc.Customer;
            }

            d.Product = c;

            // d.Quantities.Add(0);

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreateProduct";


            var tempo = DateTime.Now.Subtract(inizio);
            Console.WriteLine(tempo);
            return View("CreateProduct", d);
        }

    }
}
