using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PapiroMVC.Models;
using Services;
using System.Linq;
using System.Collections.Generic;
using PapiroMVC.ServiceLayer;

namespace UnitTestPapiroMVC
{
    [TestClass]
    public class LabelTest
    {
        //[TestMethod]
        //public void Rigido()
        //{
        //    IDocumentRepository docRep = new DocumentRepository();

        //    Estimate doc = new Estimate();
        //    doc.EstimateNumber = "0";

        //    Product prod = Creator.InitProduct("SuppRigidi", new ProductTaskNameRepository(), new FormatsNameRepository(), new TypeOfTaskRepository());

        //    IProductRepository prodRep = new ProductRepository();
        //    prod.CodProduct = prodRep.GetNewCode(prod);
        //    prod.ProductParts.FirstOrDefault().Format = "15x21";
        //    prod.ProductParts.FirstOrDefault().SubjectNumber = 1;

        //    var art = prod.ProductParts.FirstOrDefault().ProductPartPrintableArticles.FirstOrDefault();

        //    #region Printable Article
        //    IArticleRepository artRep = new ArticleRepository();
        //    var artFormList = artRep.GetAll().OfType<RigidPrintableArticle>().FirstOrDefault();

        //    art.TypeOfMaterial = artFormList.TypeOfMaterial;
        //    art.NameOfMaterial = artFormList.NameOfMaterial;
        //    art.Weight = artFormList.Weight;
        //    art.Color = artFormList.Color;
        //    #endregion

        //    #region ViewModel
        //    ProductViewModel pv = new ProductViewModel();
        //    pv.Product = prod;
        //    prod.ProductCodeRigen();

        //    pv.Quantities.Add(10);
        //    #endregion

        //    DocumentProduct dp = new DocumentProduct();
        //    dp.Document = null;
        //    dp.CodProduct = pv.Product.CodProduct;
        //    dp.Product = pv.Product;
        //    dp.Quantity = pv.Quantities.FirstOrDefault();

        //    dp.InitCost();

        //    doc.DocumentProducts.Add(dp);

        //    ITaskExecutorRepository tsk = new TaskExecutorRepository();

        //    Guid guid = new Guid();
        //    foreach (var cost in dp.Costs)
        //    {
        //        var cv = cost.MakeCostDetail2(tsk.GetAll(), artRep.GetAll(), dp.Costs.AsQueryable<Cost>(), guid);
        //        cost.Update();
        //    }

        //    dp.UpdateCost();

        //    Console.WriteLine(dp.TotalAmount);


        //    #region Salvataggio


        //    prodRep.Add(prod);
        //    prodRep.Save();

        //    docRep.Add(doc);
        //    docRep.Save();

        //    #endregion

        //}

        [TestMethod]
        public void NewProductJustDocument()
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
            Product prod = p.InitProduct("EtichetteRotolo", new ProductTaskNameRepository(), new FormatsNameRepository(), new TypeOfTaskRepository());

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

            pv.Quantities.Add(1000);
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

            Assert.IsTrue(fine < 4);
        }

        [TestMethod]
        public void CloneObject()
        {
            IDocumentRepository docRep = new DocumentRepository();
            IProductRepository prodRep = new ProductRepository();
            ICostDetailRepository cdetRep = new CostDetailRepository();

            PapiroService p = new PapiroService();
            p.DocumentRepository = docRep;
            p.CostDetailRepository = cdetRep;

            p.DocumentRepository.SetDbName("castello");
            p.CostDetailRepository.SetDbName("castello");

            Document doc = docRep.GetSingle("00000J");
            var prod = docRep.GetDocumentProductByCodProduct(doc.DocumentProducts.First().CodProduct).FirstOrDefault();

            DocumentProduct prod2 = (DocumentProduct)prod.Clone();
            prod2.CodDocumentProduct = "";
            prod2.Document = null;

            doc.DocumentProducts.Add(prod2);
            doc.DocumentProductsCodeRigen(true);

            p.DocumentRepository.Edit(doc);
            p.DocumentRepository.Save();

            p.DocumentRepository.SetDbName("castello");

            //array di sostituzione dei codici
            Dictionary<string, string> trans = new Dictionary<string, string>();
            foreach (var c in prod.Costs)
            {
                var y = p.CostDetailRepository.GetSingleSimple(c.CodCost);

                if (y != null)
                {
                    var x = (CostDetail)y.Clone();
                    x.CodCostDetail = x.CodCostDetail.Replace(prod.CodDocumentProduct, prod2.CodDocumentProduct);
                    x.CodCost = x.CodCostDetail;

                    x.CostDetailCostCodeRigen();

                    if (x.CodComputedBy != null)
                    {
                        x.CodComputedBy = x.CodComputedBy.Replace(prod.CodDocumentProduct, prod2.CodDocumentProduct);
                    }

                    p.CostDetailRepository.Add(x);
                }
            }

            p.CostDetailRepository.Save();


            doc = docRep.GetSingle("00000J");

            Console.WriteLine("");
        }
    }
}
