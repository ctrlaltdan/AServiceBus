using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;

namespace AServiceBus.Transport.AzureServiceBus
{
    internal class MessageSerializer
    {
        private readonly JsonSerializerSettings _settings;

        public MessageSerializer()
        {
            _settings = new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                TypeNameHandling = TypeNameHandling.All,
                Error = delegate (object sender, ErrorEventArgs args)
                {
                    Log.Error(args.ErrorContext.Error, args.ErrorContext.Error.Message);
                    args.ErrorContext.Handled = true;
                }
            };
        }

        public string ToString(dynamic contract)
        {
            return JsonConvert.SerializeObject(contract, _settings);
        }

        public dynamic ToObject(string json)
        {
            return JsonConvert.DeserializeObject(json, _settings);
        }
    }
}
