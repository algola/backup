using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PapiroMVC.Areas.Warehouse.Controllers
{
    public partial class WarehouseController : PapiroMVC.Controllers.ControllerAlgolaBase
    {
        //
        // GET: /DataBase/Home/
        public ActionResult Index()
        {
            return View();
        }

    }
}
