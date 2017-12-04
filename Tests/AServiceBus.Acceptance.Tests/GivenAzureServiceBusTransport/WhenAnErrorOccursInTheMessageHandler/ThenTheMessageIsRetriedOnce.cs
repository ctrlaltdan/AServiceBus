using System;
using System.Threading.Tasks;
using AServiceBus.Acceptance.Tests.Framework;
using AServiceBus.Acceptance.Tests.Framework.Commands;
using AServiceBus.Acceptance.Tests.Framework.Queues;
using NUnit.Framework;

namespace AServiceBus.Acceptance.Tests.GivenAzureServiceBusTransport.WhenAnErrorOccursInTheMessageHandler
{
    [TestFixture]
    public class ThenTheMessageIsRetriedOnce : EventTesting
    {
        [Test]
        public async Task Subject()
        {
            var id = Guid.NewGuid();

            SetEventExpectation<FailOnceV1>(x => x.Id == id && x.DeliveryAttempt == 1);
            SetEventExpectation<FailOnceV1>(x => x.Id == id && x.DeliveryAttempt == 2);

            var command = new FailOnceV1
            {
                Id = id
            };

            await AzureServiceBusBootstrap.Bus.SendAsync<TestQueue>(command);

            VerifyEventExpectations();
        }
    }
}
