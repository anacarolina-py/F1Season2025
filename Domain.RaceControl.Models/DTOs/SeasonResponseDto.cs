using System.Text.Json.Serialization;

namespace Domain.RaceControl.Models.DTOs;

public class SeasonResponseDto
{
    [JsonPropertyName("id_season")]
    public string? IdSeason { get; init; }

    [JsonPropertyName("season_title")]
    public string? SeasonTitle { get; init; }
}
