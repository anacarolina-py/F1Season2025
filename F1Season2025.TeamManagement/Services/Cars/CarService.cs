using Domain.TeamManagement.Models.DTOs.Cars;
using Domain.TeamManagement.Models.Entities;
using F1Season2025.TeamManagement.Repositories.Cars.Interfaces;
using F1Season2025.TeamManagement.Services.Cars.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace F1Season2025.TeamManagement.Services.Cars;

public class CarService : ICarService
{
    private readonly ICarRepository _carRepository;
    private readonly ILogger _logger;

    public CarService(ICarRepository carRepository, ILogger<CarService> logger)
    {
        _carRepository = carRepository;
        _logger = logger;
    }

    public async Task CreateCarAsync(CarRequestDTO carDTO)
    {
        #region Validation
        if (string.IsNullOrEmpty(carDTO.Model))
        {
            _logger.LogWarning("Attempted to create a car with an empty model.");
            throw new ArgumentException("Car model cannot be null or empty.", nameof(carDTO.Model));
        }
        if(carDTO.Model.Trim().Length is not 5) { 
            _logger.LogWarning("Attempted to create a car with invalid model length: {Length}", carDTO.Model.Length);
            throw new ArgumentException("Car model must be exactly 5 characters long.", nameof(carDTO.Model));
        }
        
        if (carDTO.Weight < 700 || carDTO.Weight > 1000)
        {
            _logger.LogWarning("Attempted to create a car with invalid weight: {Weight}", carDTO.Weight);
            throw new ArgumentOutOfRangeException(nameof(carDTO.Weight), "Car weight must be between 700 and 1000 kg.");
        }
        #endregion

        try
        {
            _logger.LogInformation("Creating a new car with model: {Model}", carDTO.Model);
            var cars = await _carRepository.GetCarsByModelAsync(carDTO.Model);

            if (cars.Count > 1) { 
                _logger.LogWarning("Multiple cars found with the same model: {Model}", carDTO.Model);
                throw new InvalidOperationException($"Multiple cars found with the model: {carDTO.Model}");
            }

            var newCar = new Car(carDTO.Model, carDTO.Weight);
            await _carRepository.CreateCarAsync(newCar);
        }
        catch(SqlException ex)
        {
            _logger.LogError(ex, "Error occurred while logging car creation for model: {Model}", carDTO.Model);
            throw;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error occurred while logging car creation for model: {Model}", carDTO.Model);
            throw;
        }
    }

