using Domain.RaceControl.Models.Entities.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.RaceControl.Models.Entities;

public class Session
{
    [BsonId]
    public ObjectId Id { get; private set; }
    public SessionResult _results { get; private set; }
    public int Order { get; private set; }
    public EType Type { get; private set; }
    public EStatus Status { get; private set; } = EStatus.Schedule;

    public Session(EType type, int order)
    {
        Id = ObjectId.GenerateNewId();
        Type = type;
        Order = order;
    }

    public void Start() 
        => Status = EStatus.Live;

    public void Finished()
        => Status = EStatus.Finished;

    public void UpdateResults(SessionResult sessionResults)
    {
        if (sessionResults is null)
            throw new ArgumentNullException("The list can't be empty");

        var results = new SessionResult(sessionResults.Drivers, sessionResults.Teams);
        _results = results;
    }
}
