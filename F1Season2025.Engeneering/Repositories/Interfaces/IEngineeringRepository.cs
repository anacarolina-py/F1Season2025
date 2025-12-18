using Domain.Engeneering.Models.DTOs;
using F1Season2025.Engineering.Data.SQL;

namespace F1Season2025.Engineering.Repositories.Interfaces
{
    public interface IEngineeringRepository
    {
        public Task UpdateCar(int carId, decimal ca, decimal cp);

        public Task UpdateHandicap(int driverId, decimal handicap);

        public Task UpdateQualifyingPD(int driverId, decimal pd);
         
        public Task UpdateRacePD(int driverId, decimal pd);
       
        public Task<IEnumerable<CarStatusDTO>> GetAllCarsWithStatus();
        
        public Task<IEnumerable<DriverHandicapDTO>> GetAllDriversHandicaps();
          

        public Task<IEnumerable<DriverQualificationDTO>> GetQualificationsPds();
       
        public Task<IEnumerable<DriverPdDTO>> GetDriversRacePd();
            
    

}
}
