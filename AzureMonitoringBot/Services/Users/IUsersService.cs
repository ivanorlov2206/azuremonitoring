using AzureMonitoringBot.Models;
using MongoDB.Bson;

namespace AzureMonitoringBot.Services
{
    public interface IUsersService
    {
        Task<User> GetUserById(ObjectId userId);
        Task CreateUser(User user);
        Task<User> GetUserByChatId(string chatId);
        Task Update(ObjectId userId, User user);
    }
}
