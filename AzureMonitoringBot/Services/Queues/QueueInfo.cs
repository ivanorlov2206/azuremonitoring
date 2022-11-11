namespace AzureMonitoringBot.Services.Queues
{
    public class QueueInfo
    {
        public List<QueueMessage> Messages { get; set; }
        public int MessagesCount { get; set; }
    }
}
