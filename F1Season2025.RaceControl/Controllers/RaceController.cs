using Domain.RaceControl.Models.DTOs;
using F1Season2025.RaceControl.Services.Intefaces;
using Microsoft.AspNetCore.Mvc;

namespace F1Season2025.RaceControl.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RaceController : ControllerBase
{
    private readonly ILogger<RaceController> _logger;
    private readonly IRaceService _raceService;

    public RaceController(ILogger<RaceController> logger, IRaceService raceService)
    {
        _logger = logger;
        _raceService = raceService;
    }

    [HttpGet]
    public async Task<ActionResult<List<RaceControlResponseDto>>> GetAllRacesSeasonAsync()
    {
        try
        {
            _logger.LogInformation("Get all races...");

            var races = await _raceService.GetAllRacesSeasonAsync();
            if (races.Count < 1)
            {
                _logger.LogInformation("Not found races");
                return NotFound("Register not found");
            }

            return Ok(races);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error at get all races");
            return Problem(ex.Message);
        }
    }

    [HttpPost("{idCircuit}")]
    public async Task<ActionResult<RaceControlResponseDto>> CreateRaceAsync(string idCircuit)
    {
        try
        {
            _logger.LogInformation("Create race");

            if (idCircuit is null || string.IsNullOrWhiteSpace(idCircuit))
                BadRequest("Id can't be null");

            var race = await _raceService.CreateRaceAsync(idCircuit);

            if (race is null)
                return NotFound("Race not found");

            return Created(nameof(GetRaceByIdCircuit), race);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error at create race");
            return Problem(ex.Message);
        }
    }

    [HttpGet("{idCircuit}")]
    public async Task<ActionResult<RaceControlResponseDto>> GetRaceByIdCircuit(string idCircuit)
    {
        try
        {
            var circuit = await _raceService.GetRaceSeasonByIdCircuitAsync(idCircuit);

            if (circuit is null)
                return NotFound("Register not found");

            return Ok(circuit);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error at get race");
            return Problem(ex.Message);
        }
    }

    [HttpPost("{idCircuit}/simulate/fp/{number}/start")]
    public async Task<ActionResult<RaceControlResponseDto>> StartFreePracticeAsync(string idCircuit, int number)
    {
        try
        {
            var race = await _raceService.StartFreePracticeAsync(idCircuit, number);

            if (race is null)
                return BadRequest("Free practice not found");

            return Ok(race);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error at create first free practice");
            return Problem(ex.Message);
        }
    }
}
