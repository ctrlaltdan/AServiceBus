using System.Threading;
using System.Threading.Tasks;
using AServiceBus.Core.Receiver;
using Microsoft.ServiceBus.Messaging;

namespace AServiceBus.Transport.AzureServiceBus
{
    internal class Consumer : IConsumer
    {
        private readonly BrokeredMessageProcessor _brokeredMessageProcessor;
        private readonly MessageReceiver _messageReceiver;
        private readonly CancellationTokenSource _cancellationTokenSource;

        internal static Consumer For(BrokeredMessageProcessor brokeredMessageProcessor, MessageReceiver receiver, CancellationTokenSource cancellationTokenSource)
        {
            return new Consumer(brokeredMessageProcessor, receiver, cancellationTokenSource);
        }

        private Consumer(BrokeredMessageProcessor brokeredMessageProcessor, MessageReceiver messageReceiver, CancellationTokenSource cancellationTokenSourceSource)
        {
            _brokeredMessageProcessor = brokeredMessageProcessor;
            _messageReceiver = messageReceiver;
            _cancellationTokenSource = cancellationTokenSourceSource;
        }

        public async Task StartConsumingMessagesAsync()
        {
            var doneReceiving = new TaskCompletionSource<bool>();

            _cancellationTokenSource.Token.Register(
                async () =>
                {
                    await _messageReceiver.CloseAsync();
                    doneReceiving.SetResult(true);
                });
            
            _messageReceiver.OnMessageAsync(
                async message => await _brokeredMessageProcessor.Process(message),
                new OnMessageOptions
                {
                    AutoComplete = false,
                    MaxConcurrentCalls = 10
                }
            );

            await doneReceiving.Task;
        }
    }
}
