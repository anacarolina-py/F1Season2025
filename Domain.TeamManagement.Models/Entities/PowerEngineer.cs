using Domain.TeamManagement.Models.Entities.Abstracts;

namespace Domain.TeamManagement.Models.Entities;

public  class PowerEngineer : AEngineer
{
    public int PowerEngineerId { get; private set; }

    public PowerEngineer(string firstName, string lastName, int age)
        : base(firstName, lastName, age)
    {
    }
}
