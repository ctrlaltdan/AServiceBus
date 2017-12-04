using System.Threading.Tasks;

namespace AServiceBus.Core.Receiver
{
    public interface IConsumer
    {
        Task StartConsumingMessagesAsync();
    }
}
