using AzureMonitoringBot.Enums;
using AzureMonitoringBot.Models;
using MongoDB.Bson;

namespace AzureMonitoringBot.Services.UsersConnectionData
{
    public static class UserConnectionDataBuilder
    {
        public static UserConnectionData Build(string userDefinedName, string connectionName, ObjectId userId, ConnectionStatus status, string connectionString)
        {
            return new UserConnectionData
            {
                Id = ObjectId.GenerateNewId(),
                ConnectionName = connectionName,
                UserDefinedName = userDefinedName,
                ConnectionString = connectionString,
                Status = status,
                LastSyncDate = DateTime.MinValue,
                ConnectionType = ConnectionType.AzureQueue,
                UserId = userId
            };
        }
    }
}
