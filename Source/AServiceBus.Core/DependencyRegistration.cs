using AServiceBus.Core.Bootstrap;
using AServiceBus.Core.Receiver;
using AServiceBus.Core.Sender;

namespace AServiceBus.Core
{
    public static class DependencyConfiguration
    {
        public static IConfigureDependencies AddCoreDependencies(this IConfigureDependencies configuration
            //, BusConfigurationBuilder busConfigurationBuilder
            )
        {
            configuration.Add<IBus, Bus>();
            configuration.Add<IMonitor, Monitor>();
            configuration.Add<HandlerFactory>();
            configuration.Add<IBusConfiguration, BusConfiguration>();
            configuration.Add<BusConfigurationBuilder>();
            return configuration;
        }
    }
}
