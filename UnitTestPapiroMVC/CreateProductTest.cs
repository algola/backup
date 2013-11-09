using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PapiroMVC.Models;
using PapiroMVC.Areas.Working.Controllers;
using Moq;
using Services;

/*
namespace UnitTestPapiroMVC
{
    [TestClass]
    public class CreateProductTest
    {
        [TestMethod]
        public void CreateProduct()
        {

            var totR = new TypeOfTaskRepository();
            var doR = new DocumentRepository();
            var fnR = new FormatsNameRepository();
            var prR = new ProductRepository();
            var teR = new TaskExecutorRepository();

            var csR = new Mock<CustomerSupplierRepository>();
            var arR = new Mock<ArticleRepository>();
            var prod = new Mock<ProductBookSheet>();
            var meR= new Mock<MenuProductRepository>();

            var ptnR = new ProductTaskNameRepository();

  //          var c = new DocumentController(doR,totR,fnR,prR,teR,arR.Object,csR.Object,meR.Object);

            var c = new ProductController(prR,totR,meR.Object,ptnR,fnR,doR);

            var pvm= new Mock<ProductViewModel>();

        }
    }
}


*/