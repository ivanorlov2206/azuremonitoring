namespace AzureMonitoringBot.Services.ChatServices
{
    public interface IMessengerApiClient
    {
        Task SetWebhook(string url);
        Task DeleteWebhooks();
        Task SendMessage(string chatId, string text);
    }
}
