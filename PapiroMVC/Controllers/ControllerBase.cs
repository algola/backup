using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Providers.Entities;
using System.Web.Security;
using PapiroMVC.Validation;
using System.Diagnostics;
using System.Text.RegularExpressions;
using PapiroMVC.Models;
using PapiroMVC.Model;
using System.IO;

namespace PapiroMVC.Controllers
{

    public class ControllerBase : Controller
    {
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
                CurrentDatabase = "MYSQL5_898486_user1";
                CurrentUserDatabase = "papiro1";
            }
            else
                ViewData["CurrentUser"] = null;

//MULTILANGUE
        
        }

        protected override void ExecuteCore()
        {

            base.ExecuteCore();
        }

        public void UpdateDatabase()
        {

            dbEntities ctx = new dbEntities();

            if (CurrentDatabase != null)
            {
                ctx.Database.Connection.ConnectionString = ctx.Database.Connection.ConnectionString.Replace("db", CurrentDatabase);
                ctx.Database.Connection.Open();
            }
            var tables = new List<IDDL>();

            tables.Add(new TaskExecutorsDDL());
            tables.Add(new ArticlesDDL());
            tables.Add(new CustomerSupplierDLL());

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

    }
}