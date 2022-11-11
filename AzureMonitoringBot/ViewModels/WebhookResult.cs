using Newtonsoft.Json;

namespace AzureMonitoringBot.ViewModels
{
    [JsonObject]
    public class WebhookResult
    {
        [JsonProperty]
        public string Result { get; set; }
    }
}
