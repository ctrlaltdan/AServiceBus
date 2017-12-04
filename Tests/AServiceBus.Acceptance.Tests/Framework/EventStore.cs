using System.Reactive.Subjects;

namespace AServiceBus.Acceptance.Tests.Framework
{
    public class EventStore
    {
        public static readonly Subject<object> Events = new Subject<object>();
        public static void ReportEvent(object @event)
        {
            Events.OnNext(@event);
        }
    }
}
