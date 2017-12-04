using System;
using System.Threading.Tasks;
using AServiceBus.Core.Bootstrap;
using AServiceBus.Core.Domain;

namespace AServiceBus.Transport.AzureServiceBus
{
    internal class SubscriptionFactory : ISubscriptionFactory
    {
        private readonly AzureServiceBusConfiguration _configuration;

        public SubscriptionFactory(AzureServiceBusConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<ISubscription> FromAsync(Type eventType, string subscriptionName)
        {
            return Subscription.ForAsync(_configuration, eventType, subscriptionName);
        }
    }
}
