using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;

namespace PapiroMVC.Areas.DataBase
{
    public class DataBaseAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DataBase";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            // TRANSLATION ROUTING 
            CultureInfo cultureEN = CultureInfo.GetCultureInfo("en-US");
            CultureInfo cultureIT = CultureInfo.GetCultureInfo("it-IT");

            DictionaryRouteValueTranslationProvider translationProvider = new DictionaryRouteValueTranslationProvider(
                new List<RouteValueTranslation> {
                    new RouteValueTranslation(cultureIT, "HomeDb", "personalizza"),
                    new RouteValueTranslation(cultureIT, "Article", "materiali_per_la_stampa"),
                    new RouteValueTranslation(cultureIT, "CustomerSupplier", "anagrafica_clienti_fornitori_tipografia"),
                    new RouteValueTranslation(cultureIT, "IndexSheetPrintableArticle", "a_foglio_stampa_digitale_offset"),
                    new RouteValueTranslation(cultureIT, "IndexRollPrintableArticle", "a_rotolo_stampa_etichette_digitale_offset"),
                    new RouteValueTranslation(cultureIT, "IndexRigidPrintableArticle", "rigido_forex_vetrofanie_plotter_uv"),
                }
            );

            context.MapTranslatedRoute(
                "DataBase_Translated",
                "database/{controller}/{action}/{id}",
                new { controller = "DataBase", action = "Index", id = UrlParameter.Optional },            
                new { controller = translationProvider, action = translationProvider },
                true);

            //context.MapRoute(
            //    "DataBase_default",
            //    "Anagrafica/{controller}/{action}/{id}",
            //    new { controller = "DataBase", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}
