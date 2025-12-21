using System.Text.Json.Serialization;

namespace Domain.RaceControl.Models.DTOs;

public class SessionResponseDto
{

    [JsonPropertyName("session_result")]
    public SessionResultResponseDto SessionResult { get; init; }

    [JsonPropertyName("type")]
    public string Type { get; init; }

    [JsonPropertyName("status")]
    public string Status { get; init; }
}
