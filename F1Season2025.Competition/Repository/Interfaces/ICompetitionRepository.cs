using Domain.Competition.Models.Entities;
namespace F1Season2025.Competition.Repository.Interfaces
{
    public interface ICompetitionRepository
    {
        Task AddCompetitionAsync(Competitions competition);

        Task UpdateStatusRaceAsync(Competitions competition);

        Task<Competitions?> GetCompetitionByRoundAsync(int round);

        Task<int> GetActiveCompetitionsCountAsync();

        Task<IEnumerable<Competitions>> GetAllCompetitionsAsync();
    }
}
