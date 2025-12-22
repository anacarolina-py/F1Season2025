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
            var clientEngeneering = _factory.CreateClient("EngineeringClient");

            _logger.LogInformation("Get all active drivers");
            var driversResponse = await clientTeamManagement.GetFromJsonAsync<List<DriverResponseDTO>>("api/driver/actives");

            _logger.LogInformation("Get all active teams");
            var teamsResponse = await clientTeamManagement.GetFromJsonAsync<List<TeamResponseDTO>>("api/team/actives");

            _logger.LogInformation("Get handicap drivers");
            var handicapDrivers = await clientEngeneering.GetFromJsonAsync<List<DriverHandicapDTO>>("api/engineering/driver/handicaps");

            if (driversResponse is null)
                throw new Exception("Drivers not found");

            var drivers = new List<DriverChampionship>();
            var teams = new List<ConstructorChampionship>();

            foreach (var driver in driversResponse)
            {
                var team = teamsResponse.Where(t => t.TeamId == driver.TeamId).FirstOrDefault();

                var handicap = handicapDrivers.FirstOrDefault(d => d.Id == driver.StaffId);

                if (handicap is null)
                {
                    throw new Exception($"Handicap not found for driver {driver.FirstName}");
                }


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

    public async Task<RaceControlResponseDto> StartQualifyingAsync(string idCircuit)
    {
        try
        {
            var race = await _raceRepository.GetRaceSeasonByIdCircuitAsync(idCircuit);
            if (race == null)
            {
                throw new Exception("Circuit not found or not implemented");
            }

            var session = race.Session.FirstOrDefault(s => s.Type == EType.Qualifying);

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

    public async Task<RaceControlResponseDto> FinishQualifyingAsync(string idCircuit)
    {
        try
        {
            var race = await _raceRepository.GetRaceSeasonByIdCircuitAsync(idCircuit);
            if (race == null)
            {
                throw new Exception("Circuit not found or not implemented");
            }

            _logger.LogInformation("Create Engeneering client");
            var clientEngeneering = _factory.CreateClient("EngeneeringClient");

            var carsResponse = await clientEngeneering.GetFromJsonAsync<List<EngineeringCarDto>>("api/engeneering/cars");
            var handcapResponse = await clientEngeneering.GetFromJsonAsync<List<EngineeringHandicapDto>>("api/engeneering/driver/handicaps");

            var qualiSession = race.Session.FirstOrDefault(s => s.Type == EType.Qualifying);
            var drivers = qualiSession.SessionResult.Drivers;
            var teams = qualiSession.SessionResult.Teams;

            var qualiResults = new List<(DriverChampionship Driver, decimal PD)>();
            var randomLuck = new Random();

            foreach (var driver in drivers)
            {
                var car = carsResponse.FirstOrDefault(c => c.Id == driver.IdTeam);
                var handicapInfo = handcapResponse.FirstOrDefault(h => h.Id == driver.IdDriver);
                decimal ca = car?.AerodynamicCoefficient ?? 0;
                decimal cp = car?.PowerCoefficient ?? 0;
                decimal h = handicapInfo?.Handicap ?? 0;
                int luck = randomLuck.Next(1, 11);

                decimal pd = (ca * 0.4m) + (cp * 0.4m) - h + luck;

                driver.SetHandicap(h);
                driver.SetPerformancePoints(pd);

                qualiResults.Add((driver, pd));
            }

            var sortGrid = qualiResults.OrderByDescending(g => g.PD).ToList();

            var finalDriversList = new List<DriverChampionship>();
            int gridPosition = 1;

            foreach (var item in sortGrid)
            {
                item.Driver.SetGridPosition(gridPosition);
                finalDriversList.Add(item.Driver);
                gridPosition++;
            }

            foreach (var team in teams)
            {
                await clientEngeneering.PostAsync($"api/engeneering/qualifying/{team.IdTeam}", null);
            }

            var sessionResult = new SessionResult(finalDriversList, teams);

            race.UpdateResultsSession(EType.Qualifying, sessionResult);
            race.FinishedSession(EType.Qualifying);

            _logger.LogInformation("Qualifying finished. Grid positions updated!");

            return await _raceRepository.UpdateSessionAsync(race);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error at finish qualifying");
            throw new Exception(ex.Message);
        }
    }

    public async Task<RaceControlResponseDto> StartMainRaceAsync(string idCircuit)
    {
        try
        {
            var race = await _raceRepository.GetRaceSeasonByIdCircuitAsync(idCircuit);
            if (race is null)
            {
                throw new Exception("Circuit not found or not implemented");
            }
            var qualifyingSession = race.Session.FirstOrDefault(s => s.Type == EType.Qualifying);
            if (qualifyingSession.Status != EStatus.Finished)
            {

                throw new Exception("Qualifying must be finished before starting the race.");
            }
            var raceControl = race.Session.FirstOrDefault(s => s.Type == EType.MainRace);
            if (raceControl.Status == EStatus.Live)
            {
                throw new Exception("Cannot start the session because it is already live.");
            }
            _logger.LogInformation($"Green flag waved!! Start your engines, the main race has begun in circuit: {idCircuit}");

            race.StartSession(EType.MainRace);

            return await _raceRepository.UpdateSessionAsync(race);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error at start main race");
            throw;
        }
    }
    public async Task<RaceControlResponseDto> FinishMainRaceAsync(string idCircuit)
    {
        try
        {
            var race = await _raceRepository.GetRaceSeasonByIdCircuitAsync(idCircuit);
            if (race is null)
            {
                throw new Exception("Circuit not found or not implemented");
            }

            var clientEngeneering = _factory.CreateClient("EngeneeringClient");
            var clientCompetition = _factory.CreateClient("CompetitionClient");

            var carsResponse = await clientEngeneering.GetFromJsonAsync<List<EngineeringCarDto>>("api/engeneering/cars");
            var handcapResponse = await clientEngeneering.GetFromJsonAsync<List<EngineeringHandicapDto>>("api/engeneering/driver/handicaps");

            var qualiSession = race.Session.FirstOrDefault(s => s.Type == EType.Qualifying);
            var grindDrivers = qualiSession.SessionResult.Drivers;
            var teams = qualiSession.SessionResult.Teams;

            var raceResults = new List<(DriverChampionship Driver, decimal PD)>();
            var randomLuck = new Random();

            foreach (var driver in grindDrivers)
            {
                var car = carsResponse.FirstOrDefault(c => c.Id == driver.IdTeam);
                var handicapInfo = handcapResponse.FirstOrDefault(h => h.Id == driver.IdDriver);
                decimal ca = car?.AerodynamicCoefficient ?? 0;
                decimal cp = car?.PowerCoefficient ?? 0;
                decimal h = handicapInfo?.Handicap ?? 0;
                int luck = randomLuck.Next(1, 11);
                decimal pd = (ca * 0.4m) + (cp * 0.4m) - h + luck;
                driver.SetHandicap(h);
                driver.SetPerformancePoints(pd);
                raceResults.Add((driver, pd));
            }

            var finalOrder = raceResults.OrderByDescending(r => r.PD).ToList();
            int[] pointsSession = { 25, 18, 15, 12, 10, 8, 6, 4, 2, 1 };

            var finalDriversList = new List<DriverChampionship>();
            int position = 1;

            foreach (var item in finalOrder)
            {
                var driver = item.Driver;

                int pointsEarned = position <= pointsSession.Length ? pointsSession[position - 1] : 0;

                driver.SetPlacing(position);
                driver.SetPoints(pointsEarned);

                finalDriversList.Add(driver);
                position++;
            }

            foreach (var team in teams)
            {
                await clientEngeneering.PostAsync($"api/engeneering/race/{team.IdTeam}", null);
            }
            var sessionResult = new SessionResult(finalDriversList, teams);
            race.UpdateResultsSession(EType.MainRace, sessionResult);

            var raceSessionFinished = race.Session.FirstOrDefault(s => s.Type == EType.MainRace);
            if (raceSessionFinished != null)
            {
                raceSessionFinished.Finished();
            }
            await _raceRepository.UpdateSessionAsync(race);

            var attCalendar = new CompetitionRaceResultDto
            {
                CircuitId = idCircuit,
                Results = finalDriversList.Select(d => new CompetitionDriverResultDto
                {
                    DriverId = d.IdDriver,
                    Position = d.Placing,
                    Points = d.Points,
                    FastestLap = false
                }).ToList()
            };
            try
            {
                await clientCompetition.PostAsJsonAsync("api/competition/finishrace", attCalendar);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating competition calendar: {ex.Message}");
            }
            return race.ToDto();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error finishing main race");
            throw;
        }
    }
}
