namespace Domain.TeamManagement.Models.DTOs.Staffs.Engineers.AerodynamicEngineers;

public class AerodynamicEngineerResponseDTO
{
    public int PowerEngineerId { get; init; }

    public int EngineerId { get; init; }

    public int StaffId { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }

    public int Age { get; init; }

    public decimal Experience { get; init; }

    public string Status { get; init; }
}
