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
                    new RouteValueTranslation(cultureIT, "HomeDb", "configuratore-stampa-e-carta"),
                    new RouteValueTranslation(cultureIT, "Article", "materiale-in-anagrafica"),
                    new RouteValueTranslation(cultureIT, "CustomerSupplier", "anagrafica-clienti-fornitori-tipografia"),
                    new RouteValueTranslation(cultureIT, "IndexSheetPrintableArticle", "a-foglio-stampa-digitale-offset"),
                    new RouteValueTranslation(cultureIT, "IndexRollPrintableArticle", "a-rotolo-stampa-etichette-digitale-offset"),
                    new RouteValueTranslation(cultureIT, "IndexRigidPrintableArticle", "rigido-forex-vetrofanie-plotter-uv"),
                    new RouteValueTranslation(cultureIT, "TaskExecutor", "macchina-da-stampa-o-lavorazione"),
                
                
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
