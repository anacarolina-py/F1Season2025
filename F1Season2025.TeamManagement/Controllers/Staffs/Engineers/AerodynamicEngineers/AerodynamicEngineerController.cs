using Domain.TeamManagement.Models.DTOs.Staffs.Engineers.AerodynamicEngineers;
using F1Season2025.TeamManagement.Services.Staffs.Engineers.AerodynamicEngineers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace F1Season2025.TeamManagement.Controllers.Staffs.Engineers.AerodynamicEngineers;

[Route("api/[controller]")]
[ApiController]
public class AerodynamicEngineerController : ControllerBase
{
    private readonly IAerodynamicEngineerService _aerodynamicEngineerService;
    private readonly ILogger _logger;

    public AerodynamicEngineerController(IAerodynamicEngineerService aerodynamicEngineerService, ILogger<AerodynamicEngineerController> logger)
    {
        _aerodynamicEngineerService = aerodynamicEngineerService;
        _logger = logger;
    }

    [HttpGet("heartbeat")]
    public ActionResult GetHeartBeat()
    {
        return Ok("AerodynamicEngineer is Ok");
    }

    [HttpPost]
    public async Task<ActionResult> CreateAerodynamicEngineerAsync([FromBody] AerodynamicEngineerRequestDTO aerodynamicEngineerDTO)
    {
        try
        {
            _logger.LogInformation("Creating a new aerodynamic engineer");
            await _aerodynamicEngineerService.CreateAerodynamicEngineerAsync(aerodynamicEngineerDTO);
            return Created();
        }catch(SqlException ex)
        {
            _logger.LogError($"Database error creating aerodynamic engineer: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (ArgumentException ex)
        {
            _logger.LogError($"Error creating aerodynamic engineer: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError($"Error creating aerodynamic engineer: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating aerodynamic engineer: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("aerodynamicengineerids/{aerodynamicEngineerId}")]
    public async Task<ActionResult<AerodynamicEngineerResponseDTO>> GetAerodynamicEngineerByAerodynamicEngineerIdAsync(int aerodynamicEngineerId)
    {
        try
        {
            _logger.LogInformation("Searching for aerodynamic engineer");
            var aerodynamicEngineer = await _aerodynamicEngineerService.GetAerodynamicEngineerByAerodynamicEngineerIdAsync(aerodynamicEngineerId);
            if (aerodynamicEngineer is null)
            {
                _logger.LogWarning("Aerodynamic engineer not found");
                return NotFound("Aerodynamic engineer not found");
            }
            return Ok(aerodynamicEngineer);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving aerodynamic engineer: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("engineerids/{engineerId}")]
    public async Task<ActionResult<AerodynamicEngineerResponseDTO>> GetAerodynamicEngineerByEngineerIdAsync(int engineerId)
    {
        try
        {
            _logger.LogInformation("Searching for aerodynamic engineer by engineer ID");
            var aerodynamicEngineer = await _aerodynamicEngineerService.GetAerodynamicEngineerByEngineerIdAsync(engineerId);
            if (aerodynamicEngineer is null)
            {
                _logger.LogWarning("Aerodynamic engineer not found");
                return NotFound("Aerodynamic engineer not found");
            }
            return Ok(aerodynamicEngineer);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving aerodynamic engineer: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("staffids/{staffId}")]
    public async Task<ActionResult<AerodynamicEngineerResponseDTO>> GetAerodynamicEngineerByStaffIdAsync(int staffId)
    {
        try
        {
            _logger.LogInformation("Searching for aerodynamic engineer by staff ID");
            var aerodynamicEngineer = await _aerodynamicEngineerService.GetAerodynamicEngineerByStaffIdAsync(staffId);
            if (aerodynamicEngineer is null)
            {
                _logger.LogWarning("Aerodynamic engineer not found");
                return NotFound("Aerodynamic engineer not found");
            }
            return Ok(aerodynamicEngineer);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving aerodynamic engineer: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("actives")]
    public async Task<ActionResult<List<AerodynamicEngineerResponseDTO>>> GetAllActiveAerodynamicEngineersAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving all active aerodynamic engineers");
            var aerodynamicEngineers = await _aerodynamicEngineerService.GetActiveAerodynamicEngineersAsync();

            if(aerodynamicEngineers.Count is 0)
            {
                _logger.LogWarning("No active aerodynamic engineers found");
                return NotFound("No active aerodynamic engineers found");
            }

            return Ok(aerodynamicEngineers);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving active aerodynamic engineers: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("inactives")]
    public async Task<ActionResult<List<AerodynamicEngineerResponseDTO>>> GetAllInactiveAerodynamicEngineersAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving all inactive aerodynamic engineers");
            var aerodynamicEngineers = await _aerodynamicEngineerService.GetInactiveAerodynamicEngineersAsync();

            if (aerodynamicEngineers.Count is 0)
            {
                _logger.LogWarning("No inactive aerodynamic engineers found");
                return NotFound("No inactive aerodynamic engineers found");
            }

            return Ok(aerodynamicEngineers);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving inactive aerodynamic engineers: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<AerodynamicEngineerResponseDTO>>> GetAllAerodynamicEngineersAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving all aerodynamic engineers");
            var aerodynamicEngineers = await _aerodynamicEngineerService.GetAllAerodynamicEngineersAsync();

            if (aerodynamicEngineers.Count is 0)
            {
                _logger.LogWarning("No aerodynamic engineers found");
                return NotFound("No aerodynamic engineers found");
            }

            return Ok(aerodynamicEngineers);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving aerodynamic engineers: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("ids/{aerodynamicEngineerId}")]
    public async Task<ActionResult> ChangeAerodynamicEngineerStatusByAerodynamicEngineerIdAsync(int aerodynamicEngineerId)
    {
        try
        {
            _logger.LogInformation("Changing aerodynamic engineer status");
            await _aerodynamicEngineerService.ChangeAerodynamicEngineerStatusByAerodynamicEngineerIdAsync(aerodynamicEngineerId);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            _logger.LogError($"Error changing aerodynamic engineer status: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError($"Error changing aerodynamic engineer status: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error changing aerodynamic engineer status: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
}
