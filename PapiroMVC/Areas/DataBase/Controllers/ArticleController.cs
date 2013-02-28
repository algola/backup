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
using PapiroMVC.Validation;

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
            return View (new ArticleAutoChangesViewModel());
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
            //this feature is needed when in the view there are more than one input (submit button) form
            //Action Method speci
            ViewBag.ActionMethod = "CreateSheetPrintableArticle";
            return View(new SheetPrintableArticleViewModel());
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateSheetPrintableArticle(SheetPrintableArticleViewModel c)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    //if code is empty then sistem has to assign one
//                    if (c.Article.CodArticle == null)
                    {
                        c.Article.CodArticle = articleRepository.GetNewCode(c.Article, customerSupplierRepository, c.SupplierMaker, c.SupplyerBuy);                    

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
                    c.Article.ArticleName = c.Article.ToString();                                                                                        

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

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreateSheetPrintableArticle";
            return View("CreateSheetPrintableArticle", c);
        }


        [HttpGet]
        public ActionResult CreateRollPrintableArticle()
        {
            ViewBag.ActionMethod = "CreateRollPrintableArticle";
            return View(new RollPrintableArticleViewModel());
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateRollPrintableArticle(RollPrintableArticleViewModel c)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    //if code is empty then sistem has to assign one
//                    if (c.Article.CodArticle == null)
                    {
                        c.Article.CodArticle = articleRepository.GetNewCode(c.Article, customerSupplierRepository,c.SupplierMaker,c.SupplyerBuy);                    
                    }

                    //                        c.RollPrintableArticleCuttedCost.TimeStampTable = DateTime.Now;
                    c.RollPrintableArticleStandardCost.TimeStampTable = DateTime.Now;

                    //                       c.RollPrintableArticleCuttedCost.CodArticle = a.CodArticle;
                    //                       c.RollPrintableArticleCuttedCost.CodArticleCost = a.CodArticle + "_CTC";
                    c.RollPrintableArticleStandardCost.CodArticle = c.Article.CodArticle;
                    c.RollPrintableArticleStandardCost.CodArticleCost = c.Article.CodArticle + "_STD";

                    //rigeneration name of article
                    c.Article.ArticleName = c.Article.ToString();

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

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreateRollPrintableArticle";
            return View("CreateRollPrintableArticle", c);

        }



        [HttpGet]
        public ActionResult WizardRollPrintableArticle()
        {
            return View(new RollPrintableArticleViewModelWizard());
        }

        public ActionResult WizardRollPrintableArticle(RollPrintableArticleViewModelWizard c)
        {
            RollPrintableArticle a;
            RollPrintableArticleStandardCost cost;

            if (ModelState.IsValid)
            {
                try
                {
                    //we have to count weights and widths and use 2 counter to 

                    //WeightS
                    foreach (var weight in c.Weights)
	                {
                        //control if weigth is valid
                        if (weight > 0)
                        foreach (var width in c.Widths)
                        {
                            if (width > 0)
                            {

                                c.Article.Weight = (long)weight;
                                c.Article.Width = width;

                                c.Article.CodArticle = articleRepository.GetNewCode(c.Article, customerSupplierRepository, c.SupplierMaker, c.SupplyerBuy);                    

                                //c.RollPrintableArticleCuttedCost.TimeStampTable = DateTime.Now;
                                c.RollPrintableArticleStandardCost.TimeStampTable = DateTime.Now;

                                //                       c.RollPrintableArticleCuttedCost.CodArticle = c.Article.CodArticle;
                                //                       c.RollPrintableArticleCuttedCost.CodArticleCost = c.Article.CodArticle + "_CTC";
                                c.RollPrintableArticleStandardCost.CodArticle = c.Article.CodArticle;
                                c.RollPrintableArticleStandardCost.CodArticleCost = c.Article.CodArticle + "_STD";


                                c.Article.ArticleName = c.Article.ToString();

                                c.Article.TimeStampTable = DateTime.Now;

                                a = (RollPrintableArticle)c.Article.Clone();
                                
                                //cost = (RollPrintableArticleStandardCost)c.RollPrintableArticleStandardCost.Clone();
                                //a.ArticleCosts.Clear();
                                //a.ArticleCosts.Add(cost);
                                
                                articleRepository.Add(a);
                                articleRepository.Save();
                            }
                        }
	                }

                   
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }
            return View(c);
        }

        [HttpGet]
        public ActionResult WizardSheetPrintableArticle()
        {
            return View(new SheetPrintableArticleViewModelWizard());
        }

        public ActionResult WizardSheetPrintableArticle(SheetPrintableArticleViewModelWizard c)
        {
            SheetPrintableArticle a;

            if (ModelState.IsValid)
            {
                try
                {
                    //we have to count weights and widths and use 2 counter to 

                    //WeightS
                    foreach (var weight in c.Weights)
                    {
                        //control if weigth is valid
                        if (weight > 0)
                            foreach (var format in c.Formats)
                            {
                                if (format != "")
                                {

                                    c.Article.Weight = (long)weight;
                                    c.Article.Format = format;

                                    c.Article.CodArticle = articleRepository.GetNewCode(c.Article, customerSupplierRepository, c.SupplierMaker, c.SupplyerBuy);

                                    c.SheetPrintableArticleCuttedCost.TimeStampTable = DateTime.Now;
                                    c.SheetPrintableArticlePakedCost.TimeStampTable = DateTime.Now;
                                    c.SheetPrintableArticlePalletCost.TimeStampTable = DateTime.Now;

                                    c.SheetPrintableArticleCuttedCost.CodArticle = c.Article.CodArticle;
                                    c.SheetPrintableArticleCuttedCost.CodArticleCost = c.Article.CodArticle + "_CTC";
                                    c.SheetPrintableArticlePakedCost.CodArticle = c.Article.CodArticle;
                                    c.SheetPrintableArticlePakedCost.CodArticleCost = c.Article.CodArticle + "_PKC";
                                    c.SheetPrintableArticlePalletCost.CodArticle = c.Article.CodArticle;
                                    c.SheetPrintableArticlePalletCost.CodArticleCost = c.Article.CodArticle + "_PLC";

                                    c.Article.ArticleName = c.Article.ToString();

                                    c.Article.TimeStampTable = DateTime.Now;

                                    a = (SheetPrintableArticle)c.Article.Clone();

                                    articleRepository.Add(a);
                                    articleRepository.Save();
                                }
                            }
                    }


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

            //get producer and maker

            if (viewModel.Article.CodArticle == "")
                return HttpNotFound();

            ViewBag.ActionMethod = "EditSheetPrintableArticle";
            return View(viewModel);
        }


        public ActionResult EditRollPrintableArticle(string id)
        {
            RollPrintableArticleViewModel viewModel = new RollPrintableArticleViewModel();
            viewModel.Article = (RollPrintableArticle)articleRepository.GetSingle(id);

            //get producer and maker

            if (viewModel.Article.CodArticle == "")
                return HttpNotFound();

            //is used to know where we are from and go
            ViewBag.ActionMethod = "EditRollPrintableArticle";
            return View(viewModel);
        }

        #endregion

        //
        // POST: /Article/Edit/5
        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditSheetPrintableArticle(SheetPrintableArticleViewModel c)
        {                                
            if (ModelState.IsValid) 
            {
                try
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

                    articleRepository.Edit(c.Article);
                    articleRepository.Save();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //If we come here, something went wrong. Return it back.        
            return View(c);       
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditRollPrintableArticle(RollPrintableArticleViewModel c)
        {
            if (ModelState.IsValid)
            {
                try
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

                    articleRepository.Edit(c.Article);
                    articleRepository.Save();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //If we come here, something went wrong. Return it back.        
            return View(c);
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
