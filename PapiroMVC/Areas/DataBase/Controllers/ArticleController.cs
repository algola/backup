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

namespace PapiroMVC.Areas.DataBase.Controllers
{
    public partial class ArticleController : PapiroMVC.Controllers.ControllerBase
    {
        
        private readonly IArticleRepository articleRepository;
        private readonly ICustomerSupplierRepository customerSupplierRepository;

        protected dbEntities db;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            articleRepository.SetDbName(CurrentDatabase);
            customerSupplierRepository.SetDbName(CurrentDatabase);
        }

        public ArticleController(IArticleRepository _articleDataRep, ICustomerSupplierRepository _dataRepCS)
        {
            articleRepository = _articleDataRep;
            customerSupplierRepository = _dataRepCS;   
        }

        //
        // GET: /Article/

        public ActionResult Index()
        {
            return View(articleRepository.GetAll().ToList());
        }

        //
        // GET: /Article/Details/5

        public ActionResult Details(string id)
        {
            Article article = db.articles.Find(id); 
            return View(article);
        }

        [HttpGet]
        public ActionResult CreateSheetPrintableArticle()
        {
            return View(new SheetPrintableArticleViewModel());
        }

        public ActionResult CreateSheetPrintableArticle(SheetPrintableArticleViewModel c)
        {

            foreach (var item in ModelState)
            {
                
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //if code is empty then sistem has to assign one
                    if (c.Article.CodArticle == null)
                    {

                        CustomerSupplier[] customerSuppliers = customerSupplierRepository.GetAll().ToArray();

                        var filteredItems = customerSuppliers.Where(
                            item => item.BusinessName.IndexOf(c.SupplierMaker, StringComparison.InvariantCultureIgnoreCase) >= 0);

                        if (filteredItems.Count() == 0) throw new Exception();

                        c.Article.CodSupplierMaker = filteredItems.Single().CodCustomerSupplier;

                        customerSuppliers = customerSupplierRepository.GetAll().ToArray();

                        var filteredItems2 = customerSuppliers.Where(
                            item => item.BusinessName.IndexOf(c.SupplyerBuy, StringComparison.InvariantCultureIgnoreCase) >= 0);

                        if (filteredItems2.Count() == 0) throw new Exception();

                        //if #suppliers < 1 then no supplier has selected correctly and then thow error
                        c.Article.CodSupplierBuy = filteredItems2.Single().CodCustomerSupplier;


                        var csCode = (from COD in articleRepository.GetAll() select COD.CodArticle).Max();
                        if (csCode == null)
                            csCode = "0";
                        c.Article.CodArticle = AlphaCode.GetNextCode(csCode);

                        c.SheetPrintableArticleCuttedCost.TimeStampTable = DateTime.Now;
                        c.SheetPrintableArticlePakedCost.TimeStampTable = DateTime.Now;
                        c.SheetPrintableArticlePalletCost.TimeStampTable = DateTime.Now;

                        c.SheetPrintableArticleCuttedCost.CodArticle = c.Article.CodArticle;
                        c.SheetPrintableArticleCuttedCost.CodArticleCost = c.Article.CodArticle + "_CTC";
                        c.SheetPrintableArticlePakedCost.CodArticle = c.Article.CodArticle;
                        c.SheetPrintableArticlePakedCost.CodArticleCost = c.Article.CodArticle + "_PKC";
                        c.SheetPrintableArticlePalletCost.CodArticle = c.Article.CodArticle;
                        c.SheetPrintableArticlePalletCost.CodArticleCost = c.Article.CodArticle + "_PLC";

                    }

                    //rigeneration name of article
                    c.Article.ArticleName = c.Article.TypeOfMaterial + " " +
                                            c.Article.NameOfMaterial + " " +
                                            c.Article.Weight + " " +
                                            c.Article.Format;                                                                                        

                    c.Article.TimeStampTable = DateTime.Now;
                    articleRepository.Add(c.Article);
                    articleRepository.Save();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }
            return View(c);
        }

                
        //
        // GET: /Article/Edit/5
        public ActionResult Edit(string id)
        {
            //this is a common point where edit function is called
            //base on type we have to call right method

            //load article
            var article = articleRepository.GetSingle(id);
            ActionResult ret = null;

            //check type
            
            switch (article.TypeOfArticle)
            {
                case Article.ArticleType.SheetPrintableArticle:
                    {
                        ret = RedirectToAction("EditSheetPrintableArticle", "Article", new { id = id }); 
                        break;
                    }
                         
                case Article.ArticleType.RollPrintableArticle:
                    {
                        ret = RedirectToAction("EditRollPrintableArticle", "Article", new { id = id }); 
                        break;
                    }
    
                case Article.ArticleType.RigidPrintableArticle:
                    {
                        ret = RedirectToAction("EditRigidPrintableArticle", "Article", new { id = id }); 
                        break;
                    }

                case Article.ArticleType.ObjectPrintableArticle:
                    {
                        ret = RedirectToAction("EditObjectPrintableArticle", "Article", new { id = id }); 
                        break;
                    }
            }

            return ret;
        }

