using System.Threading;
using AServiceBus.Core.Receiver;

namespace AServiceBus.Transport.AzureServiceBus
{
    internal class ConsumerFactory : IConsumerFactory
    {
        private readonly BrokeredMessageProcessor _brokeredMessageProcessor;

        public ConsumerFactory(BrokeredMessageProcessor brokeredMessageProcessor)
        {
            _brokeredMessageProcessor = brokeredMessageProcessor;
        }

        public IConsumer For(IReceiver receiver, CancellationTokenSource cancellationTokenSource)
        {
            var baseType = (Receiver) receiver;
            return Consumer.For(_brokeredMessageProcessor, baseType.MessageReceiver, cancellationTokenSource);
        }
    }
}
