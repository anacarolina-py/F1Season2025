using F1Season2025.TeamManagement.Services.Staffs.Engineers.PowerEngineers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace F1Season2025.TeamManagement.Controllers.Staffs.Engineers.PowerEngineers;

[Route("api/[controller]")]
[ApiController]
public class PowerEngineerController : ControllerBase
{
    private readonly IPowerEngineerService _powerEngineerService;
    private readonly ILogger _logger;

    public PowerEngineerController(IPowerEngineerService powerEngineerService, ILogger<PowerEngineerController> logger)
    {
        _powerEngineerService = powerEngineerService;
        _logger = logger;
    }
}
