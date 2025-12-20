using F1Season2025.Engineering.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace F1Season2025.Engineering.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EngineeringController : ControllerBase
    {
        private readonly EngineeringService _engineeringService;
        private readonly ILogger<EngineeringController> _logger;

        public EngineeringController(EngineeringService engineeringService, ILogger<EngineeringController> logger)
        {
            _engineeringService = engineeringService;
            _logger = logger;
        }

        //gatilhos de evolução

        [HttpPost("practice/{teamId}")]
        public async Task<IActionResult> Practice(int teamId)
        {
            _logger.LogInformation("Processing Practice");
            await _engineeringService.ProcessPractice(teamId);
            return Ok();
        }

        [HttpPost("qualifying/{teamId}")]
        public async Task<IActionResult> Qualifying(int teamId)
        {
            _logger.LogInformation("Processing Qualifying");
            await _engineeringService.ProcessQualifying(teamId);
            return Ok();
        }

        [HttpPost("race/{teamId}")]
        public async Task<IActionResult> Race(int teamId)
        {
            _logger.LogInformation("Processing Race");
            await _engineeringService.ProcessRace(teamId);
            return Ok();
        }

        //informações pós evolução

        [HttpGet("cars")]
        public async Task<IActionResult> GetCarsWithStatus()
        {
            try
            {
                _logger.LogInformation("Loading the vehicle list.");
                var cars = await _engineeringService.GetAllCarsWithStatus();
                return Ok(cars);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while loading the vehicle list." + ex.Message);
                return Problem(ex.Message);
            }
        }

        [HttpGet("driver/handicaps")]
        public async Task<IActionResult> GetAllDriversHandicaps()
        {
            try
            {
                _logger.LogInformation("Loading the drivers list.");
                var drivers = await _engineeringService.GetAllDriversHandicaps();
                return Ok(drivers);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while loading the drivers list." + ex.Message);
                return Problem(ex.Message);
            }
        }

        [HttpGet("drivers/qualification")]
        public async Task<IActionResult> GetQualificationsPds()
        {
            try
            {
                _logger.LogInformation("Loading the drivers list with theris qualifications.");
                var drivers = await _engineeringService.GetQualificationsPds();
                return Ok(drivers);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while loading the drivers list." + ex.Message);
                return Problem(ex.Message);
            }
        }
        [HttpGet("pd")]
        public async Task<IActionResult> GetDriversRacePd()
        {
            try
            {
                _logger.LogInformation("Loading the drivers list with theris pds.");
                var drivers = await _engineeringService.GetDriversRacePd();
                return Ok(drivers);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while loading the drivers list." + ex.Message);
                return Problem(ex.Message);
            }
        }

        
    }
}
