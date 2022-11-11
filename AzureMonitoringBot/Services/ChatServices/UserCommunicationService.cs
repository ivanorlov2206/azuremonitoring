using AzureMonitoringBot.Models;
using AzureMonitoringBot.Services.Queues;
using AzureMonitoringBot.Services.Users;
using AzureMonitoringBot.Services.UsersConnectionData;

namespace AzureMonitoringBot.Services.ChatServices.Telegram
{
    public class UserCommunicationService : IUserCommunicationService
    {
        private readonly IMessengerApiClient _messengerClient;
        private readonly IUsersConnectionDataService _usersConnectionDataService;
        private readonly IUsersService _usersService;
        private readonly IQueuesService _queuesService;

        public UserCommunicationService(IMessengerApiClient messengerClient, IUsersConnectionDataService usersConnectionDataService,
                                                    IQueuesService queuesService, IUsersService usersService)
        {
            _messengerClient = messengerClient;
            _usersConnectionDataService = usersConnectionDataService;
            _usersService = usersService;
            _queuesService = queuesService;
        }

        public async Task SendMessage(string chatId, string msg)
        {
            await _messengerClient.SendMessage(chatId, msg);
        }

        private async Task ProcessListCommand(User user)
        {
            var connections = await _usersConnectionDataService.GetConnectionsByUserId(user.Id);
            var resultText = "";
            for (var connectionNum = 0; connectionNum < connections.Count; connectionNum++)
            {
                resultText += $"{connectionNum + 1}) Azure queue - {connections[connectionNum].UserDefinedName}\n\n";
            }
            await SendMessage(user.ChatId, resultText);
        }

        private async Task ProcessGetMessagesCommand(string messageText, User user)
        {
            var parts = messageText.Split(" ");
            var chatId = user.ChatId;
            if (parts.Length > 1)
            {
                var conn = (await _usersConnectionDataService.GetConnectionsByUserIdAndStatus(Enums.ConnectionStatus.READY, user.Id))
                    .Where(conn => conn.UserDefinedName == parts[1]).FirstOrDefault();
                if (conn != null)
                {
                    var queue = await _queuesService.GetQueue(conn.ConnectionName, conn.ConnectionString);
                    var resMessage = $"Messages in {conn.UserDefinedName} ({conn.ConnectionName}):\n\n===\n\n";
                    foreach (var msg in queue.Messages.OrderBy(msg => msg.Date))
                    {
                        resMessage += msg.Text + "\n\n===\n\n";
                    }
                    await SendMessage(chatId, resMessage);
                }
            }
            else
            {
                throw new Exception("Specify queue name!");
            }
        }

        private async Task ProcessCommands(string messageText, User user)
        {
            var chatId = user.ChatId;
            if (string.Equals(messageText.Trim(), "/add"))
            {
                await SendMessage(chatId, "Please enter connection name");
                user.Stage = Enums.Stage.WAIT_FOR_CONNECTION_NAME;
            }
            else if (string.Equals(messageText.Trim(), "/list"))
            {
                await ProcessListCommand(user);
            }
            else if (messageText.StartsWith("/messages"))
            {
                await ProcessGetMessagesCommand(messageText, user);
            }
            else
            {
                await SendMessage(chatId, "I don't know this command :(");
            }
        }

        private async Task RequestForName(string messageText, User user)
        {
            var chatId = user.ChatId;
            var connectionData = UserConnectionDataBuilder.Build(messageText, "", user.Id,
                        Enums.ConnectionStatus.PENDING_QUEUE_NAME, "");
            await _usersConnectionDataService.Create(connectionData);
            user.Stage = Enums.Stage.WAIT_FOR_QUEUE_NAME;
            await SendMessage(chatId, "Now please enter queue name");
        }

        private async Task RequestForQueueName(string messageText, User user)
        {
            var chatId = user.ChatId;
            var connection = (await _usersConnectionDataService.GetConnectionsByUserIdAndStatus(Enums.ConnectionStatus.PENDING_QUEUE_NAME, user.Id)).FirstOrDefault();
            if (connection == null)
            {
                throw new Exception("Something went wrong");
            }
            connection.ConnectionName = messageText.Trim();
            connection.Status = Enums.ConnectionStatus.PENDING_CONNECTION_STRING;
            await _usersConnectionDataService.Update(connection.Id, connection);
            user.Stage = Enums.Stage.WAIT_FOR_CONNECTION_STRING;
            await SendMessage(chatId, "Please enter connection string");
        }


        private async Task RequestForConnectionString(User user, string messageText)
        {
            var chatId = user.ChatId;
            var connection = (await _usersConnectionDataService.GetConnectionsByUserIdAndStatus(Enums.ConnectionStatus.PENDING_CONNECTION_STRING,
                                                                                                        user.Id)).FirstOrDefault();
            if (connection == null)
            {
                throw new Exception("Something went wrong");
            }
            connection.ConnectionString = messageText;
            connection.Status = Enums.ConnectionStatus.READY;
            await _usersConnectionDataService.Update(connection.Id, connection);
            await SendMessage(chatId, "Connection successfully added!");
            user.Stage = Enums.Stage.WAIT_FOR_COMMAND;
        }

        public async Task ProcessUserMessage(string messageText, string chatId)
        {
            var user = await _usersService.GetUserByChatId(chatId.ToString());
            if (user == null)
            {
                user = UserBuilder.CreateNewUser(chatId.ToString());
                await _usersService.CreateUser(user);
            }
            if (string.Equals(messageText, "/start"))
            {
                user.Stage = Enums.Stage.NEW_USER;
            }
            switch (user.Stage)
            {
                case Enums.Stage.NEW_USER:
                    await SendMessage(chatId, "Welcome to azure queue monitoring bot! To add new queue for monitoring," +
                                                " enter /add command, /list - to list connections");
                    user.Stage = Enums.Stage.WAIT_FOR_COMMAND;
                    break;
                case Enums.Stage.WAIT_FOR_COMMAND:
                    await ProcessCommands(messageText, user);
                    break;
                case Enums.Stage.WAIT_FOR_CONNECTION_NAME:
                    await RequestForName(messageText, user);
                    break;
                case Enums.Stage.WAIT_FOR_QUEUE_NAME:
                    await RequestForQueueName(messageText, user);
                    break;
                case Enums.Stage.WAIT_FOR_CONNECTION_STRING:
                    await RequestForConnectionString(user, messageText);
                    break;
            }
            await _usersService.Update(user.Id, user);
        }
    }
}
