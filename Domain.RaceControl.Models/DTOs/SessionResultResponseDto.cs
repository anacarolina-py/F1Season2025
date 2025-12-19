using Domain.RaceControl.Models.Entities;
using System.Text.Json.Serialization;

namespace Domain.RaceControl.Models.DTOs;

public class SessionResultResponseDto
{

    [JsonPropertyName("drivers")]
    public List<DriverChampionshipResponseDto> Drivers { get; init; } = [];

    [JsonPropertyName("teams")]
    public List<ConstructorChampionshipResponseDto> Teams { get; init; } = [];
}
