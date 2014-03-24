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
    public partial class ArticleController : PapiroMVC.Controllers.ControllerAlgolaBase
    {
        private readonly IArticleRepository articleRepository;
        private readonly ICustomerSupplierRepository customerSupplierRepository;

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

            this.Disposables.Add(articleRepository);
            this.Disposables.Add(articleRepository);
        }

        //
        // GET: /Article/
        public ActionResult Index()
        {
            return View(new ArticleAutoChangesViewModel());
        }

        public ActionResult IndexSheetPrintableArticle()
        {
            return View(new SheetPrintableArticleAutoChanges());
        }

        public ActionResult IndexRollPrintableArticle()
        {
            return View(new RollPrintableArticleAutoChanges());
        }

        public ActionResult IndexRigidPrintableArticle()
        {
            return View(new RigidPrintableArticleAutoChanges());
        }

        public ActionResult IndexNoPrintable()
        {
            return View();
           // return View(new NoPrintableAutoChanges());
        }


        [AuthorizeUser]
        [HttpGet]
        public ActionResult CreateSheetPrintableArticle()
        {
            //this feature is needed when in the view there are more than one input (submit button) form
            //Action Method speci
            ViewBag.ActionMethod = "CreateSheetPrintableArticle";
            return View(new SheetPrintableArticleViewModel());
        }

        [HttpParamAction]
        [AuthorizeUser]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateSheetPrintableArticle(SheetPrintableArticleViewModel c)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    c.Article.CodArticle = articleRepository.GetNewCode(c.Article, customerSupplierRepository, c.SupplierMaker, c.SupplyerBuy);
                    articleRepository.Add(c.Article);
                    //rigeneration name of article
                    c.Article.ArticleName = c.Article.ToString();
                    articleRepository.Save();
                    return Json(new { redirectUrl = Url.Action("Index") });
                }
                catch (NoSupplierException)
                {
                    ModelState.AddModelError("PersError", "NoSupplierError");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }

            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreateSheetPrintableArticle";
            return PartialView("_EditAndCreateSheetPrintableArticle", c);
        }


        [AuthorizeUser]
        [HttpParamAction]
        [HttpGet]
        public ActionResult CreateRollPrintableArticle()
        {
            //used to understand default actionmethod  when there are more then one submit button
            ViewBag.ActionMethod = "CreateRollPrintableArticle";
            return View(new RollPrintableArticleViewModel());
        }

        [HttpParamAction]
        [AuthorizeUser]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateRollPrintableArticle(RollPrintableArticleViewModel c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    c.Article.CodArticle = articleRepository.GetNewCode(c.Article, customerSupplierRepository, c.SupplierMaker, c.SupplyerBuy);
                    articleRepository.Add(c.Article);
                    //rigeneration name of article
                    c.Article.ArticleName = c.Article.ToString();
                    articleRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexRollPrintableArticle") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreateRollPrintableArticle";
            return PartialView("_EditAndCreateRollPrintableArticle", c);

        }

        [AuthorizeUser]
        [HttpParamAction]
        [HttpGet]
        public ActionResult CreateRigidPrintableArticle()
        {
            //used to understand default actionmethod  when there are more then one submit button
            ViewBag.ActionMethod = "CreateRigidPrintableArticle";
            return View(new RigidPrintableArticleViewModel());
        }


        [AuthorizeUser]
        [HttpParamAction]
        [HttpGet]
        public ActionResult CreateNoPrintable()
        {
            //used to understand default actionmethod  when there are more then one submit button
            ViewBag.ActionMethod = "CreateNoPrintable";
            return View(new NoPrintableViewModel());
        }

        [HttpParamAction]
        [AuthorizeUser]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateNoPrintable(NoPrintableViewModel c)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    c.Article.CodArticle = articleRepository.GetNewCode(c.Article, customerSupplierRepository, c.SupplierMaker, c.SupplyerBuy);

                    c.Article.ArticleName = c.Article.ToString();
                    articleRepository.Add(c.Article);

                    articleRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexNoPrintable") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreateNoPrintable";
            return PartialView("_EditAndCreateNoPrintable", c);

        }

        [HttpParamAction]
        [AuthorizeUser]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateRigidPrintableArticle(RigidPrintableArticleViewModel c)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    c.Article.CodArticle = articleRepository.GetNewCode(c.Article, customerSupplierRepository, c.SupplierMaker, c.SupplyerBuy);

                    c.Article.ArticleName = c.Article.ToString();
                    articleRepository.Add(c.Article);

                    articleRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexRigidPrintableArticle") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //view name is needed for reach right view because to using more than one submit we have to use "Action" in action method name
            ViewBag.ActionMethod = "CreateRigidPrintableArticle";
            return PartialView("_EditAndCreateRigidPrintableArticle", c);

        }

        [HttpGet]
        public ActionResult WizardRollPrintableArticle()
        {
            return View(new RollPrintableArticleViewModelWizard());
        }

        public ActionResult WizardRollPrintableArticle(RollPrintableArticleViewModelWizard c)
        {
            RollPrintableArticle a;

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

                    return Json(new { redirectUrl = Url.Action("IndexRollPrintableArticle") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }
            return PartialView("_WizardRollPrintableArticle", c);
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
                                    c.Article.ArticleName = c.Article.ToString();

                                    a = c.Article.Clone();

                                    articleRepository.Add(a);
                                    articleRepository.Save();

                                }
                            }
                    }

                    return Json(new { redirectUrl = Url.Action("IndexSheetPrintableArticle") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }
            return PartialView("_WizardSheetPrintableArticle", c);
        }

        #region Edit

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
                        ret = RedirectToAction("EditSheetPrintableArticle", "Article", new { id = id });
                        break;

                case Article.ArticleType.RollPrintableArticle:
                        ret = RedirectToAction("EditRollPrintableArticle", "Article", new { id = id });
                        break;

                case Article.ArticleType.RigidPrintableArticle:
                        ret = RedirectToAction("EditRigidPrintableArticle", "Article", new { id = id });
                        break;

                case Article.ArticleType.ObjectPrintableArticle:
                        ret = RedirectToAction("EditObjectPrintableArticle", "Article", new { id = id });
                        break;

                case Article.ArticleType.NoProntable:
                        ret = RedirectToAction("EditNoPrintable", "Article", new { id = id });
                        break;

            }

            return ret;
        }


        public ActionResult EditSheetPrintableArticle(string id)
        {
            SheetPrintableArticleViewModel viewModel = new SheetPrintableArticleViewModel();
            viewModel.Article = (SheetPrintableArticle)articleRepository.GetSingle(id);

            //get producer and maker

            if (viewModel.Article.CodArticle == "")
                return HttpNotFound();

            ViewBag.ActionMethod = "EditSheetPrintableArticle";
            return View("EditSheetPrintableArticle", viewModel);
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
            return View("EditRollPrintableArticle", viewModel);
        }

        public ActionResult EditRigidPrintableArticle(string id)
        {
            RigidPrintableArticleViewModel viewModel = new RigidPrintableArticleViewModel();
            viewModel.Article = (RigidPrintableArticle)articleRepository.GetSingle(id);

            //get producer and maker

            if (viewModel.Article.CodArticle == "")
                return HttpNotFound();

            //is used to know where we are from and go
            ViewBag.ActionMethod = "EditRigidPrintableArticle";
            return View("EditRigidPrintableArticle", viewModel);
        }

        public ActionResult EditNoPrintable(string id)
        {
            NoPrintableViewModel viewModel = new NoPrintableViewModel();
            viewModel.Article = (NoPrintable)articleRepository.GetSingle(id);

            //get producer and maker

            if (viewModel.Article.CodArticle == "")
                return HttpNotFound();

            //is used to know where we are from and go
            ViewBag.ActionMethod = "EditNoPrintable";
            return View("EditNoPrintable", viewModel);
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

                    PapiroMVC.Models.CustomerSupplier[] customerSuppliers = customerSupplierRepository.GetAll().ToArray();

                    var filteredItems = customerSuppliers.Where(
                        item => item.BusinessName.IndexOf(c.SupplierMaker, StringComparison.InvariantCultureIgnoreCase) >= 0);

                    if (filteredItems.Count() == 0) throw new Exception();

                    c.Article.CodSupplierMaker = filteredItems.First().CodCustomerSupplier;

                    customerSuppliers = customerSupplierRepository.GetAll().ToArray();

                    var filteredItems2 = customerSuppliers.Where(
                        item => item.BusinessName.IndexOf(c.SupplyerBuy, StringComparison.InvariantCultureIgnoreCase) >= 0);

                    if (filteredItems2.Count() == 0) throw new Exception();

                    //if #suppliers < 1 then no supplier has selected correctly and then thow error
                    c.Article.CodSupplierBuy = filteredItems2.First().CodCustomerSupplier;

                    articleRepository.Edit(c.Article);
                    articleRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexSheetPrintableArticle") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            //If we come here, something went wrong. Return it back.      

            ViewBag.ActionMethod = "EditSheetPrintableArticle";
            return PartialView("_EditAndCreateSheetPrintableArticle", c);
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditRigidPrintableArticle(RigidPrintableArticleViewModel c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    PapiroMVC.Models.CustomerSupplier[] customerSuppliers = customerSupplierRepository.GetAll().ToArray();

                    var filteredItems = customerSuppliers.Where(
                        item => item.BusinessName.IndexOf(c.SupplierMaker, StringComparison.InvariantCultureIgnoreCase) >= 0);

                    if (filteredItems.Count() == 0) throw new Exception();

                    c.Article.CodSupplierMaker = filteredItems.First().CodCustomerSupplier;

                    //                    customerSuppliers = customerSupplierRepository.GetAll().ToArray();

                    var filteredItems2 = customerSuppliers.Where(
                        item => item.BusinessName.IndexOf(c.SupplyerBuy, StringComparison.InvariantCultureIgnoreCase) >= 0);

                    if (filteredItems2.Count() == 0) throw new Exception();

                    //if #suppliers < 1 then no supplier has selected correctly and then thow error
                    c.Article.CodSupplierBuy = filteredItems2.First().CodCustomerSupplier;

                    articleRepository.Edit(c.Article);
                    articleRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexRigidPrintableArticle") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    Console.WriteLine(error);
                }
            }

            //If we come here, something went wrong. Return it back. 
            //multi submit
            ViewBag.ActionMethod = "EditRigidPrintableArticle";
            return PartialView("_EditAndCreateRigidPrintableArticle", c);
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditNoPrintable(NoPrintableViewModel c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    PapiroMVC.Models.CustomerSupplier[] customerSuppliers = customerSupplierRepository.GetAll().ToArray();

                    var filteredItems = customerSuppliers.Where(
                        item => item.BusinessName.IndexOf(c.SupplierMaker, StringComparison.InvariantCultureIgnoreCase) >= 0);

                    if (filteredItems.Count() == 0) throw new Exception();

                    c.Article.CodSupplierMaker = filteredItems.First().CodCustomerSupplier;

                    //                    customerSuppliers = customerSupplierRepository.GetAll().ToArray();

                    var filteredItems2 = customerSuppliers.Where(
                        item => item.BusinessName.IndexOf(c.SupplyerBuy, StringComparison.InvariantCultureIgnoreCase) >= 0);

                    if (filteredItems2.Count() == 0) throw new Exception();

                    //if #suppliers < 1 then no supplier has selected correctly and then thow error
                    c.Article.CodSupplierBuy = filteredItems2.First().CodCustomerSupplier;

                    articleRepository.Edit(c.Article);
                    articleRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexNoPrintable") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    Console.WriteLine(error);
                }
            }

            //If we come here, something went wrong. Return it back. 
            //multi submit
            ViewBag.ActionMethod = "EditNoPrintable";
            return PartialView("_EditAndCreateNoPrintable", c);
        }


        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditRollPrintableArticle(RollPrintableArticleViewModel c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    PapiroMVC.Models.CustomerSupplier[] customerSuppliers = customerSupplierRepository.GetAll().ToArray();

                    var filteredItems = customerSuppliers.Where(
                        item => item.BusinessName.IndexOf(c.SupplierMaker, StringComparison.InvariantCultureIgnoreCase) >= 0);

                    if (filteredItems.Count() == 0) throw new Exception();

                    c.Article.CodSupplierMaker = filteredItems.First().CodCustomerSupplier;

                    //                    customerSuppliers = customerSupplierRepository.GetAll().ToArray();

                    var filteredItems2 = customerSuppliers.Where(
                        item => item.BusinessName.IndexOf(c.SupplyerBuy, StringComparison.InvariantCultureIgnoreCase) >= 0);

                    if (filteredItems2.Count() == 0) throw new Exception();

                    //if #suppliers < 1 then no supplier has selected correctly and then thow error
                    c.Article.CodSupplierBuy = filteredItems2.First().CodCustomerSupplier;

                    articleRepository.Edit(c.Article);
                    articleRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexRollPrintableArticle") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    Console.WriteLine(error);
                }
            }

            //If we come here, something went wrong. Return it back. 
            //multi submit
            ViewBag.ActionMethod = "EditRollPrintableArticle";
            return PartialView("_EditAndCreateRollPrintableArticle", c);
        }


        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return Json(new { redirectUrl = Url.Action("Index") });
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

                return Json(new { redirectUrl = Url.Action("Index") });
            }
            catch
            {
                return View();
            }
        }
    }
}
