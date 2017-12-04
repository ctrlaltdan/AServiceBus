using AServiceBus.Core.Receiver;
using Microsoft.ServiceBus.Messaging;

namespace AServiceBus.Transport.AzureServiceBus
{
    public abstract class Receiver : IReceiver
    {
        public virtual MessageReceiver MessageReceiver { get; }

        protected Receiver(MessageReceiver messageReceiver)
        {
            MessageReceiver = messageReceiver;
        }
    }
}
