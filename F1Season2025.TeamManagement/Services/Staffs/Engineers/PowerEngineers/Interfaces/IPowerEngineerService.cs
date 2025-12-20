using Domain.TeamManagement.Models.DTOs.Staffs.Engineers.PowerEngineers;

namespace F1Season2025.TeamManagement.Services.Staffs.Engineers.PowerEngineers.Interfaces;

public interface IPowerEngineerService
{
    Task CreatePowerEngineerAsync(PowerEngineerRequestDTO powerEngineerDTO);
    Task<PowerEngineerResponseDTO?> GetPowerEngineerByPowerEngineerIdAsync(int powerEngineerId);
    Task<PowerEngineerResponseDTO?> GetPowerEngineerByEngineerIdAsync(int engineerId);
    Task<PowerEngineerResponseDTO?> GetPowerEngineerByStaffIdAsync(int staffId);
    Task<List<PowerEngineerResponseDTO>> GetAllPowerEngineersAsync();
    Task<List<PowerEngineerResponseDTO>> GetActivePowerEngineersAsync();
    Task<List<PowerEngineerResponseDTO>> GetInactivePowerEngineersAsync();
}
