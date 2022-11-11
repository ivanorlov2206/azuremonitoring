namespace AzureMonitoringBot.Models
{
    public class AzureMonitoringDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string UsersCollectionName { get; set; } = null!;
        public string UserConnectionDataCollectionName { get; set; } = null!;
    }
}
