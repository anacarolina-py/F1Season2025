using Domain.Competition.Models.Entities;
using MongoDB.Bson;
namespace F1Season2025.Competition.Repository.Interfaces
{
    public interface ICompetitionRepository
    {
        Task AddCompetitionAsync(Competitions competition);
        Task UpdateStatusRaceAsync(Competitions competition);
        Task<Competitions?> GetCompetitionByRoundAsync(int round);
        Task<int> GetActiveCompetitionsCountAsync();
        Task<IEnumerable<Competitions>> GetAllCompetitionsAsync();
        Task UpdateActiveStatusAsync(ObjectId id, bool isActive);
        Task <Competitions?> GetbyCompetitionByIdAsync(ObjectId id);
        Task<bool> CheckCompetitionExistsAsync(string circuitName, string country);
    }
}
