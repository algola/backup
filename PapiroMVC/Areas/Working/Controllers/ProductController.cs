using PapiroMVC.Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PapiroMVC.Areas.Working.Controllers
{
    public class ProductController : PapiroMVC.Controllers.ControllerAlgolaBase
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

        public ProductController(IArticleRepository _articleDataRep, ICustomerSupplierRepository _dataRepCS)
        {
            articleRepository = _articleDataRep;
            customerSupplierRepository = _dataRepCS;   
        }


        //
        // GET: /Working/Product/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Working/Product/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Working/Product/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Working/Product/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Working/Product/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Working/Product/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
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
        // GET: /Working/Product/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Working/Product/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
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
