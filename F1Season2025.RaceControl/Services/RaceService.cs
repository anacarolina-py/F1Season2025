using Domain.Competition.Models.Entities;
using Domain.RaceControl.Models.DTOs;
using Domain.RaceControl.Models.Entities;
using Domain.RaceControl.Models.Entities.Enums;
using Domain.RaceControl.Models.Extensions;
using F1Season2025.RaceControl.Repositories.Interfaces;
using F1Season2025.RaceControl.Services.Intefaces;

namespace F1Season2025.RaceControl.Services;

public class RaceService : IRaceService
{
    private readonly ILogger<RaceService> _logger;
    private readonly IRaceRepository _raceRepository;
    private readonly IHttpClientFactory _factory;


    public RaceService(ILogger<RaceService> logger, IRaceRepository raceRepository, IHttpClientFactory factory)
    {
        _logger = logger;
        _raceRepository = raceRepository;
        _factory = factory;
    }

    public async Task<RaceControlResponseDto> CreateRaceAsync(string idCircuit)
    {
        try
        {
            var client = _factory.CreateClient("CompetitionClient");

            var existCircuit = await GetRaceSeasonByIdCircuitAsync(idCircuit);

            if (existCircuit is not null)
                throw new Exception("Race created on this circuit");

            var circuit = await client.GetFromJsonAsync<Circuit>($"api/competition/circuit/{idCircuit}");

            if (circuit is null)
                throw new ArgumentNullException("Circuit not exists");

            if (circuit.IsActive == false)
                throw new Exception("Circuit is not active");

            var circuitRace = new CircuitRace(circuit.Id.ToString(), circuit.NameCircuit, circuit.Country, circuit.Laps);

            var season = new Season(Guid.NewGuid().ToString(), "F1 Temporada 2025");

            var raceGrandPix = new RaceGrandPix(circuitRace, season);

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
            var race = await _raceRepository.GetRaceSeasonByIdCircuitAsync(idCircuit);

            if (race is null)
                throw new Exception("Circuit not found or not implemented");

            var raceDto = number switch
            {
                1 => await FinishFreePractice1(race),
                2 => await FinishFreePractice2(race),
                3 => await FinishFreePractice3(race),
                _ => null
            };

            return raceDto;
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

    public async Task<RaceControlResponseDto> StartQualifyingAsync(string idCircuit)
    {
        try
        {
            var race = await _raceRepository.GetRaceSeasonByIdCircuitAsync(idCircuit);

            if (race is null)
                throw new Exception("Circuit not found or not implemented");

            var raceDto = await StartQualifyingAsync(race);
            return raceDto;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<RaceControlResponseDto> FinishQualifyingAsync(string idCircuit)
    {
        try
        {
            var race = await _raceRepository.GetRaceSeasonByIdCircuitAsync(idCircuit);

            if (race is null)
                throw new Exception("Circuit not found or not implemented");

            var raceDto = await StartQualifyingAsync(race);
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

            return await _raceRepository.UpdateSessionAsync(race);
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
            var session = race.Session.Where(s => s.Type == EType.FreePractice2).FirstOrDefault();

            if (session.Status == EStatus.Live)
                throw new Exception("Cannot start the session because it is already live.");

            _logger.LogInformation("Start free practice");
            race.StartSession(EType.FreePractice2);

            return await _raceRepository.UpdateSessionAsync(race);
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
            var session = race.Session.Where(s => s.Type == EType.FreePractice3).FirstOrDefault();

            if (session.Status == EStatus.Live)
                throw new Exception("Cannot start the session because it is already live.");

            _logger.LogInformation("Start free practice");
            race.StartSession(EType.FreePractice3);

            return await _raceRepository.UpdateSessionAsync(race);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error at start free practice");
            throw new Exception(ex.Message);
        }
    }

    private async Task<RaceControlResponseDto> FinishFreePractice1(RaceGrandPix race)
    {
        try
        {
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

            var sessionResult = new SessionResult(drivers, constructors);

            _logger.LogInformation("Update data");
            race.UpdateResultsSession(EType.FreePractice1, sessionResult);

            return await _raceRepository.UpdateSessionAsync(race);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error at finish free practice");
            throw new Exception(ex.Message);
        }
    }

    private async Task<RaceControlResponseDto> FinishFreePractice2(RaceGrandPix race)
    {
        try
        {
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

            var sessionResult = new SessionResult(drivers, constructors);

            _logger.LogInformation("Update data");
            race.UpdateResultsSession(EType.FreePractice2, sessionResult);

            return await _raceRepository.UpdateSessionAsync(race);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error at finish free practice");
            throw new Exception(ex.Message);
        }
    }

    private async Task<RaceControlResponseDto> FinishFreePractice3(RaceGrandPix race)
    {
        try
        {
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

            var sessionResult = new SessionResult(drivers, constructors);

            _logger.LogInformation("Update data");
            race.UpdateResultsSession(EType.FreePractice3, sessionResult);

            return await _raceRepository.UpdateSessionAsync(race);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error at finish free practice");
            throw new Exception(ex.Message);
        }
    }

    private async Task<RaceControlResponseDto> StartQualifyingAsync(RaceGrandPix race)
    {
        try
        {
            var session = race.Session.Where(s => s.Type == EType.Qualifying).FirstOrDefault();

            if (session.Status == EStatus.Live)
                throw new Exception("Cannot start the session because it is already live.");

            _logger.LogInformation("Start qualifying");
            race.StartSession(EType.Qualifying);

            _logger.LogInformation("Update data");
            return await _raceRepository.UpdateSessionAsync(race);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private async Task<RaceControlResponseDto> FinishQualifyAsync(RaceGrandPix race)
    {
        try
        {
            var drivers = race.Session.Select(s => s.SessionResult.Drivers);
            var teams = race.Session.Select(s => s.SessionResult.Teams);

            

            _logger.LogInformation("Update data");
            //race.UpdateResultsSession(EType.FreePractice3, sessionResult);

            return await _raceRepository.UpdateSessionAsync(race);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
