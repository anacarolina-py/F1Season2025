using Domain.TeamManagement.Models.DTOs.Staffs.Engineers.PowerEngineers;
using Domain.TeamManagement.Models.Entities;

namespace F1Season2025.TeamManagement.Repositories.Staffs.Engineers.PowerEngineers.Interfaces;

public interface IPowerEngineerRepository
{
    Task CreatePowerEngineerAsync(PowerEngineer powerEngineer);
    Task<PowerEngineerResponseDTO?> GetPowerEngineerByPowerEngineerIdAsync(int powerEngineerId);
    Task<PowerEngineerResponseDTO?> GetPowerEngineerByEngineerIdAsync(int engineerId);
    Task<PowerEngineerResponseDTO?> GetPowerEngineerByStaffIdAsync(int staffId);
    Task<List<PowerEngineerResponseDTO>> GetAllPowerEngineersAsync();
    Task<List<PowerEngineerResponseDTO>> GetActivePowerEngineersAsync();
    Task<List<PowerEngineerResponseDTO>> GetInactivePowerEngineersAsync();

    Task ChangePowerEngineerStatusByPowerEngineerIdAsync(int powerEngineerId, string newStatus);
}
