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

namespace PapiroMVC.Controllers
{

    public class ControllerBase : Controller
    {
        public MembershipUser CurrentUser
        { 
            get; 
            set; 
        }

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

            tables.Add(new PrinterMachinesDDL());
            tables.Add(new ArticlesDDL());
            tables.Add(new CustomerSupplierDLL());

            foreach (var item in tables)
            {
                item.UpdateSchema(ctx);
            }

            ctx.Dispose();
        }
    }
}