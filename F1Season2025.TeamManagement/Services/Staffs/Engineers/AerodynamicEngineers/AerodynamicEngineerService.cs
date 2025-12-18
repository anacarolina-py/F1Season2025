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
}
