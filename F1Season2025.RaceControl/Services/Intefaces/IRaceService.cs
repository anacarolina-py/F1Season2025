using Domain.RaceControl.Models.DTOs;

namespace F1Season2025.RaceControl.Services.Intefaces;

public interface IRaceService
{
    Task<List<RaceControlResponseDto>> GetAllRacesSeasonAsync();
}
