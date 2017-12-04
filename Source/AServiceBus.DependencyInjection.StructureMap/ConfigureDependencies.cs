using System.Reflection;
using AServiceBus.Core;
using AServiceBus.Core.Receiver;
using StructureMap;

namespace AServiceBus.DependencyInjection.StructureMap
{
    public class ConfigureDependencies : IConfigureDependencies
    {
        public static IConfigureDependencies Initialise()
        {
            return new ConfigureDependencies(new Container());
        }

        private readonly IContainer _container;
        private readonly IResolver _resolver;

        public ConfigureDependencies(IContainer container)
        {
            _container = container;
            
            _resolver = new Resolver(_container);

            Add(_resolver);
        }

        public IConfigureDependencies ScanForHandlers(Assembly assembly)
        {
            _container.Configure(config =>
            {
                config.Scan(_ =>
                {
                    _.Assembly(assembly);
                    _.ConnectImplementationsToTypesClosing(typeof(IHandle<>));
                });
            });
            return this;
        }

        public IConfigureDependencies Add<TConcrete>()
            where TConcrete : class
        {
            _container.Configure(config =>
            {
                config.For<TConcrete>();
            });

            return this;
        }

        public IConfigureDependencies Add<TInstance>(TInstance instance)
            where TInstance : class
        {
            _container.Configure(config =>
            {
                config.For<TInstance>().Use(_ => instance);
            });

            return this;
        }

        public IConfigureDependencies Add<TInterface, TConcrete>()
            where TConcrete : TInterface
        {
            _container.Configure(config =>
            {
                config.For<TInterface>().Use<TConcrete>();
            });

            return this;
        }

        public IConfigureDependencies AddSingleton<TConcrete>() where TConcrete : class
        {
            _container.Configure(config =>
            {
                config.ForSingletonOf<TConcrete>();
            });

            return this;
        }

        public IConfigureDependencies AddSingleton<TInterface, TConcrete>() where TConcrete : TInterface
        {
            _container.Configure(config =>
            {
                config.ForSingletonOf<TInterface>().Use<TConcrete>();
            });

            return this;
        }

        public IResolver Build()
        {
            return _resolver;
        }
    }
}
