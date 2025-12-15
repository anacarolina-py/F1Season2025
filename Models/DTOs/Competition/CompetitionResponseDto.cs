namespace Domain.Competition.Models.DTOs.Competition;

public class CompetitionResponseDto
{
    public Guid Id { get; init; }
    public int Round { get; init; }
    public string circuitName { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public bool IsCompleted { get; init; }
}
