using Dapper;
using Domain.Engeneering.Models.DTOs;
using F1Season2025.Engineering.Data.SQL;
using F1Season2025.Engineering.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using RabbitMQ.Client;

namespace F1Season2025.Engineering.Repositories
{
    public class EngineeringRepository : IEngineeringRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public EngineeringRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task UpdateCar(int carId, decimal ca, decimal cp)
        {
            using var connection = _connectionFactory.GetConnection();
            connection.Open();
            var sql = @"IF EXISTS (SELECT 1 FROM Cars WHERE Id = @CarId)
                                 BEGIN
                                    UPDATE Cars
                                    SET AerodynamicCoefficient = @Ca,
                                        PowerCoefficient = @Cp
                                    WHERE Id = @CarId
                                 END
                                 ELSE
                                 BEGIN
                                    INSERT INTO Cars (Id, AerodynamicCoefficient, PowerCoefficient)
                                    VALUES (@CarId, @Ca, @Cp)
                                 END";
              
            await connection.ExecuteAsync(sql, new 
            { CarId = carId,
              Ca = ca,
              Cp = cp
            });
        }

        public async Task UpdateHandicap(int driverId, decimal handicap)
        {
            using var connection = _connectionFactory.GetConnection();
            connection.Open();

            var sql = @"IF EXISTS (SELECT 1 FROM Drivers WHERE Id = @DriverId)
                        BEGIN
                            UPDATE Drivers
                            SET Handicap = @Handicap
                            WHERE Id = @DriverId
                        END
                        ELSE
                        BEGIN
                        INSERT INTO Drivers (Id, Handicap)
                        VALUES (@DriverId, @Handicap)
                        END";
                              

            await connection.ExecuteAsync(sql, new
            {
                DriverId = driverId,
                Handicap = handicap
            });
        }

        public async Task UpdateQualifyingPD(int driverId, decimal pd)
        {
            using var connection = _connectionFactory.GetConnection();
            connection.Open();

            var sql = @"IF EXISTS (SELECT 1 FROM Drivers WHERE Id = @DriverId)
                        BEGIN
                            UPDATE Drivers
                            SET QualifyingPd = @Pd
                            WHERE Id = @DriverId
                        END
                        ELSE
                        BEGIN
                            INSERT INTO Drivers (Id, QualifyingPd)
                            VALUES (@DriverId, @Pd)
                        END";
                                 

            await connection.ExecuteAsync(sql, new
            {
                DriverId = driverId,
                Pd = pd
            });
        }

        public async Task UpdateRacePD(int driverId,decimal pd)
        {
            using var connection = _connectionFactory.GetConnection();
            connection.Open();

            var sql = @"IF EXISTS (SELECT 1 FROM Drivers WHERE Id = @DriverId)
                        BEGIN
                            UPDATE Drivers
                            SET RacePd = @Pd
                            WHERE Id = @DriverId
                        END
                        ELSE
                        BEGIN
                            INSERT INTO Drivers (Id, RacePd)
                            VALUES (@DriverId, @Pd)
                        END";

            await connection.ExecuteAsync(sql, new
            {
                DriverId = driverId,
                Pd = pd
            });
        }

        public async Task<IEnumerable<CarStatusDTO>> GetAllCarsWithStatus()
        {
            using var connection = _connectionFactory.GetConnection();
            connection.Open();

            var sql = @"SELECT Id,
                        AerodynamicCoefficient,
                        PowerCoefficient
                        FROM Cars
                        ORDER BY Id";

            return await connection.QueryAsync<CarStatusDTO>(sql);
        }
       public async Task<IEnumerable<DriverHandicapDTO>> GetAllDriversHandicaps()
        {
            using var connection = _connectionFactory.GetConnection();
            connection.Open();

            var sql = @"SELECT Id,
                        Handicap
                        FROM Drivers
                        ORDER BY Id";

            return await connection.QueryAsync<DriverHandicapDTO>(sql);
        }

        public async Task<IEnumerable<DriverQualificationDTO>> GetQualificationsPds()
        {
            using var connection = _connectionFactory.GetConnection();
            connection.Open();

            var sql = @"SELECT Id, QualifyingPd
                        FROM Drivers
                        WHERE QualifyingPd IS NOT NULL
                        ORDER BY QualifyingPd DESC";

            return await connection.QueryAsync<DriverQualificationDTO>(sql);
        }

        public async Task<IEnumerable<DriverPdDTO>> GetDriversRacePd()
        {
            using var connection = _connectionFactory.GetConnection();
            connection.Open();
            var sql = @"SELECT Id, Racepd
                        FROM Drivers
                        WHERE RacePd IS NOT NULL
                        ORDER BY RacePd DESC";

            return await connection.QueryAsync<DriverPdDTO>(sql);
        }
    }
}
