namespace Domain.Competition.Models.DTOs.Competition;

public class CompetitionResponseDto
{
    public string Id { get; init; } = string.Empty;
    public int Round { get; init; }
    public string circuitName { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public bool IsActive { get; init; }
}
