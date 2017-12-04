using System.Threading.Tasks;
using AServiceBus.Contracts;
using AServiceBus.Core.Receiver;

namespace AServiceBus.Core.Domain
{
    public interface IQueue : IReceiver
    {
        string QueueName { get; }
        IQueueInstance QueueInstance { get; }
        Task SendAsync(ICommand command);
    }
}
