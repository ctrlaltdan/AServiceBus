using System;
using AServiceBus.Contracts;

namespace AServiceBus.Acceptance.Tests.Framework.Events
{
    public class TestEventV1 : IEvent
    {
        public Guid Id { get; set; }
    }
}
