using Dapper;
using Domain.TeamManagement.Models.DTOs.Cars;
using Domain.TeamManagement.Models.DTOs.Cars.Relashionships;
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
            return await _connection.QueryFirstOrDefaultAsync<CarResponseDTO>(sqlSelectCarbyId, new { CarId = carId });
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

    public async Task<CarPowerEngineerResponseDTO?> GetPowerEngineerCarRelationshipAsync(int carId, int powerEngineerId)
    {
        var sqlSelectPowerEngineerCarRelashionship = @"SELECT PowerEngineerId,CarId,Status
                                                      FROM CarsPower
                                                      WHERE CarId = @CarId AND 
                                                            PowerEngineerId = @PowerEngineerId";
        try
        {
            _logger.LogInformation("Retrieving power engineer and car relationship from the database.");
            return await _connection.QueryFirstOrDefaultAsync<CarPowerEngineerResponseDTO>(sqlSelectPowerEngineerCarRelashionship, new
            {
                CarId = carId,
                PowerEngineerId = powerEngineerId
            });
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL error occurred while retrieving power engineer and car relationship.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving power engineer and car relationship.");
            throw;
        }
    }

    public async Task<int> GetPowerEngineerCarCountAsync(int carId)
    {
        var sqlCountPowerEngineerCar = @"SELECT COUNT(*)
                                         FROM CarsPower
                                         WHERE Status = 'Ativo' AND 
                                               CarId = @CarId";

        try
        {
            _logger.LogInformation("Counting active power engineers assigned to car with Id: {CarId}", carId);
            return await _connection.ExecuteScalarAsync<int>(sqlCountPowerEngineerCar, new { CarId = carId });
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL error occurred while counting power engineers for car.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while counting power engineers for car.");
            throw;
        }
    }

    public async Task ReactivatePowerEngineerCarRelationshipAsync(int carId, int powerEngineerId)
    {
        var sqlReactivatePowerEngineerCarRelashionship = @"UPDATE CarsPower
                                                          SET Status = 'Ativo'
                                                          WHERE CarId = @CarId AND 
                                                                PowerEngineerId = @PowerEngineerId";

        try
        {
            _logger.LogInformation("Reactivating power engineer and car relationship in the database.");
            await _connection.ExecuteAsync(sqlReactivatePowerEngineerCarRelashionship, new
            {
                CarId = carId,
                PowerEngineerId = powerEngineerId
            });
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL error occurred while reactivating power engineer and car relationship.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while reactivating power engineer and car relationship.");
            throw;
        }
    }

    public async Task AssignPowerEngineerToCarAsync(int carId, int powerEngineerId)
    {
        var sqlInsertPowerEngineerCarRelashionship = @"INSERT INTO CarsPower(CarId, PowerEngineerId, Status)
                                                       VALUES(@CarId, @PowerEngineerId, @Status)";

        try
        {
            _logger.LogInformation("Assigning power engineer to car in the database.");
            await _connection.ExecuteAsync(sqlInsertPowerEngineerCarRelashionship, new
            {
                CarId = carId,
                PowerEngineerId = powerEngineerId,
                Status = "Ativo"
            });

        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL error occurred while assigning power engineer to car.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while assigning power engineer to car.");
            throw;
        }
    }
    public async Task<CarAerodynamicEngineerResponseDTO?> GetAerodynamicEngineerCarRelationshipAsync(int carId, int aerodynamicEngineerId)
    {
        var sqlSelectAerodynamicEngineerCarRelashionship = @"SELECT AerodynamicEngineerId, CarId, Status
                                                      FROM CarsAerodynamic
                                                      WHERE CarId = @CarId AND 
                                                            AerodynamicEngineerId = @AerodynamicEngineerId";
        try
        {
            _logger.LogInformation("Retrieving aerodynamic engineer and car relationship from the database.");
            return await _connection.QueryFirstOrDefaultAsync<CarAerodynamicEngineerResponseDTO>(sqlSelectAerodynamicEngineerCarRelashionship, new
            {
                CarId = carId,
                AerodynamicEngineerId = aerodynamicEngineerId
            });
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL error occurred while retrieving aerodynamic engineer and car relationship.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving aerodynamic engineer and car relationship.");
            throw;
        }
    }

    public async Task<int> GetAerodynamicEngineerCarCountAsync(int carId)
    {
        var sqlCountAerodynamicEngineerCar = @"SELECT COUNT(*)
                                         FROM CarsAerodynamic
                                         WHERE Status = 'Ativo' AND 
                                               CarId = @CarId";

        try
        {
            _logger.LogInformation("Counting active aerodynamic engineers assigned to car with Id: {CarId}", carId);
            return await _connection.ExecuteScalarAsync<int>(sqlCountAerodynamicEngineerCar, new { CarId = carId });
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL error occurred while counting aerodynamic engineers for car.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while counting aerodynamic engineers for car.");
            throw;
        }
    }

    public async Task ReactivateAerodynamicEngineerCarRelationshipAsync(int carId, int aerodynamicEngineerId)
    {
        var sqlReactivateAerodynamicEngineerCarRelashionship = @"UPDATE CarsAerodynamic
                                                          SET Status = 'Ativo'
                                                          WHERE CarId = @CarId AND 
                                                                AerodynamicEngineerId = @AerodynamicEngineerId";

        try
        {
            _logger.LogInformation("Reactivating aerodynamic engineer and car relationship in the database.");
            await _connection.ExecuteAsync(sqlReactivateAerodynamicEngineerCarRelashionship, new
            {
                CarId = carId,
                AerodynamicEngineerId = aerodynamicEngineerId
            });
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL error occurred while reactivating aerodynamic engineer and car relationship.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while reactivating aerodynamic engineer and car relationship.");
            throw;
        }
    }

    public async Task AssignAerodynamicEngineerToCarAsync(int carId, int aerodynamicEngineerId)
    {
        var sqlInsertAerodynamicEngineerCarRelashionship = @"INSERT INTO CarsAerodynamic(CarId, AerodynamicEngineerId, Status)
                                                       VALUES(@CarId, @AerodynamicEngineerId, @Status)";

        try
        {
            _logger.LogInformation("Assigning aerodynamic engineer to car in the database.");
            await _connection.ExecuteAsync(sqlInsertAerodynamicEngineerCarRelashionship, new
            {
                CarId = carId,
                AerodynamicEngineerId = aerodynamicEngineerId,
                Status = "Ativo"
            });

        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL error occurred while assigning aerodynamic engineer to car.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while assigning aerodynamic engineer to car.");
            throw;
        }
    }
}
