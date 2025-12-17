using Domain.Competition.Models.Entities;
using F1Season2025.Competition.Data;
using F1Season2025.Competition.Repository.Interfaces;
using MongoDB.Driver;

namespace F1Season2025.Competition.Repository
{
    public class CompetitionRepository : ICompetitionRepository
    {
        private readonly ILogger<CompetitionRepository> _logger;
        private readonly IMongoCollection<Competitions> _collection;

        public CompetitionRepository(ILogger<CompetitionRepository> logger, ConnectionDB connection) 
        {
            _logger = logger;
            _collection = connection.GetMongoCollection<Competitions>("competitions");
        }

        public async Task AddCompetitionAsync(Competitions competition)
        {
            _logger.LogInformation("Adding competition for round {Round}", competition.Round);

            await _collection.InsertOneAsync(competition);
        }

        public async Task<Competitions?> GetCompetitionByRoundAsync(int round)
        {
            _logger.LogInformation("Searching competition for round {Round}", round);

            return await _collection
                .Find(c => c.Round == round)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetActiveCompetitionsCountAsync()
        {
            _logger.LogInformation("Counting active competitions");

            return (int)await _collection
                .CountDocumentsAsync(c => c.IsActive);
        }

        public async Task UpdateStatusRaceAsync(Competitions competition)
        {
            _logger.LogInformation("Updating competition for round {Round}", competition.Round);

            await _collection.ReplaceOneAsync(
                c => c.Id == competition.Id,
                competition);
        }

        public async Task<IEnumerable<Competitions>> GetAllCompetitionsAsync()
        {
            _logger.LogInformation("Listing all competitions");

            return await _collection
                .Find(_ => true)
                .ToListAsync();
        }
    }
}
