namespace Domain.TeamManagement.Models.DTOs.Staffs.Drivers;

public class FullInfoDriverResponseDTO
{
    public int DriverId { get; init; }

    public int StaffId { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }

    public int Age { get; init; }

    public decimal Handicap { get; init; }

    public decimal PerformancePoints { get; init; }

    public decimal Experience { get; init; }
}
