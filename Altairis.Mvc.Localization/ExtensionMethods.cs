using System;
using System.Web;

namespace Altairis.Mvc.Localization {

    internal static class ExtensionMethods {

        /// <summary>
        /// Determines whether the specified request is probably caused by ASP.NET infrastructure.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// 	<c>true</c> if the specified request is infrastructure request otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Infrastructure requests include requests to <c>~/Trace.axd</c>, <c>~/WebResource.axd</c>,
        /// <c>~/ScriptResource.axd</c>,  <c>*_AppService.axd</c> and <c>~/App_*</c>.
        /// </remarks>
        public static bool IsInfrastructureRequest(this HttpRequest request) {
            string virtualPath = request.AppRelativeCurrentExecutionFilePath;
            return
                virtualPath.Equals("~/Trace.axd", StringComparison.OrdinalIgnoreCase) ||            // tracing
                virtualPath.Equals("~/WebResource.axd", StringComparison.OrdinalIgnoreCase) ||      // web resources
                virtualPath.Equals("~/ScriptResource.axd", StringComparison.OrdinalIgnoreCase) ||   // AJAX Extensions
                virtualPath.EndsWith("_AppService.axd", StringComparison.OrdinalIgnoreCase) ||      // AJAX Extensions
                virtualPath.StartsWith("~/App_", StringComparison.OrdinalIgnoreCase);               // App_* folders
        }
    }
}