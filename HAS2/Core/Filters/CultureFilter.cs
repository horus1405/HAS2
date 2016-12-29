using System.Web.Mvc;
using HAS2.Core.Utilities;

namespace HAS2.Core.Filters
{
    public class CultureFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //We'll have 4 sources for language:
            //1.Demanded via URL
            //2.Cookie storage
            //3.Session 
            //4.Browser

            CultureManager.SetDemandedCultureAsDefault();
            base.OnActionExecuting(filterContext);
        }

        
    }
}