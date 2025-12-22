using System.ComponentModel.DataAnnotations;

namespace Domain.TeamManagement.Models.DTOs.Staffs.Engineers.PowerEngineers;

public class PowerEngineerRequestDTO
{
    [Required(ErrorMessage = "The power engineer first name is required")]
    public string FirstName { get; init; }

    [Required(ErrorMessage = "The power engineer last name is required")]
    public string LastName { get; init; }

    [Required(ErrorMessage = "The power engineer age is required")]
    public int Age { get; init; }
}
