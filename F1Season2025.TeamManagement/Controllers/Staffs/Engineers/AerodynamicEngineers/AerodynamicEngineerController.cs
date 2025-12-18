using F1Season2025.TeamManagement.Services.Staffs.Engineers.AerodynamicEngineers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace F1Season2025.TeamManagement.Controllers.Staffs.Engineers.AerodynamicEngineers;

[Route("api/[controller]")]
[ApiController]
public class AerodynamicEngineerController : ControllerBase
{
    private readonly IAerodynamicEngineerService _aerodynamicEngineerService;
    private readonly ILogger _logger;

    public AerodynamicEngineerController(IAerodynamicEngineerService aerodynamicEngineerService, ILogger<AerodynamicEngineerController> logger)
    {
        _aerodynamicEngineerService = aerodynamicEngineerService;
        _logger = logger;
    }

    [HttpGet("heartbeat")]
    public ActionResult GetHeartBeat()
    {
        return Ok("AerodynamicEngineer is Ok");
    }
}
