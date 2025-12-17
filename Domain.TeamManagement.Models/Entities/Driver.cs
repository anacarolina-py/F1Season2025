using Domain.TeamManagement.Models.Entities.Abstracts;

namespace Domain.TeamManagement.Models.Entities
{
    public class Driver : AStaff
    {
        public int DriverId { get; private set; }

        public decimal PerformancePoints { get; private set; }

        public decimal Handicap { get; private set; }

    }
}
