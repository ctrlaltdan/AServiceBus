using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AServiceBus.Contracts;
using AServiceBus.Core;
using AServiceBus.Core.Receiver;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace AServiceBus.Transport.AzureServiceBus
{
    internal class Queue : Receiver, IQueue
    {
        public IQueueInstance QueueInstance { get; }
        public string QueueName { get; }

        private readonly MessageSender _messageSender;
        private readonly MessageSerializer _messageSerializer;

        public static async Task<IQueue> ForAsync(AzureServiceBusConfiguration configuration, MessageSerializer messageSerializer, IQueueInstance queue)
        {
            var queueName = Conventions.QueuePath(queue.Name);
            
            var namespaceManager = NamespaceManager.CreateFromConnectionString(configuration.ConnectionString);
            if (!namespaceManager.QueueExists(queueName))
                await namespaceManager.CreateQueueAsync(queueName);

            var messagingFactory = MessagingFactory.CreateFromConnectionString(configuration.ConnectionString);

            var sender = await messagingFactory.CreateMessageSenderAsync(queueName);
            var receiver = await messagingFactory.CreateMessageReceiverAsync(queueName, ReceiveMode.PeekLock);
            
            return new Queue(sender, receiver, messageSerializer, queueName, queue);
        }

        private Queue(
            MessageSender sender, 
            MessageReceiver receiver, 
            MessageSerializer messageSerializer, 
            string queueName, 
            IQueueInstance queueInstance)
            : base (receiver)
        {
            _messageSender = sender;
            _messageSerializer = messageSerializer;
            QueueInstance = queueInstance;
            QueueName = queueName;
        }

        public async Task SendAsync(ICommand command)
        {
            var messageId = Guid.NewGuid().ToString("N");
            var correlationId = Guid.NewGuid().ToString("N");
            var receivedAt = DateTimeProvider.Instance.Now;
            var content = _messageSerializer.ToString(command);
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
