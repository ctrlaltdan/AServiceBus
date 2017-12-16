using System;
using AServiceBus.Core.Receiver;

namespace AServiceBus.Core.Domain
{
    public interface ISubscription : IReceiver
    {
        Type EventType { get; }
    }
}
