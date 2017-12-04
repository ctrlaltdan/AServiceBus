using System;
using System.IO;
using System.Text;
using AServiceBus.Contracts;
using AServiceBus.Core;
using Microsoft.ServiceBus.Messaging;

namespace AServiceBus.Transport.AzureServiceBus
{
    internal class BrokeredMessageFactory
    {
        private readonly MessageSerializer _messageSerializer;

        public BrokeredMessageFactory(MessageSerializer messageSerializer)
        {
            _messageSerializer = messageSerializer;
        }

        public BrokeredMessage From(IMessage message)
        {
            var messageId = Guid.NewGuid().ToString("N");
            var correlationId = Guid.NewGuid().ToString("N");
            var receivedAt = DateTimeProvider.Instance.Now;
            var content = _messageSerializer.ToString(message);
            var bytes = Encoding.UTF8.GetBytes(content);
            using (var stream = new MemoryStream(bytes))
            {
                return new BrokeredMessage(stream)
                {
                    ContentType = "application/json",
                    MessageId = messageId,
                    CorrelationId = correlationId,
                    ScheduledEnqueueTimeUtc = receivedAt.UtcDateTime
                };
            }
        }

        public dynamic From(Stream stream)
        {
            var body = new StreamReader(stream, true).ReadToEnd();
            return _messageSerializer.ToObject(body);
        }
    }
}
