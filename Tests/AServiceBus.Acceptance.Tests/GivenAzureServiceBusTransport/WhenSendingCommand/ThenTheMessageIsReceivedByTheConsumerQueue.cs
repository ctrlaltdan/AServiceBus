using System;
using System.Threading.Tasks;
using AServiceBus.Acceptance.Tests.Framework;
using AServiceBus.Acceptance.Tests.Framework.Commands;
using AServiceBus.Acceptance.Tests.Framework.Queues;
using NUnit.Framework;

namespace AServiceBus.Acceptance.Tests.GivenAzureServiceBusTransport.WhenSendingCommand
{
    [TestFixture]
    public class ThenTheMessageIsReceivedByTheConsumerQueue : EventTesting
    {
        [Test]
        public async Task Subject()
        {   
            var id = Guid.NewGuid();

            SetEventExpectation<TestCommandV1>(x => x.Id == id);

            var command = new TestCommandV1
            {
                Id = id
            };

            await AzureServiceBusBootstrap.Bus.SendAsync<TestQueue>(command);
            
            VerifyEventExpectations();
        }
    }
}