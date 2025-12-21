using Domain.Engeneering.Models.DTOs;
using F1Season2025.Engineering.Repositories;
using Infrastructure.Engeneering.Data.Client;
using System;
using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using F1Season2025.Engineering.Services.Interfaces;

namespace F1Season2025.Engineering.Services
{
    public class EngineeringService : IEngineeringService
    {
        private readonly ILogger<EngineeringService> _logger;
        private readonly TeamManagementClient _teamClient;
        private readonly EngineeringRepository _engineeringRepository;


        public EngineeringService(ILogger<EngineeringService> logger, TeamManagementClient teamClient, EngineeringRepository engineeringRepository)
        {
            _logger = logger;
            _teamClient = teamClient;
            _engineeringRepository = engineeringRepository;

        }
        public async Task ProcessPractice(int teamId)
        {
            try
            {
                var cars = await _teamClient.GetEngineeringInfo(teamId)
                    ?? Enumerable.Empty<EngineeringInfoDTO>();

                foreach (var data in cars)
                {
                    await EvolveCar(data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while processing practice" + ex.Message);
            }
        }
        public async Task ProcessQualifying(int teamId)
        {
            try
            {
                var cars = await _teamClient.GetEngineeringInfo(teamId)
                    ?? Enumerable.Empty<EngineeringInfoDTO>();

                foreach (var data in cars)
                {
                    await EvolveCar(data);

                    var pd = CalculatePd(
                    data.AerodynamicCoefficient,
                    data.PowerCoefficient,
                    data.DriverHandicap);

                    await _engineeringRepository.UpdateQualifyingPD(
                        data.DriverId,
                        pd);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while processing qualifying" + ex.Message);
            }
        }
        public async Task ProcessRace(int teamId)
        {
            try
            {
                var info = await _teamClient.GetEngineeringInfo(teamId)
                    ?? Enumerable.Empty<EngineeringInfoDTO>();

                foreach (var data in info)
                {

                    await EvolveCar(data);
                    await EvolveDriverHandicap(data);

                    var pd = CalculatePd(
                        data.AerodynamicCoefficient,
                        data.PowerCoefficient,
                        data.DriverHandicap);

                    await _engineeringRepository.UpdateRacePD(
                        data.DriverId,
                        pd);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while processing race" + ex.Message);
            }
        }



        public async Task EvolveCar(EngineeringInfoDTO data)
        {
            try
            {
                decimal ca = data.AerodynamicCoefficient;

                decimal cp = data.PowerCoefficient;

                if (data.EngineerExperienceCa.HasValue)
                {
                    int randomNumberCa = Random.Shared.Next(-1000, 1001);

                    decimal randomCa = randomNumberCa / 1000m;

                    decimal incrementCa = Math.Round((data.EngineerExperienceCa.Value)
                        * randomCa, 3, MidpointRounding.AwayFromZero);

                    ca += incrementCa;

                    if (ca < 0.000m)
                    {
                        ca = 0.000m;
                    }
                    if (ca > 10.000m)
                    {
                        ca = 10.000m;
                    }

                    if (data.EngineerExperienceCp.HasValue)
                    {
                        int randomNumberCp = Random.Shared.Next(-1000, 1001);

                        decimal randomCp = randomNumberCp / 1000m;

                        decimal incrementCp = Math.Round((data.EngineerExperienceCp.Value)
                            * randomCp, 3, MidpointRounding.AwayFromZero);

                        ca += incrementCp;

                        if (cp < 0.000m)
                        {
                            cp = 0.000m;
                        }
                        if (ca > 10.000m)
                        {
                            ca = 10.000m;
                        }
                    }

                    await _engineeringRepository.UpdateCar(
                         data.CarId,
                         Math.Round(ca, 3),
                         Math.Round(cp, 3)
                         );

                    data.AerodynamicCoefficient = ca;
                    data.PowerCoefficient = cp;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while updating car" + ex.Message);
            }
        }

        public async Task EvolveDriverHandicap(EngineeringInfoDTO data)
        {
            try
            {
                decimal newHandicap = Math.Round(data.DriverHandicap -
                    (data.DriverExperience * 0.5m), 3, MidpointRounding.AwayFromZero);

                if (newHandicap < 0.000m)
                    newHandicap = 0.000m;

                await _engineeringRepository.UpdateHandicap(
                    data.DriverId,
                    newHandicap);

                data.DriverHandicap = newHandicap;

            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while updating handicap" + ex.Message);
            }
        }

        private decimal CalculatePd(decimal ca, decimal cp, decimal handicap)
        {
            int luck = Random.Shared.Next(1, 11);

            decimal pd = (ca * 0.4m) + (cp * 0.4m) - handicap + luck;

            return Math.Round(pd, 3);
        }

        public async Task<IEnumerable<CarStatusDTO>> GetAllCarsWithStatus()
        {
            try
            {
                _logger.LogInformation("Getting all the cars.");
                return await _engineeringRepository.GetAllCarsWithStatus();

            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while retreving the cars" + ex.Message);
                throw;
            }
        }
        public async Task<IEnumerable<DriverHandicapDTO>> GetAllDriversHandicaps()
        {
            try
            {
                _logger.LogInformation("Getting all the drivers.");
                return await _engineeringRepository.GetAllDriversHandicaps();

            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while retrieving the drivers" + ex.Message);
                throw;
            }
        }
        public async Task<IEnumerable<DriverQualificationDTO>> GetQualificationsPds()
        {
            try
            {
                _logger.LogInformation("Getting all the drivers with their qualifications.");
                return await _engineeringRepository.GetQualificationsPds();

            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while retrieving drivers." + ex.Message);
                throw;
            }
        }
        public async Task<IEnumerable<DriverPdDTO>> GetDriversRacePd()
        {
            try
            {
                _logger.LogInformation("Getting all drivers with their pds.");
                return await _engineeringRepository.GetDriversRacePd();

            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while retrieving drivers." + ex.Message);
                throw;
            }
        }

    }
}
