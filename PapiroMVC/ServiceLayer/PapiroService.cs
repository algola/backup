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
            product = new ProductSingleSheet();

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
                product.DCut = 0.5;
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
                id == "SuppRigidi" ||
                id == "Poster")
            {
                product = new ProductRigid();
                product.ShowDCut = true;
                product.DCut = 0;
                product.DCut1 = 20;
                product.DCut2 = 1;

            }

            if (
                id == "PVC" ||
                id == "Manifesti" ||
                id == "Striscioni")
            {
                product = new ProductRigid();
            }


            if (
                id == "EtichetteRotolo" ||
                id == "EtichettaControRotolo" ||
                id == "EtichettaControColloRotolo")
            {
                product = new ProductSingleLabelRoll();
            }



            product.CodMenuProduct = id;
            product.ProductTaskName = prodTskNameRepository.GetAllById(id);
            product.FormatsName = formatsRepository.GetAllById(id);

            product.SystemTaskList = typeOfTaskRepository.GetAll().ToList();
            product.InitProduct();

            return product;

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

            var costsProd = DocumentRepository.GetCostsByCodDocumentProduct(id).ToList();
            var idRet = costsProd.FirstOrDefault().DocumentProduct.CodProduct;

            //process all cost in DocumentProduct
            foreach (var codCost in costsProd.Select(x => x.CodCost))
            {
                //if cost is just Processed in the same session (this session, identify by guid)
                //is not processed again
                if (!CostDetailRepository.IsJustSaved(codCost, guid))
                {
                    //if costs has to be processed, EditCostAutomatically is called
                    var cv = EditCostAutomatically(codCost, guid);

                    DocumentRepository.SetDbName(CurrentDatabase);
                    CostDetailRepository.SetDbName(CurrentDatabase);

                    var inizio2 = DateTime.Now;
                    SaveCostDetailAutomatically(cv);
                    var tempo2 = DateTime.Now.Subtract(inizio2);
                    Console.Write(tempo2);
                }
                else
                {
                    Console.WriteLine("");
                }
            }

            var fine = DateTime.Now;

            var tempo = fine.Subtract(inizio);
            Console.WriteLine(tempo.TotalSeconds);
        }

        /// <summary>
        /// Action load Cost and generates related CosteDetail if it doesn't exist
        /// </summary>
        /// <param name="id"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public CostDetail EditCostAutomatically(string id, Guid guid)
        {
            CostDetail cv = CostDetailRepository.GetSingle(id);
            Cost cost = DocumentRepository.GetCost(id);

            //spostare questa logica nella classe 
            if (cv == null)
            {
                if (cost.CodProductPartPrintableArticle != null)
                {
                    var codDP = cost.CodDocumentProduct;
                    var productPartPrintableArticle = cost.ProductPartsPrintableArticle;
                    var productPart = cost.ProductPartsPrintableArticle.ProductPart;
                    var task = productPart.ProductPartTasks.FirstOrDefault(x => x.OptionTypeOfTask.CodTypeOfTask.Contains("STAMPA"));

                    cost = DocumentRepository.GetCost(task.Costs.FirstOrDefault(x => x.CodDocumentProduct == codDP).CodCost);
                }

                //if it is a implant cost!!! (ci pensarà la lavorazione stampa a creare l'impianto!!!!!
                if (cost.CodProductPartImplantTask != null)
                {
                    var codDP = cost.CodDocumentProduct;

                    var productPart = cost.ProductPartImplantTask.ProductPart;
                    var task = productPart.ProductPartTasks.FirstOrDefault(x => x.OptionTypeOfTask.CodTypeOfTask.Contains("STAMPA"));

                    cost = DocumentRepository.GetCost(task.Costs.FirstOrDefault(x => x.CodDocumentProduct == codDP).CodCost);
                }

                cv = cost.MakeCostDetail(TaskExecutorRepository.GetAll(), ArticleRepository.GetAll());

                if (cv != null)
                {
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
                        id = cv.ComputedBy.CodCostDetail;
                        cv = CostDetailRepository.GetSingle(id);
                        cost = DocumentRepository.GetCost(id);
                        break;
                    case CostDetail.CostDetailType.PrintedRigidArticleCostDetail:
                        id = cv.ComputedBy.CodCostDetail;
                        cv = CostDetailRepository.GetSingle(id);
                        cost = DocumentRepository.GetCost(id);
                        break;
                    case CostDetail.CostDetailType.PrintedRollArticleCostDetail:
                        break;
                    default:
                        break;
                }

                Console.WriteLine("");
                cv.InitCostDetail(TaskExecutorRepository.GetAll(), ArticleRepository.GetAll());
            }

            return cv;
        }

        public void SaveCostDetailAutomatically(CostDetail cv)
        {
            if (cv != null)
            {

                var pPart = cv.ProductPart;

                switch (cv.TypeOfCostDetail)
                {
                    //if it is a printing... we have to 

                    case CostDetail.CostDetailType.PrintingLabelRollCostDetail:
                    case CostDetail.CostDetailType.PrintingSheetCostDetail:

                        if (cv.Computes.Count == 0)
                        {
                            var costs = DocumentRepository.GetCostsByCodDocumentProduct(cv.TaskCost.CodDocumentProduct);

                            var temp = costs.ToList();

                            List<PrintedArticleCostDetail> x = ((PrintingCostDetail)cv).GetRelatedPrintedCostDetail(ArticleRepository.GetAll(), costs);
                            foreach (var item in x)
                            {
                                item.ComputedBy = cv;
                                item.InitCostDetail(null, ArticleRepository.GetAll());
                                //item.UpdateCost();
                                //item.GetCostFromList(articleRepository.GetAll());
                                CostDetailRepository.Add(item);
                            }
                        }

                        CostDetailRepository.Add(cv);
                        CostDetailRepository.Save();
                        //aggiorna il costo rigenerando prima i coefficienti

                        var inizio = DateTime.Now;

                        UpdateCost(cv.CodCost);

                        var tempo = DateTime.Now.Subtract(inizio);

                        Console.Write(tempo);

                        break;
                    case CostDetail.CostDetailType.PrintingRollCostDetail:
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

        /// <summary>
        /// Uptate cost in Cost from CostDetail
        /// </summary>
        /// <param name="id"></param>
        private void UpdateCost(string id)
        {

            CostDetail cv = CostDetailRepository.GetSingle(id);

            var inizio = DateTime.Now;

            cv.InitCostDetail(TaskExecutorRepository.GetAll(), ArticleRepository.GetAll());

            var tempo = DateTime.Now.Subtract(inizio);
            Console.WriteLine(tempo);

            cv.TaskCost.Update();
            foreach (var item in cv.Computes)
            {
                UpdateCost(item.CodCost);
            }

            CostDetailRepository.Edit(cv);
            CostDetailRepository.Save();

            //dopo il salvataggio del dettaglio del costo voglio aggiornare il cost!!!!
            cv.TaskCost.DocumentProduct.UpdateCost();

            DocumentRepository.Edit(cv.TaskCost.DocumentProduct.Document);
            DocumentRepository.Save();

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
