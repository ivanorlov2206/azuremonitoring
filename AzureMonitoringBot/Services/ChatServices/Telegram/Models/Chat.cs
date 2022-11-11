using System.Text.Json.Serialization;

namespace AzureMonitoringBot.Services.ChatServices.Telegram.Models
{
    public class Chat
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}
