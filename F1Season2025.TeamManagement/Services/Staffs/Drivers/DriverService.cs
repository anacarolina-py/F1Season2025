using F1Season2025.TeamManagement.Repositories.Staffs.Drivers.Interfaces;
using F1Season2025.TeamManagement.Services.Staffs.Drivers.Interfaces;

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
}
