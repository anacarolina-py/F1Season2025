using Domain.RaceControl.Models.DTOs;
using F1Season2025.RaceControl.Repositories.Interfaces;
using F1Season2025.RaceControl.Services.Intefaces;

namespace F1Season2025.RaceControl.Services;

public class RaceService : IRaceService
{
    private readonly ILogger<RaceService> _logger;
    private readonly IRaceRepository _raceRepository;

    public RaceService(ILogger<RaceService> logger, IRaceRepository raceRepository)
    {
        _logger = logger;
        _raceRepository = raceRepository;
    }

    public async Task<List<RaceControlResponseDto>> GetAllRacesSeasonAsync()
    {
        try
        {
            return await _raceRepository.GetAllRacesSeasonAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
