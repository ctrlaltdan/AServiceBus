using System;
using System.Threading.Tasks;
using AServiceBus.Core.Domain;

namespace AServiceBus.Core.Bootstrap
{
    public interface ISubscriptionFactory
    {
        Task<ISubscription> FromAsync(Type eventType, string subscriptionName);
    }
}