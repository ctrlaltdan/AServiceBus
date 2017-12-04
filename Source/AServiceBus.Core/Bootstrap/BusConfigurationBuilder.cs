using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AServiceBus.Contracts;
using Serilog;

namespace AServiceBus.Core.Bootstrap
{
    public class BusConfigurationBuilder
    {
        private readonly IQueueFactory _queueFactory;
        private readonly ITopicFactory _topicFactory;
        private readonly ISubscriptionFactory _subscriptionFactory;

        private readonly IList<IQueueInstance> _queuesRequiringCreation;
        private readonly IList<Type> _eventsRequiringCreation;
        private readonly IList<EventSubscription> _eventSubscriptions;

        public BusConfigurationBuilder(IQueueFactory queueFactory, ITopicFactory topicFactory, ISubscriptionFactory subscriptionFactory)
        {
            _queueFactory = queueFactory;
            _topicFactory = topicFactory;
            _subscriptionFactory = subscriptionFactory;

            _queuesRequiringCreation = new List<IQueueInstance>();
            _eventsRequiringCreation = new List<Type>();
            _eventSubscriptions = new List<EventSubscription>();
        }

        public BusConfigurationBuilder SubscribeToEvent<TEvent>(string subscriptionName)
            where TEvent : class, IEvent
        {
            PublishEvent<TEvent>();

            var eventType = typeof(TEvent);
            if (_eventSubscriptions.Any(e => e.EventType == eventType && e.SubscriptionName == subscriptionName))
                return this;

            _eventSubscriptions.Add(EventSubscription.For(eventType, subscriptionName));
            return this;
        }

        public BusConfigurationBuilder SubscribeToQueue<TQueueInstance>()
            where TQueueInstance : struct, IQueueInstance
        {
            if (_queuesRequiringCreation.Any(queue => queue is TQueueInstance))
                return this;

            var queueInstance = Activator.CreateInstance<TQueueInstance>();

            _queuesRequiringCreation.Add(queueInstance);
            return this;
        }

        public BusConfigurationBuilder PublishEvent<TEvent>()
            where TEvent : class, IEvent
        {
            var eventType = typeof(TEvent);
            if (_eventsRequiringCreation.Contains(eventType))
                return this;

            _eventsRequiringCreation.Add(eventType);
            return this;
        }

        public BusConfigurationBuilder UseLogger(ILogger logger)
        {
            Logging.SetLogger(logger);
            return this;
        }

        public async Task BuildAsync()
        {
            if (!Logging.HasInitialisedLogging)
                throw new Exception("Logging has not been initialised.");
            
            BusConfiguration.Queues = await Queues.ForAsync(_queueFactory, _queuesRequiringCreation);
            BusConfiguration.Topics = await Topics.ForAsync(_topicFactory, _eventsRequiringCreation);
            BusConfiguration.Subscriptions = await Subscriptions.ForAsync(_subscriptionFactory, _eventSubscriptions);
        }
    }
}
