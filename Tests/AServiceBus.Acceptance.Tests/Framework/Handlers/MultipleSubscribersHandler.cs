using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AServiceBus.Acceptance.Tests.Framework.Events;
using AServiceBus.Core.Receiver;

namespace AServiceBus.Acceptance.Tests.Framework.Handlers
{
    public class MultipleSubscribersHandler : IHandle<MultipleSubscribersEventV1>
    {
        public static List<Guid> ReceivedMessageIds = new List<Guid>();

        private static readonly object Gate = new object();

        public Task HandleAsync(MultipleSubscribersEventV1 message)
        {
            lock (Gate)
            {
                var id = message.Id;
                ReceivedMessageIds.Add(id);
                message.ReceivedCount = ReceivedMessageIds.Count(m => m == id);
                EventStore.ReportEvent(message);
                return Task.CompletedTask;
            }
        }
    }
}
