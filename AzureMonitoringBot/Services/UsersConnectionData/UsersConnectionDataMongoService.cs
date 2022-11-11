using AzureMonitoringBot.Enums;
using AzureMonitoringBot.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AzureMonitoringBot.Services
{
    public class UsersConnectionDataMongoService : IUsersConnectionDataService
    {
        private readonly IMongoCollection<UserConnectionData> _usersConnectionDataCollection;

        public UsersConnectionDataMongoService(IOptions<AzureMonitoringDatabaseSettings> options)
        {
            var mongoClient = new MongoClient(options.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(options.Value.DatabaseName);
            _usersConnectionDataCollection = mongoDatabase.GetCollection<UserConnectionData>(options.Value.UserConnectionDataCollectionName);
        }
        public async Task Create(UserConnectionData data)
        {
            await _usersConnectionDataCollection.InsertOneAsync(data);
        }
        public async Task<List<UserConnectionData>> GetConnectionsByUserId(ObjectId userId)
        {
            var res = await _usersConnectionDataCollection.FindAsync(data => data.UserId == userId);
            return await res.ToListAsync();
        }

        public async Task<List<UserConnectionData>> GetAllConnections()
        {
            var res =  await _usersConnectionDataCollection.FindAsync(_ => true);
            return await res.ToListAsync();
        }

        public async Task<List<UserConnectionData>> GetConnectionsByUserIdAndStatus(ConnectionStatus status, ObjectId userId)
        {
            var res = await _usersConnectionDataCollection.FindAsync(data => data.Status == status && data.UserId == userId);
            return await res.ToListAsync();
        }

        public async Task Update(ObjectId id, UserConnectionData data)
        {
            await _usersConnectionDataCollection.ReplaceOneAsync(data => data.Id == id, data);
        }


    }
}
