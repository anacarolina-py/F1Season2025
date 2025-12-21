using Domain.TeamManagement.Models.DTOs.Staffs.Drivers;
using F1Season2025.TeamManagement.Services.Staffs.Drivers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace F1Season2025.TeamManagement.Controllers.Staffs.Drivers;

[Route("api/[controller]")]
[ApiController]
public class DriverController : ControllerBase
{
    private readonly IDriverService _driverService;
    private readonly ILogger _logger;

    public DriverController(IDriverService driverService, ILogger<DriverController> logger)
    {
        _driverService = driverService;
        _logger = logger;
    }

    [HttpGet("heartbeat")]
    public ActionResult GetHeartBeat()
    {
        return Ok("Driver is Ok");
    }

    [HttpPost]
    public async Task<ActionResult> CreateDriverAsync([FromBody] DriverRequestDTO driverDTO)
    {
        try
        {
            _logger.LogInformation("Creating a new driver");
            await _driverService.CreateDriverAsync(driverDTO);
            return Created();
        }
        catch (SqlException ex)
        {
            _logger.LogError($"Database error creating driver: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (ArgumentException ex)
        {
            _logger.LogError($"Error creating driver: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError($"Error creating driver: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating driver: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("driverids/{driverId}")]
    public async Task<ActionResult<DriverResponseDTO>> GetDriverByDriverIdAsync(int driverId)
    {
        try
        {
            _logger.LogInformation("Searching for driver");
            var driver = await _driverService.GetDriverByDriverIdAsync(driverId);
            if (driver is null)
            {
                _logger.LogWarning("Driver with ID {DriverId} not found.", driverId);
                return NotFound($"Driver with ID {driverId} not found.");
            }
            return Ok(driver);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving driver: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("staffids/{staffId}")]
    public async Task<ActionResult<DriverResponseDTO>> GetDriverByStaffIdAsync(int staffId)
    {
        try
        {
            _logger.LogInformation("Searching for driver by staff ID");
            var driver = await _driverService.GetDriverByStaffIdAsync(staffId);
            if (driver is null)
            {
                _logger.LogWarning("Driver with Staff ID {StaffId} not found.", staffId);
                return NotFound($"Driver with Staff ID {staffId} not found.");
            }
            return Ok(driver);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving driver: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("actives")]
    public async Task<ActionResult<List<DriverResponseDTO>>> GetActiveDriversAsync()
    {
        try
        {
            _logger.LogInformation("Searching for active drivers");
            var drivers = await _driverService.GetActiveDriversAsync();
            if(drivers.Count is 0)
            {
                _logger.LogWarning("No active drivers found.");
                return NotFound("No active drivers found.");
            }
            return Ok(drivers);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving active drivers: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("inactives")]
    public async Task<ActionResult<List<DriverResponseDTO>>> GetInactiveDriversAsync()
    {
        try
        {
            _logger.LogInformation("Searching for inactive drivers");
            var drivers = await _driverService.GetInactiveDriversAsync();
            if (drivers.Count is 0)
            {
                _logger.LogWarning("No inactive drivers found.");
                return NotFound("No inactive drivers found.");
            }
            return Ok(drivers);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving inactive drivers: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<DriverResponseDTO>>> GetAllDriversAsync()
    {
        try
        {
            _logger.LogInformation("Searching for all drivers");
            var drivers = await _driverService.GetAllDriversAsync();
            if (drivers.Count is 0)
            {
                _logger.LogWarning("No drivers found.");
                return NotFound("No drivers found.");
            }
            return Ok(drivers);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving all drivers: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("ids/{driverId}")]
    public async Task<ActionResult> ChangeDriverStatusByDriverIdAsync(int driverId)
    {
        try
        {
            _logger.LogInformation("Changing driver status");
            await _driverService.ChangeDriverStatusByDriverIdAsync(driverId);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            _logger.LogError($"Error changing driver status: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError($"Error changing driver status: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error changing driver status: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
}
