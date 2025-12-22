using Domain.Competition.Models.DTOs.Competition;
using Domain.Competition.Models.DTOs.Competitions;
using F1Season2025.Competition.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace F1Season2025.Competition.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitionController : ControllerBase
    {
        private readonly ICompetitionService _competitionService;
        private readonly ILogger<CompetitionController> _logger;

        public CompetitionController(ICompetitionService competitionService)
        {
            _competitionService = competitionService;
        }

        [HttpPost("register-circuit")]
        public async Task<IActionResult> RegisterCircuitAsync([FromBody] CreateCircuitDto requestDto)
        {
            try
            {
                var circuit = await _competitionService.RegisterCircuitAsync(requestDto.Name, requestDto.Country, requestDto.Laps);
                return Ok(
                    new
                    {
                        Id = circuit.Id.ToString(),
                        Name = circuit.NameCircuit,
                        Country = circuit.Country,
                        Laps = circuit.Laps
                    }
                    );
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = 500,
                    error = "Internal Error",
                    message = ex.Message
                });
            }
        }
        [HttpPost("calendar")]
        public async Task<IActionResult> CreateCalendarAsync([FromBody] CompetitionRequestDto requestDto)
        {
            try
            {
                var calendar = await _competitionService.AddToCalendarSeasonAsync(requestDto);
                return Ok(calendar);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, new
                {
                    status = 400,
                    error = "Invalid data",
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    new
                    {
                        status = 500,
                        error = "Internal Error",
                        message = ex.Message
                    });
            }
        }
        [HttpGet("validate-start/{round}")]
        public async Task<IActionResult> ValidateStartCompetitionAsync(int round)
        {
            try
            {
                var result = await _competitionService.ValidateStartCompetitionAsync(round);
                if (!result.CanStart)
                {
                    return StatusCode(400, new
                    {
                        status = 400,
                        error = "Blocked Start",
                        message = result.Message
                    });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = 500,
                    error = "Internal Error",
                    message = ex.Message
                });
            }
        }
        [HttpPatch("start/{round}")]
        public async Task<IActionResult> StartSimulationAsync(int round)
        {
            try
            {
                await _competitionService.StartSimulationAsync(round);
                return Ok(new
                {
                    message = $"Simulation for round {round} started successfully."
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new
                {
                    status = 404,
                    error = "Not found",
                    message = "Race not found."
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    status = 400,
                    error = "The race could not be started.",
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = 500,
                    error = "Failed to Start",
                    message = ex.Message
                });
            }
        }
        [HttpPatch("complete/{round}")]
        public async Task<IActionResult> CompleteSimulationAsync(int round)
        {
            try
            {
                var nextRace = await _competitionService.CompleteSimulationAsync(round);
                if (nextRace is null)
                {
                    return Ok(new
                    {
                        Message = "Season 2025 is over! The champion has been decided."
                    });
                }
                return Ok(new
                {
                    Message = $"Simulation for round {round} completed successfully.",
                    NextRace = nextRace
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { status = 404, error = "Not found", message = "Race not found to complete." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = 500, error = "Failed to Finish", message = ex.Message });
            }
        }
        [HttpGet("circuit/{circuitId}")]
        public async Task<IActionResult> GetCircuitDetailsAsync(string circuitId)
        {
            try
            {
                var circuit = await _competitionService.GetCircuitDetailsAsync(circuitId);
                return Ok(circuit);
            }
            catch (ArgumentException)
            {
                return BadRequest(new { status = 400, error = "Invalid ID", message = "The circuit ID format is incorrect." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { status = 404, error = "Circuit does not exist", message = "No circuits were found with that ID." });
            }
        }
        [HttpGet("circuits")]
        public async Task<IActionResult> GetAllCircuitsAsync()
        {
            try
            {
                var circuits = await _competitionService.GetAllCircuitsAsync();
                return Ok(circuits);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = 500, error = "Internal Error", message = ex.Message });
            }
        }
        [HttpGet("season")]
        public async Task<IActionResult> GetSeasonAsync()
        {
            try
            {
                var season = await _competitionService.GetSeasonAsync();
                return Ok(season);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = 500,
                    error = "Internal Error",
                    message = ex.Message
                });
            }
        }
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateRaceStatusAsync(string id, [FromBody] bool isActive)
        {
            try
            {
                await _competitionService.UpdateRaceStatusAsync(id, isActive);
                return Ok(new
                {
                    Sucess = true,
                    Message = isActive ? "Race successfully activated." : "Race deactivated.",
                    RaceId = id
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new
                {
                    statusCode = 404,
                    error = "Not Found",
                    message = "Race not found."
                });
            }
            catch (ArgumentException)
            {
                return NotFound(new
                {
                    status = 400,
                    error = "Invalid format.",
                    message = "The ID format is incorrect."
                });

            }
        }

        [HttpPost("season/start")]
        public async Task<IActionResult> StartSeasonAsync()
        {
            try
            {
                await _competitionService.StartSeasonAsync();


                return Ok(new { Message = "Season started successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpPost("finishrace")]
        public async Task<IActionResult> FinishRaceAsync([FromBody] CompetitionRaceResultDto raceResults)
        {
            try
            {
                _logger.LogInformation("Received race results for processing.");
                await _competitionService.ProcessRaceFinishAsync(raceResults);

                return Ok(
                    new
                    {
                        Message = "Race results processed and competition updated."
                    });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new {error = ex.Message});

            }
            catch (Exception ex)
            {
                _logger.LogError("Erro processing race");
                return StatusCode(500, new
                {
                    status = 500,
                    error = "Internal Error",
                    message = ex.Message
                });
            }
        }       
    }
}
