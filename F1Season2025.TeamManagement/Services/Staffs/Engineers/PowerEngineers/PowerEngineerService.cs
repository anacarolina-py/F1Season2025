using Domain.TeamManagement.Models.DTOs.Staffs.Engineers.PowerEngineers;
using Domain.TeamManagement.Models.Entities;
using F1Season2025.TeamManagement.Repositories.Staffs.Engineers.PowerEngineers.Interfaces;
using F1Season2025.TeamManagement.Services.Staffs.Engineers.PowerEngineers.Interfaces;
using Microsoft.Data.SqlClient;

namespace F1Season2025.TeamManagement.Services.Staffs.Engineers.PowerEngineers
{
    public class PowerEngineerService : IPowerEngineerService
    {
        private readonly IPowerEngineerRepository _powerEngineerRepository;
        private readonly ILogger _logger;

        public PowerEngineerService(IPowerEngineerRepository powerEngineerService, ILogger<PowerEngineerService> logger)
        {
            _powerEngineerRepository = powerEngineerService;
            _logger = logger;
        }

        public async Task CreatePowerEngineerAsync(PowerEngineerRequestDTO powerEngineerDTO)
        {
            #region Validation
            if (string.IsNullOrEmpty(powerEngineerDTO.FirstName))
            {
                _logger.LogWarning("Attempted to create an power engineer with an empty first name.");
                throw new ArgumentException("First name cannot be null or empty.", nameof(powerEngineerDTO.FirstName));
            }
            if (powerEngineerDTO.FirstName.Length < 3 || powerEngineerDTO.FirstName.Length > 255)
            {
                _logger.LogWarning("Attempted to create an power engineer with an invalid first name length: {Length}.", powerEngineerDTO.FirstName.Length);
                throw new ArgumentException("First name must be between 3 and 255 characters long.", nameof(powerEngineerDTO.FirstName));
            }
            if (powerEngineerDTO.LastName.Length < 3 || powerEngineerDTO.LastName.Length > 255)
            {
                _logger.LogWarning("Attempted to create an power engineer with an invalid last name length: {Length}.", powerEngineerDTO.LastName.Length);
                throw new ArgumentException("Last name must be between 3 and 255 characters long.", nameof(powerEngineerDTO.LastName));
            }
            if (string.IsNullOrEmpty(powerEngineerDTO.LastName))
            {
                _logger.LogWarning("Attempted to create an power engineer with an empty last name.");
                throw new ArgumentException("Last name cannot be null or empty.", nameof(powerEngineerDTO.LastName));
            }
            if (powerEngineerDTO.FirstName.Any(char.IsDigit) || powerEngineerDTO.LastName.Any(char.IsDigit))
            {
                _logger.LogWarning("Validation failed: Name contains digits.");
                throw new ArgumentException("Names cannot contain digits.");
            }
            if (powerEngineerDTO.Age < 17 || powerEngineerDTO.Age > 120)
            {
                _logger.LogWarning("Attempted to create an power engineer with an invalid age: {Age}.", powerEngineerDTO.Age);
                throw new ArgumentOutOfRangeException(nameof(powerEngineerDTO.Age), "Age must be between 17 and 120.");
            }
            #endregion

            try
            {
                _logger.LogInformation("Creating a new power engineer: {FirstName} {LastName}, Age: {Age}.", powerEngineerDTO.FirstName, powerEngineerDTO.LastName, powerEngineerDTO.Age);

                var newPowerEngineer = new PowerEngineer(powerEngineerDTO.FirstName, powerEngineerDTO.LastName, powerEngineerDTO.Age);
                await _powerEngineerRepository.CreatePowerEngineerAsync(newPowerEngineer);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "A SQL error occurred while creating a new power engineer.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new power engineer.");
                throw;
            }
        }

        public async Task<List<PowerEngineerResponseDTO>> GetActivePowerEngineersAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving active power engineers.");
                return await _powerEngineerRepository.GetActivePowerEngineersAsync();
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "A SQL error occurred while retrieving active power engineers.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving active power engineers.");
                throw;
            }
        }

        public async Task<PowerEngineerResponseDTO?> GetPowerEngineerByPowerEngineerIdAsync(int powerEngineerId)
        {
            try
            {
                _logger.LogInformation("Retrieving power engineer with ID: {PowerEngineerId}.", powerEngineerId);
                return await _powerEngineerRepository.GetPowerEngineerByPowerEngineerIdAsync(powerEngineerId);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "A SQL error occurred while retrieving power engineer with ID: {PowerEngineerId}.", powerEngineerId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving power engineer with ID: {PowerEngineerId}.", powerEngineerId);
                throw;
            }
        }

        public async Task<PowerEngineerResponseDTO?> GetPowerEngineerByEngineerIdAsync(int engineerId)
        {
            try
            {
                _logger.LogInformation("Retrieving power engineer with Engineer ID: {EngineerId}.", engineerId);
                return await _powerEngineerRepository.GetPowerEngineerByEngineerIdAsync(engineerId);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "A SQL error occurred while retrieving power engineer with Engineer ID: {EngineerId}.", engineerId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving power engineer with Engineer ID: {EngineerId}.", engineerId);
                throw;
            }
        }

        public async Task<PowerEngineerResponseDTO?> GetPowerEngineerByStaffIdAsync(int staffId)
        {
            try
            {
                _logger.LogInformation("Retrieving power engineer with Staff ID: {StaffId}.", staffId);
                return await _powerEngineerRepository.GetPowerEngineerByStaffIdAsync(staffId);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "A SQL error occurred while retrieving power engineer with Staff ID: {StaffId}.", staffId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving power engineer with Staff ID: {StaffId}.", staffId);
                throw;
            }
        }

        public async Task<List<PowerEngineerResponseDTO>> GetAllPowerEngineersAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving all power engineers.");
                return await _powerEngineerRepository.GetAllPowerEngineersAsync();
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "A SQL error occurred while retrieving all power engineers.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all power engineers.");
                throw;
            }
        }

        public async Task<List<PowerEngineerResponseDTO>> GetInactivePowerEngineersAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving inactive power engineers.");
                return await _powerEngineerRepository.GetInactivePowerEngineersAsync();
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "A SQL error occurred while retrieving inactive power engineers.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving inactive power engineers.");
                throw;
            }
        }
    }
}
