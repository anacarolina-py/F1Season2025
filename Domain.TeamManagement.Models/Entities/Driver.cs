using Domain.TeamManagement.Models.Entities.Abstracts;

namespace Domain.TeamManagement.Models.Entities;

public class Driver : AStaff
{
    public int DriverId { get; private set; }

    public decimal PerformancePoints { get; private set; }

    public decimal Handicap { get; private set; }

    public Driver(int driverId,string firstName, string lastName, int age)
        : base(firstName, lastName, age)
    {
        this.DriverId = driverId;
        this.PerformancePoints = 0;
        this.Handicap = (decimal)new Random().Next(5000,10000) / (decimal)100;
    }

}
