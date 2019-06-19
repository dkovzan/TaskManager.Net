using AutoMapper;
using Ninject;
using Ninject.Modules;
using TaskManager.BLL.Mapping;

namespace TaskManager.WEB.Mapping
{
    public class AutoMapperModule : NinjectModule
    {
        public override void Load()
        {
            var mapperConfiguration = CreateConfiguration();

            Bind<MapperConfiguration>().ToConstant(mapperConfiguration).InSingletonScope();

            Bind<IMapper>().ToMethod(ctx =>
                 new Mapper(mapperConfiguration, type => ctx.Kernel.Get(type)));
        }

        private MapperConfiguration CreateConfiguration()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BLLMappingProfile());
                cfg.AddProfile(new WebMappingProfile());
            });

            config.AssertConfigurationIsValid();

            return config;
        }
    }
}
