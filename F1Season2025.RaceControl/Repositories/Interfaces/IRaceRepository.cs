using Domain.RaceControl.Models.DTOs;

namespace F1Season2025.RaceControl.Repositories.Interfaces;

public interface IRaceRepository
{
    Task<List<RaceControlResponseDto>> GetAllRacesSeasonAsync();
}
