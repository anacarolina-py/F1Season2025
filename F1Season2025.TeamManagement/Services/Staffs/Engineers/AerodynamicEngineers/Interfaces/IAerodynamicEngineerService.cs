using Domain.TeamManagement.Models.DTOs.Staffs.Engineers.AerodynamicEngineers;
using Microsoft.AspNetCore.Mvc;

namespace F1Season2025.TeamManagement.Services.Staffs.Engineers.AerodynamicEngineers.Interfaces;

public interface IAerodynamicEngineerService
{
    Task CreateAerodynamicEngineerAsync(AerodynamicEngineerRequestDTO aerodynamicEngineerDTO);

    Task<AerodynamicEngineerResponseDTO?> GetAerodynamicEngineerByAerodynamicEngineerIdAsync(int aerodynamicEngineerId);

    Task<AerodynamicEngineerResponseDTO?> GetAerodynamicEngineerByEngineerIdAsync(int engineerId);

    Task<AerodynamicEngineerResponseDTO?> GetAerodynamicEngineerByStaffIdAsync(int staffId);

    Task<List<AerodynamicEngineerResponseDTO>> GetAllAerodynamicEngineersAsync();

    Task<List<AerodynamicEngineerResponseDTO>> GetActiveAerodynamicEngineersAsync();

    Task<List<AerodynamicEngineerResponseDTO>> GetInactiveAerodynamicEngineersAsync();

    Task ChangeAerodynamicEngineerStatusByAerodynamicEngineerIdAsync(int aerodynamicEngineerId);
}
