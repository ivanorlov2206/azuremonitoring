using System.Text.Json.Serialization;

namespace AzureMonitoringBot.Services.ChatServices.Telegram.Models
{
    public class IncomingMessage
    {
        [JsonPropertyName("message_id")]
        public int MessageId { get; set; }
        [JsonPropertyName("chat")]
        public Chat Chat { get; set; }
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
