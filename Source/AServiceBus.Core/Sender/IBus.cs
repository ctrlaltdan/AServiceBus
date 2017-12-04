using System.Threading.Tasks;
using AServiceBus.Contracts;

namespace AServiceBus.Core.Sender
{
    public interface IBus
    {
        Task PublishAsync(IEvent message);
        Task SendAsync<TQueueInstance>(ICommand command) where TQueueInstance : struct, IQueueInstance;
    }
}