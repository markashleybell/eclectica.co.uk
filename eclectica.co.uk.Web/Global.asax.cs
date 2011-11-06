using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using eclectica.co.uk.Web.Infrastructure;
using MvcMiniProfiler;

namespace eclectica.co.uk.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Home Page", // Route name
                "", // URL with parameters
                new { controller = "Entry", action = "Index" } // Parameter defaults
            );

            routes.MapRoute(
                "Main RSS Feed", // Route name
                "feed/main", // URL with parameters
                new { controller = "Entry", action = "Index", view = "RssFull" } // Parameter defaults
            );

            routes.MapRoute(
                "Headlines RSS Feed", // Route name
                "feed/summary", // URL with parameters
                new { controller = "Entry", action = "Index", view = "RssHeadlines" } // Parameter defaults
            );

            routes.MapRoute(
                "Facebook RSS Feed", // Route name
                "feed/facebook", // URL with parameters
                new { controller = "Entry", action = "Index", view = "RssFacebook" } // Parameter defaults
            );

            routes.MapRoute(
                "About Page", // Route name
                "about", // URL with parameters
                new { controller = "Template", action = "About" } // Parameter defaults
            );

            routes.MapRoute(
                "Links Page", // Route name
                "links", // URL with parameters
                new { controller = "Template", action = "Links" } // Parameter defaults
            );

            routes.MapRoute(
                "Random Entry", // Route name
                "random", // URL with parameters
                new { controller = "Entry", action = "Random" } // Parameter defaults
            );

            routes.MapRoute(
                "Search Results", // Route name
                "search", // URL with parameters
                new { controller = "Template", action = "Search" } // Parameter defaults
            );

            routes.MapRoute(
                "Archive", // Route name
                "tags/{tagName}", // URL with parameters
                new { controller = "Template", action = "Tags", tagName = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "Archives", // Route name
                "{year}/{month}", // URL with parameters
                new { controller = "Entry", action = "Archive", year = 1970, month = 1 }, // Parameter defaults
                new { year = @"\d{4}", month = @"\d{2}" }
            );

            routes.MapRoute(
                "Home Page Paging", // Route name
                "p/{page}", // URL with parameters
                new { controller = "Entry", action = "Index", page = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "Admin Area", // Route name
                "admin/{action}", // URL with parameters
                new { controller = "Admin", action = "Posts" } // Parameter defaults
            );

            routes.MapRoute(
                "Entry", // Route name
                "{url}", // URL with parameters
                new { controller = "Entry", action = "Detail", url = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Entry", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());
        }

        protected void Application_BeginRequest()
        {
            if (Request.IsLocal)
            {
                MiniProfiler.Start();
            }
        }

        protected void Application_EndRequest()
        {
            if (Request.IsLocal)
            {
                MiniProfiler.Stop();
            }
        }
    }
}