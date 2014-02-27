using PapiroMVC.Models;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PapiroMVC
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
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

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            ModelBinders.Binders.Add(typeof(Product), new ProductModelBinder());
            ModelBinders.Binders.Add(typeof(ProductPart), new ProductPartModelBinder());
            ModelBinders.Binders.Add(typeof(ProductPartsPrintableArticle), new ProductPartsPrintableArticleModelBinder());
            ModelBinders.Binders.Add(typeof(ProductPartTask), new ProductPartTaskModelBinder());

            ModelMetadataProviders.Current = new CustomModelMetadataProvider();
            ValueProviderFactories.Factories.Add(new JsonValueProviderFactory());

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            //json formatter
            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize;

            json.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;

            json.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;

         //   json.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Objects;
        }
    }
}
