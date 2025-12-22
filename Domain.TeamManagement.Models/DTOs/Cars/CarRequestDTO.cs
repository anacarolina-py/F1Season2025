using System.ComponentModel.DataAnnotations;

namespace Domain.TeamManagement.Models.DTOs.Cars;

public class CarRequestDTO
{
    [Required(ErrorMessage = "The car model is required")]
    public string Model { get; init; }

    [Required(ErrorMessage = "The car weight is required")]
    public decimal Weight { get; init; }
}
