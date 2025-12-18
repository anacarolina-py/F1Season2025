using Dapper;
using Domain.Engeneering.Models.DTOs;
using Infrastructure.Engeneering.Data.SQL;
using Microsoft.Data.SqlClient;
using RabbitMQ.Client;

namespace F1Season2025.Engineering.Repositories
{
    public class EngineeringRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public EngineeringRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public void UpdateCar(int carId, decimal ca, decimal cp)
        {
            using var connection = _connectionFactory.GetConnection();
            connection.Open();
            var sql = @"IF EXISTS (SELECT 1 FROM Cars WHERE Id = @carId)
                                 BEGIN
                                    UPDATE Cars
                                    SET AerodynamicCoefficient = @ca,
                                        PowerCoefficient = @cp
                                    WHERE Id = @carId
                                 END
                                 ELSE
                                 BEGIN
                                    INSERT INTO Cars (Id, AerodynamicCoefficient, PowerCoefficient)
                                    VALUES (@carId, @ca, @cp)
                                 END";
              
            connection.Execute(sql, new 
            { Id = carId,
              AerodynamicCoefficient = ca,
              PowerCoefficient = cp
            });
        }

        public void UpdateHandicap(int driverId, decimal handicap)
        {
            using var connection = _connectionFactory.GetConnection();
            connection.Open();

            var sql = @"   UPDATE Drivers
                                    SET Handicap = @Handicap,
                                    WHERE Id = @DriverId";
                              

            connection.Execute(sql, new
            {
                Id = driverId,
                Handicap = handicap
            });
        }

        public void UpdateQualifyingPD(int driverId, decimal pd)
        {
            using var connection = _connectionFactory.GetConnection();
            connection.Open();

            var sql = @"IF EXISTS (SELECT 1 FROM Drivers WHERE Id = @DriverId)
                                 BEGIN
                                    UPDATE Drivers
                                    SET QualifyingPd = @Pd,
                                    WHERE Id = @DriverId
                                 END
                                 ELSE
                                 BEGIN
                                    INSERT INTO Driver (Id, QualifyingPd)
                                    VALUES (@DriverId, @Pd)
                                 END";

            connection.Execute(sql, new
            {
                Id = driverId,
                Pd = pd
            });
        }

        public void UpdateRacePD(int driverId, decimal pd)
        {
            using var connection = _connectionFactory.GetConnection();
            connection.Open();

            var sql = @"UPDATE Drivers
                                 SET RacePd = @Pd
                                 WHERE Id = @DriverId";

            connection.Execute(sql, new
            {
                Id = driverId,
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
