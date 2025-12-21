using System.ComponentModel.DataAnnotations;

namespace Domain.TeamManagement.Models.DTOs.Staffs.Bosses
{
    public class BossRequestDTO
    {
        [Required(ErrorMessage = "The boss first name is required")]
        public string FirstName { get; init; }

        [Required(ErrorMessage = "The boss last name is required")]
        public string LastName { get; init; }

        [Required(ErrorMessage = "The boss age is required")]
        public int Age { get; init; }
    }
}
