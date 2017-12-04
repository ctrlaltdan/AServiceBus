using AServiceBus.Core;
using AServiceBus.Core.Bootstrap;
using AServiceBus.Core.Receiver;

namespace AServiceBus.Transport.AzureServiceBus
{
    public static class DependencyConfiguration
    {
        public static IConfigureDependencies AddAzureServiceBusTransportProvider(this IConfigureDependencies configuration, AzureServiceBusConfiguration azureServiceBusConfiguration)
        {
            configuration.Add(azureServiceBusConfiguration);
            configuration.Add<IQueueFactory, QueueFactory>();
            configuration.Add<ITopicFactory, TopicFactory>();
            configuration.Add<ISubscriptionFactory, SubscriptionFactory>();
            configuration.Add<IConsumerFactory, ConsumerFactory>();
            configuration.Add<MessageSerializer>();
            configuration.Add<BrokeredMessageFactory>();
            return configuration;
        }
    }
}
