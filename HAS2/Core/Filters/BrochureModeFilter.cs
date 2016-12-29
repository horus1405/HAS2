using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HAS2.Core.Filters
{
    /// <summary>
    /// Redirects to home page if Brochre Mode is set to true
    /// </summary>
    public class BrochureModeFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Result = new RedirectResult(HAS2.Core.Utilities.Configuration.SiteUrl);
        }
    }
}
