using Domain.Competition.Models.DTOs.Competition;
using Domain.Competition.Models.DTOs.Competitions;
using Domain.Competition.Models.Entities;

namespace F1Season2025.Competition.Services.Interfaces
{
    public interface ICompetitionService
    {
        Task<Circuit> RegisterCircuitAsync (string nameCircuit, string country, int laps);
        Task<CompetitionResponseDto> AddToCalendarSeasonAsync(CompetitionRequestDto competitionRequestDto);
        Task<ValidateStartDto> ValidateStartCompetitionAsync(int round);
        Task StartSimulationAsync (int round);
        Task <CompetitionResponseDto?> CompleteSimulationAsync(int round);
        Task<Circuit> GetCircuitDetailsAsync(string circuitId);
        Task<IEnumerable<CircuitResponseDto>> GetAllCircuitsAsync();
        Task<IEnumerable<CompetitionResponseDto>> GetSeasonAsync();
        Task UpdateRaceStatusAsync(string id, bool isActive);
        Task StartSeasonAsync();
        Task ProcessRaceFinishAsync(CompetitionRaceResultDto raceResults);
        Task <IEnumerable<DriverStandingResponseDto>> GetDriverStandingsAsync();

    }
}
