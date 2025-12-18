namespace Domain.TeamManagement.Models.Entities;

public class Team
{
    public int TeamId { get; private set; }

    public string Name { get; private set; }

    public string Status { get; private set; }

    //CREATE TEAM CONSTRUCTOR
    public Team(string Name)
    {
        this.Name = Name;
        this.Status = "Inactive";
    }
}
