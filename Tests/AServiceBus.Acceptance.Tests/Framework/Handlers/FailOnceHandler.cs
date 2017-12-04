using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AServiceBus.Acceptance.Tests.Framework.Commands;
using AServiceBus.Core.Receiver;

namespace AServiceBus.Acceptance.Tests.Framework.Handlers
{
    public class FailOnceHandler : IHandle<FailOnceV1>
    {
        public static List<Guid> FailedMessageIds = new List<Guid>();

        public Task HandleAsync(FailOnceV1 message)
        {
            if (!FailedMessageIds.Contains(message.Id))
            {
                FailedMessageIds.Add(message.Id);
                message.DeliveryAttempt = 1;
                EventStore.ReportEvent(message);
                throw new Exception("Testing failed once error handling.");
            }

            message.DeliveryAttempt = 2;
            EventStore.ReportEvent(message);
            return Task.CompletedTask;
        }
    }
}
