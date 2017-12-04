using System.Threading;
using System.Threading.Tasks;

namespace AServiceBus.Core.Receiver
{
    internal class Listener
    {
        private CancellationTokenSource _cancellationTokenSource;

        private readonly IConsumerFactory _consumerFactory;
        private readonly IReceiver _receiver;

        public static Listener For(IConsumerFactory consumerFactory, IReceiver receiver)
        {
            return new Listener(consumerFactory, receiver);
        }

        private Listener(IConsumerFactory consumerFactory, IReceiver receiver)
        {
            _consumerFactory = consumerFactory;
            _receiver = receiver;
        }

        public void StartListening()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            Task.Factory.StartNew(async () =>
                {
                    //Logging.Logger.Debug($"Listening to queue: {_queue.QueueName}");
                    
                    await _consumerFactory
                        .For(_receiver, _cancellationTokenSource)
                        .StartConsumingMessagesAsync();
                })
                .Unwrap()
                .ContinueWith(LogTaskEndState);
        }

        public void StopListening()
        {
            Logging.Logger.Debug("Listener cancellation token requested.");
            _cancellationTokenSource.Cancel();
        }

        private static void LogTaskEndState(Task task)
        {
            if (task.IsFaulted)
                Logging.Logger.Error(task.Exception, "Thread stopped due to an unhandled exception.");
            else
                Logging.Logger.Debug($"Thread stopped at state: {task.Status}.");
        }
    }
}