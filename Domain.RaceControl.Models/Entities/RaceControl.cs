using Domain.RaceControl.Models.Entities.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.RaceControl.Models.Entities;

public class RaceControl
{
    [BsonId]
    public ObjectId Id { get; init; }
    public Circuit Circuit { get; private set; }
    public Season Season { get; private set; }
    private readonly List<Session> _session = new();
    public IReadOnlyCollection<Session> Session => _session;

    public RaceControl(Circuit circuit, Season season)
    {
        Id = ObjectId.GenerateNewId();
        Circuit = circuit;
        Season = season;

        _session.Add(new Session(EType.FreePractice1, 1));
        _session.Add(new Session(EType.FreePractice2, 2));
        _session.Add(new Session(EType.FreePractice3, 3));
        _session.Add(new Session(EType.Qualifying, 4));
        _session.Add(new Session(EType.MainRace, 5));
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
