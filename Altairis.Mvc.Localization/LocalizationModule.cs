using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Altairis.Mvc.Localization.Configuration;

namespace Altairis.Mvc.Localization {
    public class LocalizationModule : IHttpModule {
        IEnumerable<ExceptionElement> exceptions;
        IEnumerable<LocaleElement> locales;
        IEnumerable<MappingElement> mappings;
        string fallbackPrefix;

        public void Dispose() {
            // NOOP
        }

        public void Init(HttpApplication context) {
            // Read configuration
            var config = ConfigurationManager.GetSection("altairis.mvc/localization") as LocalizationSection;
            this.fallbackPrefix = config.FallbackLocale;
            this.exceptions = config.Exceptions.Cast<ExceptionElement>();
            this.locales = config.Locales.Cast<LocaleElement>();
            this.mappings = config.Mappings.Cast<MappingElement>();

            // Add begin request handler
            context.BeginRequest += context_BeginRequest;
        }

        void context_BeginRequest(object sender, EventArgs e) {
            var app = sender as HttpApplication;

            // If it's infrastructure request, do nothing
            if (app.Request.IsInfrastructureRequest()) return;

            // Check if URL is in exceptions
            var path = app.Request.Url.PathAndQuery;
            if (this.exceptions.Any(x => x.Match == ExceptionMatchType.StartsWith && path.StartsWith(x.Path, StringComparison.OrdinalIgnoreCase))) return;
            if (this.exceptions.Any(x => x.Match == ExceptionMatchType.EndsWith && path.EndsWith(x.Path, StringComparison.OrdinalIgnoreCase))) return;
            if (this.exceptions.Any(x => x.Match == ExceptionMatchType.Exact && path.Equals(x.Path, StringComparison.OrdinalIgnoreCase))) return;
            if (this.exceptions.Any(x => x.Match == ExceptionMatchType.Regex && Regex.IsMatch(path, x.Path))) return;

            // Get prefix (first directory in path) and check if its valid
            var prefix = GetPrefixFromPath(path);
            var locale = this.locales.SingleOrDefault(x => x.Prefix.Equals(prefix, StringComparison.OrdinalIgnoreCase));

            // If prefix is valid locale, set cultures and carry on
            if (locale != null) {
                Thread.CurrentThread.CurrentCulture = locale.Culture;
                Thread.CurrentThread.CurrentUICulture = locale.UiCulture;
                return;
            }

            // Find best locale prefix
            prefix = this.GetPrefixFromMap(app.Request.UserLanguages);

            // Redirect to prefixed page
            var newUri = ("/" + prefix + path).TrimEnd('/');
            app.Response.Redirect(newUri, true);
        }

        private string GetPrefixFromMap(IEnumerable<string> acceptLanguages) {
            // Return fallback prefix if no accept-language header is present
            if (acceptLanguages == null || !acceptLanguages.Any()) return this.fallbackPrefix;

            // Try each accepted language
            foreach (var lang in acceptLanguages) {
                var map = this.mappings.FirstOrDefault(x => lang.StartsWith(x.Language, StringComparison.OrdinalIgnoreCase));
                if (map != null) return map.Locale;
            }

            // Nothing found, return fallback
            return this.fallbackPrefix;
        }

        private static string GetPrefixFromPath(string path) {
            if (path == null) throw new ArgumentNullException("path");
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "path");

            var pathData = path.Split('/');
            if (pathData.Length < 2) return null;
            return pathData[1];
        }


    }
}
