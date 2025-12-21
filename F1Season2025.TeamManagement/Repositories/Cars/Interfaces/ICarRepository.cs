using Domain.TeamManagement.Models.DTOs.Cars;
using Domain.TeamManagement.Models.DTOs.Cars.Relashionships;
using Domain.TeamManagement.Models.Entities;

namespace F1Season2025.TeamManagement.Repositories.Cars.Interfaces;

public interface ICarRepository
{
    Task CreateCarAsync(Car car);
    Task<List<CarResponseDTO>> GetCarsByModelAsync(string carModel);

    Task<List<CarResponseDTO>> GetAllCarsAsync();

    Task<CarResponseDTO?> GetCarByIdAsync(int carId);

    Task<List<CarResponseDTO>> GetActiveCarsAsync();

    Task<List<CarResponseDTO>> GetInactiveCarsAsync();
    
    Task ChangeCarStatusByCarIdAsync(int carId, string newStatus);

    Task<CarPowerEngineerResponseDTO?> GetPowerEngineerCarRelationshipAsync(int carId, int powerEngineerId);

    Task<int> GetPowerEngineerCarCountAsync(int carId);

    Task ReactivatePowerEngineerCarRelationshipAsync(int carId, int powerEngineerId);

    Task AssignPowerEngineerToCarAsync(int carId, int powerEngineerId);
    Task<CarAerodynamicEngineerResponseDTO?> GetAerodynamicEngineerCarRelationshipAsync(int carId, int aerodynamicEngineerId);

    Task<int> GetAerodynamicEngineerCarCountAsync(int carId);

    Task ReactivateAerodynamicEngineerCarRelationshipAsync(int carId, int aerodynamicEngineerId);

    Task AssignAerodynamicEngineerToCarAsync(int carId, int aerodynamicEngineerId);
}
