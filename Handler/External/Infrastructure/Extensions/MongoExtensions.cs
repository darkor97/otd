using Handler.Domain;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;

namespace Handler.Infrastructure.Extensions
{
    internal static class MongoExtensions
    {
        internal static void MapMongoEntities(this IMongoDatabase db)
        {
            BsonClassMap.RegisterClassMap<Odds>(item =>
            {
                item.AutoMap();
                item.MapIdField(x => x.Id).SetIdGenerator(StringObjectIdGenerator.Instance);
            });
        }
    }
}
