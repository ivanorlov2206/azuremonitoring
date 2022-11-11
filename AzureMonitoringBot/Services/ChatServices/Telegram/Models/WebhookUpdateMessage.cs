using System.Text.Json.Serialization;

namespace AzureMonitoringBot.Services.ChatServices.Telegram.Models
{
    public class WebhookUpdateMessage
    {
        [JsonPropertyName("update_id")]
        public int UpdateId { get; set; }
        [JsonPropertyName("message")]
        public IncomingMessage Message { get; set; }
    }
}
