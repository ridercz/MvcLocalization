using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Altairis.Mvc.Localization.DemoSite {

    [Route("")]
    public class HomeController : Controller {

        [Route("")]
        public ActionResult Index() {
            return this.View("~/Views/Shared/View.cshtml");
        }

        [Route("Other/{id:int=0}")]
        public ActionResult OtherAction(int? id) {
            return this.View("~/Views/Shared/View.cshtml");
        }

    }
}
