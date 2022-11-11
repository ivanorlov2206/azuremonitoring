using AzureMonitoringBot.Services;
using AzureMonitoringBot.Services.ChatServices.Telegram;
using AzureMonitoringBot.Services.ChatServices.Telegram.Models;
using AzureMonitoringBot.ViewModels;
using Microsoft.AspNetCore.Mvc;
using AzureMonitoringBot.Services.Queues;
using AzureMonitoringBot.Services.ChatServices;

namespace AzureMonitoringBot.Controllers
{
    [Route("api/telegramWebhook")]
    [ApiController]
    public class TelegramController : ControllerBase
    {
        private readonly IUserCommunicationService _userCommunicationService;

        public TelegramController(IUserCommunicationService userCommunicationService)
        {
            _userCommunicationService = userCommunicationService;
        }

        

        [HttpPost]
        public async Task<WebhookResult> ReceiveWebhook(WebhookUpdateMessage message)
        {
            await _userCommunicationService.ProcessUserMessage(message.Message.Text, message.Message.Chat.Id.ToString());
            return new WebhookResult
            {
                Result = "Ok"
            };
        }
    }
}
