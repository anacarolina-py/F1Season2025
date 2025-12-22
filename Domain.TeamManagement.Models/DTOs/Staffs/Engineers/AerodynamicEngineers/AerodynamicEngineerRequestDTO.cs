using System.ComponentModel.DataAnnotations;

namespace Domain.TeamManagement.Models.DTOs.Staffs.Engineers.AerodynamicEngineers;

public class AerodynamicEngineerRequestDTO
{
    [Required(ErrorMessage = "The aerodynamic engineer first name is required")]
    public string FirstName { get; init; }

    [Required(ErrorMessage = "The aerodynamic engineer last name is required")]
    public string LastName { get; init; }

    [Required(ErrorMessage = "The aerodynamic engineer age is required")]
    public int Age { get; init; }
}
