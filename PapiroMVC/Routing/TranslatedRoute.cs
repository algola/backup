﻿using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Routing;

namespace System.Web.Routing
{
    public class TranslatedRoute : Route
    {
        public const string DetectedCultureKey = "__ROUTING_DETECTED_CULTURE";

        public bool SetDetectedCulture { get; set; }

        public RouteValueDictionary RouteValueTranslationProviders { get; private set; }

        public TranslatedRoute(string url, RouteValueDictionary defaults, RouteValueDictionary routeValueTranslationProviders, bool setDetectedCulture, IRouteHandler routeHandler)
            : base(url, defaults, routeHandler)
        {
            this.RouteValueTranslationProviders = routeValueTranslationProviders;
            this.SetDetectedCulture = setDetectedCulture;
        }

        public TranslatedRoute(string url, RouteValueDictionary defaults, RouteValueDictionary routeValueTranslationProviders, RouteValueDictionary constraints, bool setDetectedCulture, IRouteHandler routeHandler)
            : base(url, defaults, constraints, routeHandler)
        {
            this.RouteValueTranslationProviders = routeValueTranslationProviders;
            this.SetDetectedCulture = setDetectedCulture;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            RouteData routeData = base.GetRouteData(httpContext);
            if (routeData == null) return null;

            // Translate route values
            foreach (KeyValuePair<string, object> pair in this.RouteValueTranslationProviders)
            {
                IRouteValueTranslationProvider translationProvider = pair.Value as IRouteValueTranslationProvider;
                if (translationProvider != null
                    && routeData.Values.ContainsKey(pair.Key))
                {
                    RouteValueTranslation translation = translationProvider.TranslateToRouteValue(
                        routeData.Values[pair.Key].ToString(),
                        CultureInfo.CurrentCulture);

                    routeData.Values[pair.Key] = translation.RouteValue;

                    // Store detected culture
                    if (routeData.DataTokens[DetectedCultureKey] == null)
                    {
                        routeData.DataTokens.Add(DetectedCultureKey, translation.Culture);
                    }

                    // Set detected culture
                    if (this.SetDetectedCulture)
                    {
                        System.Threading.Thread.CurrentThread.CurrentCulture = translation.Culture;
                        System.Threading.Thread.CurrentThread.CurrentUICulture = translation.Culture;
                    }
                }
            }

            return routeData;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            if (requestContext.RouteData.DataTokens.ContainsKey("GoingToRenderChildAction") && (bool)requestContext.RouteData.DataTokens["GoingToRenderChildAction"] == true)
            {
                return base.GetVirtualPath(requestContext, values);
            }

            RouteValueDictionary translatedValues = values;

            // Translate route values
            foreach (KeyValuePair<string, object> pair in this.RouteValueTranslationProviders)
            {
                IRouteValueTranslationProvider translationProvider = pair.Value as IRouteValueTranslationProvider;
                if (translationProvider != null
                    && translatedValues.ContainsKey(pair.Key))
                {
                    RouteValueTranslation translation =
                        translationProvider.TranslateToTranslatedValue(
                            translatedValues[pair.Key].ToString(), Thread.CurrentThread.CurrentCulture);

                    translatedValues[pair.Key] = translation.TranslatedValue;
                }
            }

            return base.GetVirtualPath(requestContext, translatedValues);
        }
    }
}