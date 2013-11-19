using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PapiroMVC.Models;
using PapiroMVC.Areas.Working.Controllers;
using Moq;
using Services;


namespace UnitTestPapiroMVC
{
    [TestClass]
    public class CreateProductTest
    {
        [TestMethod]
        public void CreateProduct()
        {
            String id = "4-0-1";

            CostDetailRepository cdRepository = new CostDetailRepository();
            ArticleRepository artRepository = new ArticleRepository();
            DocumentRepository docRepository = new DocumentRepository();
            TaskExecutorRepository taskExecutorRepository = new TaskExecutorRepository();

            Cost cost = docRepository.GetCost(id);
            var costDetail = cdRepository.GetSingle(id);
            costDetail.InitCostDetail(taskExecutorRepository.GetAll(), artRepository.GetAll(), cost);

            cdRepository.Add(costDetail);
            cdRepository.Save();


        }
    }
}
