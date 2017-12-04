using System;
using AServiceBus.Core;

namespace AServiceBus.Transport.AzureServiceBus
{
    internal class Conventions
    {
        public static string TopicPath(Type eventType)
        {
            return $"{MessageBrokerContext.Environment}.{eventType.FullName}".ToLower();
        }

        public static string QueuePath(string queueName)
        {
            return $"{MessageBrokerContext.Environment}.{queueName}".ToLower();
        }
    }
}