        /*
        public ActionResult EditSheetPrintableArticle(string id)
        {
            var model = articleDataRep.GetSingle(id);
            if (model == null)
                return HttpNotFound();

            return View(model);
        }
        */
        #region Edit

        public ActionResult EditSheetPrintableArticle(string id)
        {
            SheetPrintableArticleViewModel viewModel=new SheetPrintableArticleViewModel();
            viewModel.Article = (SheetPrintableArticle) articleRepository.GetSingle(id);

            if (viewModel.Article.CodArticle == "")
                return HttpNotFound();

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult EditRollPrintableArticle(string id)
        {
            RollPrintableArticleViewModel viewModel = new RollPrintableArticleViewModel();
            viewModel.Article = (RollPrintableArticle)articleRepository.GetSingle(id);

            if (viewModel.Article.CodArticle == "")
                return HttpNotFound();

            return View(viewModel);
        }

        public ActionResult EditObjectPrintableArticle(string id)
        {
            ObjectPrintableArticleViewModel viewModel = new ObjectPrintableArticleViewModel();
            viewModel.Article = (ObjectPrintableArticle)articleRepository.GetSingle(id);

            if (viewModel.Article.CodArticle == "")
                return HttpNotFound();

            return View(viewModel);
        }

        public ActionResult EditRigidPrintableArticle(string id)
        {
            RigidPrintableArticleViewModel viewModel = new RigidPrintableArticleViewModel();
            viewModel.Article = (RigidPrintableArticle)articleRepository.GetSingle(id);

            if (viewModel.Article.CodArticle == "")
                return HttpNotFound();

            return View(viewModel);
        }


        #endregion

        //
        // POST: /Article/Edit/5
        [HttpPost]
        public ActionResult EditSheetPrintableArticle(SheetPrintableArticleViewModel item)
        {                                
            if (ModelState.IsValid) 
            {            
                try 
                {
                    articleRepository.Edit(item.Article);
                    articleRepository.Save();
                    return RedirectToAction("Index");
                } 
                
                catch (Exception ex) 
                {                
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //If we come here, something went wrong. Return it back.        
            return View(item);       
        }

        /*
         * 
         *         //
        // POST: /Article/Edit/5
        [HttpPost]
        public ActionResult EditSheetPrintableArticle(SheetPrintableArticle item)
        {
            
                    
            if (ModelState.IsValid) 
            {            
                try 
                {
                    articleDataRep.Edit(item);
                    articleDataRep.Save();
                    return RedirectToAction("Index");
                } 
                
                catch (Exception ex) 
                {                
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //If we come here, something went wrong. Return it back.        
            return View(item);       
        }


        */

        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Article/Delete/5

        public ActionResult Delete(string id)
        {
            return View();
        }

        //
        // POST: /Article/Delete/5

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
