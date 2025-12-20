using Domain.RaceControl.Models.Entities.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.RaceControl.Models.Entities;

public class RaceGrandPix
{
    [BsonId]
    public ObjectId Id { get; init; }

    [BsonElement("circuit")]
    public Circuit Circuit { get; private set; }

    [BsonElement("season")]
    public Season Season { get; private set; }

    [BsonElement("session")]
    public List<Session> Session { get; private set; }

    [BsonConstructor]
    public RaceGrandPix(Circuit circuit, Season season, List<Session> session)
    {
        Id = ObjectId.GenerateNewId();
        Circuit = circuit;
        Season = season;

        Session = session ?? new List<Session> {
            new (EType.FreePractice1, 1),
            new (EType.FreePractice2, 2),
            new (EType.FreePractice3, 3),
            new (EType.Qualifying, 4),
            new (EType.MainRace, 5)
        };
    }

    public void SetSession(List<Session> session)
    {
        Session.AddRange(session);
    }

    public void StartSession(EType type)
    {
        var session = Session.FirstOrDefault(s => s.Type == type);
        if (session is null)
            throw new ArgumentNullException("Sessão não existe.");

        var existSession = Session.Any(s => s.Order < session.Order
                                && s.Status != EStatus.Finished);

        if (session.Status == EStatus.Finished)
            throw new Exception("You can't start sessions finished");

        if (existSession)
            throw new Exception("You must finish last sessions before start this session");

        session.Start();
    }

    public void UpdateResultsSession(EType type, SessionResult result)
    {
        var session = Session.FirstOrDefault(s => s.Type == type);

        if (session is null)
            throw new ArgumentNullException("Incorrect type session");

        if (session.Status != EStatus.Live)
            throw new Exception("You must start this session");

        var existSession = Session.Any(s => s.Order < session.Order
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
