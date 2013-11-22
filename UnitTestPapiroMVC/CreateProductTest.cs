using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PapiroMVC.Models;
using PapiroMVC.Areas.Working.Controllers;
using Moq;
using Services;
using System.Collections.Generic;


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
    }
}
