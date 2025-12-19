using Dapper;
using Domain.TeamManagement.Models.DTOs.Cars;
using Domain.TeamManagement.Models.Entities;
using F1Season2025.TeamManagement.Repositories.Cars.Interfaces;
using Infrastructure.TeamManagement.Data.SQL.Connection;
using Microsoft.Data.SqlClient;

namespace F1Season2025.TeamManagement.Repositories.Cars;

public class CarRepository : ICarRepository
{
    private readonly SqlConnection _connection;
    private readonly ILogger _logger;

    public CarRepository(ConnectionDB connection, ILogger<CarRepository> logger)
    {
        _connection = connection.GetConnection();
        _logger = logger;
    }

    public async Task CreateCarAsync(Car car)
    {
        var sqlInsertCar = @"INSERT INTO Cars(Model, AerodynamicCoefficient, PowerCoefficient, Weight, Status)
                             VALUES (@Model, @AerodynamicCoefficient, @PowerCoefficient, @Weight, @Status)";

        try
        {
            _logger.LogInformation("Creating a new car in the database.");
            await _connection.ExecuteAsync(sqlInsertCar, new
            {
                Model = car.Model,
                AerodynamicCoefficient = car.AerodynamicCoefficient,
                PowerCoefficient = car.PowerCoefficient,
                Weight = car.Weight,
                Status = car.Status
            });
        }
        catch (SqlException sqlEx)
        {
            _logger.LogError(sqlEx, "SQL error occurred while creating a new car.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a new car.");
            throw;
        }
    }

    public async Task<List<CarResponseDTO>> GetCarsByModelAsync(string carModel)
    {
        var sqlSelectCarsByModel = @"SELECT CarId, Model, AerodynamicCoefficient, PowerCoefficient, Weight, Status
                                     FROM Cars
                                     WHERE Model = @Model
                                     ORDER BY Model ASC";

        try
        {
            _logger.LogInformation("Retrieving cars ordered by model from the database.");
            return (await _connection.QueryAsync<CarResponseDTO>(sqlSelectCarsByModel, new { Model = carModel })).ToList();
        }
        catch (SqlException sqlEx)
        {
            _logger.LogError(sqlEx, "SQL error occurred while retrieving cars by model.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving cars by model.");
            throw;
        }
    }

    public async Task<List<CarResponseDTO>> GetAllCarsAsync()
    {
        var sqlSelectCarsByModel = @"SELECT CarId, Model, AerodynamicCoefficient, PowerCoefficient, Weight, Status
                                     FROM Cars
                                     ORDER BY Model ASC,CarId ASC";

        try
        {
            _logger.LogInformation("Retrieving all cars ordered by model from the database.");
            return (await _connection.QueryAsync<CarResponseDTO>(sqlSelectCarsByModel)).ToList();
        }
        catch (SqlException sqlEx)
        {
            _logger.LogError(sqlEx, "SQL error occurred while retrieving all cars.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all cars.");
            throw;
        }
    }
    public async Task<CarResponseDTO?> GetCarByIdAsync(int carId)
    {
        var sqlSelectCarbyId = @"SELECT CarId, Model, AerodynamicCoefficient, PowerCoefficient, Weight, Status
                                     FROM Cars
                                     WHERE CarId = @CarId";

        try
        {
            _logger.LogInformation("Retrieving car by Id from the database.");
            return await _connection.QueryFirstOrDefaultAsync<CarResponseDTO>(sqlSelectCarbyId, new {CarId = carId});
        }
        catch (SqlException sqlEx)
        {
            _logger.LogError(sqlEx, "SQL error occurred while retrieving car by Id.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving car by Id.");
            throw;
        }
    }

    public async Task<List<CarResponseDTO>> GetActiveCarsAsync()
    {
        var sqlSelectAllActiveCars = @"SELECT CarId, Model, AerodynamicCoefficient, PowerCoefficient, Weight, Status
                                     FROM Cars
                                     WHERE Status = 'Ativo'
                                     ORDER BY Model ASC,CarId ASC";

        try
        {
            _logger.LogInformation("Retrieving all active cars ordered by model from the database.");
            return (await _connection.QueryAsync<CarResponseDTO>(sqlSelectAllActiveCars)).ToList();
        }
        catch (SqlException sqlEx)
        {
            _logger.LogError(sqlEx, "SQL error occurred while retrieving all active cars.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all active cars.");
            throw;
        }
    }
    public async Task<List<CarResponseDTO>> GetInactiveCarsAsync()
    {
        var sqlSelectAllInactiveCars = @"SELECT CarId, Model, AerodynamicCoefficient, PowerCoefficient, Weight, Status
                                     FROM Cars
                                     WHERE Status = 'Inativo'
                                     ORDER BY Model ASC,CarId ASC";

        try
        {
            _logger.LogInformation("Retrieving all inactive cars ordered by model from the database.");
            return (await _connection.QueryAsync<CarResponseDTO>(sqlSelectAllInactiveCars)).ToList();
        }
        catch (SqlException sqlEx)
        {
            _logger.LogError(sqlEx, "SQL error occurred while retrieving all inactive cars.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all inactive cars.");
            throw;
        }
    }

    public async Task ChangeCarStatusByCarIdAsync(int carId, string newStatus)
    {
        var sqlChangeCarStatus = @"EXEC sp_ChangeCarStatus @CarId,@NewStatus;";
        try
        {
            _logger.LogInformation("Changing car status by Id in the database.");
            await _connection.ExecuteAsync(sqlChangeCarStatus, new
            {
                CarId = carId,
                NewStatus = newStatus
            });
        }
        catch (SqlException sqlEx)
        {
            _logger.LogError(sqlEx, "SQL error occurred while changing car status by Id.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while changing car status by Id.");
            throw;
        }
    }

}
