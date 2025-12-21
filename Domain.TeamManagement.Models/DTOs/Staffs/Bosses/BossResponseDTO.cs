namespace Domain.TeamManagement.Models.DTOs.Staffs.Bosses
{
    public class BossResponseDTO
    {
        public int BossId { get; init; }

        public int StaffId { get; init; }

        public string FirstName { get; init; }

        public string LastName { get; init; }

        public int Age { get; init; }

        public decimal Experience { get; init; }

        public string Status { get; init; }
    }
}
