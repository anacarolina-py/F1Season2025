namespace Domain.TeamManagement.Models.DTOs.Cars;

public class CarResponseDTO
{
    public int CarId { get; init; }

    public string Model { get; init; }

    public decimal AerodynamicCoefficient { get; init; }

    public decimal PowerCoefficient { get; init; }

    public decimal Weight { get; init; }

    public string Status { get; init; }
}
