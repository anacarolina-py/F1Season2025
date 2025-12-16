using Domain.Competition.Models.DTOs.Competition;
using Domain.Competition.Models.Entities;
using F1Season2025.Competition.Repository.Interfaces;
using F1Season2025.Competition.Services.Interfaces;
using Domain.Competition.Models.Extensions;
using MongoDB.Bson;

namespace F1Season2025.Competition.Services
{
    public class CompetitionService : ICompetitionService
    {
        private readonly ICircuitRepository _circuits;
        private readonly ICompetitionRepository _competitions;

        public CompetitionService(ICircuitRepository circuits, ICompetitionRepository competitions)
        {
            _circuits = circuits;
            _competitions = competitions;
        }
        public async Task<Circuit> RegisterCircuitAsync(string nameCircuit, string country, int laps)
        {
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

            int activeCount = await _competitions.GetActiveCompetitionsCountAsync();
            bool isActive = activeCount < 24;

            var competition = new Competitions(requestDto.Round, circuit, isActive);

            await _competitions.AddCompetitionAsync(competition);

            return competition.ToResponse();
        }

        public async Task<ValidateStartDto> ValidateStartCompetitionAsync(int round)
        {
            var targetCompetition = _competitions.GetCompetitionByRoundAsync(round);

            if (targetCompetition is null)
            {
                return new ValidateStartDto { 
                    CanStart = false, 
                    Message = "Race not found.", 
                    Round = round };
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
            var competition = await _competitions.GetCompetitionByRoundAsync(round);
            if (competition is null)
            {
                throw new KeyNotFoundException($"Competition with round {round} not found.");
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
                throw new ArgumentException($"Invalid CircuitId format. {circuitId}");
            }
            var circuit = await _circuits.GetCircuitByIdAsync(id);
            if (circuit is null)
            {
                throw new KeyNotFoundException($"Circuit with Id {circuitId} not found.");
            }
            return circuit;
        }
        public async Task <IEnumerable<CompetitionResponseDto>> GetSeasonAsync()
        {
            var competitions = await _competitions.GetAllCompetitionsAsync();
            return competitions.Select(c => c.ToResponse());
        }

    }
}
