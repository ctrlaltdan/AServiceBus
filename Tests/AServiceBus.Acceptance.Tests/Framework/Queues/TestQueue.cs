using AServiceBus.Contracts;

namespace AServiceBus.Acceptance.Tests.Framework.Queues
{
    public struct TestQueue : IQueueInstance
    {
        public string Name => "test-queue";
    }
}
