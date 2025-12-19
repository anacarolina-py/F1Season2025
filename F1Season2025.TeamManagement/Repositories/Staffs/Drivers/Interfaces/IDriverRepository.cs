using Domain.TeamManagement.Models.DTOs.Staffs.Drivers;
using Domain.TeamManagement.Models.Entities;

namespace F1Season2025.TeamManagement.Repositories.Staffs.Drivers.Interfaces;

public interface IDriverRepository
{
    Task CreateDriverAsync(Driver driver);
    Task<DriverResponseDTO?> GetDriverByDriverIdAsync(int driverId);
    Task<DriverResponseDTO?> GetDriverByStaffIdAsync(int staffId);
    Task<List<DriverResponseDTO>> GetAllDriversAsync();
    Task<List<DriverResponseDTO>> GetActiveDriversAsync();
    Task<List<DriverResponseDTO>> GetInactiveDriversAsync();
}
