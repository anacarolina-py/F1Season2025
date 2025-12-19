using Domain.RaceControl.Models.DTOs;
using Domain.RaceControl.Models.Entities;
using Domain.RaceControl.Models.Entities.Enums;
using Domain.RaceControl.Models.Extensions;
using F1Season2025.RaceControl.Repositories.Interfaces;
using F1Season2025.RaceControl.Services.Intefaces;
using System.Diagnostics;

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

    public async Task<RaceControlResponseDto> CreateRaceAsync(string idCircuit)
    {
        try
        {
            var existCircuit = await GetRaceSeasonByIdCircuitAsync(idCircuit);

            if (existCircuit is not null)
                throw new Exception("Race created on this circuit");

            var circuit = new Circuit(idCircuit, "Interlagos", "Brasil", 57);
            var season = new Season(Guid.NewGuid().ToString(), "F1 Temporada 2025");
            var raceGrandPix = new RaceGrandPix(circuit, season, null);

            return await _raceRepository.CreateRace(raceGrandPix);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<RaceControlResponseDto> FinishFreePracticeAsync(string idCircuit, int number)
    {
        try
        {
            var race = await GetRaceSeasonByIdCircuitAsync(idCircuit);

            if (race is null)
                throw new Exception("Circuit not found or not implemented");

            var raceDto = number switch
            {
                1 => await FinishFreePractice1(race),
                2 => await FinishFreePractice2(race),
                3 => await FinishFreePractice3(race),
                _ => null
            };

            _logger.LogInformation("Get all teams");
            var constructors = new List<ConstructorChampionship>
            {
                new(1, "Ferrari"),
                new(2, "Red Bull"),
                new(3, "Mercedes Benz"),
            };

            _logger.LogInformation("Get all drivers");
            var drivers = new List<DriverChampionship>
            {
                new(1, "Hamilton", 22, 1, "Ferrari"),
                new(2, "Verstappen", 33, 2, "Red Bull"),
                new(3, "Norris", 44, 3, "Mercedes Benz")
            };

            _logger.LogInformation("Update data");
            race.UpdateResultsSession(EType.FreePractice2, sessionResult);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
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

    public async Task<RaceControlResponseDto?> GetRaceSeasonByIdCircuitAsync(string idCircuit)
    {
        try
        {
            var race = await _raceRepository.GetRaceSeasonByIdCircuitAsync(idCircuit);
            return race.ToDto();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<RaceControlResponseDto> StartFreePracticeAsync(string idCircuit, int number)
    {
        try
        {
            var race = await _raceRepository.GetRaceSeasonByIdCircuitAsync(idCircuit);

            if (race is null)
                throw new Exception("Circuit not found or not implemented");

            var raceDto = number switch
            {
                1 => await StartFreePractice1(race),
                2 => await StartFreePractice2(race),
                3 => await StartFreePractice3(race),
                _ => null
            };

            return raceDto;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private async Task<RaceControlResponseDto> StartFreePractice1(RaceGrandPix race)
    {
        try
        {
            var session = race.Session.Where(s => s.Type == EType.FreePractice1).FirstOrDefault();

            if (session.Status == EStatus.Live)
                throw new Exception("Cannot start the session because it is already live.");

            _logger.LogInformation("Start free practice");
            race.StartSession(EType.FreePractice1);

            _logger.LogInformation("Update data");

            return await _raceRepository.StartFreePractice(race);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private async Task<RaceControlResponseDto> StartFreePractice2(RaceGrandPix race)
    {
        try
        {
            var session = race.Session.Where(s => s.Type == EType.FreePractice1).FirstOrDefault();

            if (session.Status == EStatus.Live)
                throw new Exception("Cannot start the session because it is already live.");

            _logger.LogInformation("Start free practice");
            race.StartSession(EType.FreePractice2);

            return await _raceRepository.StartFreePractice(race);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private async Task<RaceControlResponseDto> StartFreePractice3(RaceGrandPix race)
    {
        try
        {
            var session = race.Session.Where(s => s.Type == EType.FreePractice1).FirstOrDefault();

            if (session.Status == EStatus.Live)
                throw new Exception("Cannot start the session because it is already live.");

            _logger.LogInformation("Start free practice");
            race.StartSession(EType.FreePractice2);

            return await _raceRepository.StartFreePractice(race);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error at start free practice");
            throw new Exception(ex.Message);
        }
    }


}
