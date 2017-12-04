using Serilog;

namespace AServiceBus.Core
{
    public class Logging
    {
        public static ILogger Logger { get; private set; }
        public static bool HasInitialisedLogging => Logger != null;
        
        public static void SetLogger(ILogger logger)
        {
            Logger = logger;
        }
    }
}