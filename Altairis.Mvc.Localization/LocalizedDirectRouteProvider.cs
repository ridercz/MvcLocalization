using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Routing;

namespace Altairis.Mvc.Localization {

    public class LocalizedDirectRouteProvider : IDirectRouteProvider {

        public IReadOnlyList<RouteEntry> GetDirectRoutes(ControllerDescriptor controllerDescriptor, IReadOnlyList<ActionDescriptor> actionDescriptors, IInlineConstraintResolver constraintResolver) {
            // Get routes from default provider
            var defaultProvider = new DefaultDirectRouteProvider();
            var routes = defaultProvider.GetDirectRoutes(controllerDescriptor, actionDescriptors, constraintResolver);

            // Create new constraint to accept only supported url prefixes
            var lc = new LocaleConstraint();

            // Add locale parameter and constraint to every route
            foreach (var item in routes) {
                item.Route.Url = "{locale}/" + item.Route.Url;
                item.Route.Constraints.Add("locale", lc);
            }
            return routes;
        }
    }
}