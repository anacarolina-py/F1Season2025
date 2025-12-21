namespace Domain.Competition.Models.DTOs.Competition;

public class CompetitionResponseDto
{
    public string Id { get; init; } = string.Empty;
    public int Round { get; init; }
    public string CircuitName { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public int Laps { get; init; }
    public string Status { get; init; } = string.Empty;
    public bool IsActive { get; init; }
}
