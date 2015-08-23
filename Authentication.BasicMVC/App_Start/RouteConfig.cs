using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Authentication.BasicMVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );


            routes.MapRoute(
                name: "APIRoute",
                url: "API/{controller}/{id}",
                defaults: new { controller = "Authentication", id = UrlParameter.Optional }, namespaces: new[] { "Authentication.BasicMVC.Controllers.API" }
            );
        }
    }
}
