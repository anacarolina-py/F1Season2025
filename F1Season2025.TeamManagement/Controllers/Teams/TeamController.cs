using Domain.TeamManagement.Models.DTOs.Teams;
using Domain.TeamManagement.Models.DTOs.Teams.Relashionships;
using F1Season2025.TeamManagement.Services.Teams.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace F1Season2025.TeamManagement.Controllers.Teams;

[Route("api/[controller]")]
[ApiController]
public class TeamController : ControllerBase
{
    private readonly ITeamService _teamService;
    private readonly ILogger _logger;

    public TeamController(ITeamService teamService, ILogger<TeamController> logger)
    {
        _teamService = teamService;
        _logger = logger;
    }

    [HttpGet("heartbeat")]
    public IActionResult GetHeartBeat()
    {
        return Ok("Team is Ok");
    }

    [HttpPost]
    public async Task<ActionResult> CreateTeamAsync([FromBody] TeamRequestDTO teamDTO)
    {
        try
        {
            _logger.LogInformation("Creating a new team");
            await _teamService.CreateTeamAsync(teamDTO);
            return Created();
        }
        catch (ArgumentException ex)
        {
            _logger.LogError($"Error creating team: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError($"Error creating team: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating team: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("id/{teamId}")]
    public async Task<ActionResult<TeamResponseDTO>> GetTeamByIdAsync(int teamId)
    {
        try
        {
            _logger.LogInformation("Searching for team");
            var team = await _teamService.GetTeamByIdAsync(teamId);

            if (team is null)
                return NotFound("Team not found.");

            return Ok(team);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error searching for team: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("name/{teamName}")]
    public async Task<ActionResult<TeamResponseDTO>> GetTeamByNameAsync(string teamName)
    {
        try
        {
            _logger.LogInformation("Searching for team");
            var team = await _teamService.GetTeamByNameAsync(teamName);

            if (team is null)
                return NotFound("Team not found.");

            return Ok(team);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error searching for team: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("actives")]
    public async Task<ActionResult<List<TeamResponseDTO>>> GetActiveTeamsAsync()
    {
        try
        {
            _logger.LogInformation("Searching for active teams");
            var teams = await _teamService.GetActiveTeamsAsync();

            if (teams.Count is 0)
                return NotFound("There are not active teams.");

            return Ok(teams);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error searching for teams: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("inactives")]
    public async Task<ActionResult<List<TeamResponseDTO>>> GetInactiveTeamsAsync()
    {
        try
        {
            _logger.LogInformation("Searching for inactive teams");
            var team = await _teamService.GetInactiveTeamsAsync();

            if (team is null)
                return NotFound("There are not inactive teams");

            return Ok(team);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error searching for teams: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<TeamResponseDTO>>> GetAllTeamsAsync()
    {
        try
        {
            _logger.LogInformation("Searching for teams");
            var team = await _teamService.GetAllTeamsAsync();

            if (team is null)
                return NotFound("There are not teams yet.");

            return Ok(team);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error searching for teams: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("activeperformances")]
    public async Task<ActionResult<List<TeamPerformanceResponseDTO>>> GetActivePerformanceTeamsAsync()
    {
        try
        {
            _logger.LogInformation("Searching for active performance teams");
            var teams = await _teamService.GetActivePerformanceTeamsAsync();

            if (teams.Count is 0)
                return NotFound("There are not active performance teams.");

            return Ok(teams);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error searching for teams: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("preparing/ids/{teamId}")]
    public async Task<ActionResult> PrepareTeamByTeamIdAsync(int teamId)
    {
        try
        {
            _logger.LogInformation("Changing team status");
            await _teamService.PrepareTeamByTeamIdAsync(teamId);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            _logger.LogError($"Error changing team status: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError($"Error changing team status: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error changing team status: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("turnon/ids/{teamId}")]
    public async Task<ActionResult> TurnOnTeamByTeamIdAsync(int teamId)
    {
        try
        {
            _logger.LogInformation("Changing team status");
            await _teamService.TurnOnTeamByTeamIdAsync(teamId);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            _logger.LogError($"Error changing team status: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError($"Error changing team status: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error changing team status: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("turnoff/ids/{teamId}")]
    public async Task<ActionResult> TurnOffTeamByTeamIdAsync(int teamId)
    {
        try
        {
            _logger.LogInformation("Changing team status");
            await _teamService.TurnOffTeamByTeamIdAsync(teamId);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            _logger.LogError($"Error changing team status: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError($"Error changing team status: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error changing team status: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("validate")]
    public async Task<ActionResult<TeamsValidateResponseDTO>> ValidateTeamsAsync()
    {
        try
        {
            _logger.LogInformation("Validating teams");
            var isValid = await _teamService.ValidateTeamsAsync();
            return Ok(isValid);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error validating teams: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("engineeringinfo/{teamId}")]
    public async Task<IActionResult> GetEngineeringInfo(int teamId)
    {
        try
        {
            _logger.LogInformation("Getting information for engineering");

            var result = await _teamService.GetEngineeringInfo(teamId);

            if (result == null || !result.Any())
                return NoContent();

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving engineering info");

            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                message = "Internal server error",
            });
        }
    }

    [HttpPost("teamids/{teamId}/driverids/{driverId}/")]
    public async Task<ActionResult> AssignDriverToTeamAsync(int teamId,int driverId)
    {
        try
        {
            _logger.LogInformation("Assigning driver with DriverId {DriverId} to team with TeamId {TeamId}.", driverId, teamId);
            await _teamService.AssignDriverToTeamAsync(teamId, driverId);
            return Created();
        }
        catch (SqlException ex)
        {
            _logger.LogError($"Database error assigning driver to team: {ex.Message}" );
            return BadRequest($"{ex.Message}");
        }
        catch (ArgumentException ex)
        {
            _logger.LogError($"Error assigning driver to team: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError($"Error assigning driver to team: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError( $"Unexpected error assigning driver to team: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("teamids/{teamId}/carids/{carId}/")]
    public async Task<ActionResult> AssignCarToTeamAsync(int teamId, int carId)
    {
        try
        {
            _logger.LogInformation("Assigning car with CarId {CarId} to team with TeamId {TeamId}.", carId, teamId);
            await _teamService.AssignCarToTeamAsync(teamId, carId);
            return Created();
        }
        catch (SqlException ex)
        {
            _logger.LogError($"Database error assigning car to team: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (ArgumentException ex)
        {
            _logger.LogError($"Error assigning car to team: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError($"Error assigning car to team: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Unexpected error assigning car to team: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("teamids/{teamId}/bossids/{bossId}/")]
    public async Task<ActionResult> AssignBossToTeamAsync(int teamId, int bossId)
    {
        try
        {
            _logger.LogInformation("Assigning boss with BossId {BossId} to team with TeamId {TeamId}.", bossId, teamId);
            await _teamService.AssignBossToTeamAsync(teamId, bossId);
            return Created();
        }
        catch (SqlException ex)
        {
            _logger.LogError($"Database error assigning boss to team: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (ArgumentException ex)
        {
            _logger.LogError($"Error assigning boss to team: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError($"Error assigning boss to team: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Unexpected error assigning boss to team: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("teamids/{teamId}/aeroengineerids/{aerodynamicEngineerId}/")]
    public async Task<ActionResult> AssignAerodynamicEngineerToTeamAsync(int teamId, int aerodynamicEngineerId)
    {
        try
        {
            _logger.LogInformation("Assigning aerodynamic engineer with AerodynamicEngineerId {AerodynamicEngineerId} to team with TeamId {TeamId}.", aerodynamicEngineerId, teamId);
            await _teamService.AssignAerodynamicEngineerToTeamAsync(teamId, aerodynamicEngineerId);
            return Created();
        }
        catch (SqlException ex)
        {
            _logger.LogError($"Database error assigning aerodynamic engineer to team: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (ArgumentException ex)
        {
            _logger.LogError($"Error assigning aerodynamic engineer to team: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError($"Error assigning aerodynamic engineer to team: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Unexpected error assigning aerodynamic engineer to team: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("teamids/{teamId}/pengineerids/{powerEngineerId}/")]
    public async Task<ActionResult> AssignPowerEngineerToTeamAsync(int teamId, int powerEngineerId)
    {
        try
        {
            _logger.LogInformation("Assigning power engineer with PowerEngineerId {PowerEngineerId} to team with TeamId {TeamId}.", powerEngineerId, teamId);
            await _teamService.AssignPowerEngineerToTeamAsync(teamId, powerEngineerId);
            return Created();
        }
        catch (SqlException ex)
        {
            _logger.LogError($"Database error assigning power engineer to team: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (ArgumentException ex)
        {
            _logger.LogError($"Error assigning power engineer to team: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError($"Error assigning power engineer to team: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Unexpected error assigning power engineer to team: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
}

