using Domain.Competition.Models.DTOs.Competition;
using Domain.Competition.Models.Entities;
using F1Season2025.Competition.Repository.Interfaces;
using F1Season2025.Competition.Services.Interfaces;
using Domain.Competition.Models.Extensions;
using MongoDB.Bson;
using Domain.Competition.Models.Entities.Enum;
using Domain.Competition.Models.DTOs.Competitions;

namespace F1Season2025.Competition.Services
{
    public class CompetitionService : ICompetitionService
    {
        private readonly ICircuitRepository _circuits;
        private readonly ICompetitionRepository _competitions;
        private readonly ITeamServiceClient _teamServiceClient;

        public CompetitionService(ICircuitRepository circuits, ICompetitionRepository competitions, ITeamServiceClient teamServiceClient)
        {
            _circuits = circuits;
            _competitions = competitions;
            _teamServiceClient = teamServiceClient;
        }
        public async Task<Circuit> RegisterCircuitAsync(string nameCircuit, string country, int laps)
        {
            bool circuitExists = await _circuits.ExistCircuitNameCountryAsync(nameCircuit, country);
            if (circuitExists)
            {
                throw new InvalidOperationException($"The circuit {nameCircuit} in {country} is already registered.");
            }
            var circuit = new Circuit(nameCircuit, country, laps);

            await _circuits.AddCircuitAsync(circuit);

            return circuit;
        }
        public async Task<CompetitionResponseDto> AddToCalendarSeasonAsync(CompetitionRequestDto requestDto)
        {
            if (!ObjectId.TryParse(requestDto.CircuitId, out ObjectId NewcircuitId))
            {
                throw new ArgumentException($"Invalid CircuitId format. {requestDto.CircuitId}");
            }

            var circuit = await _circuits.GetCircuitByIdAsync(NewcircuitId);

            if (circuit is null)
            {
                throw new ArgumentException($"Circuit with Id {requestDto.CircuitId} not found.");
            }
            if (!circuit.IsActive)
            {
                throw new InvalidOperationException($"The circuit {circuit.NameCircuit} Cannot add an inactive circuit to the season calendar.");
            }
            bool circuitExists = await _competitions.CheckCompetitionExistsAsync(circuit.NameCircuit, circuit.Country);
            if (circuitExists)
            {
                throw new InvalidOperationException($"The circuit {circuit.NameCircuit} is already registered in the season calendar.");
            }
            int activeCount = await _competitions.GetActiveCompetitionsCountAsync();
            bool maxRaces = activeCount < 24;

            var newRound = activeCount + 1;
            var competition = new Competitions(newRound, circuit, maxRaces);

            await _competitions.AddCompetitionAsync(competition);

            return competition.ToResponse();
        }

        public async Task<ValidateStartDto> ValidateStartCompetitionAsync(int round)
        {
            var targetCompetition = _competitions.GetCompetitionByRoundAsync(round);

            if (targetCompetition is null)
            {
                return new ValidateStartDto
                {
                    CanStart = false,
                    Message = "Race not found.",
                    Round = round
                };
            }

            var previusRace = round > 1 ? await _competitions.GetCompetitionByRoundAsync(round - 1) : null;

            var (isValid, error) = targetCompetition.Result.ValidateCircuitRace(previusRace);

            return new ValidateStartDto
            {
                CanStart = isValid,
                Message = error,
                Round = round
            };
        }

