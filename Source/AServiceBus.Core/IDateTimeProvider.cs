using System;

namespace AServiceBus.Core
{
    public interface IDateTimeProvider
    {
        DateTimeOffset Now { get; }
    }
}
