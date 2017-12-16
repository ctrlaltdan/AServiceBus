using System;
using System.Threading.Tasks;
using AServiceBus.Contracts;

namespace AServiceBus.Core.Domain
{
    public interface ITopic
    {
        Type EventType { get; }
        Task Publish(IEvent @event);
    }
}
