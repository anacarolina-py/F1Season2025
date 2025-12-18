using Domain.RaceControl.Models.Entities;
using Infrastructure.RaceControl.Data.Mongo.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.RaceControl.Data.Mongo;

public class MongoContext
{
    private readonly IMongoDatabase _database;

    public MongoContext(MongoSettings settings)
    {
        var connectionString = settings.ConnectionString;
        var databaseName = settings.DatabaseName;

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<RaceGrandPix> RaceControls
        => _database.GetCollection<RaceGrandPix>("RaceControl");
}
