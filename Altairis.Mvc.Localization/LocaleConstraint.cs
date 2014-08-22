using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Altairis.Mvc.Localization.Configuration;

namespace Altairis.Mvc.Localization {

    public class LocaleConstraint : IRouteConstraint {
        private IEnumerable<string> knownLocalePrefixes;

        public LocaleConstraint() {
            // Get list of supported locales
            var cfg = System.Configuration.ConfigurationManager.GetSection("altairis.mvc/localization") as LocalizationSection;
            this.knownLocalePrefixes = cfg.Locales.Cast<LocaleElement>().Select(x => x.Prefix);
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection) {
            var locale = values[parameterName] as string;
            if (string.IsNullOrWhiteSpace(locale)) return false;
            return this.knownLocalePrefixes.Contains(locale, StringComparer.OrdinalIgnoreCase);
        }
    }
}