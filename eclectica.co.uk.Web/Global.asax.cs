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
                "404 Page", // Route name
                "notfound", // URL with parameters
                new { controller = "Error", action = "NotFound" } // Parameter defaults
            );

            routes.MapRoute(
                "Xml SiteMap", // Route name
                "sitemap", // URL with parameters
                new { controller = "Entry", action = "XmlSiteMap" } // Parameter defaults
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
                "Authentication", // Route name
                "auth/{action}", // URL with parameters
                new { controller = "Auth", action = "Logon" } // Parameter defaults
            );

            routes.MapRoute(
                "Short Link", // Route name
                "{id}", // URL with parameters
                new { controller = "Redirect", action = "Redirect", id = "0" },
                new { id = @"\d{5}" } 
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
            if (httpException == null)
                httpException = new HttpException(500, "Internal Server Error", exception);

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