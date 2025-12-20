using Domain.RaceControl.Models.DTOs;
using Domain.RaceControl.Models.Entities;
using Domain.RaceControl.Models.Entities.Enums;
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

    public async Task<RaceControlResponseDto> CreateRace(RaceGrandPix race)
    {
        try
        {
            _logger.LogInformation("Create race and implement first practice");
            await _mongoRaceControl.InsertOneAsync(race);
            var returnedRace = await _mongoRaceControl.Find(r => r.Id == race.Id).FirstOrDefaultAsync();

            return returnedRace.ToDto();
        }
        catch (MongoException mongoEx)
        {
            _logger.LogError(mongoEx.StackTrace, "Mongo error at get all races");
            throw new MongoException(mongoEx.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error");
            throw new Exception(ex.Message);
        }
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
            _logger.LogError(mongoEx.StackTrace, "Mongo error at get all races");
            throw new MongoException(mongoEx.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error");
            throw new Exception(ex.Message);
        }
    }

    public async Task<RaceGrandPix?> GetRaceSeasonByIdCircuitAsync(string idCircuit)
    {
        try
        {
            _logger.LogInformation("Find race by id circuit");
            var race = await _mongoRaceControl
                .Find(r => r.Circuit.IdCircuit == idCircuit)
                .FirstOrDefaultAsync();

            if (race is null)
                return null;

            return race;
        }
        catch (MongoException mongoEx)
        {
            _logger.LogError(mongoEx.StackTrace, "Mongo error at get race by id circuit");
            throw new MongoException(mongoEx.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error at get race by id circuit");
            throw new Exception(ex.Message);
        }
    }

    public async Task<RaceControlResponseDto> UpdateSessionAsync(RaceGrandPix raceGrandPix)
    {
        try
        {
            var filter = Builders<RaceGrandPix>
                .Filter
                .Eq(f => f.Circuit.IdCircuit, raceGrandPix.Circuit.IdCircuit);

            var update = Builders<RaceGrandPix>
                .Update
                .Set("Session", raceGrandPix.Session.ToList());

            await _mongoRaceControl
                .UpdateOneAsync(filter, update);

            var race =  await _mongoRaceControl
                .Find(r => r.Circuit.IdCircuit == raceGrandPix.Circuit.IdCircuit)
                .FirstOrDefaultAsync();

            return race.ToDto();
        }
        catch (MongoException mongoEx)
        {
            _logger.LogError(mongoEx.StackTrace, "Mongo error at start first free practice");
            throw new MongoException(mongoEx.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error at start first free practice");
            throw new Exception(ex.Message);
        }
    }

}
