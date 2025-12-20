using Domain.TeamManagement.Models.DTOs.Staffs.Engineers.AerodynamicEngineers;
using Domain.TeamManagement.Models.Entities;
using F1Season2025.TeamManagement.Repositories.Staffs.Engineers.AerodynamicEngineers.Interfaces;
using F1Season2025.TeamManagement.Services.Staffs.Engineers.AerodynamicEngineers.Interfaces;
using Microsoft.Data.SqlClient;

namespace F1Season2025.TeamManagement.Services.Staffs.Engineers.AerodynamicEngineers;

public class AerodynamicEngineerService : IAerodynamicEngineerService
{
    private readonly IAerodynamicEngineerRepository _aerodynamicEngineerRepository;
    private readonly ILogger _logger;

    public AerodynamicEngineerService(IAerodynamicEngineerRepository aerodynamicEngineerRepository, ILogger<AerodynamicEngineerService> logger)
    {
        _aerodynamicEngineerRepository = aerodynamicEngineerRepository;
        _logger = logger;
    }

    public async Task CreateAerodynamicEngineerAsync(AerodynamicEngineerRequestDTO aerodynamicEngineerDTO)
    {
        #region Validation
        if (string.IsNullOrEmpty(aerodynamicEngineerDTO.FirstName))
        {
            _logger.LogWarning("Attempted to create an aerodynamic engineer with an empty first name.");
            throw new ArgumentException("First name cannot be null or empty.", nameof(aerodynamicEngineerDTO.FirstName));
        }
        if (aerodynamicEngineerDTO.FirstName.Length < 3 || aerodynamicEngineerDTO.FirstName.Length > 255)
        {
            _logger.LogWarning("Attempted to create an aerodynamic engineer with an invalid first name length: {Length}.", aerodynamicEngineerDTO.FirstName.Length);
            throw new ArgumentException("First name must be between 3 and 255 characters long.", nameof(aerodynamicEngineerDTO.FirstName));
        }
        if (aerodynamicEngineerDTO.LastName.Length < 3 || aerodynamicEngineerDTO.LastName.Length > 255)
        {
            _logger.LogWarning("Attempted to create an aerodynamic engineer with an invalid last name length: {Length}.", aerodynamicEngineerDTO.LastName.Length);
            throw new ArgumentException("Last name must be between 3 and 255 characters long.", nameof(aerodynamicEngineerDTO.LastName));
        }
        if (string.IsNullOrEmpty(aerodynamicEngineerDTO.LastName))
        {
            _logger.LogWarning("Attempted to create an aerodynamic engineer with an empty last name.");
            throw new ArgumentException("Last name cannot be null or empty.", nameof(aerodynamicEngineerDTO.LastName));
        }
        if (aerodynamicEngineerDTO.FirstName.Any(char.IsDigit) || aerodynamicEngineerDTO.LastName.Any(char.IsDigit))
        {
            _logger.LogWarning("Validation failed: Name contains digits.");
            throw new ArgumentException("Names cannot contain digits.");
        }
        if (aerodynamicEngineerDTO.Age < 17 || aerodynamicEngineerDTO.Age > 120)
        {
            _logger.LogWarning("Attempted to create an aerodynamic engineer with an invalid age: {Age}.", aerodynamicEngineerDTO.Age);
            throw new ArgumentOutOfRangeException(nameof(aerodynamicEngineerDTO.Age), "Age must be between 17 and 120.");
        }
        #endregion

        try
        {
            _logger.LogInformation("Creating a new aerodynamic engineer: {FirstName} {LastName}, Age: {Age}.", aerodynamicEngineerDTO.FirstName, aerodynamicEngineerDTO.LastName, aerodynamicEngineerDTO.Age);

            var newAerodynamicEngineer = new AerodynamicEngineer(aerodynamicEngineerDTO.FirstName, aerodynamicEngineerDTO.LastName, aerodynamicEngineerDTO.Age);
            await _aerodynamicEngineerRepository.CreateAerodynamicEngineerAsync(newAerodynamicEngineer);
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "A SQL error occurred while creating a new aerodynamic engineer.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a new aerodynamic engineer.");
            throw;
        }
    }

    public async Task<List<AerodynamicEngineerResponseDTO>> GetActiveAerodynamicEngineersAsync()
    {
        try 
        { 
            _logger.LogInformation("Retrieving active aerodynamic engineers.");
            return await _aerodynamicEngineerRepository.GetActiveAerodynamicEngineersAsync();
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "A SQL error occurred while retrieving active aerodynamic engineers.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving active aerodynamic engineers.");
            throw;
        }
    }

    public async Task<AerodynamicEngineerResponseDTO?> GetAerodynamicEngineerByAerodynamicEngineerIdAsync(int aerodynamicEngineerId)
    {
        try 
        { 
            _logger.LogInformation("Retrieving aerodynamic engineer with ID: {AerodynamicEngineerId}.", aerodynamicEngineerId);
            return await _aerodynamicEngineerRepository.GetAerodynamicEngineerByAerodynamicEngineerIdAsync(aerodynamicEngineerId);
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "A SQL error occurred while retrieving aerodynamic engineer with ID: {AerodynamicEngineerId}.", aerodynamicEngineerId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving aerodynamic engineer with ID: {AerodynamicEngineerId}.", aerodynamicEngineerId);
            throw;
        }
    }

    public async Task<AerodynamicEngineerResponseDTO?> GetAerodynamicEngineerByEngineerIdAsync(int engineerId)
    {
        try 
        { 
            _logger.LogInformation("Retrieving aerodynamic engineer with Engineer ID: {EngineerId}.", engineerId);
            return await _aerodynamicEngineerRepository.GetAerodynamicEngineerByEngineerIdAsync(engineerId);
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "A SQL error occurred while retrieving aerodynamic engineer with Engineer ID: {EngineerId}.", engineerId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving aerodynamic engineer with Engineer ID: {EngineerId}.", engineerId);
            throw;
        }
    }

    public async Task<AerodynamicEngineerResponseDTO?> GetAerodynamicEngineerByStaffIdAsync(int staffId)
    {
        try 
        { 
            _logger.LogInformation("Retrieving aerodynamic engineer with Staff ID: {StaffId}.", staffId);
            return await _aerodynamicEngineerRepository.GetAerodynamicEngineerByStaffIdAsync(staffId);
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "A SQL error occurred while retrieving aerodynamic engineer with Staff ID: {StaffId}.", staffId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving aerodynamic engineer with Staff ID: {StaffId}.", staffId);
            throw;
        }
    }

    public async Task<List<AerodynamicEngineerResponseDTO>> GetAllAerodynamicEngineersAsync()
    {
        try 
        { 
            _logger.LogInformation("Retrieving all aerodynamic engineers.");
            return await _aerodynamicEngineerRepository.GetAllAerodynamicEngineersAsync();
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "A SQL error occurred while retrieving all aerodynamic engineers.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all aerodynamic engineers.");
            throw;
        }
    }

    public async Task<List<AerodynamicEngineerResponseDTO>> GetInactiveAerodynamicEngineersAsync()
    {
        try { 
            _logger.LogInformation("Retrieving inactive aerodynamic engineers.");
            return await _aerodynamicEngineerRepository.GetInactiveAerodynamicEngineersAsync();
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "A SQL error occurred while retrieving inactive aerodynamic engineers.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving inactive aerodynamic engineers.");
            throw;
        }
    }
}
