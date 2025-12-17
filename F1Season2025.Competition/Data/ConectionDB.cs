using Domain.Competition.Models.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace F1Season2025.Competition.Data
{
    public class ConnectionDB
    {
        private readonly IMongoDatabase _database;

        public ConnectionDB(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var client = new MongoClient(mongoDbSettings.Value.ConnectionURI);
            _database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
        }

        public IMongoCollection<T> GetMongoCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }

}
