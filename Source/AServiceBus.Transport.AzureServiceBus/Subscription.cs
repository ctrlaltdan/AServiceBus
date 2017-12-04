using System;
using System.Threading.Tasks;
using AServiceBus.Core.Domain;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace AServiceBus.Transport.AzureServiceBus
{
    internal class Subscription : Receiver, ISubscription
    {
        public Type EventType { get; }
        
        internal static async Task<ISubscription> ForAsync(AzureServiceBusConfiguration configuration, Type eventType, string subscriptionName)
        {
            var topicPath = Conventions.TopicPath(eventType);
            
            var namespaceManager = NamespaceManager.CreateFromConnectionString(configuration.ConnectionString);
            var subscriptionExists = await namespaceManager.SubscriptionExistsAsync(topicPath, subscriptionName);
            if (!subscriptionExists)
                await namespaceManager.CreateSubscriptionAsync(topicPath, subscriptionName);

            var messagingFactory = MessagingFactory.CreateFromConnectionString(configuration.ConnectionString);

            var subscriptionPath = SubscriptionClient.FormatSubscriptionPath(topicPath, subscriptionName);
            
            var receiver = await messagingFactory.CreateMessageReceiverAsync(subscriptionPath, ReceiveMode.PeekLock);
            
            return new Subscription(receiver, eventType);
        }

        public Subscription(MessageReceiver receiver, Type eventType)
            : base(receiver)
        {
            EventType = eventType;
        }
    }
}
