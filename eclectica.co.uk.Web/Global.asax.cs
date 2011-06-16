using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using eclectica.co.uk.Web.Infrastructure;
using eclectica.co.uk.Service.Configuration;
using MvcMiniProfiler;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

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
                "About Page", // Route name
                "about", // URL with parameters
                new { controller = "Template", action = "About" } // Parameter defaults
            );

            routes.MapRoute(
                "Tag Imdex and Entries By Tag", // Route name
                "tags/{tag}", // URL with parameters
                new { controller = "Template", action = "Tags", tag = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "Home Page Paging", // Route name
                "p/{page}", // URL with parameters
                new { controller = "Entry", action = "Index", page = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{page}", // URL with parameters
                new { controller = "Entry", action = "Index", page = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());

            Initialization.InitializeDb();
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
            MiniProfiler.Stop();
        }
    }
}