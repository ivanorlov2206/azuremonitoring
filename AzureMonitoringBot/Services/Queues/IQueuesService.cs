namespace AzureMonitoringBot.Services.Queues
{
    public interface IQueuesService
    {
        Task<QueueInfo> GetQueue(string queueName, string connectionString);
    }
}
