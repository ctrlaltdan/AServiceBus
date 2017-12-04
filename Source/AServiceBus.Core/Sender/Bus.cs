using System.Threading.Tasks;
using AServiceBus.Contracts;
using AServiceBus.Core.Bootstrap;

namespace AServiceBus.Core.Sender
{
    internal class Bus : IBus
    {
        private readonly IBusConfiguration _busConfiguration;

        public Bus(IBusConfiguration busConfiguration)
        {
            _busConfiguration = busConfiguration;
        }
        
        public Task SendAsync<TQueueInstance>(ICommand command)
            where TQueueInstance : struct, IQueueInstance
        {
            var queue = _busConfiguration.GetQueue<TQueueInstance>();
            return queue.SendAsync(command);
        }

        public Task PublishAsync(IEvent message)
        {
            var eventHandler = _busConfiguration.GetTopic(message.GetType());
            return eventHandler.Publish(message);
        }
    }
}