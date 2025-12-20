using Domain.TeamManagement.Models.DTOs.Cars;
using Microsoft.AspNetCore.Mvc;

namespace F1Season2025.TeamManagement.Services.Cars.Interfaces;

public interface ICarService
{
    Task CreateCarAsync(CarRequestDTO carDTO);
    Task<List<CarResponseDTO>> GetCarsByModelAsync(string carModel);

    Task<CarResponseDTO?> GetCarByIdAsync(int carId);

    Task<List<CarResponseDTO>> GetAllCarsAsync();

    Task<List<CarResponseDTO>> GetActiveCarsAsync();

    Task<List<CarResponseDTO>> GetInactiveCarsAsync();

    Task ChangeCarStatusByCarIdAsync(int carId);

    Task AssignPowerEngineerToCarAsync(int carId, int powerEngineerId);
}
