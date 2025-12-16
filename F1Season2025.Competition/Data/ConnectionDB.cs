using Domain.Competition.Models.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace F1Season2025.Competition.Data
{
    public class ConnectionDB
    {
        public readonly IMongoCollection<Circuit> mongoCollection;

        public ConnectionDB(IOptions<MongoDbSettings> mongoDbSettings)
        {
            MongoClient client = new MongoClient(mongoDbSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            mongoCollection = database.GetCollection<Circuit>(mongoDbSettings.Value.CollectionName);
        }

        public IMongoCollection<Circuit> GetMongoCollection()
        {
            return mongoCollection;
        }
    }

}
