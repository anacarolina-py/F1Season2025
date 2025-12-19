using System.Text.Json.Serialization;

namespace Domain.RaceControl.Models.DTOs;

public class DriverChampionshipResponseDto
{
    [JsonPropertyName("name_driver")]
    public string NameDriver { get; private set; }

    [JsonPropertyName("number")]
    public int Number { get; private set; }

    [JsonPropertyName("name_team")]
    public string NameTeam { get; private set; }

    [JsonPropertyName("points")]
    public int Points { get; private set; }

    [JsonPropertyName("placing")]
    public int Placing { get; private set; }

    [JsonPropertyName("wins")]
    public int Wins { get; private set; }

    [JsonPropertyName("grid_position")]
    public int GridPosition { get; private set; }
}
