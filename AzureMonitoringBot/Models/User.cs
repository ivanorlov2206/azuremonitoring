using AzureMonitoringBot.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AzureMonitoringBot.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.Int32)]
        public Stage Stage { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string ChatId { get; set; }
    }
}
