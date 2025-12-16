using Domain.Competition.Models.DTOs.Competition;
using Domain.Competition.Models.Entities;

namespace Domain.Competition.Models.Extensions;

public static class CompetitionExtension
{
    public static CompetitionResponseDto ToResponse (this Competitions competition)
    {
        return new CompetitionResponseDto
        {
            Id = competition.Id.ToString(),
            Round = competition.Round,
            circuitName = competition.Circuit.NameCircuit ?? "Loading",
            Status = competition.Status.ToString(),
            IsActive = competition.IsActive
        };
    }
}
