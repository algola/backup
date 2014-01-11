using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PapiroMVC.Models;
using Services;
using PapiroMVC.DbCodeManagement;
using PapiroMVC.Areas.DataBase.ViewModels;
using PapiroMVC.Validation;

namespace PapiroMVC.Areas.DataBase.Controllers
{

    public partial class CustomerSupplierController : PapiroMVC.Controllers.ControllerAlgolaBase
    {
        
        //
        // GET: /CustomerSupplier/
        private readonly ITypeOfBaseRepository typeOfBaseRepository;
        private readonly ICustomerSupplierRepository customerSupplierRepository;
        private readonly ICustomerSupplierBaseRepository customerSupplierBaseRepository;

        public CustomerSupplierController(ICustomerSupplierRepository _customerSupplierRepository, ICustomerSupplierBaseRepository _clisupBaseDataRep, ICustomerSupplierRepository _clisupDataRep, ITypeOfBaseRepository _typeOfBaseDataRep)       
        { 
            this.customerSupplierRepository = _customerSupplierRepository;
            typeOfBaseRepository = _typeOfBaseDataRep;
            customerSupplierRepository = _clisupDataRep;
            customerSupplierBaseRepository = _clisupBaseDataRep;
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            customerSupplierRepository.SetDbName(CurrentDatabase);

            typeOfBaseRepository.SetDbName(CurrentDatabase);
            customerSupplierRepository.SetDbName(CurrentDatabase);
            customerSupplierBaseRepository.SetDbName(CurrentDatabase);
        }



        public ActionResult Index()
        {
            CustomerSupplierIndexViewModel ret = new CustomerSupplierIndexViewModel();
            ret.List = customerSupplierRepository.GetAll().ToList();
            return View(ret);
        }


        [HttpPost]
        public ActionResult Searching(CustomerSupplierSearchOption sOption)
        {

            var containBusinessName = customerSupplierRepository.FindBy(x => x.BusinessName.Contains(sOption.BusinessName ?? "")
                &&x.CustomerSupplierBases.Select(y=>y.City.Contains(sOption.City ?? "")).Count()>0).ToList();

            return PartialView("_List", containBusinessName.ToList());
        }

        //
        // GET: /CustomerSupplier/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


