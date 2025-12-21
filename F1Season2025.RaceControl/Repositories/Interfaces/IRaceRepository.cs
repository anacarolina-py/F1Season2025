using Domain.RaceControl.Models.DTOs;
using Domain.RaceControl.Models.Entities;

namespace F1Season2025.RaceControl.Repositories.Interfaces;

public interface IRaceRepository
{
    Task<List<RaceControlResponseDto>> GetAllRacesSeasonAsync();
    Task<RaceControlResponseDto> CreateRace(RaceGrandPix race);
    Task<RaceGrandPix?> GetRaceSeasonByIdCircuitAsync(string idCircuit);
    Task<RaceControlResponseDto> UpdateSessionAsync(RaceGrandPix raceGrandPix);
}
