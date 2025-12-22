using Domain.TeamManagement.Models.DTOs.Cars;
using Domain.TeamManagement.Models.Entities;
using F1Season2025.TeamManagement.Services.Cars.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace F1Season2025.TeamManagement.Controllers.Cars;

[Route("api/[controller]")]
[ApiController]
public class CarController : ControllerBase
{
    private readonly ICarService _carService;
    private readonly ILogger _logger;

    public CarController(ICarService carService, ILogger<CarController> logger)
    {
        _carService = carService;
        _logger = logger;
    }

    [HttpGet("heartbeat")]
    public ActionResult GetHeartBeat()
    {
        return Ok("Car is Ok");
    }

    [HttpPost]
    public async Task<ActionResult> CreateCarAsync([FromBody] CarRequestDTO carDTO)
    {
        try
        {
            _logger.LogInformation("Creating a new car");
            await _carService.CreateCarAsync(carDTO);
            return Created();
        }
        catch(SqlException ex)
        {
            _logger.LogError($"Error creating car: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch(ArgumentOutOfRangeException ex)
        {
            _logger.LogError($"Error creating car: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch(ArgumentException ex)
        {
            _logger.LogError($"Error creating car: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch(InvalidOperationException ex)
        {
            _logger.LogError($"Error creating car: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch(Exception ex)
        {
            _logger.LogError($"Error creating car: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("models/{carModel}")]
    public async Task<ActionResult<List<CarResponseDTO>>> GetCarsByModelAsync(string carModel)
    {
        try
        {
            _logger.LogInformation("Getting cars by model");

            var cars = await _carService.GetCarsByModelAsync(carModel);

            if(cars.Count is 0)
                return NotFound("Model not found.");

            return Ok(cars);
        }
        catch(SqlException ex)
        {
            _logger.LogError($"Error getting cars by model: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch(ArgumentException ex)
        {
            _logger.LogError($"Error getting cars by model: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch(InvalidOperationException ex)
        {
            _logger.LogError($"Error getting cars by model: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch(Exception ex)
        {
            _logger.LogError($"Error getting cars by model: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("ids/{carId}")]
    public async Task<ActionResult<CarResponseDTO>> GetCarsByIdAsync(int carId)
    {
        try
        {
            _logger.LogInformation("Getting car by id");

            var car = await _carService.GetCarByIdAsync(carId);

            if (car is null)
                return NotFound("Car not found.");

            return Ok(car);
        }
        catch (SqlException ex)
        {
            _logger.LogError($"Error getting car by id: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (ArgumentException ex)
        {
            _logger.LogError($"Error getting car by id: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError($"Error getting car by id: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting car by id: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<CarResponseDTO>>> GetAllCarsAsync()
    {
        try
        {
            _logger.LogInformation("Getting all cars");
            var cars = await _carService.GetAllCarsAsync();

            if (cars.Count is 0)
                return NotFound("No cars found.");

            return Ok(cars);
        }
        catch (SqlException ex)
        {
            _logger.LogError($"Error getting all cars: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError($"Error getting all cars: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting all cars: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("actives")]
    public async Task<ActionResult<List<CarResponseDTO>>> GetActiveCarsAsync()
    {
        try
        {
            _logger.LogInformation("Getting all active cars");
            var cars = await _carService.GetActiveCarsAsync();

            if (cars.Count is 0)
                return NotFound("No cars found.");

            return Ok(cars);
        }
        catch(SqlException ex)
        {
            _logger.LogError($"Error getting active cars: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError($"Error getting active cars: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting active cars: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("inactives")]
    public async Task<ActionResult<List<CarResponseDTO>>> GetInactiveCarsAsync()
    {
        try
        {
            _logger.LogInformation("Getting all inactive cars");
            var cars = await _carService.GetInactiveCarsAsync();

            if (cars.Count is 0)
                return NotFound("No cars found.");

            return Ok(cars);
        }
        catch(SqlException ex)
        {
            _logger.LogError($"Error getting inactive cars: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError($"Error getting inactive cars: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting inactive cars: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("ids/{carId}")]
    public async Task<ActionResult> ChangeCarStatusByIdAsync(int carId) { 
        try
        {
            _logger.LogInformation("Changing car status by id");
            await _carService.ChangeCarStatusByCarIdAsync(carId);
            return NoContent();
        }
        catch (SqlException ex)
        {
            _logger.LogError($"Error changing car status by id: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (ArgumentException ex)
        {
            _logger.LogError($"Error changing car status by id: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError($"Error changing car status by id: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error changing car status by id: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("carids/{carId}/pengineerids/{pengineerId}")]
    public async Task<ActionResult> AssignPowerEngineerToCarAsync(int carId, int pengineerId)
    {
        try
        {
            _logger.LogInformation("Assigning power engineer to car");

            await _carService.AssignPowerEngineerToCarAsync(carId, pengineerId);
            return Created();
        }
        catch (SqlException ex)
        {
            _logger.LogError($"Error assigning power engineer to car: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (ArgumentException ex)
        {
            _logger.LogError($"Error assigning power engineer to car: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError($"Error assigning power engineer to car: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error assigning power engineer to car: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("carids/{carId}/aeroengineerids/{aeroEngineerId}")]
    public async Task<ActionResult> AssignAerodynamicEngineerToCarAsync(int carId, int aeroEngineerId)
    {
        try
        {
            _logger.LogInformation("Assigning aerodynamic engineer to car");

            await _carService.AssignAerodynamicEngineerToCarAsync(carId, aeroEngineerId);
            return Created();
        }
        catch (SqlException ex)
        {
            _logger.LogError($"Error assigning aerodynamic engineer to car: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (ArgumentException ex)
        {
            _logger.LogError($"Error assigning aerodynamic engineer to car: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError($"Error assigning aerodynamic engineer to car: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error assigning aerodynamic engineer to car: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("carids/{carId}/driverids/{driverId}")]
    public async Task<ActionResult> AssignDriverToCarAsync(int carId, int driverId)
    {
        try
        {
            _logger.LogInformation("Assigning driver to car");

            await _carService.AssignDriverToCarAsync(carId, driverId);
            return Created();
        }
        catch (SqlException ex)
        {
            _logger.LogError($"Error assigning driver to car: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (ArgumentException ex)
        {
            _logger.LogError($"Error assigning driver to car: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError($"Error assigning driver to car: {ex.Message}");
            return BadRequest($"{ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error assigning driver to car: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
}