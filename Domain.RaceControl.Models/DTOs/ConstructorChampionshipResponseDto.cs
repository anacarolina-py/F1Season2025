using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Domain.RaceControl.Models.DTOs;

public class ConstructorChampionshipResponseDto
{

    [JsonPropertyName("name_team")]
    public string? NameTeam { get; init; }

    [JsonPropertyName("total_points")]
    public int TotalPoints { get; init; }

    [JsonPropertyName("placing")]
    public int Placing { get; init; }

    [JsonPropertyName("wins")]
    public int Wins { get; init; }
}
