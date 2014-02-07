﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PapiroMVC.Models;
using Moq;
using Services;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using PapiroMVC.ServiceLayer;

namespace UnitTestPapiroMVC
{
    [TestClass]
    public class RigidTest
    {
        [TestMethod]
        public void Rigido()
        {
            IDocumentRepository docRep = new DocumentRepository();

            Estimate doc = new Estimate();
            doc.EstimateNumber = "0";

            Product prod = Creator.InitProduct("SuppRigidi", new ProductTaskNameRepository(), new FormatsNameRepository(), new TypeOfTaskRepository());

            IProductRepository prodRep = new ProductRepository();
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

            #region ViewModel
            ProductViewModel pv = new ProductViewModel();
            pv.Product = prod;
            prod.ProductCodeRigen();

            pv.Quantities.Add(10);
            #endregion

            DocumentProduct dp = new DocumentProduct();
            dp.Document = null;
            dp.CodProduct = pv.Product.CodProduct;
            dp.Product = pv.Product;
            dp.Quantity = pv.Quantities.FirstOrDefault();

            dp.InitCost();

            doc.DocumentProducts.Add(dp);

            ITaskExecutorRepository tsk = new TaskExecutorRepository();

            Guid guid = new Guid();
            foreach (var cost in dp.Costs)
            {
                var cv = cost.MakeCostDetail2(tsk.GetAll(), artRep.GetAll(), dp.Costs.AsQueryable<Cost>(), guid);
                cost.Update();
            }

            dp.UpdateCost();

            Console.WriteLine(dp.TotalAmount);


            #region Salvataggio


            prodRep.Add(prod);
            prodRep.Save();

            docRep.Add(doc);
            docRep.Save();

            #endregion

        }

        [TestMethod]
        public void Test1()
        {

            var inizio = DateTime.Now;


            for (int i = 0; i < 10; i++)
            {

                IDocumentRepository docRep = new DocumentRepository();
                Estimate doc = new Estimate();
                doc.EstimateNumber = "0";


                var iii = DateTime.Now;

                doc.CodDocument = docRep.GetNewCode(doc);
                docRep.Add(doc);

                //save the document head
                docRep.Save();

                //work with product
                Product prod = Creator.InitProduct("SuppRigidi", new ProductTaskNameRepository(), new FormatsNameRepository(), new TypeOfTaskRepository());

                IProductRepository prodRep = new ProductRepository();
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

                //rigenero
                prod.ProductCodeRigen();
                prodRep.Add(prod);
                prodRep.Save();

                var fff = DateTime.Now.Subtract(iii);
                Console.Write(fff);

                #region ViewModel
                ProductViewModel pv = new ProductViewModel();
                pv.Product = prod;
                prod.ProductCodeRigen();

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

                PapiroService p = new PapiroService();
                p.DocumentRepository = docRep;//new DocumentRepository();
                p.CostDetailRepository = new CostDetailRepository();
                p.TaskExecutorRepository = new TaskExecutorRepository();
                p.ArticleRepository = new ArticleRepository();

                var step = DateTime.Now;

                p.EditOrCreateAllCost(dp.CodDocumentProduct);


            }
            
            var fine = DateTime.Now.Subtract(inizio).TotalSeconds/10;
            Console.Write(fine);
            Assert.IsTrue(fine < 4);
        }

    }
}
