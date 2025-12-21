using Domain.Competition.Models.Entities;
using F1Season2025.Competition.Data;
using F1Season2025.Competition.Repository.Interfaces;
using MongoDB.Bson;
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
            _logger.LogInformation($"Adding competition for round {competition.Round}");

            await _collection.InsertOneAsync(competition);
        }

        public async Task<Competitions?> GetCompetitionByRoundAsync(int round)
        {
            _logger.LogInformation($"Searching competition for round {round}");

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
            _logger.LogInformation($"Updating competition for round {competition.Round}");

            var updateRace = Builders<Competitions>.Update.Set(r => r.Status, competition.Status);

            var updatedRace = await _collection.UpdateOneAsync(r => r.Round == competition.Round, updateRace);
        }

        public async Task<IEnumerable<Competitions>> GetAllCompetitionsAsync()
        {
            _logger.LogInformation("Listing all competitions");

            return await _collection
                .Find(_ => true)
                .ToListAsync();
        }
        public async Task UpdateActiveStatusAsync(ObjectId id, bool isActive)
        {
            _logger.LogInformation($"Updating active status for competition with id: {id}");

            var filterId = Builders<Competitions>.Filter.Eq(c => c.Id, id);

            var updateStatus = Builders<Competitions>.Update.Set(c => c.IsActive, isActive);

            await _collection.UpdateOneAsync(filterId, updateStatus);
        }
        public async Task<Competitions?> GetbyCompetitionByIdAsync(ObjectId id)
        {
            _logger.LogInformation($"Searching competition with id: {id}");

            return await _collection
                .Find(c => c.Id == id)
                .FirstOrDefaultAsync();
        }
        public async Task<bool> CheckCompetitionExistsAsync(string circuitName, string country)
        {
            _logger.LogInformation($"Checking existence of competition with: {circuitName}");

            return await _collection.Find(c => c.Circuit.NameCircuit == circuitName && c.Circuit.Country == country).AnyAsync();
        }
    }
}
