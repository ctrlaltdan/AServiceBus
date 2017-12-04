using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AServiceBus.Acceptance.Tests.Framework.Commands;
using AServiceBus.Core.Receiver;

namespace AServiceBus.Acceptance.Tests.Framework.Handlers
{
    public class FailTwiceHandler : IHandle<FailTwiceV1>
    {
        public static List<Guid> FailedOnceMessageIds = new List<Guid>();
        public static List<Guid> FailedTwiceMessageIds = new List<Guid>();

        public Task HandleAsync(FailTwiceV1 message)
        {
            if (!FailedOnceMessageIds.Contains(message.Id))
            {
                FailedOnceMessageIds.Add(message.Id);
                message.DeliveryAttempt = 1;
                EventStore.ReportEvent(message);
                throw new Exception("Testing failed twice error handling (1).");
            }

            if (!FailedTwiceMessageIds.Contains(message.Id))
            {
                FailedTwiceMessageIds.Add(message.Id);
                message.DeliveryAttempt = 2;
                EventStore.ReportEvent(message);
                throw new Exception("Testing failed twice error handling (2).");
            }

            message.DeliveryAttempt = 3;
            EventStore.ReportEvent(message);
            return Task.CompletedTask;
        }
    }
}
