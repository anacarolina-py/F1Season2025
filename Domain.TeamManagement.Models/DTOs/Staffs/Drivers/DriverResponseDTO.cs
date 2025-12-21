namespace Domain.TeamManagement.Models.DTOs.Staffs.Drivers;

public class DriverResponseDTO
{
    public int DriverId { get; init; }

    public decimal Handicap { get; init; }

    public decimal PerformancePoints { get; init; }

    public int StaffId { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }

    public int Age { get; init; }

    public decimal Experience { get; init; }

    public string Status { get; init; }

    public int TeamId { get; init; }


}
