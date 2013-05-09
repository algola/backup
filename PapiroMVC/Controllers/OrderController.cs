using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PapiroMVC.ViewModel;
using Services;
using PapiroMVC.Models;

namespace PapiroMVC.Controllers
{
    public class OrderController : Controller
    {
        private ICustomerSupplierRepository dataRepCS;

        public OrderController(ICustomerSupplierRepository dataRepCS)
        {
            this.dataRepCS = dataRepCS;
        }

        public ActionResult OrderDetail(Order o)
        {
            return View(o);
        }

        //
        // GET: /Order/Details/5
        [HttpGet]
        public ActionResult Order()
        {
            Order order;

            //try to get partial completed order
            order = (Order) TempData["Order"];

            //if no-Order partial completed
            //create new one!
            if (order == null)
            {
                //create new one
                order = new Order()
                {
                    Customer = new Customer()
                };
                
            }

            //save it into TempData
            TempData["Order"] = order;            

            //Send order to the View
            return View(order);
        }

        [HttpGet]
        public ActionResult SelectCustomerOrder()
        {            
            //Use Controller to know where go-back
            ViewBag.Controller = "Order";
            //load customer list
            return View(dataRepCS.GetAll().OfType<Customer>().ToList());
        }

        [HttpGet]
        public ActionResult CustomerOrderSelected(string CodCustomerSupplier)
        {
            // now I have the customer and I can save it into ordere entity
            // but first I've to load it from TempData
            Order order = (Order)TempData["Order"];

            order.Customer = (Customer) dataRepCS.GetSingle(CodCustomerSupplier);
            
            //save changes into TempData... so after redirect it is available 
            //in action method Order GET
            TempData["Order"] = order;
            
            return Json(new { redirectUrl = Url.Action("Order")});
        }

        [HttpPost]
        public ActionResult Order(Order o)
        {
            try
            {
               //TODO: save changes into database... and redirect to anywhere
                return Json(new { redirectUrl = Url.Action("OrderDetail", o) });
            }
            catch
            {
                return View(o);
            }
        }
    }

}
