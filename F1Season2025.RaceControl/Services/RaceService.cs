using Domain.Competition.Models.Entities;
using Domain.Engeneering.Models.DTOs;
using Domain.RaceControl.Models.DTOs;
using Domain.RaceControl.Models.Entities;
using Domain.RaceControl.Models.Entities.Enums;
using Domain.RaceControl.Models.Extensions;
using Domain.TeamManagement.Models.DTOs.Staffs.Drivers;
using Domain.TeamManagement.Models.DTOs.Teams;
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

            var circuitRace = new CircuitRace(idCircuit, circuit.NameCircuit, circuit.Country, circuit.Laps);

            var raceGrandPix = new RaceGrandPix(circuitRace);

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

            var raceDto = await FinishQualifyAsync(race);
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
            _logger.LogInformation("Create Team management client");
            var clientTeamManagement = _factory.CreateClient("TeamManagementClient");

            _logger.LogInformation("Create Engeneering client");
            var clientEngeneering = _factory.CreateClient("EngeneeringClient");

            _logger.LogInformation("Get all active drivers");
            var driversResponse = await clientTeamManagement.GetFromJsonAsync<List<DriverResponseDTO>>("api/drivers/actives");

            _logger.LogInformation("Get all active teams");
            var teamsResponse = await clientTeamManagement.GetFromJsonAsync<List<TeamResponseDTO>>("api/teams/actives");

            _logger.LogInformation("Get handicap drivers");
            var handicapDrivers = await clientEngeneering.GetFromJsonAsync<List<DriverHandicapDTO>>("api/engeneering/driver/handicap");

            if (driversResponse is null)
                throw new Exception("Drivers not found");

            var drivers = new List<DriverChampionship>();
            var teams = new List<ConstructorChampionship>();

            foreach (var driver in driversResponse)
            {
                var team = teamsResponse.Where(t => t.TeamId == driver.TeamId).FirstOrDefault();

                var handicap = handicapDrivers.Where(d => d.Id == driver.StaffId).FirstOrDefault();

                drivers.Add(new DriverChampionship(driver.StaffId, driver.FirstName, driver.DriverId, driver.TeamId, team.Name, handicap.Handicap));
            }

            foreach (var team in teamsResponse)
            {
                teams.Add(new ConstructorChampionship(team.TeamId, team.Name));
            }

            foreach (var team in teams)
            {
                await clientEngeneering.PostAsJsonAsync($"api/engeneering/practice/", team.IdTeam);
            }

            var sessionResult = new SessionResult(drivers, teams);

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

            _logger.LogInformation("Create Engeneering client");
            var clientEngeneering = _factory.CreateClient("EngeneeringClient");

            var sessionResult = race.Session.Select(s => s.SessionResult);

            var drivers = sessionResult.Select(s => s.Drivers).FirstOrDefault();
            var teams = sessionResult.Select(s => s.Teams).FirstOrDefault();

            _logger.LogInformation("Get handicap drivers");
            var handicapDrivers = await clientEngeneering.GetFromJsonAsync<List<DriverHandicapDTO>>("api/engeneering/driver/handicap");

            foreach (var driver in drivers)
            {
                var handicap = handicapDrivers.Where(d => d.Id == driver.IdDriver).FirstOrDefault();

                driver.SetHandicap(handicap.Handicap);
            }

            foreach (var team in teams)
            {
                await clientEngeneering.PostAsJsonAsync($"api/engeneering/practice/", team.IdTeam);
            }

            var session = new SessionResult(drivers, teams);

            _logger.LogInformation("Update data");
            race.UpdateResultsSession(EType.FreePractice2, session);

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
            _logger.LogInformation("Create Engeneering client");
            var clientEngeneering = _factory.CreateClient("EngeneeringClient");

            var sessionResult = race.Session.Select(s => s.SessionResult);

            var drivers = sessionResult.Select(s => s.Drivers).FirstOrDefault();
            var teams = sessionResult.Select(s => s.Teams).FirstOrDefault();

            _logger.LogInformation("Get handicap drivers");
            var handicapDrivers = await clientEngeneering.GetFromJsonAsync<List<DriverHandicapDTO>>("api/engeneering/driver/handicap");

            foreach (var driver in drivers)
            {
                var handicap = handicapDrivers.Where(d => d.Id == driver.IdDriver).FirstOrDefault();

                driver.SetHandicap(handicap.Handicap);
            }

            foreach (var team in teams)
            {
                await clientEngeneering.PostAsJsonAsync($"api/engeneering/practice/", team.IdTeam);
            }

            var session = new SessionResult(drivers, teams);

            _logger.LogInformation("Update data");
            race.UpdateResultsSession(EType.FreePractice3, session);

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
            _logger.LogInformation("Create Engeneering client");
            var clientEngeneering = _factory.CreateClient("EngeneeringClient");

            var sessionResult = race.Session.Select(s => s.SessionResult);

            var drivers = sessionResult.Select(s => s.Drivers).FirstOrDefault();
            var teams = sessionResult.Select(s => s.Teams).FirstOrDefault();

            _logger.LogInformation("Get handicap drivers");
            var handicapDrivers = await clientEngeneering.GetFromJsonAsync<List<DriverHandicapDTO>>("api/engeneering/driver/handicap");

            foreach (var driver in drivers)
            {
                var handicap = handicapDrivers.Where(d => d.Id == driver.IdDriver).FirstOrDefault();

                driver.SetHandicap(handicap.Handicap);
            }

            foreach (var team in teams)
            {
                await clientEngeneering.PostAsJsonAsync($"api/engeneering/qualifying/", team.IdTeam);
            }



            var session = new SessionResult(drivers, teams);
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
