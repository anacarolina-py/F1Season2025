using System.ComponentModel.DataAnnotations;

namespace Domain.TeamManagement.Models.DTOs.Staffs.Drivers;

public class DriverRequestDTO
{
    [Required(ErrorMessage = "The driver id is required")]
    public int DriverId { get; init; }

    [Required(ErrorMessage = "The driver first name is required")]
    public string FirstName { get; init; }

    [Required(ErrorMessage = "The driver last name is required")]
    public string LastName { get; init; }

    [Required(ErrorMessage = "The driver age is required")]
    public int Age { get; init; }
}
