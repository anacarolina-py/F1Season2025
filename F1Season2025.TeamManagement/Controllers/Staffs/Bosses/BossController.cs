using Domain.TeamManagement.Models.DTOs.Staffs.Bosses;
using F1Season2025.TeamManagement.Services.Staffs.Bosses.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace F1Season2025.TeamManagement.Controllers.Staffs.Bosses
{
    [Route("api/[controller]")]
    [ApiController]
    public class BossController : ControllerBase
    {
        private readonly IBossService _bossService;
        private readonly ILogger _logger;

        public BossController(IBossService bossService, ILogger<BossController> logger)
        {
            _bossService = bossService;
            _logger = logger;
        }

        [HttpGet("heartbeat")]
        public ActionResult GetHeartBeat()
        {
            return Ok("Boss is Ok");
        }

        [HttpPost]
        public async Task<ActionResult> CreateBossAsync([FromBody] BossRequestDTO bossDTO)
        {
            try
            {
                _logger.LogInformation("Creating a new boss");
                await _bossService.CreateBossAsync(bossDTO);
                return Created();
            }
            catch (SqlException ex)
            {
                _logger.LogError($"Database error creating boss: {ex.Message}");
                return BadRequest($"{ex.Message}");
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error creating boss: {ex.Message}");
                return BadRequest($"{ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"Error creating boss: {ex.Message}");
                return BadRequest($"{ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating boss: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("bossids/{bossId}")]
        public async Task<ActionResult<BossResponseDTO>> GetBossByBossIdAsync(int bossId)
        {
            try
            {
                _logger.LogInformation("Searching for boss");
                var boss = await _bossService.GetBossByBossIdAsync(bossId);
                if (boss is null)
                {
                    _logger.LogWarning("Boss with ID {BossId} not found", bossId);
                    return NotFound($"Boss with ID {bossId} not found");
                }
                return Ok(boss);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving boss: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("staffids/{staffId}")]
        public async Task<ActionResult<BossResponseDTO>> GetBossByStaffIdAsync(int staffId)
        {
            try
            {
                _logger.LogInformation("Searching for boss by staff ID");
                var boss = await _bossService.GetBossByStaffIdAsync(staffId);
                if (boss is null)
                {
                    _logger.LogWarning("Boss with Staff ID {StaffId} not found", staffId);
                    return NotFound($"Boss with Staff ID {staffId} not found");
                }
                return Ok(boss);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving boss: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<BossResponseDTO>>> GetAllBossesAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all bosses");
                var bosses = await _bossService.GetAllBossesAsync();

                if (bosses.Count is 0)
                    return NotFound("No bosses found");

                return Ok(bosses);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving bosses: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("actives")]
        public async Task<ActionResult<List<BossResponseDTO>>> GetActiveBossesAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving active bosses");
                var bosses = await _bossService.GetActiveBossesAsync();

                if (bosses.Count is 0)
                    return NotFound("No active bosses found");

                return Ok(bosses);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving active bosses: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpGet("inactives")]
        public async Task<ActionResult<List<BossResponseDTO>>> GetInactiveBossesAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving inactive bosses");
                var bosses = await _bossService.GetInactiveBossesAsync();

                if (bosses.Count is 0)
                    return NotFound("No inactive bosses found");

                return Ok(bosses);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving inactive bosses: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("ids/{bossId}")]
        public async Task<ActionResult> ChangeBossStatusByBossIdAsync(int bossId)
        {
            try
            {
                _logger.LogInformation("Changing boss status");
                await _bossService.ChangeBossStatusByBossIdAsync(bossId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error changing boss status: {ex.Message}");
                return BadRequest($"{ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"Error changing boss status: {ex.Message}");
                return BadRequest($"{ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error changing boss status: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
