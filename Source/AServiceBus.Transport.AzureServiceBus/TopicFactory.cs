using System;
using System.Threading.Tasks;
using AServiceBus.Core.Bootstrap;
using AServiceBus.Core.Domain;

namespace AServiceBus.Transport.AzureServiceBus
{
    internal class TopicFactory : ITopicFactory
    {
        private readonly AzureServiceBusConfiguration _configuration;
        private readonly MessageSerializer _messageSerializer;

        public TopicFactory(AzureServiceBusConfiguration configuration, MessageSerializer messageSerializer)
        {
            _configuration = configuration;
            _messageSerializer = messageSerializer;
        }

        public Task<ITopic> From(Type eventType)
        {
            return Topic.ForAsync(_configuration, _messageSerializer, eventType);
        }
    }
}
