using log4net;
using Ninject;
using Ninject.Web.Mvc;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TaskManager.BLL.Infrastructure;
using TaskManager.WEB.Mapping;

namespace TaskManager.WEB
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected void Application_Start()
        {
            _logger.Info("Starting application...");
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var kernel = new StandardKernel(
                new ServiceModule("EntitiesContext"),
                new AutoMapperModule());
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }

        protected void Application_Error()
        {
            _logger.Error(Server.GetLastError());
        }

    }
}
