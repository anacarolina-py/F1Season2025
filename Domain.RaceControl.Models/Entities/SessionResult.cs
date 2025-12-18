using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.RaceControl.Models.Entities;

public class SessionResult
{
    [BsonId]
    public ObjectId Id { get; init; } 
    public List<DriverChampionship> Drivers { get; private set; }
    public List<ConstructorChampionship> Teams { get; private set; }

    public SessionResult(List<DriverChampionship> drivers, List<ConstructorChampionship> teams)
    {
        Id = ObjectId.GenerateNewId();
        Drivers = drivers;
        Teams = teams;
    }
}
