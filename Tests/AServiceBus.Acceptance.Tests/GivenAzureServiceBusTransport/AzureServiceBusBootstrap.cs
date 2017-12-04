using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AServiceBus.Acceptance.Tests.Framework.Events;
using AServiceBus.Acceptance.Tests.Framework.Queues;
using AServiceBus.Core;
using AServiceBus.Core.Bootstrap;
using AServiceBus.Core.Receiver;
using AServiceBus.Core.Sender;
using AServiceBus.DependencyInjection.StructureMap;
using AServiceBus.Transport.AzureServiceBus;
using NUnit.Framework;
using Serilog;

namespace AServiceBus.Acceptance.Tests.GivenAzureServiceBusTransport
{
    [SetUpFixture]
    public class AzureServiceBusBootstrap
    {
        public static IBus Bus;
        private IMonitor _monitor;
        private IResolver _resolver;

        [OneTimeSetUp]
        public async Task StartApplication()
        {
            MessageBrokerContext.SetEnvironment(Configuration.Environment);

            var azureConfiguration = new AzureServiceBusConfiguration
            {
                ConnectionString = File.ReadAllText($"{AppContext.BaseDirectory}\\ConnectionString.txt")
            };

            var logInstance = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Environment", "Testing")
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();

            _resolver = ConfigureDependencies.Initialise()
                .AddAzureServiceBusTransportProvider(azureConfiguration)
                .AddCoreDependencies()
                .ScanForHandlers(Assembly.GetExecutingAssembly())
                .Build();

            var configurationBuilder = _resolver.GetInstance<BusConfigurationBuilder>();

            await configurationBuilder
                .SubscribeToQueue<TestQueue>()
                .SubscribeToEvent<TestEventV1>(Configuration.Subscription1)
                .SubscribeToEvent<MultipleSubscribersEventV1>(Configuration.Subscription1)
                .SubscribeToEvent<MultipleSubscribersEventV1>(Configuration.Subscription2)
                .PublishEvent<TestEventV1>()
                .PublishEvent<MultipleSubscribersEventV1>()
                .UseLogger(logInstance)
                .BuildAsync();

            _monitor = _resolver.GetInstance<IMonitor>();
            _monitor.Start();

            Bus = _resolver.GetInstance<IBus>();
        }

        [OneTimeTearDown]
        public void StopApplication()
        {
            _monitor.Stop();

            Thread.Sleep(TimeSpan.FromSeconds(5));
        }
    }
}
