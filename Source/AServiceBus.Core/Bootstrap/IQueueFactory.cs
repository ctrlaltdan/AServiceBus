using System.Threading.Tasks;
using AServiceBus.Contracts;
using AServiceBus.Core.Domain;

namespace AServiceBus.Core.Bootstrap
{
    public interface IQueueFactory
    {
        Task<IQueue> FromAsync(IQueueInstance queueInstance);
    }
}