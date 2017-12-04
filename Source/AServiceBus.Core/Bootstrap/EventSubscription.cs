using System;

namespace AServiceBus.Core.Bootstrap
{
    public class EventSubscription
    {
        public readonly Type EventType;
        public readonly string SubscriptionName;

        public static EventSubscription For(Type eventType, string subscriptionName)
        {
            return new EventSubscription(eventType, subscriptionName);
        }

        private EventSubscription(Type eventType, string subscriptionName)
        {
            EventType = eventType;
            SubscriptionName = subscriptionName;
        }
    }
}
