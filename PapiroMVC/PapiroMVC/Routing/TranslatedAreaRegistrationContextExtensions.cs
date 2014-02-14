using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace System.Web.Routing
{
    public static class TranslatedAreaRegistrationContextExtensions
    {
        public static Route MapTranslatedRoute(this AreaRegistrationContext areaContext, string name, string url, object defaults, object routeValueTranslationProviders, bool setDetectedCulture)
        {
            Route route = new TranslatedRoute(
                url,
                new RouteValueDictionary(defaults),
                new RouteValueDictionary(routeValueTranslationProviders),
                setDetectedCulture,
                new MvcRouteHandler());

            if (route.DataTokens == null)
                route.DataTokens = new RouteValueDictionary();
            
            route.DataTokens["area"] = areaContext.AreaName;

            // disabling the namespace lookup fallback mechanism keeps this areas from accidentally picking up
            // controllers belonging to other areas
            bool useNamespaceFallback = (areaContext.Namespaces == null || areaContext.Namespaces.Count == 0);
//            route.DataTokens["UseNamespaceFallback"] = useNamespaceFallback;

            areaContext.Routes.Add(route);

            return route;
        }
/*
        public static TranslatedRoute MapTranslatedRoute(this AreaRegistrationContext context, string name, string url, object defaults, object routeValueTranslationProviders, bool setDetectedCulture)
        {
            TranslatedRoute route = new TranslatedRoute(
                url,
                new RouteValueDictionary(defaults),
                new RouteValueDictionary(routeValueTranslationProviders),
                setDetectedCulture,
                new MvcRouteHandler());

            context.Routes.Add(name, route);

            return route;
        }

        public static TranslatedRoute MapTranslatedRoute(this AreaRegistrationContext context, string name, string url, object defaults, object routeValueTranslationProviders, object constraints, bool setDetectedCulture)
        {
            TranslatedRoute route = new TranslatedRoute(
                url,
                new RouteValueDictionary(defaults),
                new RouteValueDictionary(routeValueTranslationProviders),
                new RouteValueDictionary(constraints),
                setDetectedCulture,
                new MvcRouteHandler());
            context.Routes.Add(name, route);
            return route;
        }
 */
    }
}
