using Domain.TeamManagement.Models.DTOs.Cars;
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
}
