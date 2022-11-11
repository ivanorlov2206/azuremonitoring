using AzureMonitoringBot.Enums;
using AzureMonitoringBot.Models;
using MongoDB.Bson;

namespace AzureMonitoringBot.Services
{
    public interface IUsersConnectionDataService
    {
        Task Create(UserConnectionData data);
        Task<List<UserConnectionData>> GetConnectionsByUserId(ObjectId userId);
        Task<List<UserConnectionData>> GetAllConnections();
        Task<List<UserConnectionData>> GetConnectionsByUserIdAndStatus(ConnectionStatus status, ObjectId userId);
        Task Update(ObjectId id, UserConnectionData data);
    }
}
