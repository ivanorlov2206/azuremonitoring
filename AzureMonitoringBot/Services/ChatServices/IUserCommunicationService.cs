namespace AzureMonitoringBot.Services.ChatServices
{
    public interface IUserCommunicationService
    {
        Task SendMessage(string chatId, string msg);
        Task ProcessUserMessage(string messageText, string chatId);
    }
}
