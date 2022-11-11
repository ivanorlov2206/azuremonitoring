
using System.Text.Json.Serialization;

namespace AzureMonitoringBot.Services.ChatServices.Telegram
{
    public class Webhook
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
