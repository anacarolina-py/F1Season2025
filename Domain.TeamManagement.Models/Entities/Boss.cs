using Domain.TeamManagement.Models.Entities.Abstracts;

namespace Domain.TeamManagement.Models.Entities;

public class Boss : AStaff
{
    public int BossId { get; private set; }
}
