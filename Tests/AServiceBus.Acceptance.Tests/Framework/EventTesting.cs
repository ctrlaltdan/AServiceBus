using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace AServiceBus.Acceptance.Tests.Framework
{
    public class EventTesting
    {
        private static readonly TimeSpan TimeoutPeriod = new TimeSpan(0, 0, 0, 20);
        private readonly List<Task> _expectedEventChecks = new List<Task>();
        
        protected void SetEventExpectation<TEvent>(Func<TEvent, bool> predicate)
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

            _expectedEventChecks.Add(expectedEventCheck);
        }

        protected void VerifyEventExpectations()
        {
            Task.WaitAll(_expectedEventChecks.ToArray());
            _expectedEventChecks.Clear();
        }
    }
}
