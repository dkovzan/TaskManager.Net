using Ninject.Modules;
using Ninject.Web.Common;
using TaskManager.DAL;

namespace TaskManager.BLL.Infrastructure
{
    public class ServiceModule : NinjectModule
    {
        private readonly string _connectionString;
        public ServiceModule(string connection)
        {
            _connectionString = connection;
        }
        public override void Load()
        {
            Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope().WithConstructorArgument(_connectionString);
        }
    }
}