    public async Task<List<CarResponseDTO>> GetCarsByModelAsync(string carModel)
    {
        try
        {
            _logger.LogInformation("Retrieving cars with model: {Model}", carModel);
            return await _carRepository.GetCarsByModelAsync(carModel);
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving cars with model: {Model}", carModel);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving cars with model: {Model}", carModel);
            throw;
        }
    }

    public async Task<CarResponseDTO?> GetCarByIdAsync(int carId)
    {
        try
        {
            _logger.LogInformation("Retrieving car with ID: {Id}", carId);
            return await _carRepository.GetCarByIdAsync(carId);
        }
        catch(SqlException ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving car with ID: {Id}", carId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving car with ID: {Id}", carId);
            throw;
        }
    }

    public async Task<List<CarResponseDTO>> GetAllCarsAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving all cars.");
            return await _carRepository.GetAllCarsAsync();
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all cars.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all cars.");
            throw;
        }
    }

    public async Task<List<CarResponseDTO>> GetActiveCarsAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving all active cars.");
            return await _carRepository.GetActiveCarsAsync();
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all active cars.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all active cars.");
            throw;
        }
    }
    
    public async Task<List<CarResponseDTO>> GetInactiveCarsAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving all inactive cars.");
            return await _carRepository.GetInactiveCarsAsync();
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all inactive cars.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all inactive cars.");
            throw;
        }
    }

    public async Task ChangeCarStatusByCarIdAsync(int carId)
    {
        try
        {
            _logger.LogInformation("Changing car status by Id in the database.");
            var car = await GetCarByIdAsync(carId);

            if (car is null)
            {
                _logger.LogWarning("Car with Id {CarId} not found.", carId);
                throw new KeyNotFoundException($"Car with Id {carId} not found.");
            }

            var newStatus = car.Status is "Ativo" ? "Inativo" : "Ativo";

            await _carRepository.ChangeCarStatusByCarIdAsync(carId, newStatus);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while changing car status by Id.");
            throw;
        }
    }

    public async Task AssignPowerEngineerToCarAsync(int carId, int powerEngineerId)
    {
        try
        {
            _logger.LogInformation("Assigning Power Engineer with ID: {PowerEngineerId} to Car with ID: {CarId}", powerEngineerId, carId);

            var relashionship = await _carRepository.GetPowerEngineerCarRelationshipAsync(carId, powerEngineerId);
            if (relashionship is not null)
            {

                if (await _carRepository.GetPowerEngineerCarCountAsync(carId) > 0)
                {
                    _logger.LogInformation("Car with ID: {CarId} already has an assigned Power Engineer.", carId);
                    throw new InvalidOperationException($"Car with ID: {carId} already has an assigned Power Engineer.");
                }

                if (relashionship.Status is "Ativo")
                {
                    _logger.LogInformation("Power Engineer with ID: {PowerEngineerId} is already assigned to Car with ID: {CarId}", powerEngineerId, carId);
                    throw new InvalidOperationException($"Power Engineer with ID: {powerEngineerId} is already assigned to Car with ID: {carId}");
                }

                await _carRepository.ReactivatePowerEngineerCarRelationshipAsync(carId, powerEngineerId);

            }
            else
            {
                await _carRepository.AssignPowerEngineerToCarAsync(carId, powerEngineerId);
            }

        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "Error occurred while assigning Power Engineer with ID: {PowerEngineerId} to Car with ID: {CarId}", powerEngineerId, carId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while assigning Power Engineer with ID: {PowerEngineerId} to Car with ID: {CarId}", powerEngineerId, carId);
            throw;
        }
    }

    public async Task AssignAerodynamicEngineerToCarAsync(int carId, int aerodynamicEngineerId)
    {
        try
        {
            _logger.LogInformation("Assigning Aerodynamic Engineer with ID: {AerodynamicEngineerId} to Car with ID: {CarId}", aerodynamicEngineerId, carId);

            var relashionship = await _carRepository.GetAerodynamicEngineerCarRelationshipAsync(carId, aerodynamicEngineerId);
            if (relashionship is not null)
            {

                if (await _carRepository.GetAerodynamicEngineerCarCountAsync(carId) > 0)
                {
                    _logger.LogInformation("Car with ID: {CarId} already has an assigned Aerodynamic Engineer.", carId);
                    throw new InvalidOperationException($"Car with ID: {carId} already has an assigned Aerodynamic Engineer.");
                }

                if (relashionship.Status is "Ativo")
                {
                    _logger.LogInformation("Aerodynamic Engineer with ID: {AerodynamicEngineerId} is already assigned to Car with ID: {CarId}", aerodynamicEngineerId, carId);
                    throw new InvalidOperationException($"Aerodynamic Engineer with ID: {aerodynamicEngineerId} is already assigned to Car with ID: {carId}");
                }

                await _carRepository.ReactivateAerodynamicEngineerCarRelationshipAsync(carId, aerodynamicEngineerId);

            }
            else 
            { 
                await _carRepository.AssignAerodynamicEngineerToCarAsync(carId, aerodynamicEngineerId);
            }
                
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "Error occurred while assigning Aerodynamic Engineer with ID: {AerodynamicEngineerId} to Car with ID: {CarId}", aerodynamicEngineerId, carId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while assigning Aerodynamic Engineer with ID: {AerodynamicEngineerId} to Car with ID: {CarId}", aerodynamicEngineerId, carId);
            throw;
        }
    }


    public async Task AssignDriverToCarAsync(int carId, int driverId)
    {
        try
        {
            _logger.LogInformation("Assigning Driver with ID: {DriverId} to Car with ID: {CarId}", driverId, carId);

            var relashionship = await _carRepository.GetDriverCarRelationshipAsync(carId, driverId);
            if (relashionship is not null)
            {

                if (await _carRepository.GetDriverCarCountAsync(carId) > 0)
                {
                    _logger.LogInformation("Car with ID: {CarId} already has an assigned Driver.", carId);
                    throw new InvalidOperationException($"Car with ID: {carId} already has an assigned Driver.");
                }

                if (relashionship.Status is "Ativo")
                {
                    _logger.LogInformation("Driver with ID: {DriverId} is already assigned to Car with ID: {CarId}", driverId, carId);
                    throw new InvalidOperationException($"Driver with ID: {driverId} is already assigned to Car with ID: {carId}");
                }

                await _carRepository.ReactivateDriverCarRelationshipAsync(carId, driverId);

            }
            else
            {
                await _carRepository.AssignDriverToCarAsync(carId, driverId);
            }

        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "Error occurred while assigning Driver with ID: {DriverId} to Car with ID: {CarId}", driverId, carId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while assigning Driver with ID: {DriverId} to Car with ID: {CarId}", driverId, carId);
            throw;
        }
    }
}
