namespace AzureMonitoringBot.Services.Queues
{
    public class QueueMessage
    {
        public string Text { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}
