using Infrastructure.RaceControl.Data.Mongo.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.RaceControl.Data.Mongo;

public class MongoContext
{
    private readonly IMongoDatabase _database;

    public MongoContext(IOptions<MongoSettings> settings)
    {
        var connectionString = settings.Value.ConnectionString;
        var databaseName = settings.Value.DatabaseName;

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<Domain.RaceControl.Models.Entities.RaceControl> RaceControls
        => _database.GetCollection<Domain.RaceControl.Models.Entities.RaceControl>("RaceControl");
}
