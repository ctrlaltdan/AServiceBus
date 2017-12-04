using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AServiceBus.Core.Domain;

namespace AServiceBus.Core.Bootstrap
{
    public class Subscriptions
    {
        private readonly IList<ISubscription> _subscriptions;

        public static async Task<Subscriptions> ForAsync(ISubscriptionFactory subscriptionFactory, IList<EventSubscription> eventSubscriptions)
        {
            var tasks = eventSubscriptions
                .Select(e => subscriptionFactory.FromAsync(e.EventType, e.SubscriptionName))
                .ToList();

            await Task.WhenAll(tasks);

            var subscriptions = tasks
                .Select(task => task.Result)
                .ToList();

            return new Subscriptions(subscriptions);
        }

        private Subscriptions(IList<ISubscription> subscriptions)
        {
            _subscriptions = subscriptions;
        }

        public IReadOnlyCollection<ISubscription> GetAll()
        {
            return _subscriptions.ToList();
        }
    }
}
