using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HAS2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {

            string[] sections = new string[] { "Setup", "Sales", "Inventory"};

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            foreach (string section in sections)
            {
                routes.MapRoute(
                    name: section,
                    url: section+"/{controller}/{action}/{id}",
                    defaults: new { controller = section, action = "Index", id = UrlParameter.Optional }
                );
            }

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                );
        }
    }
}
