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
using SchemaManagemet;
using System.Threading.Tasks;
using Services;

namespace PapiroMVC.Controllers
{

    public class ControllerAlgolaBase : AsyncController
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
                CurrentDatabase = userName;
                CurrentUserDatabase = "root";
            }
            else
                ViewData["CurrentUser"] = null;

        
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