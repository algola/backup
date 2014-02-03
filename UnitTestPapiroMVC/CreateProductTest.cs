using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PapiroMVC.Models;
using PapiroMVC.Areas.Working.Controllers;
using Moq;
using Services;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Json;
using PapiroMVC.Json;
using System.Web.Script.Serialization;

namespace UnitTestPapiroMVC
{
    [TestClass]
    public class CreateProductTest
    {
        [TestMethod]
        public void CreateFromZero()
        {
            String id = "6-0-0";

            DocumentRepository documentRepository = new DocumentRepository();
            CostDetailRepository costDetailRepository = new CostDetailRepository();
            ArticleRepository articleRepository = new ArticleRepository();
            DocumentRepository docRepository = new DocumentRepository();
            TaskExecutorRepository taskExecutorRepository = new TaskExecutorRepository();

            Cost cost = docRepository.GetCost(id);
            var cv = new PrintedSheetArticleCostDetail();
            //            cv.TaskCost = cost;
            cv.CodCost = cost.CodCost;
            cv.CodCostDetail = cost.CodCost;

            //            cv.CostDetailCostCodeRigen();

            costDetailRepository.Add(cv);
            costDetailRepository.Save();

        }

        [TestMethod]
        public void CreatePPrintableCostFromPrinting()
        {
            String id = "6-0-1";

            DocumentRepository documentRepository = new DocumentRepository();
            CostDetailRepository costDetailRepository = new CostDetailRepository();
            ArticleRepository articleRepository = new ArticleRepository();
            DocumentRepository docRepository = new DocumentRepository();
            TaskExecutorRepository taskExecutorRepository = new TaskExecutorRepository();

            Cost cost = docRepository.GetCost(id);
            var cv = costDetailRepository.GetSingle(id);

            var costs = documentRepository.GetCostsByCodDocumentProduct(cv.TaskCost.CodDocumentProduct);
            List<PrintedArticleCostDetail> x = ((PrintingCostDetail)cv).GetRelatedPrintedCostDetail(articleRepository.GetAll(), costs);
            foreach (var item in x)
            {
                item.GetCostFromList(articleRepository.GetAll());
                costDetailRepository.Add(item);
            }

            //            costDetailRepository.Add(costDetail);
            costDetailRepository.Save();


        }

        public ProductController InitProductController()
        {
            IProductRepository productRepository = new ProductRepository();
            ITypeOfTaskRepository typeOfTaskRepository = new TypeOfTaskRepository();
            IMenuProductRepository menuProduct = new MenuProductRepository();
            IProductTaskNameRepository productTaskName = new ProductTaskNameRepository();
            IFormatsNameRepository formatsName = new FormatsNameRepository();
            IDocumentRepository documentRepository = new DocumentRepository();
            IArticleRepository articleRepository = new ArticleRepository();

            return new ProductController(productRepository,
                typeOfTaskRepository,
                menuProduct,
                productTaskName,
                formatsName,
                documentRepository,
                articleRepository);
        }

        public ProductViewModel CreateProductGet(string id)
        {
            //this test check if product is generated correctly
            var productController = InitProductController();
            var result = productController.CreateProduct(id) as ViewResult;

            var product = result.ViewData.Model as ProductViewModel;

            return product;
        }

        [TestMethod]
        public void CreateProductPost()
        {
            var prdController = InitProductController();

            //call get method
            var p = CreateProductGet("BigliettiVisita");

            //USER INTERFACE SIMULATION
            //this is biglietto da visita
            var part = p.Product.ProductParts.FirstOrDefault();
            part.Format = "10x10";
            var pArticle = part.ProductPartPrintableArticles.FirstOrDefault();

            pArticle.TypeOfMaterial = "Patinata Lucida";
            pArticle.NameOfMaterial = "garda";
            pArticle.Color = "bianca";

            pArticle.Weight = 170;

            var tsk = part.ProductPartTasks.SingleOrDefault(x => x.CodOptionTypeOfTask.Contains("STAMPA"));
            tsk.CodOptionTypeOfTask = "STAMPAOFFeDIGITALE_FRONTE_COL";

            var result = prdController.CreateProduct(p) as JsonResult;

            //estraggo il redirect e controllo se è uguale a quello che mi aspetto
            string ret = GetValueFromJsonResult<string>(result, "redirectUrl");
            Assert.IsTrue(ret.Contains(""));

        }

        private T GetValueFromJsonResult<T>(JsonResult jsonResult, string propertyName)
        {
            var property =
                jsonResult.Data.GetType().GetProperties()
                .Where(p => string.Compare(p.Name, propertyName) == 0)
                .FirstOrDefault();

            if (null == property)
                throw new ArgumentException("propertyName not found", "propertyName");
            return (T)property.GetValue(jsonResult.Data, null);
        }
    }
}