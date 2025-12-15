using Domain.Competition.Models.DTOs.Competition;
namespace Domain.Competition.Models.Extensions;

public static class CompetitionExtension
{
    public static CompetitionResponseDto ToResponseCompetition (this Domain.Competition.Models.Entities.Competition competition)
    {
        return new CompetitionResponseDto
        {
            Id = competition.Id,
            Round = competition.Round,
            circuitName = competition.Circuit.NameCircuit ?? "Loading",
            Status = competition.Status.ToString(),
            IsCompleted = competition.IsCompleted,
        };
    }
}
