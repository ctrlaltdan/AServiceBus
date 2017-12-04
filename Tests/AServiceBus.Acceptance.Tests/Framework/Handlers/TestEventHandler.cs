using System.Threading.Tasks;
using AServiceBus.Acceptance.Tests.Framework.Events;
using AServiceBus.Core.Receiver;

namespace AServiceBus.Acceptance.Tests.Framework.Handlers
{
    public class TestEventHandler : IHandle<TestEventV1>
    {
        public Task HandleAsync(TestEventV1 message)
        {
            EventStore.ReportEvent(message);
            return Task.CompletedTask;
        }
    }
}
