using System.Threading;

namespace AServiceBus.Core.Receiver
{
    public interface IConsumerFactory
    {
        IConsumer For(IReceiver queue, CancellationTokenSource cancellationTokenSource);
    }
}
