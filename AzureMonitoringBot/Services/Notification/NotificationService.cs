using AzureMonitoringBot.Services.ChatServices;
using AzureMonitoringBot.Services.ChatServices.Telegram;
using AzureMonitoringBot.Services.Queues;

namespace AzureMonitoringBot.Services.Notification
{
    public class NotificationService : IHostedService
    {
        private Timer? _timer = null;
        private readonly IUsersConnectionDataService _usersConnectionDataService;
        private readonly IUsersService _usersService;
        private readonly IMessengerApiClient _messengerClient;
        private readonly IQueuesService _queuesService;

        public NotificationService(IUsersConnectionDataService usersConnectionDataService, IUsersService usersService,
                                        IQueuesService queuesService, IMessengerApiClient messengerClient)
        {
            _usersConnectionDataService = usersConnectionDataService;
            _usersService = usersService;
            _queuesService = queuesService;
            _messengerClient = messengerClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(CheckConnections, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        public async void CheckConnections(object? state) {
            var connections = (await _usersConnectionDataService.GetAllConnections()).Where(conn => conn.Status == Enums.ConnectionStatus.READY);
            foreach (var connection in connections)
            {
                var queue = await _queuesService.GetQueue(connection.ConnectionName, connection.ConnectionString);
                var lastMessages = queue.Messages.Where(msg => msg.Date >= connection.LastSyncDate).ToList();
                if (lastMessages.Count > 0)
                {
                    var user = await _usersService.GetUserById(connection.UserId);
                    await _messengerClient.SendMessage(user.ChatId,  
                        $"⚠You have {lastMessages.Count} new messages in {connection.UserDefinedName} - {connection.ConnectionName}!" +
                        $"\n\nTotal in queue: {queue.MessagesCount} messages;\n\n" +
                        $"To show messages enter '/messages {connection.UserDefinedName}'");

                    connection.LastSyncDate = DateTime.UtcNow;
                    await _usersConnectionDataService.Update(connection.Id, connection);
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
