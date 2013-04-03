using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WR.Blog.Mvc
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "BlogList",
            //    url: "Blog",
            //    defaults: new { controller = "Blog", action = "List" }
            //);

            //routes.MapRoute(
            //    name: "Contact",
            //    url: "Contact",
            //    defaults: new { controller = "Home", action = "Contact" }
            //);

            routes.MapRoute(
                name: "Content",
                url: "{urlSegment}",
                defaults: new { controller = "Blog", action = "ContentPage", urlSegment = string.Empty }
            );

            routes.MapRoute(
                name: "BlogPage",
                url: "Blog/{year}/{month}/{day}/{urlSegment}",
                defaults: new { controller = "Blog", action = "Index", year = UrlParameter.Optional, month = UrlParameter.Optional, day = UrlParameter.Optional, urlSegment = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Blog", action = "List", id = UrlParameter.Optional }
            );
        }
    }
}