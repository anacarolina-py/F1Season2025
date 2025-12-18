using Domain.TeamManagement.Models.Entities.Abstracts;

namespace Domain.TeamManagement.Models.Entities;

public class Driver : AStaff
{
    public int DriverId { get; private set; }

    public decimal PerformancePoints { get; private set; }

    public decimal Handicap { get; private set; }

    public Driver(string firstName, string lastName, int age, decimal performancePoints, decimal handicap)
        : base(firstName, lastName, age)
    {
        PerformancePoints = performancePoints;
        Handicap = handicap;
    }

}
