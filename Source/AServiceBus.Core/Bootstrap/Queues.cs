using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AServiceBus.Contracts;
using AServiceBus.Core.Domain;

namespace AServiceBus.Core.Bootstrap
{
    internal class Queues
    {
        private readonly ICollection<IQueue> _queues;

        public static async Task<Queues> ForAsync(IQueueFactory queueFactory, ICollection<IQueueInstance> queueInstances)
        {
            var queueTasks = queueInstances
                .Select(queueFactory.FromAsync)
                .ToList();

            await Task.WhenAll(queueTasks);

            var queues = queueTasks
                .Select(task => task.Result)
                .ToList();

            return new Queues(queues);
        }
        
        private Queues(ICollection<IQueue> queues)
        {
            _queues = queues;
        }

        public IQueue GetBy<TQueueInstance>()
        {
            var queue = _queues.SingleOrDefault(q => q.QueueInstance is TQueueInstance);
            if (queue == null)
                throw new Exception("Queue has not been registered");

            return queue;
        }

        public IReadOnlyCollection<IQueue> GetAll()
        {
            return _queues.ToList();
        }
    }
}