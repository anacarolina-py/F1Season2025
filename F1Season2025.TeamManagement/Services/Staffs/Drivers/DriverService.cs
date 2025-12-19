using Domain.TeamManagement.Models.DTOs.Staffs.Drivers;
using Domain.TeamManagement.Models.Entities;
using F1Season2025.TeamManagement.Repositories.Staffs.Drivers.Interfaces;
using F1Season2025.TeamManagement.Services.Staffs.Drivers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace F1Season2025.TeamManagement.Services.Staffs.Drivers;

public class DriverService : IDriverService
{
    private readonly IDriverRepository _driverRepository;
    private readonly ILogger _logger;

    public DriverService(IDriverRepository driverRepository, ILogger<DriverService> logger)
    {
        _driverRepository = driverRepository;
        _logger = logger;
    }

    public async Task ChangeDriverStatusByDriverIdAsync(int driverId)
    {
        try 
        { 
            _logger.LogInformation("Changing status for driver with DriverId: {DriverId}.", driverId);

            var driver = await _driverRepository.GetDriverByDriverIdAsync(driverId);

            if(driver is null)
            {
                _logger.LogWarning("No driver found with DriverId {DriverId}. Status change aborted.", driverId);
                throw new InvalidOperationException($"No driver found with DriverId {driverId}.");
            }

            var newStatus = driver.Status is "Ativo" ? "Inativo" : "Ativo";

            await _driverRepository.ChangeDriverStatusByDriverIdAsync(driverId,newStatus);
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL error occurred while changing status for driver with DriverId {DriverId}: {Message}", driverId, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while changing status for driver with DriverId {DriverId}: {Message}", driverId, ex.Message);
            throw;
        }
    }

    public async Task CreateDriverAsync(DriverRequestDTO driverDTO)
    {
        #region Validation
        if(driverDTO.DriverId < 1 || driverDTO.DriverId > 99) { 
            _logger.LogWarning("Attempted to create a driver with an invalid DriverId: {DriverId}.", driverDTO.DriverId);
            throw new ArgumentOutOfRangeException(nameof(driverDTO.DriverId), "DriverId must be between 1 and 99.");
        }
        if (string.IsNullOrEmpty(driverDTO.FirstName))
        {
            _logger.LogWarning("Attempted to create a driver with an empty first name.");
            throw new ArgumentException("First name cannot be null or empty.", nameof(driverDTO.FirstName));
        }
        if (driverDTO.FirstName.Length < 3 || driverDTO.FirstName.Length > 255)
        {
            _logger.LogWarning("Attempted to create a driver with an invalid first name length: {Length}.", driverDTO.FirstName.Length);
            throw new ArgumentException("First name must be between 3 and 255 characters long.", nameof(driverDTO.FirstName));
        }
        if (driverDTO.LastName.Length < 3 || driverDTO.LastName.Length > 255)
        {
            _logger.LogWarning("Attempted to create a driver with an invalid last name length: {Length}.", driverDTO.LastName.Length);
            throw new ArgumentException("Last name must be between 3 and 255 characters long.", nameof(driverDTO.LastName));
        }
        if (string.IsNullOrEmpty(driverDTO.LastName))
        {
            _logger.LogWarning("Attempted to create a driver with an empty last name.");
            throw new ArgumentException("Last name cannot be null or empty.", nameof(driverDTO.LastName));
        }
        if (driverDTO.Age < 17 || driverDTO.Age > 120)
        {
            _logger.LogWarning("Attempted to create a driver with an invalid age: {Age}.", driverDTO.Age);
            throw new ArgumentOutOfRangeException(nameof(driverDTO.Age), "Age must be between 17 and 120.");
        }
        #endregion

        try
        {
            _logger.LogInformation("Creating a new driver: {FirstName} {LastName}, Age: {Age}.", driverDTO.FirstName, driverDTO.LastName, driverDTO.Age);

            var driver = await _driverRepository.GetDriverByDriverIdAsync(driverDTO.DriverId);

            if (driver is not null)
            {
                _logger.LogWarning("A driver with DriverId {DriverId} already exists. Creation aborted.", driverDTO.DriverId);
                throw new InvalidOperationException($"A driver with DriverId {driverDTO.DriverId} already exists.");
            }

            var newDriver = new Driver(driverDTO.DriverId,driverDTO.FirstName, driverDTO.LastName, driverDTO.Age);

            await _driverRepository.CreateDriverAsync(newDriver);
        }
        catch(SqlException ex)
        {
            _logger.LogError(ex, "SQL error occurred while creating a new driver: {Message}", ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a new driver: {Message}", ex.Message);
            throw;
        }
    }

    public async Task<List<DriverResponseDTO>> GetActiveDriversAsync()
    {
        try { 
            _logger.LogInformation("Retrieving all active drivers.");

            return await _driverRepository.GetActiveDriversAsync();
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL error occurred while retrieving active drivers: {Message}", ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving active drivers: {Message}", ex.Message);
            throw;
        }
    }

    public async Task<List<DriverResponseDTO>> GetAllDriversAsync()
    {
        try { 
            _logger.LogInformation("Retrieving all drivers.");
            return await _driverRepository.GetAllDriversAsync();
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL error occurred while retrieving all drivers: {Message}", ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all drivers: {Message}", ex.Message);
            throw;
        }
    }

    public async Task<DriverResponseDTO?> GetDriverByDriverIdAsync(int driverId)
    {
        try { 
            _logger.LogInformation("Retrieving driver with DriverId: {DriverId}.", driverId);
            return await _driverRepository.GetDriverByDriverIdAsync(driverId);
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL error occurred while retrieving driver with DriverId {DriverId}: {Message}", driverId, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving driver with DriverId {DriverId}: {Message}", driverId, ex.Message);
            throw;
        }
    }

    public async Task<DriverResponseDTO?> GetDriverByStaffIdAsync(int staffId)
    {
        try { 
            _logger.LogInformation("Retrieving driver with StaffId: {StaffId}.", staffId);
            return await _driverRepository.GetDriverByStaffIdAsync(staffId);
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL error occurred while retrieving driver with StaffId {StaffId}: {Message}", staffId, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving driver with StaffId {StaffId}: {Message}", staffId, ex.Message);
            throw;
        }
    }

    public async Task<List<DriverResponseDTO>> GetInactiveDriversAsync()
    {
        try { 
            _logger.LogInformation("Retrieving all inactive drivers.");
            return await _driverRepository.GetInactiveDriversAsync();
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL error occurred while retrieving inactive drivers: {Message}", ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving inactive drivers: {Message}", ex.Message);
            throw;
        }
    }
}
