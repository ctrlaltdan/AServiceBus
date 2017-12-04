using System;
using System.Threading.Tasks;
using AServiceBus.Acceptance.Tests.Framework;
using AServiceBus.Acceptance.Tests.Framework.Events;
using NUnit.Framework;

namespace AServiceBus.Acceptance.Tests.GivenAzureServiceBusTransport.WhenPublishingAnEvent
{
    [TestFixture]
    public class WithManySubscribers : EventTesting
    {        
        [Test]
        public async Task ThenTheEventIsReceivedByAllSubscriberQueuesDerp()
        {
            var id = Guid.NewGuid();

            SetEventExpectation<MultipleSubscribersEventV1>(x => x.Id == id && x.ReceivedCount == 1);
            SetEventExpectation<MultipleSubscribersEventV1>(x => x.Id == id && x.ReceivedCount == 2);

            var message = new MultipleSubscribersEventV1
            {
                Id = id
            };

            await AzureServiceBusBootstrap.Bus.PublishAsync(message);
            
            VerifyEventExpectations();
        }
    }
}