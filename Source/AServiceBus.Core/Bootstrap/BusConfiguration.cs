using System;
using System.Collections.Generic;
using AServiceBus.Contracts;
using AServiceBus.Core.Domain;
using AServiceBus.Core.Receiver;

namespace AServiceBus.Core.Bootstrap
{
    internal class BusConfiguration : IBusConfiguration
    {
        internal static Queues Queues;
        internal static Topics Topics;
        internal static Subscriptions Subscriptions;

        public IQueue GetQueue<TQueueInstance>()
            where TQueueInstance : struct, IQueueInstance
        {
            return Queues.GetBy<TQueueInstance>();
        }
        
        public ITopic GetTopic(Type eventType)
        {
            return Topics.GetBy(eventType);
        }

        public IReadOnlyCollection<IReceiver> GetReceivers()
        {
            var queues = Queues.GetAll();
            var subscriptions = Subscriptions.GetAll();

            var receivers = new List<IReceiver>();
            receivers.AddRange(queues);
            receivers.AddRange(subscriptions);
            return receivers;
        }
    }
}
