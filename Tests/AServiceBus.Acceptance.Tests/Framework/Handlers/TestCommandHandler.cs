using System.Threading.Tasks;
using AServiceBus.Acceptance.Tests.Framework.Commands;
using AServiceBus.Core.Receiver;

namespace AServiceBus.Acceptance.Tests.Framework.Handlers
{
    public class TestCommandHandler : IHandle<TestCommandV1>
    {
        public Task HandleAsync(TestCommandV1 message)
        {
            EventStore.ReportEvent(message);
            return Task.CompletedTask;
        }
    }
}
