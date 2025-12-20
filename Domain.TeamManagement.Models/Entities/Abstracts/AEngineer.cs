namespace Domain.TeamManagement.Models.Entities.Abstracts;

public abstract class AEngineer : AStaff
{
    int EngineerId { get; set; }

    public AEngineer(string firstName, string lastName, int age)
        : base(firstName, lastName, age)
    {
    }
}
