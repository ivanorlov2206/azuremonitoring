using AzureMonitoringBot.Models;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace AzureMonitoringBot.Services.ChatServices.Telegram
{
    public class TelegramApiClient : IMessengerApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string apiKey;
        public TelegramApiClient(IHttpClientFactory httpClientFactory, IOptions<TelegramApiSettings> options)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(options.Value.BaseUrl);
            apiKey = options.Value.ApiKey;
        }

        public async Task SetWebhook(string url)
        {
            var httpReqMessage = new HttpRequestMessage(HttpMethod.Post, $"/bot{apiKey}/setWebhook")
            {
                Content = new StringContent(JsonSerializer.Serialize(new Webhook
                {
                    Url = url
                }), Encoding.UTF8, "application/json")
            };
            var res = await _httpClient.SendAsync(httpReqMessage);
            Console.WriteLine(await res.Content.ReadAsStringAsync());
        }

        public async Task DeleteWebhooks()
        {
            var httpReqMessage = new HttpRequestMessage(HttpMethod.Post, $"/bot{apiKey}/deleteWebhook");
            await _httpClient.SendAsync(httpReqMessage);
        }

        public async Task SendMessage(string chatId, string text)
        {
            var httpReqMessage = new HttpRequestMessage(HttpMethod.Post, $"/bot{apiKey}/sendMessage")
            {
                Content = new StringContent(JsonSerializer.Serialize(new OutcomeMessage
                {
                    ChatId = int.Parse(chatId),
                    Text = text
                }), Encoding.UTF8, "application/json")
            };
            var res = await _httpClient.SendAsync(httpReqMessage);
            Console.WriteLine(await res.Content.ReadAsStringAsync());
        }
    }
}
