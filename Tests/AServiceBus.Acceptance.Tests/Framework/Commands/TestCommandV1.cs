using System;
using AServiceBus.Contracts;

namespace AServiceBus.Acceptance.Tests.Framework.Commands
{
    public class TestCommandV1 : ICommand
    {
        public Guid Id { get; set; }
    }
}
