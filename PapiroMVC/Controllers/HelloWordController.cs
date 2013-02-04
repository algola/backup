using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PapiroMVC.Controllers
{
    public class HelloWordController : Controller
    {
        //
        // GET: /HelloWord/

        public ActionResult Index()
        {
            return View();
        }

    }
}
