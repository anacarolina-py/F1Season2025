using Domain.Competition.Models.DTOs.Competition;
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace F1Season2025.Competition.Services.Interfaces
{
    public interface ICompetitionService
    {
        Task<Circuit> RegisterCircuitAsync (string ameCircuit, string country, int laps);
        Task<CompetitionResponseDto> AddToCalendarSeasonAsync(CompetitionRequestDto competitionRequestDto);
        Task StartSimulationAsync (int round);
        Task<ValidateStartDto> ValidateStartCompetitionAsync(int round);
        Task CompleteCompetitionAsync(int round);
    }
}
