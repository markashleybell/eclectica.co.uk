using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using eclectica.co.uk.Web.Infrastructure;
using MvcMiniProfiler;
using MvcMiniProfiler.MVCHelpers;
using eclectica.co.uk.Web.Controllers;
using LowercaseRoutesMVC;
using System.Configuration;

namespace eclectica.co.uk.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            // filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("elmah.axd");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRouteLowercase(
                "Home Page", // Route name
                "", // URL with parameters
                new { controller = "Entry", action = "Index" } // Parameter defaults
            );

            routes.MapRouteLowercase(
                "Main RSS Feed", // Route name
                "feed/main", // URL with parameters
                new { controller = "Entry", action = "Index", view = "RssFull" } // Parameter defaults
            );

            routes.MapRouteLowercase(
                "Headlines RSS Feed", // Route name
                "feed/summary", // URL with parameters
                new { controller = "Entry", action = "Index", view = "RssHeadlines" } // Parameter defaults
            );

            routes.MapRouteLowercase(
                "Facebook RSS Feed", // Route name
                "feed/facebook", // URL with parameters
                new { controller = "Entry", action = "Index", view = "RssFacebook" } // Parameter defaults
            );

            routes.MapRouteLowercase(
                "About Page", // Route name
                "about", // URL with parameters
                new { controller = "Template", action = "About" } // Parameter defaults
            );

            routes.MapRouteLowercase(
                "404 Page", // Route name
                "notfound", // URL with parameters
                new { controller = "Error", action = "NotFound" } // Parameter defaults
            );

            routes.MapRouteLowercase(
                "Xml SiteMap", // Route name
                "sitemap", // URL with parameters
                new { controller = "Entry", action = "XmlSiteMap" } // Parameter defaults
            );

            routes.MapRouteLowercase(
                "Links Page", // Route name
                "links", // URL with parameters
                new { controller = "Template", action = "Links" } // Parameter defaults
            );

            routes.MapRouteLowercase(
                "Random Entry", // Route name
                "random", // URL with parameters
                new { controller = "Entry", action = "Random" } // Parameter defaults
            );

            routes.MapRouteLowercase(
                "Search Results", // Route name
                "search", // URL with parameters
                new { controller = "Entry", action = "Search" } // Parameter defaults
            );

            routes.MapRouteLowercase(
                "Tags", // Route name
                "tags/{tagName}", // URL with parameters
                new { controller = "Template", action = "Tags", tagName = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRouteLowercase(
                "Archives", // Route name
                "{year}/{month}", // URL with parameters
                new { controller = "Entry", action = "Archive", year = 1970, month = 1 }, // Parameter defaults
                new { year = @"\d{4}", month = @"\d{2}" }
            );

            routes.MapRouteLowercase(
                "Home Page Paging", // Route name
                "p/{page}", // URL with parameters
                new { controller = "Entry", action = "Index", page = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRouteLowercase(
                "Authentication", // Route name
                "auth/{action}", // URL with parameters
                new { controller = "Auth", action = "Logon" } // Parameter defaults
            );

            routes.MapRouteLowercase(
                "Short Link", // Route name
                "{id}", // URL with parameters
                new { controller = "Redirect", action = "RedirectToUrl", id = "0" },
                new { id = @"\d{5}" } 
            );

            routes.MapRouteLowercase(
                "Entry", // Route name
                "{url}", // URL with parameters
                new { controller = "Entry", action = "Detail", url = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRouteLowercase(
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

            var copy = ViewEngines.Engines.ToList();
            ViewEngines.Engines.Clear();
            foreach (var item in copy)
            {
                ViewEngines.Engines.Add(new ProfilingViewEngine(item));
            }
        }

        protected void Application_BeginRequest()
        {
            MiniProfiler.Start();
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            if (Context.User == null || Context.User.Identity == null || !Context.User.Identity.IsAuthenticated)
            {
                MiniProfiler.Stop(discardResults: true);
            }
        }

        protected void Application_EndRequest()
        {
            MiniProfiler.Stop();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (Context.IsCustomErrorEnabled)
                ShowCustomErrorPage(Server.GetLastError());
        }

        private void ShowCustomErrorPage(Exception exception)
        {
            HttpException httpException = exception as HttpException;

            // Access config property directly to avoid Ninject dependency
            if (httpException == null)
                httpException = new HttpException(500, ConfigurationManager.AppSettings["Error500Text"], exception);

            Response.Clear();
            Response.TrySkipIisCustomErrors = true;

            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("fromAppErrorEvent", true);

            switch (httpException.GetHttpCode())
            {
                case 403:
                    routeData.Values.Add("action", "AccessDenied");
                    break;

                case 404:
                    routeData.Values.Add("action", "NotFound");
                    break;

                case 500:
                    routeData.Values.Add("action", "ServerError");
                    break;

                default:
                    routeData.Values.Add("action", "OtherHttpStatusCode");
                    routeData.Values.Add("httpStatusCode", httpException.GetHttpCode());
                    break;
            }

            Server.ClearError();

            IController controller = new ErrorController();
            controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        }
    }
}