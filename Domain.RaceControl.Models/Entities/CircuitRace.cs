using MongoDB.Bson.Serialization.Attributes;

namespace Domain.RaceControl.Models.Entities;

public class CircuitRace
{
    [BsonElement("id_circuit")]
    public string IdCircuit { get; private set; }
    [BsonElement("name_circuit")]
    public string NameCircuit { get; private set; }
    [BsonElement("country")]
    public string Country { get; private set; }
    [BsonElement("laps")]
    public int Laps { get; private set; }

    public CircuitRace(string idCircuit, string nameCircuit, string country, int laps)
    {
        IdCircuit = idCircuit;
        NameCircuit = nameCircuit;
        Country = country;
        Laps = laps;
    }
}
