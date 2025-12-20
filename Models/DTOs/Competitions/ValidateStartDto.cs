namespace Domain.Competition.Models.DTOs.Competition
{
    public class ValidateStartDto
    {
        public bool CanStart { get; init; }
        public string Message { get; init; } = string.Empty;
        public int Round { get; init; }
    }
}
