using Braintree;
using Microsoft.AspNet.SignalR;
using PapiroMVC.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PapiroMVC.Controllers
{
    public class HomeController : ControllerAlgolaBase    
    {
        
        /// <summary>
        /// Publish error in a specific page
        /// </summary>
        /// <param name="id"></param>
        /// <param name="CodTaskExecutor"></param>
        /// <returns></returns>
        public ActionResult Error(string id)
        {
            return View(id);
        }


        [System.Web.Mvc.Authorize]
        public ActionResult Payment()
        {
            return View();
        }

        [System.Web.Mvc.Authorize]
        [HttpPost]
        public ActionResult CreateTransaction(FormCollection collection)
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = 1000.0M,
                CreditCard = new TransactionCreditCardRequest
                {                   
                    Number = collection["number"],
                    CVV = collection["cvv"],
                    ExpirationMonth = collection["month"],
                    ExpirationYear = collection["year"]
                },
                Options = new TransactionOptionsRequest
                {
                    //pay now!!
                    SubmitForSettlement = true
                }
            };

            Result<Transaction> result = Constants.Gateway.Transaction.Sale(request);

            if (result.IsSuccess())
            {
                Transaction transaction = result.Target;
                ViewData["TransactionId"] = transaction.Id;
            }
            else
            {
                ViewData["Message"] = result.Message;
            }

            return View("PaymentResult");
        }
    
        public ActionResult Price()
        {
            return View("Price");
        }

        public ActionResult Adv()
        {
            return View();
        }

        public ActionResult ECommerce()
        {
            return View();
        }


        public ActionResult Autocomplete(string term)
        {
            var items = new[] { "Modena", "Roccavivara", "Fabbrico", "Assisi", "Rolo", "Reggio Emilia" };

            /*
            var filteredItems = items.Where(
                item => item.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
                );
             */
            var filteredItems = items.Where(
                item => item.StartsWith(term, StringComparison.InvariantCultureIgnoreCase)
                );

            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ChangeCulture(string id, string returnUrl)
        {
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            var langCookie = new HttpCookie("_culture", id)
            {                               
//                HttpOnly = true
            };
            this.ControllerContext.HttpContext.Response.Cookies.Add(langCookie);

//            Response.AppendCookie(langCookie);
//            return Redirect(Request.UrlReferrer.AbsolutePath);
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return Json(new { redirectUrl = Url.Action("Index", "Home", new { area = "" }) });
            }
        }

        public async Task<ActionResult> Index()
        {
            ViewBag.Message = "Modify this template to kick-start your ASP.NET MVC application.";
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Come contattarci";

            return View();
        }

        public ActionResult UpdateDb()
        {
            return base.UpdateAs();

            //call update schema!!!
//            base.UpdateDatabase("db");

//            return View();
        }
    }
}
