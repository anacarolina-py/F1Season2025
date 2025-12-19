using Domain.TeamManagement.Models.DTOs.Staffs.Engineers.AerodynamicEngineers;
using F1Season2025.TeamManagement.Repositories.Staffs.Engineers.AerodynamicEngineers.Interfaces;
using F1Season2025.TeamManagement.Services.Staffs.Engineers.AerodynamicEngineers.Interfaces;

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

    public Task CreateAerodynamicEngineerAsync(AerodynamicEngineerRequestDTO aerodynamicEngineerDTO)
    {
        throw new NotImplementedException();
    }

    public Task<List<AerodynamicEngineerResponseDTO>> GetActiveAerodynamicEngineersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<AerodynamicEngineerResponseDTO?> GetAerodynamicEngineerByAerodynamicEngineerIdAsync(int aerodynamicEngineerId)
    {
        throw new NotImplementedException();
    }

    public Task<AerodynamicEngineerResponseDTO?> GetAerodynamicEngineerByEngineerIdAsync(int engineerId)
    {
        throw new NotImplementedException();
    }

    public Task<AerodynamicEngineerResponseDTO?> GetAerodynamicEngineerByStaffIdAsync(int staffId)
    {
        throw new NotImplementedException();
    }

    public Task<List<AerodynamicEngineerResponseDTO>> GetAllAerodynamicEngineersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<AerodynamicEngineerResponseDTO>> GetInactiveAerodynamicEngineersAsync()
    {
        throw new NotImplementedException();
    }
}
