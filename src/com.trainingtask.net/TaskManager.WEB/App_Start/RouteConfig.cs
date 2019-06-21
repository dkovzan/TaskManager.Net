using System.Web.Mvc;
using System.Web.Routing;

namespace TaskManager.WEB
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("version.txt");

            routes.IgnoreRoute("log.txt");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Project", action = "List", id = UrlParameter.Optional },
                namespaces: new[] { "TaskManager.WEB.Controllers" }
            );
        }
    }
}
