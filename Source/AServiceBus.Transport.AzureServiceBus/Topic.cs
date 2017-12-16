using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AServiceBus.Contracts;
using AServiceBus.Core;
using AServiceBus.Core.Sender;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace AServiceBus.Transport.AzureServiceBus
{
    internal class Topic : ITopic
    {
        public Type EventType { get; }

        private readonly MessageSender _messageSender;
        private readonly MessageSerializer _messageSerializer;

        internal static async Task<ITopic> ForAsync(AzureServiceBusConfiguration configuration, MessageSerializer messageSerializer, Type eventType)
        {
            var topicPath = Conventions.TopicPath(eventType);
            
            var namespaceManager = NamespaceManager.CreateFromConnectionString(configuration.ConnectionString);
            if (!namespaceManager.TopicExists(topicPath))
                await namespaceManager.CreateTopicAsync(topicPath);

            var messagingFactory = MessagingFactory.CreateFromConnectionString(configuration.ConnectionString);

            var sender = await messagingFactory.CreateMessageSenderAsync(topicPath);
            return new Topic(messageSerializer, sender, eventType);
        }

        public Topic(MessageSerializer messageSerializer, MessageSender messageSender, Type eventType)
        {
            _messageSerializer = messageSerializer;
            _messageSender = messageSender;
            EventType = eventType;
        }
        
        public async Task Publish(IEvent @event)
        {
            var messageId = Guid.NewGuid().ToString("N");
            var correlationId = Guid.NewGuid().ToString("N");
            var receivedAt = DateTimeProvider.Instance.Now;
            var content = _messageSerializer.ToString(@event);
            var bytes = Encoding.UTF8.GetBytes(content);
            using (var stream = new MemoryStream(bytes))
            {
                var brokeredMessage = new BrokeredMessage(stream)
                {
                    ContentType = "application/json",
                    MessageId = messageId,
                    CorrelationId = correlationId,
                    ScheduledEnqueueTimeUtc = receivedAt.UtcDateTime
                };

                await _messageSender.SendAsync(brokeredMessage);
            }
        }
    }
}
