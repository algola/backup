using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace PapiroMVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // TRANSLATION ROUTING 
            CultureInfo cultureEN = CultureInfo.GetCultureInfo("en-US");
            CultureInfo cultureIT = CultureInfo.GetCultureInfo("it-IT");

            DictionaryRouteValueTranslationProvider translationProvider = new DictionaryRouteValueTranslationProvider(
                new List<RouteValueTranslation> {
                    new RouteValueTranslation(cultureEN, "Home", "printing_software"),
                    new RouteValueTranslation(cultureEN, "About", "About"),
                    new RouteValueTranslation(cultureIT, "Home", "software_gestionale_per_tipografie"),
                    new RouteValueTranslation(cultureIT, "Price", "on_line_gratis_e_prezzi"),
                    new RouteValueTranslation(cultureIT, "DataBase", "gestione_personalizzata"),
                    new RouteValueTranslation(cultureIT, "HomeDb", "personalizza"),

                }
            );

            routes.MapTranslatedRoute(
                "TranslatedRoute",
                "{controller}/{action}/{id}",
            new { controller = "Home", action = "Index", id = "" },
            new { controller = translationProvider, action = translationProvider },
            true);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });

        }
    }
}