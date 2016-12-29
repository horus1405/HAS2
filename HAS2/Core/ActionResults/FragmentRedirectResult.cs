using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace HAS2.Core.ActionResults
{
    public class FragmentRedirectResult : RedirectToRouteResult
    {
        public FragmentRedirectResult(RouteValueDictionary routeValues) : base(routeValues) { }

        public FragmentRedirectResult(string routeName, RouteValueDictionary routeValues) : base(routeName, routeValues) { }

        public FragmentRedirectResult(string routeName, RouteValueDictionary routeValues, bool permanent) : base(routeName, routeValues, permanent) { }


        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (context.IsChildAction)
            {
                throw new InvalidOperationException("Cannot redirect in child action");
            }

            var destinationUrl = UrlHelper.GenerateUrl(RouteName, null /* actionName */, null /* controllerName */, RouteValues, RouteTable.Routes, context.RequestContext, false /* includeImplicitMvcValues */);
            if (String.IsNullOrEmpty(destinationUrl))
            {
                throw new InvalidOperationException("No route matched");
            }

            destinationUrl = destinationUrl.Replace("%23", "#");


            context.Controller.TempData.Keep();

            if (Permanent)
            {
                context.HttpContext.Response.RedirectPermanent(destinationUrl, endResponse: false);
            }
            else
            {
                context.HttpContext.Response.Redirect(destinationUrl, endResponse: false);
            }
        }
    }
}