using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PapiroMVC.Areas.DataBase.Controllers
{
    public partial class HomeDbController : PapiroMVC.Controllers.ControllerAlgolaBase
    {
        //
        // GET: /DataBase/Home/
        public ActionResult Index()
        {
            return View();
        }

    }
}
