using System;
using System.Collections.Generic;
using AServiceBus.Contracts;
using AServiceBus.Core.Domain;
using AServiceBus.Core.Receiver;

namespace AServiceBus.Core.Bootstrap
{
    public interface IBusConfiguration
    {
        ITopic GetTopic(Type eventType);
        IQueue GetQueue<TQueueInstance>() where TQueueInstance : struct, IQueueInstance;
        IReadOnlyCollection<IReceiver> GetReceivers();
    }
}