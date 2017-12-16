using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace AServiceBus.Acceptance.Tests.Framework
{
    public class ExpectEvents
    {
        private static readonly TimeSpan TimeoutPeriod = TimeSpan.FromSeconds(5);
        private readonly List<Task> _expectedEvents = new List<Task>();

        public static ExpectEvents Create()
        {
            return new ExpectEvents();
        }

        private ExpectEvents()
        {
        }

        public ExpectEvents OfType<TEvent>(Func<TEvent, bool> predicate)
        {
            var expectedEventCheck =
                EventStore.Events
                    .Where(x => x is TEvent)
                    .Cast<TEvent>()
                    .Where(predicate)
                    .Timeout(
                        TimeoutPeriod,
                        Observable.Throw<TEvent>(new TimeoutException("Timed out waiting to satisfy event expectation of type " + typeof(TEvent).FullName)))
                    .Take(1)
                    .Select(_ => Unit.Default)
                    .ToTask();

            _expectedEvents.Add(expectedEventCheck);
            return this;
        }

        public bool AreReceived()
        {
            try
            {
                Task.WhenAll(_expectedEvents).Wait();
            }
            catch (TimeoutException)
            {
                return false;
            }
            return true;
        }
    }
}
