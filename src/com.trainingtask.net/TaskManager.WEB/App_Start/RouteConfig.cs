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
                url: "{controller}/{action}",
                defaults: new { controller = "Account", action = "SignIn"},
                namespaces: new[] { "TaskManager.WEB.Controllers" }
            );

            routes.MapRoute(
                name: "Entities",
                url: "{controller}/{action}/{id}",
                namespaces: new[] { "TaskManager.WEB.Controllers" }
            );
        }
    }
}
