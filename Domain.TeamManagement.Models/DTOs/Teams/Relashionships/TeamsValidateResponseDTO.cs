namespace Domain.TeamManagement.Models.DTOs.Teams.Relashionships;

public class TeamsValidateResponseDTO
{
    public bool CanStart { get; set; }

    public TeamsValidateResponseDTO()
    {
        this.CanStart = false;
    }

    public void SetCanStart(bool canStart)
    {
        this.CanStart = canStart;
    }
}
