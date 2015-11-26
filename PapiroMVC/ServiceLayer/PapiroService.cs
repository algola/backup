using PapiroMVC.Models;
using PapiroMVC.Models.WebApi;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.ServiceLayer
{
    public partial class PapiroService
    {
        public IDocumentRepository DocumentRepository { get; set; }
        public ICostDetailRepository CostDetailRepository { get; set; }
        public ITaskExecutorRepository TaskExecutorRepository { get; set; }
        public IArticleRepository ArticleRepository { get; set; }
        public IProductRepository ProductRepository { get; set; }
        public ITypeOfTaskRepository TypeOfTaskRepository { get; set; }

        #region Dizionari

        private bool justInited = false;

        public Dictionary<string, Cost> DocumentCostsDic { get; set; }
        public Dictionary<string, CostDetail> CostDetailsDic { get; set; }
        private Dictionary<string, ProductPart> ProductPartsDic { get; set; }
        private Dictionary<string, TaskExecutor> TaskExecutorsDic { get; set; }

        private void CheckExistDic()
        {
            if (DocumentCostsDic == null)
            {
                DocumentCostsDic = new Dictionary<string, Cost>();
            }

            if (CostDetailsDic == null)
            {
                CostDetailsDic = new Dictionary<string, CostDetail>();
            }

            if (ProductPartsDic == null)
            {
                ProductPartsDic = new Dictionary<string, ProductPart>();
            }

            if (TaskExecutorsDic == null)
            {
                TaskExecutorsDic = new Dictionary<string, TaskExecutor>();
            }

        }


        public bool CostDetailIsJustSaved(string id, Guid guid)
        {
            var g = guid.ToString("N");
            var rr = GetCostDetailFromDictionary(id);

            if (rr != null)
            {
                return (rr.Guid.ToString() == g);
            }
            else
            {
                return false;
            }
        }

        public void SaveCostInDictionary(Cost c)
        {
            CheckExistDic();
            Cost val;
            if (DocumentCostsDic.TryGetValue(c.CodCost, out val))
            {
                // yay, value exists!
                DocumentCostsDic[c.CodCost] = c;
            }
            else
            {
                // darn, lets add the value
                DocumentCostsDic.Add(c.CodCost, c);
            }

            //aggiorno anche il costdetail con il riferimento!!!
            var x = GetCostDetailFromDictionary(c.CodCost);
            if (x != null)
            {
                x.TaskCost = c;

                if (!c.CostDetails.Contains(x))
                {
                    c.CostDetails.Add(x);
                }


            }

        }
        private Cost GetCostFromDictionary(string codCost)
        {
            CheckExistDic();
            Cost val;
            if (DocumentCostsDic.TryGetValue(codCost, out val))
            {
                return val;
            }
            else
            {
                return null;
            }
        }

        private void SaveCostDetailInDictionary(CostDetail c)
        {
            CheckExistDic();
            CostDetail val;
            if (CostDetailsDic.TryGetValue(c.CodCostDetail, out val))
            {
                // yay, value exists!
                CostDetailsDic[c.CodCostDetail] = c;
            }
            else
            {
                // darn, lets add the value
                CostDetailsDic.Add(c.CodCostDetail, c);
            }

            //aggiorno anche il costdetail con il riferimento!!!
            var x = GetCostFromDictionary(c.CodCost);
            if (x != null)
            {
                if (!x.CostDetails.Select(s => s.CodCost).Contains(c.CodCost))
                {
                    x.CostDetails.Add(c);
                }
                else
                {
                    var extract = x.CostDetails.Single(s => s.CodCost == c.CodCost);
                    x.CostDetails.Remove(extract);
                    x.CostDetails.Add(c);
                }

                c.TaskCost = x;
            }


        }
        public CostDetail GetCostDetailFromDictionary(string codCostDetail)
        {
            CheckExistDic();
            CostDetail val;
            if (CostDetailsDic.TryGetValue(codCostDetail, out val))
            {
                return val;
            }
            else
            {
                return null;
            }
        }

        private void SaveProductPartInDictionary(ProductPart c)
        {
            CheckExistDic();
            ProductPart val;
            if (ProductPartsDic.TryGetValue(c.CodProductPart, out val))
            {
                // yay, value exists!
                ProductPartsDic[c.CodProductPart] = c;
            }
            else
            {
                // darn, lets add the value
                ProductPartsDic.Add(c.CodProductPart, c);
            }

        }
        private ProductPart GetProductPartFromDictionary(string codProductPart)
        {
            CheckExistDic();
            ProductPart val;
            if (codProductPart != null && ProductPartsDic.TryGetValue(codProductPart, out val))
            {
                return val;
            }
            else
            {
                return null;
            }
        }

        private void SaveTaskExecutorInDictionary(TaskExecutor c)
        {
            CheckExistDic();
            TaskExecutor val;
            if (TaskExecutorsDic.TryGetValue(c.CodTaskExecutor, out val))
            {
                // yay, value exists!
                TaskExecutorsDic[c.CodTaskExecutor] = c;
            }
            else
            {
                // darn, lets add the value
                TaskExecutorsDic.Add(c.CodTaskExecutor, c);
            }

        }
        private TaskExecutor GetTaskExecutorFromDictionary(string codTaskExecutor)
        {
            CheckExistDic();
            TaskExecutor val;

            if (codTaskExecutor != null && TaskExecutorsDic.TryGetValue(codTaskExecutor, out val))
            {
                return val;
            }
            else
            {
                return null;
            }
        }


        #endregion


        public string CurrentDatabase { get; set; }

        /// <summary>
        /// returns Product initialization by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="prodTskNameRepository"></param>
        /// <param name="formatsRepository"></param>
        /// <param name="typeOfTaskRepository"></param>
        /// <returns></returns>
        public Product InitProduct(string id, IProductTaskNameRepository prodTskNameRepository, IFormatsNameRepository formatsRepository, ITypeOfTaskRepository typeOfTaskRepository)
        {

            Product product;
            product = new ProductEmpty();

            if (id == "Buste" ||
                id == "Volantini" ||
                id == "Pieghevoli" ||
                id == "CartaIntestata" ||
                id == "Locandine" ||
                id == "FogliMacchina")
            {
                product = new ProductSingleSheet();
            }

            if (
                id == "BigliettiVisita" ||
                id == "EtichetteCartellini" ||
                id == "CartolineInviti" ||
                id == "CartolinePostali" ||
                id == "AltriFormati")
            {
                product = new ProductSingleSheet();
                product.ShowDCut = true;
                //                product.DCut = 0.5;
                product.DCut1 = 0.5;
                product.DCut2 = 0.5;
            }

            if (id == "PuntoMetallico" ||
                id == "SpiraleMetallica" ||
                id == "BrossuraFresata" ||
                id == "BrossuraCucitaFilo" ||
                id == "RivistePostalizzazione" ||
                id == "SchedeNonRilegate")
            {
                product = new ProductBookSheet();
            }

            if (
                id == "Fotoquadri" ||
                id == "SuppRigidi")
            {
                product = new ProductRigid();
                product.ShowDCut = true;
                product.DCut = 0;
                product.DCut1 = 1;
                product.DCut2 = 1;

            }

            if (
                id == "PVC" ||
                id == "Manifesti" ||
                id == "Striscioni" ||
                id == "Poster")
            {
                product = new ProductSoft();
            }

            if (
                id == "EtichetteRotolo" ||
                id == "EtichetteSagRotolo" ||
                id.StartsWith("FasceGommateRotolo"))
            {
                product = new ProductSingleLabelRoll();
            }

            if (
                id == "EtichetteRotoloDouble")
            {
                product = new ProductDoubleLabelRoll();
            }

            product.CodMenuProduct = id;
            product.ProductTaskName = prodTskNameRepository.GetAllById(id);
            product.FormatsName = formatsRepository.GetAllById(id);

            product.SystemTaskList = typeOfTaskRepository.GetAll().ToList();

            product.ProductNameGenerator = ProductRepository.GetProductNameGenerator(id).Generator;
            product.InitProduct();

            return product;

        }

        /// <summary>
        /// Get cost and detail from CodCostDetail
        /// </summary>
        /// <param name="id"></param>
        public void InitCostDocumentProduct(string id)
        {
            var inizio1 = DateTime.Now;


            var tskExecs = TaskExecutorRepository.GetAll().ToList().AsQueryable();
            var articles = ArticleRepository.GetAll().ToList().AsQueryable();

            if (!justInited)
            {
                //salvo tutti i costi nel dizionario!!!!
                foreach (var i in DocumentRepository.GetCostsByCodDocumentProduct(id).ToList())
                {

                    SaveCostInDictionary(DocumentRepository.GetCostNoT(i.CodCost));

                    var cv = CostDetailRepository.GetSingle(i.CodCost);

                    if (cv != null)
                    {
                        cv.InitCostDetail(tskExecs, articles);
                        SaveCostDetailInDictionary(cv);
                    }

                }

                justInited = true;
            }

            var tempo1 = DateTime.Now.Subtract(inizio1);
            Console.WriteLine(tempo1.TotalMilliseconds);

            foreach (var item in CostDetailsDic.Select(x => x.Value))
            {
                InitPrinters(item);
            }


        }

        /// <summary>
        /// Creates all cost details
        /// </summary>
        /// <param name="id">CodDocumentProduct</param>
        public void EditOrCreateAllCost(string id)
        {
            var inizio = DateTime.Now;

            //set guid!!!
            Guid guid = Guid.NewGuid();
            //all costs to process

            InitCostDocumentProduct(id);

            //estraggo i costi!!!
            var costsProd = DocumentCostsDic.ToList().Select(x => x.Value);
            var idRet = costsProd.FirstOrDefault().DocumentProduct.CodProduct;

            //process all cost in DocumentProduct
            foreach (var codCost in costsProd.Select(x => x.CodCost))
            {
                //if cost is just Processed in the same session (this session, identify by guid)
                //is not processed again
                if (!CostDetailIsJustSaved(codCost, guid))
                {
                    //if costs has to be processed, EditCostAutomatically is called
                    var cv = EditCostAutomatically(codCost, guid);

                    //DocumentRepository.SetDbName(CurrentDatabase);
                    //CostDetailRepository.SetDbName(CurrentDatabase);

                    SaveCostDetailLocally(cv, guid);
                }
                else
                {
                    Console.WriteLine("");
                }
            }

            SaveCostDetailOnDisk();

        }


        private void SaveCostDetailOnDisk()
        {


            DocumentRepository.SetDbName(CurrentDatabase);
            CostDetailRepository.SetDbName(CurrentDatabase);


            foreach (var item in CostDetailsDic.ToList().Select(z => z.Value))
            {
                item.JustUpdated = false;
            }

            foreach (var item in CostDetailsDic.ToList().Select(z => z.Value))
            {
                UpdateCostLocally(item.CodCostDetail);
            }

            foreach (var item in CostDetailsDic.ToList().Select(z => z.Value))
            {
                RemoveLink(item);
                CostDetailRepository.Add(item);
                CostDetailRepository.Save();
            }

            var cvTemp = CostDetailRepository.GetSingle(CostDetailsDic.ToList().Select(z => z.Value).FirstOrDefault().CodCost);

            var codDocument = cvTemp.TaskCost.DocumentProduct.Document.CodDocument;
            var codDocumentProduct = cvTemp.TaskCost.DocumentProduct.CodDocumentProduct;

            var docs = DocumentRepository.GetSingle(codDocument);


            foreach (var item in DocumentCostsDic.Select(x => x.Value).ToList())
            {
                DocumentRepository.EditCost(item);
            }

            DocumentRepository.Save();

            var docProduct = docs.DocumentProducts.SingleOrDefault(z => z.CodDocumentProduct == codDocumentProduct);
            docProduct.UpdateTotal();

            DocumentRepository.Edit(docs);
            DocumentRepository.Save();

            foreach (var item in CostDetailsDic.ToList().Select(z => z.Value))
            {
                ReLink(item);
            }


        }


        public CostDetail SaveCostDetailFromController(CostDetail cv)
        {

            //venedo da un controller non ho i costdetail e i cost inizializzati... per cui
            //devo ottenere il CodDocumentProduct e poi inizializzare e salvare
            InitCostDocumentProduct(cv.TaskCost.CodDocumentProduct);

            SaveCostDetailLocally(cv, new Guid());
            SaveCostDetailOnDisk();

            return cv;

        }

        void RemoveLink(CostDetail cv)
        {
            if (cv != null)
            {
                cv.CostDetailCostCodeRigen();

                if (cv.TaskCost != null)
                {
                    cv.CodCost = cv.TaskCost.CodCost;
                    cv.TaskCost = null;

                    if (cv.ProductPart != null)
                    {
                        cv.CodProductPart = cv.ProductPart.CodProductPart;
                        SaveProductPartInDictionary(cv.ProductPart);
                        cv.ProductPart = null;
                    }

                    if (cv.TaskexEcutorSelected != null)
                    {
                        SaveTaskExecutorInDictionary(cv.TaskexEcutorSelected);
                        cv.TaskexEcutorSelected = null;
                    }

                    foreach (var c in cv.Computes)
                    {
                        RemoveLink(c);
                    }
                    RemoveLink(cv.ComputedBy);
                }
            }
        }

        void ReLink(CostDetail cv)
        {
            if (cv != null)
                if (cv.TaskCost == null)
                {
                    cv.TaskCost = GetCostFromDictionary(cv.CodCost);
                    cv.ProductPart = GetProductPartFromDictionary(cv.CodProductPart);
                    cv.TaskexEcutorSelected = GetTaskExecutorFromDictionary(cv.CodTaskExecutorSelected);

                    foreach (var c in cv.Computes)
                    {
                        ReLink(c);
                    }
                    ReLink(cv.ComputedBy);
                }
        }



        /// <summary>
        /// Action load Cost and generates related CosteDetail if it doesn't exist
        /// </summary>
        /// <param name="id"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public CostDetail EditCostAutomatically(string id, Guid guid)
        {
            //OLD
            //CostDetail cv = CostDetailRepository.GetSingle(id);
            CostDetail cv = GetCostDetailFromDictionary(id);

            Cost cost;
            cost = GetCostFromDictionary(id);

            //spostare questa logica nella classe 
            if (cv == null)
            {
                if (cost.CodProductPartPrintableArticle != null)
                {
                    var codDP = cost.CodDocumentProduct;
                    var productPartPrintableArticle = cost.ProductPartsPrintableArticle;
                    var productPart = cost.ProductPartsPrintableArticle.ProductPart;
                    var task = productPart.ProductPartTasks.FirstOrDefault(x => x.OptionTypeOfTask.CodTypeOfTask.Contains("STAMPA"));

                    cost = GetCostFromDictionary(task.Costs.FirstOrDefault(x => x.CodDocumentProduct == codDP).CodCost);
                }

                //if it is a implant cost!!! (ci pensarà la lavorazione stampa a creare l'impianto!!!!!
                if (cost.CodProductPartImplantTask != null)
                {
                    var codDP = cost.CodDocumentProduct;

                    var productPart = cost.ProductPartImplantTask.ProductPart;
                    //cerco la lavorazione che ha come CodProductPartTask == cost.CodProductPartImplantTask
                    //OLD   var task = productPart.ProductPartTasks.FirstOrDefault(x => x.OptionTypeOfTask.CodTypeOfTask.Contains("STAMPA"));
                    var task = productPart.ProductPartTasks.FirstOrDefault(x => x.CodProductPartTask == cost.CodProductPartImplantTask);

                    cost = GetCostFromDictionary(task.Costs.FirstOrDefault(x => x.CodDocumentProduct == codDP).CodCost);
                }

                cv = cost.MakeCostDetail(TaskExecutorRepository.GetAll(), ArticleRepository.GetAll());

                if (cv != null)
                {

                    InitPrinters(cv);
                    
                    //guid ensures that costdetail is handled only one time when cost are all processed sistematically
                    cv.Guid = guid.ToString("N");
                    //update 
                    cv.Update();
                }
            }
            else
            {
                //se è un materiale devo aprire per ora la messa in macchina
                cv.Guid = guid.ToString("N");
                switch (cv.TypeOfCostDetail)
                {
                    case CostDetail.CostDetailType.PrintingSheetCostDetail:
                        break;
                    case CostDetail.CostDetailType.PrintingRollCostDetail:

                        break;
                    case CostDetail.CostDetailType.PrintedSheetArticleCostDetail:
                    case CostDetail.CostDetailType.PrintedRigidArticleCostDetail:
                    case CostDetail.CostDetailType.PrintedRollArticleCostDetail:
                    case CostDetail.CostDetailType.ImplantCostDetail:
                    case CostDetail.CostDetailType.ImplantHotPrintingCostDetail:
                    case CostDetail.CostDetailType.ImplantMeshCostDetail:

                        id = cv.ComputedBy.CodCostDetail;
                        cv = GetCostDetailFromDictionary(id);
                        cost = GetCostFromDictionary(id);
                        break;
                    default:
                        break;
                }

                Console.WriteLine("");
                cv.InitCostDetail(TaskExecutorRepository.GetAll(), ArticleRepository.GetAll());
            }

            return cv;
        }

        public void SaveCostDetailLocally(CostDetail cv, Guid guid)
        {
            if (cv != null)
            {

                var pPart = cv.ProductPart;

                switch (cv.TypeOfCostDetail)
                {
                    //if it is a printing... we have to 

                    case CostDetail.CostDetailType.PrintingFlatRollCostDetail:
                    case CostDetail.CostDetailType.PrintingZRollCostDetail:
                    case CostDetail.CostDetailType.PrintingSheetCostDetail:
                    case CostDetail.CostDetailType.PrintingRollCostDetail:
                    case CostDetail.CostDetailType.RepassRollCostDetail:

                        if (cv.Computes.Count == 0)
                        {
                            //

                            var tempCost = DocumentCostsDic.ToList().Select(y => y.Value);

                            List<CostDetail> x = cv.CreateRelatedPrintedCostDetail(ArticleRepository.GetAll(), tempCost.AsQueryable()).Union(
                                cv.GetRelatedImplantCostDetail(cv.TaskCost.CodProductPartTask, tempCost.AsQueryable())).ToList();

                            foreach (var item in x)
                            {
                                item.ComputedBy = cv;
                                item.InitCostDetail(TaskExecutorRepository.GetAll(), ArticleRepository.GetAll());
                                if (!CostDetailIsJustSaved(item.CodCostDetail, guid))
                                {
                                    SaveCostDetailInDictionary(item);
                                }
                            }
                        }

                        //cv.TaskCost = null;
                        SaveCostDetailInDictionary(cv);
                        //aggiorna il costo rigenerando prima i coefficienti

                        var inizio = DateTime.Now;

                        //                        UpdateCost(cv.CodCostDetail);

                        var tempo = DateTime.Now.Subtract(inizio);

                        Console.Write(tempo);

                        break;

                    case CostDetail.CostDetailType.PrePostPressCostDetail:
                    case CostDetail.CostDetailType.ControlTableCostDetail:


                        SaveCostDetailInDictionary(cv);
                        //CostDetailRepository.Add(cv);
                        //CostDetailRepository.Save();
                        //aggiorna il costo rigenerando prima i coefficienti

                        //                      UpdateCost(cv.CodCostDetail);

                        break;

                    case CostDetail.CostDetailType.PrintedSheetArticleCostDetail:
                        break;
                    case CostDetail.CostDetailType.PrintedRollArticleCostDetail:
                        break;
                    default:
                        break;
                }
            }
        }


        public void InitPrinters(CostDetail cv)
        {

            //TEMPORANEOOOOOOOOO
            //devo collegare anche i costi di stampa per reperire alcune unformazioni ultili ai calcoli!!!
            if (cv.TypeOfCostDetail == CostDetail.CostDetailType.ControlTableCostDetail
                    || cv.TypeOfCostDetail == CostDetail.CostDetailType.PrePostPressCostDetail
                    || cv.TypeOfCostDetail == CostDetail.CostDetailType.RepassRollCostDetail)
            {

                cv.CodPartPrintingCostDetail = DocumentRepository.GetCostsByCodDocumentProduct(cv.TaskCost.CodDocumentProduct).Where(y => y.CodItemGraph == "ST").Select(z => z.CodCost);
                var items = cv.CodPartPrintingCostDetail.ToList();
                if (items != null)
                {
                    foreach (var item in items)
                    {
                        var cv2 = GetCostDetailFromDictionary(item);
                        if (!cv.Printers.Select(x => x.CodCost).Contains(cv2.CodCost))
                        {

                            if (!cv.Printers.Select(x=>x.CodCostDetail).Contains(cv2.CodCostDetail))
                            {
                                cv.Printers.Add(cv2);
                                cv2.InitCostDetail(TaskExecutorRepository.GetAll(), ArticleRepository.GetAll());
                            }

                        }
                    }
                }
            }
        }


        /// <summary>
        /// Uptate cost in Cost from CostDetail
        /// </summary>
        /// <param name="id"></param>
        private void UpdateCostLocally(string id)
        {

            CostDetail cv = GetCostDetailFromDictionary(id);


            if (cv != null)
            {
                if (!cv.JustUpdated)
                {
                    cv.JustUpdated = true;

                    CostDetail aux = cv;

                    //collego anche i computedby
                    while (cv != null)
                    {
                        InitPrinters(cv);
                        cv = cv.ComputedBy;
                    }

                    //ristabilisco il cv!!!
                    cv = aux;

                    var inizio = DateTime.Now;

                    cv.InitCostDetail(TaskExecutorRepository.GetAll(), ArticleRepository.GetAll());

                    var tempo = DateTime.Now.Subtract(inizio);
                    Console.WriteLine(tempo);


                    //DARIMUOVERE
                    if (cv.TaskCost.CostDetails.FirstOrDefault(x => x.CodCostDetail == cv.CodCostDetail) == null)
                    {
                        cv.TaskCost.CostDetails.Add(cv);
                    }
                    //******

                    cv.TaskCost.Update();

                    //                CostDetailRepository.Add(cv);
                    //                CostDetailRepository.Save();
                    //                SaveCostDetail(cv);

                    foreach (var item in cv.Computes)
                    {
                        UpdateCostLocally(item.CodCost);
                    }

                    var stClass = new PrintingCostDetail();
                    if (cv.GetType().IsSubclassOf(stClass.GetType()))
                    {
                        //se è una stampa devo aggiorare anche i pre e post press
                        var cdPrePostList = DocumentRepository.GetCostsByCodDocumentProduct(cv.TaskCost.CodDocumentProduct).Where(y =>
                            y.CodItemGraph != "ST" //stampa 
                            && y.CodItemGraph != ""
                            && y.CodItemGraph != null

                            ).Select(z => z.CodCost).ToList();

                        if (cdPrePostList != null)
                        {
                            foreach (var item in cdPrePostList)
                            {
                                UpdateCostLocally(item);
                            }
                        }

                    }

                    //var docs = DocumentRepository.GetSingle(cv.TaskCost.DocumentProduct.Document.CodDocument);
                    //var docProduct = docs.DocumentProducts.Where(z => z.CodDocumentProduct == cv.TaskCost.DocumentProduct.CodDocumentProduct).FirstOrDefault();

                    //docProduct.UpdateTotal();
                    //DocumentRepository.Edit(docs);
                    //DocumentRepository.Save();
                }
            }
        }



        public List<PrintableArticleApi> GetRigidList(IArticleRepository articleRepository)
        {
            articleRepository.SetDbName(CurrentDatabase);
            var x = articleRepository.GetAll().OfType<RigidPrintableArticle>().ToList();

            List<PrintableArticleApi> list = new List<PrintableArticleApi>();
            PrintableArticleApi b;

            AutoMapper.Mapper.CreateMap<RigidPrintableArticle, PrintableArticleApi>();

            foreach (var item in x)
            {
                b = new PrintableArticleApi();
                b = AutoMapper.Mapper.Map<RigidPrintableArticle, PrintableArticleApi>(item);
                list.Add(b);
            }

            return list;
        }

    }

}
