using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PapiroMVC.Controllers
{
    public class HomeController : ControllerBase
    {
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
                HttpOnly = true
            };
            Response.AppendCookie(langCookie);
//            return Redirect(Request.UrlReferrer.AbsolutePath);
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }


        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to kick-start your ASP.NET MVC application.";
            return View(new PapiroMVC.Models.DatoProva());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Come contattarci";

            return View();
        }

        public ActionResult UpdateDb()
        {
            //call update schema!!!
            base.UpdateDatabase();

            return View();
        }
    }
}
