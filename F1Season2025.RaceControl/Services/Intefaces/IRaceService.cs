using Domain.RaceControl.Models.DTOs;
using Domain.RaceControl.Models.Entities;

namespace F1Season2025.RaceControl.Services.Intefaces;

public interface IRaceService
{
    Task<List<RaceControlResponseDto>> GetAllRacesSeasonAsync();
    Task<RaceControlResponseDto> CreateRaceAsync(string idCircuit);
    Task<RaceControlResponseDto> GetRaceSeasonByIdCircuitAsync(string idCircuit);
    Task<RaceControlResponseDto> StartFreePracticeAsync(string idCircuit, int number);
    Task<RaceControlResponseDto> FinishFreePracticeAsync(string idCircuit, int number);
    Task<RaceControlResponseDto> StartQualifyingAsync(string idCircuit);
}
