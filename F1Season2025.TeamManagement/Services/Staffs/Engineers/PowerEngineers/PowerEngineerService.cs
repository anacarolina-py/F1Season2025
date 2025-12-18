using F1Season2025.TeamManagement.Repositories.Staffs.Engineers.PowerEngineers;
using F1Season2025.TeamManagement.Repositories.Staffs.Engineers.PowerEngineers.Interfaces;
using F1Season2025.TeamManagement.Services.Staffs.Engineers.PowerEngineers.Interfaces;

namespace F1Season2025.TeamManagement.Services.Staffs.Engineers.PowerEngineers
{
    public class PowerEngineerService : IPowerEngineerService
    {
        private readonly IPowerEngineerRepository _powerEngineerRepository;
        private readonly ILogger _logger;

        public PowerEngineerService(IPowerEngineerRepository powerEngineerService, ILogger<PowerEngineerService> logger)
        {
            _powerEngineerRepository = powerEngineerService;
            _logger = logger;
        }
    }
}
