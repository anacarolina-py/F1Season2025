using Domain.TeamManagement.Models.DTOs.Staffs.Drivers;
using Microsoft.AspNetCore.Mvc;

namespace F1Season2025.TeamManagement.Services.Staffs.Drivers.Interfaces;

public interface IDriverService
{
    Task CreateDriverAsync(DriverRequestDTO driverDTO);

    Task<DriverResponseDTO?> GetDriverByDriverIdAsync(int driverId);

    Task<DriverResponseDTO?> GetDriverByStaffIdAsync(int staffId);

    Task<List<DriverResponseDTO>> GetAllDriversAsync();

    Task<List<DriverResponseDTO>> GetActiveDriversAsync();

    Task<List<DriverResponseDTO>> GetInactiveDriversAsync();

    Task ChangeDriverStatusByDriverIdAsync(int driverId);
}
