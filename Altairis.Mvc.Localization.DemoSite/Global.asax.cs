using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Altairis.Mvc.Localization;

namespace Altairis.Mvc.Localization.DemoSite {
    public class MvcApplication : System.Web.HttpApplication {
        protected void Application_Start() {
            // Default routing preparations
            AreaRegistration.RegisterAllAreas();
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            // Setup localized attribute routing
            RouteTable.Routes.MapMvcAttributeRoutes(new LocalizedDirectRouteProvider());
        }
    }
}
