using System.Web.Mvc;
using System.Web.Routing;

namespace MANOR.API
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapHttpRoute(
            //name: "swagger_root",
            //routeTemplate: "",
            //defaults: null,
            //constraints: null,
            //handler: new RedirectHandler((message => message.RequestUri.ToString()), "swagger"));

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "compare", id = UrlParameter.Optional }
            );
        }
    }
}
