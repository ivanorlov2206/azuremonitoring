using System.Text.Json.Serialization;

namespace AzureMonitoringBot.Services.ChatServices.Telegram
{
    public class OutcomeMessage
    {
        [JsonPropertyName("chat_id")]
        public int ChatId { get; set; }
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
