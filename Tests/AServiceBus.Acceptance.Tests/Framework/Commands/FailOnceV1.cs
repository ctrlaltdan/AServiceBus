using System;
using AServiceBus.Contracts;

namespace AServiceBus.Acceptance.Tests.Framework.Commands
{
    public class FailOnceV1 : ICommand
    {
        public Guid Id { get; set; }
        public int DeliveryAttempt { get; set; }
    }
}
