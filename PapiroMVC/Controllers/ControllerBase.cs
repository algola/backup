using Braintree;
using PapiroMVC.Model;
using PapiroMVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PapiroMVC.Controllers
{
    /// <summary>
    /// this is the controller base Algola
    /// </summary>
    public class ControllerAlgolaBase : AsyncController
    {
        protected string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        public class Constants
        {
            public static BraintreeGateway Gateway = new BraintreeGateway
            {
                Environment = Braintree.Environment.SANDBOX,
                MerchantId = "5gm9czps9t926mfc",
                PublicKey = "bf3wvjm5pg8dc2nd",
                PrivateKey = "ba7e172f99d577277d2aaa62701da6a2"
            };
        }

        public ActionResult Errors(string id)
        {
            Console.Write(id);
            return null;
        }

        //user connected to website
        public MembershipUser CurrentUser
        {
            get;
            set;
        }

        /// <summary>
        /// database used in this sessione
        /// </summary>
        public string CurrentDatabase
        {
            get;
            set;
        }

        public string CurrentUserDatabase
        {
            get;
            set;
        }

        /// <summary>
        /// This override reads autenticated user and sets right database and user
        /// </summary>
        /// <param name="requestContext"></param>
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                string userName = requestContext.HttpContext.User.Identity.Name;
                CurrentUser = Membership.GetUser(userName);
                ViewData["CurrentUser"] = CurrentUser;
                CurrentDatabase = userName;
                CurrentUserDatabase = "root";
            }
            else
                ViewData["CurrentUser"] = null;

            if (Thread.CurrentThread.CurrentUICulture.ToString().ToLower().Contains("it"))
            {
                ViewBag.Lang = "it";
            }

            if (Thread.CurrentThread.CurrentUICulture.ToString().ToLower().Contains("en"))
            {
                ViewBag.Lang = "en";
            }

        }

        protected override void ExecuteCore()
        {
            base.ExecuteCore();
        }

        public void UpdateDatabase(string dbName)
        {
            //  profilesEntities ctxProfiles = new profilesEntities();
            ProfilesDDL tblProfile = new ProfilesDDL("profiles");

            dbEntities ctx = new dbEntities();

            tblProfile.UpdateSchema(ctx);

            if (CurrentDatabase != null)
            {
                //ctx.Database.Connection.ConnectionString = ctx.Database.Connection.ConnectionString.Replace("db", CurrentDatabase);
                ctx.Database.Connection.Open();
            }

            var tables = new List<IDDL>();

            tables.Add(new DataBaseDDL(dbName));
            tables.Add(new CustomerSupplierDLL(dbName));
            tables.Add(new TaskExecutorsDDL(dbName));
            tables.Add(new ArticlesDDL(dbName));
            tables.Add(new ProductsDDL(dbName));
            tables.Add(new DocumentsDDL(dbName));
            tables.Add(new MenuProductDDL(dbName));

            tables.Add(new CostDetailDDL(dbName));

            foreach (var item in tables)
            {
                item.UpdateSchema(ctx);
            }

            ctx.Dispose();
        }

        // This method helps to render a partial view into html string.
        // http://craftycodeblog.com/2010/05/15/asp-net-mvc-render-partial-view-to-string/
        // Credit: Kevin Craft
        public string RenderPartialViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult =
                    ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext,
                    viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        /// <summary>
        /// This method helps to get the error information from the MVC "ModelState".
        /// We can not directly send the ModelState to the client in Json. The "ModelState"
        /// object has some circular reference that prevents it to be serialized to Json.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetErrorsFromModelState()
        {
            var errors = new Dictionary<string, object>();
            foreach (var key in ModelState.Keys)
            {
                // Only send the errors to the client.
                if (ModelState[key].Errors.Count > 0)
                {
                    errors[key] = ModelState[key].Errors;
                }
            }

            return errors;
        }


        private delegate void LongTimeTask_Delegate(string s);


        // POST: /Account/Login
        [AllowAnonymous]
        [HttpPost]
        public ActionResult UpdateAs()
        {
            //http://www.codeproject.com/Articles/426120/Calling-a-method-in-Csharp-asynchronously-using-de

            LongTimeTask_Delegate d = null;
            d = new LongTimeTask_Delegate(longJob);

            IAsyncResult R = null;

            MembershipUserCollection all = Membership.GetAllUsers();
            foreach (MembershipUser item in all)
            {
                R = d.BeginInvoke(item.UserName, null, null); //invoking the method                
            }

            R = d.BeginInvoke("db", null, null);

            return RedirectToAction("Index", "Home", new { area = "" });
            //            return Json(new { redirectUrl = Url.Action("Index", "Home", new { area = "" }) },JsonRequestBehavior.AllowGet);

        }

        private void longJob(string userName)
        {
            UpdateDatabase(userName);
        }


        public ActionResult MetaGenerator(string domain, string page)
        {
            domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port).ToLower();

            var host = new System.Uri(domain).Host;
            int index = host.LastIndexOf('.'), last = 3;
            while (index > 0 && index >= last - 3)
            {
                last = index;
                index = host.LastIndexOf('.', last - 1);
            }

            var res = host.Substring(index + 1);

            res = "~/Views/Shared/" + (res == "localhost" ? "gestionestampa.com" : res);

            try
            {
                TempData["MetaTitle"] = HttpContext.GetLocalResourceObject(res, page.ToLower() + "Title") + " - PapiroStar ";
                TempData["MetaDescription"] = HttpContext.GetLocalResourceObject(res, page.ToLower() + "Description");
                TempData["MetaKeyword"] = HttpContext.GetLocalResourceObject(res, page.ToLower() + "Keyword");
                TempData["MetaRobots"] = HttpContext.GetLocalResourceObject(res, page.ToLower() + "Robots") == null ? "index,follow" : HttpContext.GetLocalResourceObject(res, page.ToLower() + "Robots");
            }
            catch (Exception)
            {
                TempData["MetaTitle"] = "PapiroStar";
                TempData["MetaDescription"] = "";
                TempData["MetaKeyword"] = "";
                TempData["MetaRobots"] = "noindex, nofollow";
            }
            return null;
        }

        /// <summary>
        /// I want to track each disposable object and dispose them when controller will be dispose
        /// </summary>
        private IList<IDisposable> disposables;

        public IList<IDisposable> Disposables
        {
            get
            {
                disposables = disposables == null ? new List<IDisposable>() : disposables;
                return disposables;
            }
            set
            {
                disposables = value;
            }
        }

        /// <summary>
        /// Dispose each object that expone IDisposable Interface
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposables != null)
            {
                foreach (var disp in disposables)
                {
                    disp.Dispose();
                }
            }

            base.Dispose(disposing);
        }

    }

}