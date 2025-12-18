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
    public async Task<ActionResult> GetAllRacesSeasonAsync()
    {
        try
        {
            _logger.LogInformation("Get all races...");
            var races = await _raceService.GetAllRacesSeasonAsync();
            if(races.Count < 1)
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
}
