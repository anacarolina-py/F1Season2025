using Domain.TeamManagement.Models.DTOs.Staffs.Engineers.PowerEngineers;
using Domain.TeamManagement.Models.Entities.Abstracts;
using F1Season2025.TeamManagement.Services.Staffs.Engineers.PowerEngineers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace F1Season2025.TeamManagement.Controllers.Staffs.Engineers.PowerEngineers;

[Route("api/[controller]")]
[ApiController]
public class PowerEngineerController : ControllerBase
{
    private readonly IPowerEngineerService _powerEngineerService;
    private readonly ILogger _logger;

    public PowerEngineerController(IPowerEngineerService powerEngineerService, ILogger<PowerEngineerController> logger)
    {
        _powerEngineerService = powerEngineerService;
        _logger = logger;
    }

    [HttpGet("heartbeat")]
    public ActionResult GetHeartBeat()
    {
        return Ok("PowerEngineer is Ok");
    }

    [HttpPost]
    public async Task<ActionResult> CreatePowerEngineerAsync([FromBody] PowerEngineerRequestDTO powerEngineerDTO)
    {
        try
        {
            _logger.LogInformation("Creating a new power engineer");
            await _powerEngineerService.CreatePowerEngineerAsync(powerEngineerDTO);
            return Created();
        }
        catch (SqlException ex)
        {
            _logger.LogError($"Database error creating power engineer: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (ArgumentException ex)
        {
            _logger.LogError($"Error creating power engineer: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError($"Error creating power engineer: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating power engineer: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("powerengineerids/{powerEngineerId}")]
    public async Task<ActionResult<PowerEngineerResponseDTO>> GetPowerEngineerByPowerEngineerIdAsync(int powerEngineerId)
    {
        try
        {
            _logger.LogInformation($"Retrieving power engineer with ID: {powerEngineerId}");
            var powerEngineer = await _powerEngineerService.GetPowerEngineerByPowerEngineerIdAsync(powerEngineerId);

            if (powerEngineer is null)
            {
                _logger.LogWarning($"Power engineer with ID: {powerEngineerId} not found");
                return NotFound("Power engineer not found");
            }

            return Ok(powerEngineer);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving power engineer: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("engineerids/{engineerId}")]
    public async Task<ActionResult<PowerEngineerResponseDTO>> GetPowerEngineerByEngineerIdAsync(int engineerId)
    {
        try
        {
            _logger.LogInformation($"Retrieving power engineer with Engineer ID: {engineerId}");
            var powerEngineer = await _powerEngineerService.GetPowerEngineerByEngineerIdAsync(engineerId);

            if (powerEngineer is null)
            {
                _logger.LogWarning($"Power engineer with Engineer ID: {engineerId} not found");
                return NotFound("Power engineer not found");
            }

            return Ok(powerEngineer);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving power engineer: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("staffids/{staffId}")]
    public async Task<ActionResult<PowerEngineerResponseDTO>> GetPowerEngineerByStaffIdAsync(int staffId)
    {
        try
        {
            _logger.LogInformation($"Retrieving power engineer with Staff ID: {staffId}");
            var powerEngineer = await _powerEngineerService.GetPowerEngineerByStaffIdAsync(staffId);

            if (powerEngineer is null)
            {
                _logger.LogWarning($"Power engineer with Staff ID: {staffId} not found");
                return NotFound("Power engineer not found");
            }

            return Ok(powerEngineer);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving power engineer: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("actives")]
    public async Task<ActionResult<List<PowerEngineerResponseDTO>>> GetAllActivePowerEngineersAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving all active power engineers");
            var powerEngineers = await _powerEngineerService.GetActivePowerEngineersAsync();

            if(powerEngineers.Count is 0)
            {
                _logger.LogWarning("No active power engineers found");
                return NotFound("No active power engineers found");
            }

            return Ok(powerEngineers);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving active power engineers: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("inactives")]
    public async Task<ActionResult<List<PowerEngineerResponseDTO>>> GetAllInactivePowerEngineersAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving all inactive power engineers");
            var powerEngineers = await _powerEngineerService.GetInactivePowerEngineersAsync();

            if (powerEngineers.Count is 0)
            {
                _logger.LogWarning("No inactive power engineers found");
                return NotFound("No inactive power engineers found");
            }

            return Ok(powerEngineers);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving inactive power engineers: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<PowerEngineerResponseDTO>>> GetAllPowerEngineersAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving all power engineers");
            var powerEngineers = await _powerEngineerService.GetAllPowerEngineersAsync();

            if (powerEngineers.Count is 0)
            {
                _logger.LogWarning("No power engineers found");
                return NotFound("No power engineers found");
            }

            return Ok(powerEngineers);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving power engineers: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
}
