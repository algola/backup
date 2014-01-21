using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PapiroMVC.Controllers
{
    public class ErrorController : ControllerAlgolaBase
    {
        public ActionResult HttpError403(string error)
        {
            ViewBag.Description = error;
            return this.View();
        }

        public ActionResult HttpError404(string error)
        {
            ViewBag.Description = error;
            return this.View();        
        }
    }
}