using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Routing;

namespace PapiroMVC.Areas.Account
{
    public class AccountAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Account";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {


            // TRANSLATION ROUTING 
            CultureInfo cultureEN = CultureInfo.GetCultureInfo("en-US");
            CultureInfo cultureIT = CultureInfo.GetCultureInfo("it-IT");

            DictionaryRouteValueTranslationProvider translationProvider = new DictionaryRouteValueTranslationProvider(
                new List<RouteValueTranslation> {
                    new RouteValueTranslation(cultureIT, "Login", "login-gestionale-per-stampa-offset-e-digitale"),
                    new RouteValueTranslation(cultureIT, "RecoveryPassword", "recupera-la-password-per-il-gestionale-per-la-stampa"),
                    new RouteValueTranslation(cultureIT, "Register", "registrazione-di-un-nuovo-centro-stampa"),
                    new RouteValueTranslation(cultureIT, "registerLink", "per-la-stampa-offset-e-digitale"),
                    new RouteValueTranslation(cultureIT, "loginLink", "gratis"),
                    new RouteValueTranslation(cultureIT, "EmailSent", "password-per-il-gestionale-inviata"),
                }
            );

            context.MapTranslatedRoute(
                    "Account_default",
                    "account/{action}/{id}",
                    new { controller = "Account", action = "Index", id = UrlParameter.Optional },
                new { controller = translationProvider, action = translationProvider },
                true);

        }
    }
}
