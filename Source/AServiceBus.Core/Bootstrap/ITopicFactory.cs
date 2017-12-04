using System;
using System.Threading.Tasks;
using AServiceBus.Core.Domain;

namespace AServiceBus.Core.Bootstrap
{
    public interface ITopicFactory
    {
        Task<ITopic> From(Type eventType);
    }
}