        /// <summary>
        /// use this method to upload file and elaborate it
        /// to load customers
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]    
        public ActionResult Index(HttpPostedFileBase file)    
        {        
            // Verify that the user selected a file        
            if (file != null && file.ContentLength > 0)         
            {            
                // extract only the fielname            
                var fileName = System.IO.Path.GetFileName(file.FileName);            
                // store the file inside ~/App_Data/uploads folder            
                var path = System.IO.Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);            
                file.SaveAs(path);        
            }        
            // redirect back to the index action to show the form once again        
            return Json(new { redirectUrl = Url.Action("Index")});            
        }

        //
        // GET: /CustomerSupplier/CreateCustomer
        public ActionResult CreateCustomer()
        {
            ViewBag.ActionMethod = "CreateCustomer";
            return View(new Customer());
        }

        private ActionResult CreateCustomerSupplier(CustomerSupplier c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //if code is empty then sistem has to assign one
                    if (c.CodCustomerSupplier == null)
                    {
                        c.CodCustomerSupplier = customerSupplierRepository.GetNewCode(c);
                    }
                    c.TimeStampTable = DateTime.Now;
                    customerSupplierRepository.Add(c);
                    customerSupplierRepository.Save();

                    TempData["CreateCustomerSupplier"] = true;
                    return Json(new { redirectUrl = Url.Action("IndexBase", new { id = c.CodCustomerSupplier })});
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }
            if (c.TypeOfCustomerSupplier == CustomerSupplier.CustomerSupplierType.Customer)
                return PartialView("_EditAndCreateCustomer", c);
            else
                return PartialView("_EditAndCreateSupplier", c);
        }

        //
        // POST: /CustomerSupplier/Create
        [HttpParamAction]
        [HttpPost]
        public ActionResult CreateCustomer(Customer c)
        {
            ViewBag.ActionMethod = "CreateCustomer";
            return CreateCustomerSupplier(c);
        }

        //
        // GET: /CustomerSupplier/CreateSuplier
        public ActionResult CreateSupplier()
        {
            ViewBag.ActionMethod = "CreateSupplier";
            return View(new Supplier());
        }

        //
        // GET: /CustomerSupplier/CreateCustomer
        public ActionResult CreateCustomerAndBase()
        {
            return View(new CustomerViewModel());
        }

        //
        // GET: /CustomerSupplier/CreateCustomer
        public ActionResult CreateSupplierAndBase()
        {
            var x = new SupplierViewModel();
            return View(x);
        }

        //
        // POST: /CustomerSupplier/CreateCustomer
        [HttpPost]
        public ActionResult CreateSupplierAndBase(SupplierViewModel c)
        {
//            var v1 =TryValidateModel(c.CustomerSupplier);
//            var v2 = TryValidateModel(c.CustomerSupplierBase);

            if (ModelState.IsValid)
            {
                try
                {
                    //if code is empty then sistem has to assign one
                    if (c.CustomerSupplier.CodCustomerSupplier == null)
                    {
                        c.CustomerSupplier.CodCustomerSupplier = customerSupplierRepository.GetNewCode(c.CustomerSupplier);
                    }
                    c.CustomerSupplier.TimeStampTable = DateTime.Now;
                    customerSupplierRepository.Add(c.CustomerSupplier);

                    c.CustomerSupplierBase.TimeStampTable = DateTime.Now;
                    c.CustomerSupplierBase.CodTypeOfBase = "0001";
                    c.CustomerSupplier.CustomerSupplierBases.Add(c.CustomerSupplierBase);

                    //clisupDataRep.Edit(c);                    
                    customerSupplierRepository.Save();
                    ViewBag.Message = "Inserimento avvenuto con successo. Continuare oppure tornare alla lista";

                    return Json(new { redirectUrl = Url.Action("CreateSupplierAndBase")});
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "";
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }
            return View(c);        

        }



        //
        // POST: /CustomerSupplier/CreateSupplier

        [HttpParamAction]
        [HttpPost]
        public ActionResult CreateSupplier(Supplier c)
        {
            ViewBag.ActionMethod = "CreateSupplier";
            return CreateCustomerSupplier(c);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var ret = customerSupplierRepository.GetSingle(id);

            if (ret.TypeOfCustomerSupplier == PapiroMVC.Models.CustomerSupplier.CustomerSupplierType.Customer)
                return RedirectToAction("EditCustomer", "CustomerSupplier", new { id = id });
            else
                return RedirectToAction("EditSupplier", "CustomerSupplier", new { id = id });
        }

        //
        // GET: /CustomerSupplier/Edit/5
        
        [HttpGet]
        public ActionResult EditCustomer(string id)
        {
            ViewBag.ActionMethod = "EditCustomer";

            return View(customerSupplierRepository.GetSingle(id));
        }

        //
        // POST: /CustomerSupplier/Edit/5
        [HttpParamAction]
        [HttpPost]
        public ActionResult EditCustomer(Customer c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    customerSupplierRepository.Edit(c);
                    customerSupplierRepository.Save();
                    return Json(new { redirectUrl = Url.Action("Index")});
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }
            ViewBag.ActionMethod = "EditCustomer";

            //If we come here, something went wrong. Return it back.        
            return PartialView("_EditAndCreateSupplier", c);                                   
        }


        //
        // GET: /CustomerSupplier/EditSupplier/5
        public ActionResult EditSupplier(string id)
        {
            ViewBag.ActionMethod = "EditSupplier";
            return View(customerSupplierRepository.GetSingle(id));
        }
        //
        // POST: /CustomerSupplier/Edit/5
        [HttpParamAction]
        [HttpPost]
        public ActionResult EditSupplier(Supplier s)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    customerSupplierRepository.Edit(s);
                    customerSupplierRepository.Save();
                    return Json(new { redirectUrl = Url.Action("Index")});
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }
            ViewBag.ActionMethod = "EditSupplier";

            //If we come here, something went wrong. Return it back.        
            return PartialView("_EditAndCreateSupplier", s);
        }

        //
        // GET: /CustomerSupplier/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /CustomerSupplier/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return Json(new { redirectUrl = Url.Action("Index")});
            }
            catch
            {
                return View();
            }
        }
    
                //
        // GET: /CustomerSupplierBase/      
        public ActionResult IndexBase(string id)
        {
            var ret = customerSupplierRepository.GetSingle(id);
            if (ret != null)
                return View(ret);
            else
                return HttpNotFound();
        }

        //
        // GET: /CustomerSupplierBase/Details/5

        public ActionResult DetailsBase(int id)
        {
            return View();
        }

        //
        // GET: /CustomerSupplierBase/Create

        public ActionResult CreateBase(string id)
        {
            //TODO Algola: cercare gli indirizzi del cliente se non c'è la sede principale impostare subito a quella
            //find all customer bases
            var codCliSupBases = (from X in customerSupplierBaseRepository.FindBy(x=>x.CodCustomerSupplier==id) select X.CodCustomerSupplierBase).Max();
            if (codCliSupBases == null)
                codCliSupBases = id + "CODA001";

            //Load each type of base
            ViewBag.TypeOfBaseList = typeOfBaseRepository.GetAll();
                        
            CustomerSupplierBase cs = new CustomerSupplierBase();
            cs.CodCustomerSupplier = id;
            cs.CodCustomerSupplierBase = AlphaCode.GetNextCode(codCliSupBases);
            //find if there is one SedePricipale
            var NumMainBase = from X in customerSupplierBaseRepository.FindBy(x => x.CodCustomerSupplier == id) where X.CodTypeOfBase == "0001" select X;
            if (NumMainBase.Count() < 1)
            {
                cs.CodTypeOfBase = "0001";
            }
            else
            {
                cs.CodTypeOfBase = "0002";
            }
//            ViewBag.TypeOfBaseList = 

            ViewBag.ActionMethod = "CreateBase";
            return View(cs);
        }

        //
        // POST: /CustomerSupplierBase/Create
        [HttpParamAction]
        [HttpPost]
        public ActionResult CreateBase(CustomerSupplierBase cs)
        {
            //Load each type of base
            ViewBag.TypeOfBaseList = typeOfBaseRepository.GetAll();
            
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add insert logic here
                    var c = customerSupplierRepository.GetSingle(cs.CodCustomerSupplier);
                    cs.TimeStampTable = DateTime.Now;
                    c.CustomerSupplierBases.Add(cs);
                    
                    //clisupDataRep.Edit(c);                    
                    customerSupplierRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexBase", new {id=cs.CodCustomerSupplier})});
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
        }

            ViewBag.ActionMethod = "CreateBase";

            //If we come here, something went wrong. Return it back.        
            return PartialView("_EditAndCreateBase",cs);            
        }
        
        //
        // GET: /CustomerSupplierBase/Edit/5

        /// <summary>
        /// Edit of Customer Supplier Base identified by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditBase(String id)
        {
            //Load each type of base
            ViewBag.TypeOfBaseList = typeOfBaseRepository.GetAll();            
            CustomerSupplierBase cs = customerSupplierBaseRepository.GetSingle(id);
            ViewBag.ActionMethod = "EditBase";

            return View(cs);
        }

        //
        // POST: /CustomerSupplierBase/Edit/5

        [HttpParamAction]
        [HttpPost]
        public ActionResult EditBase(String id, CustomerSupplierBase csB)
        {
            //Load each type of base
            ViewBag.TypeOfBaseList = typeOfBaseRepository.GetAll();

            if (ModelState.IsValid)
            {
                try
                {
                    customerSupplierBaseRepository.Edit(csB);
                    customerSupplierBaseRepository.Save();
                    return Json(new { redirectUrl = Url.Action("IndexBase", new { id = csB.CodCustomerSupplier })});
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }
            ViewBag.ActionMethod = "EditBase";

            //If we come here, something went wrong. Return it back.        
            return PartialView("_EditAndCreateBase",csB);           
        }

        //
        // GET: /CustomerSupplierBase/Delete/5

        public ActionResult DeleteBase(int id)
        {
            return View();
        }

        //
        // POST: /CustomerSupplierBase/Delete/5

        [HttpPost]
        public ActionResult DeleteBase(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                return Json(new { redirectUrl = Url.Action("Index")});
            }
            catch
            {
                return View();
            }
        }

        public ActionResult FileUpload()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult FileUpload(HttpPostedFileBase uploadFile)
        {
            if (uploadFile.ContentLength > 0)
            {
                string filePath = System.IO.Path.Combine(HttpContext.Server.MapPath("../../App_Data/uploads"),
                                                System.IO.Path.GetFileName(uploadFile.FileName));
                uploadFile.SaveAs(filePath);
            }
            return Json(new { redirectUrl = Url.Action("Index") });
        }
    }
}