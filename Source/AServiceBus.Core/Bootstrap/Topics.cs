using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AServiceBus.Core.Domain;

namespace AServiceBus.Core.Bootstrap
{
    public class Topics
    {
        private readonly ICollection<ITopic> _topics;

        public static async Task<Topics> ForAsync(ITopicFactory topicFactory, ICollection<Type> eventTypes)
        {
            var handlerTasks = eventTypes
                .Select(topicFactory.From)
                .ToList();

            await Task.WhenAll(handlerTasks);

            var taskResults = handlerTasks
                .Select(task => task.Result)
                .ToList();

            return new Topics(taskResults);
        }
        
        private Topics(ICollection<ITopic> topics)
        {
            _topics = topics;
        }

        public ITopic GetBy(Type eventType)
        {
            var eventHandler = _topics.SingleOrDefault(t => t.EventType == eventType);
            if (eventHandler == null)
                throw new Exception("Event has not been registered");

            return eventHandler;
        }

        public ICollection<ITopic> GetBy(ICollection<Type> eventTypes)
        {
            return eventTypes.Select(GetBy).ToList();
        }
    }
}