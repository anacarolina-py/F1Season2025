using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Domain.RaceControl.Models.DTOs;

public class CircuitResponseDto
{
    [JsonPropertyName("id_circuit")]
    [BsonElement("id_circuit")]
    public string? IdCircuit { get; init; }

    [JsonPropertyName("name_circuit")]
    [BsonElement("name_circuit")]
    public string? NameCircuit { get; init; }

    [BsonElement("country")]
    public string? Country { get; init; }

    [BsonElement("laps")]
    public int Laps { get; init; }
}
