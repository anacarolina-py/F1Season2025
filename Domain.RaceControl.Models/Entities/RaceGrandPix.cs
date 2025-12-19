using Domain.RaceControl.Models.Entities.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.RaceControl.Models.Entities;

public class RaceGrandPix
{
    [BsonId]
    public ObjectId Id { get; init; }

    public Circuit Circuit { get; private set; }

    public Season Season { get; private set; }

    [BsonElement("Session")]
    private readonly List<Session> _session;

    [BsonIgnore]
    public IReadOnlyCollection<Session> Session => _session;

    [BsonConstructor]
    public RaceGrandPix(Circuit circuit, Season season)
    {
        Id = ObjectId.GenerateNewId();
        Circuit = circuit;
        Season = season;

        _session = new List<Session> {
            new (EType.FreePractice1, 1),
            new (EType.FreePractice2, 2),
            new (EType.FreePractice3, 3),
            new (EType.Qualifying, 4),
            new (EType.MainRace, 5)
        };
    }

    
    protected RaceGrandPix(ObjectId id, Circuit circuit, Season season, List<Session> session)
    {
        Id = id;
        Circuit = circuit;
        Season = season;
        _session = session;
    }



    public void SetSession(List<Session> session)
    {
        _session.AddRange(session);
    }

    public void StartSession(EType type)
    {
        var session = _session.FirstOrDefault(s => s.Type == type);
        if (session is null)
            throw new ArgumentNullException("Sessão não existe.");

        var existSession = _session.Any(s => s.Order < session.Order
                                && s.Status != EStatus.Finished);

        if (session.Status == EStatus.Finished)
            throw new Exception("You can't start sessions finished");

        if (existSession)
            throw new Exception("You must finish last sessions before start this session");

        session.Start();
    }

    public void UpdateResultsSession(EType type, SessionResult result)
    {
        var session = _session.FirstOrDefault(s => s.Type == type);

        if (session is null)
            throw new ArgumentNullException("Incorrect type session");

        var existSession = _session.Any(s => s.Order < session.Order
                                && s.Status != EStatus.Finished);

        if (existSession)
            throw new Exception("You must finish last sessions before update this session");

        if (session.Status == EStatus.Finished)
            throw new Exception("You can't update sessions finished");

        if (result is null)
            throw new ArgumentNullException("Result cannot be null");

        session.UpdateResults(result);
        session.Finished();
    }
}
