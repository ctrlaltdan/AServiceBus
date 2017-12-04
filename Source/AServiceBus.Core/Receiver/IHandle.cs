using System.Threading.Tasks;

namespace AServiceBus.Core.Receiver
{
    public interface IHandle<in TMessage>
    {
        Task HandleAsync(TMessage message);
    }
}
