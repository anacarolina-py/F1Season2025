using Domain.TeamManagement.Models.DTOs.Staffs.Engineers.AerodynamicEngineers;
using Domain.TeamManagement.Models.Entities;

namespace F1Season2025.TeamManagement.Repositories.Staffs.Engineers.AerodynamicEngineers.Interfaces;

public interface IAerodynamicEngineerRepository
{
    Task CreateAerodynamicEngineerAsync(AerodynamicEngineer aerodynamicEngineer);
    Task<AerodynamicEngineerResponseDTO?> GetAerodynamicEngineerByAerodynamicEngineerIdAsync(int aerodynamicEngineerId);
    Task<AerodynamicEngineerResponseDTO?> GetAerodynamicEngineerByEngineerIdAsync(int engineerId);
    Task<AerodynamicEngineerResponseDTO?> GetAerodynamicEngineerByStaffIdAsync(int staffId);
    Task<List<AerodynamicEngineerResponseDTO>> GetAllAerodynamicEngineersAsync();
    Task<List<AerodynamicEngineerResponseDTO>> GetActiveAerodynamicEngineersAsync();
    Task<List<AerodynamicEngineerResponseDTO>> GetInactiveAerodynamicEngineersAsync();

    Task ChangeAerodynamicEngineerStatusByAerodynamicEngineerIdAsync(int aerodynamicEngineerId, string newStatus);
}
