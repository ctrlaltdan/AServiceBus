using System.Collections.Generic;
using AServiceBus.Core.Bootstrap;

namespace AServiceBus.Core.Receiver
{
    public class Monitor : IMonitor
    {
        public bool Running;

        private readonly IConsumerFactory _consumerFactory;
        private readonly IBusConfiguration _busConfiguration;
        private readonly object _lock = new object();
        private readonly IList<Listener> _activeListeners = new List<Listener>();

        public Monitor(IConsumerFactory consumerFactory, IBusConfiguration busConfiguration)
        {
            _consumerFactory = consumerFactory;
            _busConfiguration = busConfiguration;
        }

        public void Start()
        {
            if (Running)
                return;
            
            lock (_lock)
            {
                foreach (var receiver in _busConfiguration.GetReceivers())
                {
                    var listener = Listener.For(_consumerFactory, receiver);
                    listener.StartListening();
                    _activeListeners.Add(listener);
                }

                Running = true;
            }
        }
        
        public void Stop()
        {
            if (!Running)
                return;

            lock (_lock)
            {   
                foreach (var listener in _activeListeners)
                {
                    listener.StopListening();
                }

                _activeListeners.Clear();
                Running = false;
            }
        }
    }
}
