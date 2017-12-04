using System;

namespace AServiceBus.Core
{
    public class DateTimeProvider : IDateTimeProvider
    {
        static DateTimeProvider()
        {
            ResetInstance();
        }

        private DateTimeProvider()
        {
        }

        public static IDateTimeProvider Instance { get; set; }

        public DateTimeOffset Now => DateTimeOffset.UtcNow;

        public static void ResetInstance()
        {
            Instance = new DateTimeProvider();
        }
    }
}
