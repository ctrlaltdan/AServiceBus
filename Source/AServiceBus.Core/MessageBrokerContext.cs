using System.Text.RegularExpressions;

namespace AServiceBus.Core
{
    public static class MessageBrokerContext
    {
        public static string Environment { get; private set; }

        static MessageBrokerContext()
        {
            SetEnvironment(null);
        }
        
        public static void SetEnvironment(string environment)
        {
            if (string.IsNullOrWhiteSpace(environment))
            {
                var machineName = System.Environment.MachineName.ToLower();
                var regex = new Regex("[^a-z0-9-]");
                var formattedMachineName = regex.Replace(machineName, string.Empty);

                Environment = formattedMachineName;
            }
            else
            {
                Environment = environment;
            }
        }
    }
}

