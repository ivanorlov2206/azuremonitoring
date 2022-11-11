using AzureMonitoringBot.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AzureMonitoringBot.Services
{
    public class UsersMongoService : IUsersService
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UsersMongoService(IOptions<AzureMonitoringDatabaseSettings> options)
        {
            var mongoClient = new MongoClient(options.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(options.Value.DatabaseName);
            _usersCollection = mongoDatabase.GetCollection<User>(options.Value.UsersCollectionName);
        }
        public async Task CreateUser(User user)
        {
            await _usersCollection.InsertOneAsync(user);
        }

        public async Task<User> GetUserById(ObjectId userId)
        {
            return await _usersCollection.Find(user => user.Id == userId).FirstOrDefaultAsync();
        }
        public async Task<User> GetUserByChatId(string chatId)
        {
            return await _usersCollection.Find(user => user.ChatId == chatId).FirstOrDefaultAsync();
        }
        public async Task Update(ObjectId userId, User user)
        {
            await _usersCollection.ReplaceOneAsync(user => user.Id == userId, user);
        }
    }
}
