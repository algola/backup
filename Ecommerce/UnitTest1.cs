using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;
using PapiroMVC.ServiceLayer;
using PapiroMVC.Models;
using System.Linq;

namespace Ecommerce
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test1()
        {
            var inizio = DateTime.Now;

            IDocumentRepository docRep = new DocumentRepository();
            IProductRepository prodRep = new ProductRepository();

            PapiroService p = new PapiroService();
            p.DocumentRepository = docRep;
            p.CostDetailRepository = new CostDetailRepository();
            p.TaskExecutorRepository = new TaskExecutorRepository();
            p.ArticleRepository = new ArticleRepository();

            Document doc = docRep.GetEstimateEcommerce("000001");
            doc.EstimateNumber = "0";

            //work with product
            Product prod = p.InitProduct("SuppRigidi", new ProductTaskNameRepository(), new FormatsNameRepository(), new TypeOfTaskRepository());

            //------passaggio del prodotto inizializzato all'ecommerce o alla view
            prod.CodProduct = prodRep.GetNewCode(prod);
            prod.ProductParts.FirstOrDefault().Format = "15x21";
            prod.ProductParts.FirstOrDefault().SubjectNumber = 1;

            var art = prod.ProductParts.FirstOrDefault().ProductPartPrintableArticles.FirstOrDefault();

            #region Printable Article

            IArticleRepository artRep = new ArticleRepository();
            var artFormList = artRep.GetAll().OfType<RigidPrintableArticle>().FirstOrDefault();

            art.TypeOfMaterial = artFormList.TypeOfMaterial;
            art.NameOfMaterial = artFormList.NameOfMaterial;
            art.Weight = artFormList.Weight;
            art.Color = artFormList.Color;
            #endregion

            //------ritorno del prodotto modificato!!!!

            //rigenero
            prodRep.Add(prod);
            prodRep.Save();

            #region ViewModel
            ProductViewModel pv = new ProductViewModel();
            pv.Product = prod;
            //            prod.ProductCodeRigen();

            pv.Quantities.Add(10);
            #endregion

            DocumentProduct dp = new DocumentProduct();
            dp.Document = null;
            dp.CodProduct = pv.Product.CodProduct;
            dp.Product = pv.Product;
            dp.Quantity = pv.Quantities.FirstOrDefault();

            dp.InitCost();

            doc.DocumentProducts.Add(dp);

            docRep.Edit(doc);
            docRep.Save();

            var step = DateTime.Now;

            p.EditOrCreateAllCost(dp.CodDocumentProduct);

            var fine = DateTime.Now.Subtract(inizio).TotalSeconds;

            Assert.IsTrue(fine < 11);
        }

    }
}
