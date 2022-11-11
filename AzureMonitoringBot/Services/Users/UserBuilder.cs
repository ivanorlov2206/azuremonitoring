using AzureMonitoringBot.Models;

namespace AzureMonitoringBot.Services.Users
{
    public static class UserBuilder
    {
        public static User CreateNewUser(string chatId)
        {
            return new User
            {
                ChatId = chatId,
                Stage = Enums.Stage.NEW_USER,
                Id = MongoDB.Bson.ObjectId.GenerateNewId()
            };
        }
    }
}
