using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Routing;

namespace PapiroMVC.Areas.Working
{
    public class WorkingAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Working";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            // TRANSLATION ROUTING 
            CultureInfo cultureEN = CultureInfo.GetCultureInfo("en-US");
            CultureInfo cultureIT = CultureInfo.GetCultureInfo("it-IT");

            DictionaryRouteValueTranslationProvider translationProvider = new DictionaryRouteValueTranslationProvider(
                new List<RouteValueTranslation> {
                    //new RouteValueTranslation(cultureIT, "Document", "preventivi-commesse-tipografia"),
                    //new RouteValueTranslation(cultureIT, "ListEstimate", "elenco-documenti-centro-stampa"),
                    //new RouteValueTranslation(cultureIT, "CreateProduct", "inserimento-di-uno-stampato"),
                    //new RouteValueTranslation(cultureIT, "Error", "errore"),
                    //new RouteValueTranslation(cultureIT, "NoTaskEstimatedOnException", "manca-definizione-costo")
                }
            );

            context.MapTranslatedRoute(
                "Working_default",
                "working-area/{controller}/{action}/{id}",
                new { controller = "Working", action = "Index", id = UrlParameter.Optional },
                new { controller = translationProvider, action = translationProvider },
                true);

            //context.MapRoute(
            //    "Working_default",
            //    "Working/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}
