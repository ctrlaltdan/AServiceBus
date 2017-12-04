using System;
using AServiceBus.Contracts;

namespace AServiceBus.Acceptance.Tests.Framework.Events
{
    public class MultipleSubscribersEventV1 : IEvent
    {
        public Guid Id { get; set; }
        public int ReceivedCount { get; set; }
    }
}