        public async Task StartSimulationAsync(int round)
        {
            //temporada precisa ter iniciado
            var competitions = await _competitions.GetAllCompetitionsAsync();

            bool seasonStarted = false;

            foreach (var item in competitions)
            {
                if (item.Status == CompetitionStatus.InProgress || item.Status == CompetitionStatus.Finished)
                {
                    seasonStarted = true;
                    break;
                }
            }

            if (!seasonStarted)
            {
                throw new InvalidOperationException("The season has not started yet. You must start the season before simulating races.");
            }

            var competition = await _competitions.GetCompetitionByRoundAsync(round);
            if (competition is null)
            {
                throw new KeyNotFoundException($"Competition with round: {round} not found.");
            }
            if (competition.Status != CompetitionStatus.Scheduled)
            {
                throw new InvalidOperationException($"This race cannot be started because the current status is {competition.Status}.");
            }
            if (round > 1)
            {
                var previusRace = await _competitions.GetCompetitionByRoundAsync(round - 1);
                if (previusRace is null)
                {
                    throw new InvalidOperationException("Cannot start the race before completing the previous one.");
                }
                if (previusRace.Status != CompetitionStatus.Finished)
                {
                    throw new InvalidOperationException("The previus GP is not finished.");
                }

            }
            competition.StartCompetition();
            await _competitions.UpdateStatusRaceAsync(competition);

        }
        public async Task<CompetitionResponseDto?> CompleteSimulationAsync(int round)
        {
            var competition = await _competitions.GetCompetitionByRoundAsync(round);
            if (competition is null)
            {
                throw new KeyNotFoundException($"Competition with round {round} not found.");
            }
            competition.CompleteCompetition();
            await _competitions.UpdateStatusRaceAsync(competition);

            var nextCompetition = await _competitions.GetCompetitionByRoundAsync(round + 1);
            return nextCompetition?.ToResponse();
        }
        public async Task<Circuit> GetCircuitDetailsAsync(string circuitId)
        {
            if (!ObjectId.TryParse(circuitId, out ObjectId id))
            {
                throw new ArgumentException($"Invalid {circuitId} format.");
            }
            var circuit = await _circuits.GetCircuitByIdAsync(id);
            if (circuit is null)
            {
                throw new KeyNotFoundException($"Circuit with Id {circuitId} not found.");
            }
            return circuit;
        }
        public async Task<IEnumerable<CircuitResponseDto>> GetAllCircuitsAsync()
        {
            var circuits = await _circuits.GetAllCircuitsAsync();
            var circuitsDto = circuits.Select(c => c.ToCircuitResponseDto());
            return circuitsDto;
        }
        public async Task<IEnumerable<CompetitionResponseDto>> GetSeasonAsync()
        {
            var competitions = await _competitions.GetAllCompetitionsAsync();
            return competitions.Select(c => c.ToResponse());
        }
        public async Task UpdateRaceStatusAsync(string id, bool isActive)
        {
            //temporada precisa ter iniciado
            var competitions = await _competitions.GetAllCompetitionsAsync();

            bool seasonStarted = false;

            foreach (var item in competitions)
            {
                if (item.Status == CompetitionStatus.InProgress || item.Status == CompetitionStatus.Finished)
                {
                    seasonStarted = true;
                    break;
                }
            }

            if (!seasonStarted)
            {
                throw new InvalidOperationException("Cannot change race status before the season starts.");
            }

            if (!ObjectId.TryParse(id, out var objectId))
            {
                throw new ArgumentException($"Invalid CompetitionId format. {id}");
            }
            var race = await _competitions.GetbyCompetitionByIdAsync(objectId);
            if (race is null)
            {
                throw new KeyNotFoundException($"Competition {id} not found.");
            }
            if (race.Status == CompetitionStatus.Finished)
            {
                throw new InvalidOperationException("Is not possible to change the status of a ride that has already been completed.");
            }
            await _competitions.UpdateActiveStatusAsync(objectId, isActive);
        }

        public async Task StartSeasonAsync()
        {
            //busco as corridas na temporada
            var competitions = (await _competitions.GetAllCompetitionsAsync()).ToList();

            //verifico se tem 24
            if (competitions.Count != 24)
            {
                throw new InvalidOperationException("The season must have exactly 24 races to start.");
            }

            //verifico se a temporada ja começou
            foreach (var competition in competitions)
            {
                if (competition.Status == CompetitionStatus.InProgress || competition.Status == CompetitionStatus.Finished)
                {
                    throw new InvalidOperationException("Season has already started.");
                }

                var canStart = await _teamServiceClient.ValidateSeasonAsync();

                if (!canStart)
                {
                    throw new InvalidOperationException("Teams are not ready to start the season.");
                }
            }
        }
    }
}
