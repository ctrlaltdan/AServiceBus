using System;
using System.Threading.Tasks;
using AServiceBus.Acceptance.Tests.Framework;
using AServiceBus.Acceptance.Tests.Framework.Commands;
using AServiceBus.Acceptance.Tests.Framework.Queues;
using NUnit.Framework;

namespace AServiceBus.Acceptance.Tests.GivenAzureServiceBusTransport.WhenAnErrorOccursInTheMessageHandler
{
    [TestFixture]
    public class ThenTheMessageIsRetriedTwice
    {
        [Test]
        public async Task Subject()
        {
            var id = Guid.NewGuid();

            var expectedEvents = ExpectEvents
                .Create()
                .OfType<FailTwiceV1>(x => x.Id == id && x.DeliveryAttempt == 1)
                .OfType<FailTwiceV1>(x => x.Id == id && x.DeliveryAttempt == 2)
                .OfType<FailTwiceV1>(x => x.Id == id && x.DeliveryAttempt == 3);

            var command = new FailTwiceV1
            {
                Id = id
            };

            await AzureServiceBusBootstrap.Bus.SendAsync<TestQueue>(command);

            Assert.That(expectedEvents.AreReceived);
        }
    }
}
