using System;
using System.Threading.Tasks;
using AServiceBus.Acceptance.Tests.Framework;
using AServiceBus.Acceptance.Tests.Framework.Events;
using NUnit.Framework;

namespace AServiceBus.Acceptance.Tests.GivenAzureServiceBusTransport.WhenPublishingAnEvent
{
    [TestFixture]
    public class WithOneSubscriber : EventTesting
    {
        [Test]
        public async Task ThenTheEventIsReceivedByTheSubscriberQueueDerp()
        {
            var id = Guid.NewGuid();

            SetEventExpectation<TestEventV1>(x => x.Id == id);

            var message = new TestEventV1
            {
                Id = id
            };

            await AzureServiceBusBootstrap.Bus.PublishAsync(message);
            
            VerifyEventExpectations();
        }
    }
}