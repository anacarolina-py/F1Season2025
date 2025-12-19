using Domain.RaceControl.Models.Entities.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.RaceControl.Models.Entities;

public class Session
{

    [BsonId]
    public ObjectId Id { get; private set; }

    [BsonElement("session_result")]
    public SessionResult SessionResult { get; private set; }

    [BsonElement("type")]
    public EType Type { get; private set; }

    [BsonElement("order")]
    public int Order { get; private set; }

    [BsonElement("status")]
    public EStatus Status { get; private set; } = EStatus.Schedule;

    public Session(EType type, int order)
    {
        Id = ObjectId.GenerateNewId();
        Type = type;
        Order = order;
    }

    [BsonConstructor]
    private Session() { }

    public void Start()
        => Status = EStatus.Live;

    public void Finished()
        => Status = EStatus.Finished;

    public void UpdateResults(SessionResult sessionResults)
    {
        if (sessionResults is null)
            throw new ArgumentNullException("The list can't be empty");

        var results = new SessionResult(sessionResults.Drivers, sessionResults.Teams);
        SessionResult = results;
    }
}
