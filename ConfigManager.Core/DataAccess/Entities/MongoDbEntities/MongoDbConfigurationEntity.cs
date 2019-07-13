using MongoDB.Bson;

namespace ConfigManager.Core.DataAccess.Entities.MongoDbEntities
{
    internal class MongoDbConfigurationEntity : ConfigurationBaseEntity
    {
        public ObjectId Id { get; set; }
    }
}
