using CompetitionEntity = Domain.Competition.Models.Entities.Competition;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace F1Season2025.Competition.Data
{
    public class ConnectionDB
    {
        public readonly IMongoCollection<CompetitionEntity> mongoCollection;

        public ConnectionDB(IOptions<MongoDbSettings> mongoDbSettings)
        {
            MongoClient client = new MongoClient(mongoDbSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            mongoCollection = database.GetCollection<CompetitionEntity>(mongoDbSettings.Value.CollectionName);
        }

        public IMongoCollection<CompetitionEntity> GetMongoCollection()
        {
            return mongoCollection;
        }
    }

}
