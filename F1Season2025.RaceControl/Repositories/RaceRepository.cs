using Domain.RaceControl.Models.DTOs;
using Domain.RaceControl.Models.Entities;
using Domain.RaceControl.Models.Extensions;
using F1Season2025.RaceControl.Repositories.Interfaces;
using Infrastructure.RaceControl.Data.Mongo;
using MongoDB.Driver;

namespace F1Season2025.RaceControl.Repositories;

public class RaceRepository : IRaceRepository
{
    private readonly ILogger<RaceRepository> _logger;
    private readonly IMongoCollection<RaceGrandPix> _mongoRaceControl;

    public RaceRepository(ILogger<RaceRepository> logger, MongoContext mongo)
    {
        _logger = logger;
        _mongoRaceControl = mongo.RaceControls;
    }

    public async Task<List<RaceControlResponseDto>> GetAllRacesSeasonAsync()
    {
        try
        {
            _logger.LogInformation("Get all races...");
            var races = await _mongoRaceControl.Find(_ => true).ToListAsync();
            return [.. races.Select(x => x.ToDto())];
        }
        catch (MongoException mongoEx)
        {
            _logger.LogError(mongoEx, "Mongo error at get all races");
            throw new MongoException(mongoEx.Message);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error");
            throw new Exception(ex.Message);
        }
    }
}
