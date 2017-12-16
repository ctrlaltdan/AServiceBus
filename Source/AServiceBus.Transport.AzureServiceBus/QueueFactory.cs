using System.Threading.Tasks;
using AServiceBus.Contracts;
using AServiceBus.Core.Bootstrap;
using AServiceBus.Core.Receiver;

namespace AServiceBus.Transport.AzureServiceBus
{
    internal class QueueFactory : IQueueFactory
    {
        private readonly AzureServiceBusConfiguration _configuration;
        private readonly MessageSerializer _messageSerializer;

        public QueueFactory(AzureServiceBusConfiguration configuration, MessageSerializer messageSerializer)
        {
            _configuration = configuration;
            _messageSerializer = messageSerializer;
        }

        public Task<IQueue> FromAsync(IQueueInstance queue)
        {
            return Queue.ForAsync(_configuration, _messageSerializer, queue);
        }
    }
}
