using System.Text.Json.Serialization;

namespace Domain.RaceControl.Models.DTOs;

public class RaceControlResponseDto
{
    [JsonPropertyName("circuit")]
    public CircuitResponseDto Circuit { get; init; }

    [JsonPropertyName("season")]
    public SeasonResponseDto Season { get; init; }

    [JsonPropertyName("sessions")]
    public List<SessionResponseDto> Session { get; init; } = [];
}
