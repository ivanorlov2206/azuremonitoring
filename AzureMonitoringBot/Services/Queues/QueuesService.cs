using Azure.Storage.Queues;
using System.Linq;

namespace AzureMonitoringBot.Services.Queues
{
    public class QueuesService : IQueuesService
    {
        public async Task<QueueInfo> GetQueue(string queueName, string connectionString)
        {
            var queueClient = new QueueClient(connectionString, queueName, new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64,
                
            });
            var messages = (await queueClient.PeekMessagesAsync(queueClient.MaxPeekableMessages)).Value;
            var messagesCount = (await queueClient.GetPropertiesAsync()).Value.ApproximateMessagesCount;
            return new QueueInfo
            {
                Messages = messages.Select(x => new QueueMessage
                {
                    Text = x.Body.ToString(),
                    Date = x.InsertedOn.Value
                }).ToList(),
                MessagesCount = messagesCount
            };
        }
    }
}
