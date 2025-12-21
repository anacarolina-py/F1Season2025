using Domain.Engeneering.Models.DTOs;

namespace F1Season2025.Engineering.Services.Interfaces
{
    public interface IEngineeringService
    {
        public Task ProcessPractice(int teamId);
        public Task ProcessQualifying(int teamId);
        public Task ProcessRace(int teamId);
  
        public Task EvolveCar(EngineeringInfoDTO data);
        public Task EvolveDriverHandicap(EngineeringInfoDTO data);

        public Task<IEnumerable<CarStatusDTO>> GetAllCarsWithStatus();
     
        public Task<IEnumerable<DriverHandicapDTO>> GetAllDriversHandicaps();
     
        public Task<IEnumerable<DriverQualificationDTO>> GetQualificationsPds();
     
        public Task<IEnumerable<DriverPdDTO>> GetDriversRacePd();
    }
}
