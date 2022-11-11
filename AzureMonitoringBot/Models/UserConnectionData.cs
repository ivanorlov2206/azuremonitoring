using AzureMonitoringBot.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AzureMonitoringBot.Models
{
    public class UserConnectionData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        [BsonRepresentation(BsonType.String)]
        public string UserDefinedName { get; set; }
        [BsonRepresentation(BsonType.String)]
        public string ConnectionName { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId UserId { get; set; }
        [BsonRepresentation(BsonType.Int32)]
        public ConnectionStatus Status { get; set; }
        [BsonRepresentation(BsonType.String)]
        public string ConnectionString { get; set; }
        [BsonRepresentation(BsonType.Int32)]
        public ConnectionType ConnectionType { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime LastSyncDate { get; set; }
    }
}
