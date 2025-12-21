using System.Text.Json.Serialization;

namespace Domain.RaceControl.Models.DTOs;

public class DriverChampionshipResponseDto
{
    [JsonPropertyName("name_driver")]
    public string NameDriver { get; init; }

    [JsonPropertyName("number")]
    public int Number { get; init; }

    [JsonPropertyName("name_team")]
    public string NameTeam { get; init; }

    [JsonPropertyName("points")]
    public int Points { get; init; }

    [JsonPropertyName("placing")]
    public int Placing { get; init; }

    [JsonPropertyName("wins")]
    public int Wins { get; init; }

    [JsonPropertyName("grid_position")]
    public int GridPosition { get; init; }
}
