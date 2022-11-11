namespace AzureMonitoringBot.Services.ChatServices.Telegram
{
    public class WebhookService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMessengerApiClient _messengerApiClient;
        public WebhookService(IServiceProvider serviceProvider, IMessengerApiClient messengerApiClient)
        {
            _serviceProvider = serviceProvider;
            _messengerApiClient = messengerApiClient;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                await _messengerApiClient.SetWebhook("*address*/api/telegramWebhook");
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _messengerApiClient.DeleteWebhooks();
        }
    }
}
