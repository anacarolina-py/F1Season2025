using MongoDB.Bson.Serialization.Attributes;

namespace Domain.RaceControl.Models.Entities;

public class Circuit
{
    [BsonElement("id_circuit")]
    public string IdCircuit { get; private set; }
    [BsonElement("name_circuit")]
    public string NameCircuit { get; private set; }
    public string Country { get; private set; }
    public int Laps { get; private set; }

    public Circuit(string idCircuit, string nameCircuit, string country, int laps)
    {
        IdCircuit = idCircuit;
        NameCircuit = nameCircuit;
        Country = country;
        Laps = laps;
    }
}
