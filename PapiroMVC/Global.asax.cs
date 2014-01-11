using PapiroMVC.Models;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PapiroMVC
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelBinders.Binders.Add(typeof(Product), new ProductModelBinder());
            ModelBinders.Binders.Add(typeof(ProductPart), new ProductPartModelBinder());
            ModelBinders.Binders.Add(typeof(ProductPartsPrintableArticle), new ProductPartsPrintableArticleModelBinder());
            ModelBinders.Binders.Add(typeof(ProductPartTask), new ProductPartTaskModelBinder());

            ModelMetadataProviders.Current = new CustomModelMetadataProvider();

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // code to change culture

            //OLD VERSION

            //string cultureName = null;
            //// Attempt to read the culture cookie from Request
            //HttpCookie cultureCookie = Request.Cookies["_culture"];
            //if (cultureCookie != null)
            //    cultureName = cultureCookie.Value;
            //else
            //    cultureName = Request.UserLanguages[0]; // obtain it from HTTP header AcceptLanguages

            string cultureName = String.Empty;
            var dom = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

            dom = dom.ToLower();

            if (dom.Contains("localhost") || dom.Contains("stampa"))
            {
                cultureName = "it-IT";                
            }

            // Validate culture name
            cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

        }
    }


}