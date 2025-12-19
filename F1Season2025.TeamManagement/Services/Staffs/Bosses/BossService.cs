using Domain.TeamManagement.Models.DTOs.Staffs.Bosses;
using Domain.TeamManagement.Models.Entities;
using F1Season2025.TeamManagement.Repositories.Staffs.Bosses.Interfaces;
using F1Season2025.TeamManagement.Services.Staffs.Bosses.Interfaces;
using Microsoft.Data.SqlClient;

namespace F1Season2025.TeamManagement.Services.Staffs.Bosses;

public class BossService : IBossService
{
    private readonly IBossRepository _bossRepository;
    private readonly ILogger _logger;

    public BossService(IBossRepository bossRepository, ILogger<BossService> logger)
    {
        _bossRepository = bossRepository;
        _logger = logger;
    }

    public async Task CreateBossAsync(BossRequestDTO bossDTO)
    {
        #region Validation
        if (string.IsNullOrEmpty(bossDTO.FirstName))
        {
            _logger.LogWarning("Attempted to create a boss with an empty first name.");
            throw new ArgumentException("First name cannot be null or empty.", nameof(bossDTO.FirstName));
        }
        if (bossDTO.FirstName.Length < 3 || bossDTO.FirstName.Length > 255) { 
            _logger.LogWarning("Attempted to create a boss with an invalid first name length: {Length}.", bossDTO.FirstName.Length);
            throw new ArgumentException("First name must be between 3 and 255 characters long.", nameof(bossDTO.FirstName));
        }
        if(bossDTO.LastName.Length < 3 || bossDTO.LastName.Length > 255) { 
            _logger.LogWarning("Attempted to create a boss with an invalid last name length: {Length}.", bossDTO.LastName.Length);
            throw new ArgumentException("Last name must be between 3 and 255 characters long.", nameof(bossDTO.LastName));
        }
        if (string.IsNullOrEmpty(bossDTO.LastName))
        {
            _logger.LogWarning("Attempted to create a boss with an empty last name.");
            throw new ArgumentException("Last name cannot be null or empty.", nameof(bossDTO.LastName));
        }
        if(bossDTO.Age < 17 || bossDTO.Age > 120) { 
            _logger.LogWarning("Attempted to create a boss with an invalid age: {Age}.", bossDTO.Age);
            throw new ArgumentOutOfRangeException(nameof(bossDTO.Age), "Age must be between 17 and 120.");
        }
        #endregion

        try
        {  
            _logger.LogInformation("Creating a new boss: {FirstName} {LastName}, Age: {Age}.", bossDTO.FirstName, bossDTO.LastName, bossDTO.Age);

            var newBoss = new Boss(bossDTO.FirstName, bossDTO.LastName, bossDTO.Age);
            await _bossRepository.CreateBossAsync(newBoss);
        }
        catch(SqlException ex) { 
            _logger.LogError(ex, "A SQL error occurred while creating a new boss.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a new boss.");
            throw;
        }
    }

    public async Task<List<BossResponseDTO>> GetActiveBossesAsync()
    {
        try { 
            _logger.LogInformation("Retrieving all active bosses.");
            var activeBosses = await _bossRepository.GetActiveBossesAsync();
            return activeBosses;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving active bosses.");
            throw;
        }
    }

    public async Task<List<BossResponseDTO>> GetAllBossesAsync()
    {
        try { 
            _logger.LogInformation("Retrieving all bosses.");
            var allBosses = await _bossRepository.GetAllBossesAsync();
            return allBosses;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all bosses.");
            throw;
        }
    }

    public async Task<BossResponseDTO?> GetBossByBossIdAsync(int bossId)
    {
        try { 
            _logger.LogInformation("Retrieving boss with ID: {BossId}.", bossId);
            var boss = await _bossRepository.GetBossByBossIdAsync(bossId);
            return boss;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving boss with ID: {BossId}.", bossId);
            throw;
        }
    }

    public async Task<BossResponseDTO?> GetBossByStaffIdAsync(int staffId)
    {
        try { 
            _logger.LogInformation("Retrieving boss with Staff ID: {BossId}.", staffId);
            var boss = await _bossRepository.GetBossByStaffIdAsync(staffId);
            return boss;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving boss with Staff ID: {BossId}.", staffId);
            throw;
        }
    }

    public async Task<List<BossResponseDTO>> GetInactiveBossesAsync()
    {
        try { 
            _logger.LogInformation("Retrieving all inactive bosses.");
            var inactiveBosses = await _bossRepository.GetInactiveBossesAsync();
            return inactiveBosses;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving inactive bosses.");
            throw;
        }
    }
}
