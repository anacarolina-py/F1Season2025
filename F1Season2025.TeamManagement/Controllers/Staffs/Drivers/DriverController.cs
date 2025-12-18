using F1Season2025.TeamManagement.Services.Staffs.Drivers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace F1Season2025.TeamManagement.Controllers.Staffs.Drivers;

[Route("api/[controller]")]
[ApiController]
public class DriverController : ControllerBase
{
    private readonly IDriverService _driverService;
    private readonly ILogger _logger;

    public DriverController(IDriverService driverService, ILogger<DriverController> logger)
    {
        _driverService = driverService;
        _logger = logger;
    }

    [HttpGet("heartbeat")]
    public ActionResult GetHeartBeat()
    {
        return Ok("Driver is Ok");
    }

}
