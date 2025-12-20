namespace Domain.TeamManagement.Models.DTOs.Cars;

public class CarPerformanceDTO
{
    public int CarId { get; init; }
    public decimal AerodynamicCoefficient { get; init; }
    public decimal PowerCoefficient { get; init; }

    public int DriverId { get; init; }
    public decimal Handicap { get; init; }
    public decimal DriverExperience { get; init; }
    public decimal Pd { get; init; } 

    public int AerodynamicEngineerId { get; init; }
    public int PowerEngineerId { get; init; }
    public decimal AeroExperience { get; init; }
    public decimal PowerExperience { get; init; }
}
