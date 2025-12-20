using Domain.TeamManagement.Models.Entities.Abstracts;

namespace Domain.TeamManagement.Models.Entities;

public class AerodynamicEngineer : AEngineer
{
    public int AerodynamicEngineerId { get; private set; }

    public AerodynamicEngineer(string firstName, string lastName, int age)
        : base(firstName, lastName, age)
    {
    }
}
