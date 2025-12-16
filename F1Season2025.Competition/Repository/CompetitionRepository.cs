using Domain.Competition.Models.Entities;
using F1Season2025.Competition.Data;
using F1Season2025.Competition.Repository.Interfaces;
using F1Season2025.Competition.Services.Interfaces;
using MongoDB.Driver;

namespace F1Season2025.Competition.Repository
{
    public class CompetitionRepository : ICompetitionService
    {
        private readonly ILogger<CircuitRepository> _logger;
        private readonly IMongoCollection<Circuit> _collection;
        private readonly ConnectionDB _connection;

        public CompetitionRepository(ILogger<CircuitRepository> logger, ConnectionDB connection)
        {
            _logger = logger;
            _connection = connection;
            _collection = _connection.GetMongoCollection();
        }

        public async Task<Circuit> CreateCircuitAsync(Circuit circuit)
        {
            _logger.LogInformation("Creating circuit {Name} in database", circuit.NameCircuit);

            return await CreateCircuitAsync(circuit);
        }

        public async Task<List<Circuit>> GetAllCircuitAsync()
        {
            _logger.LogInformation("Listing all circuits");

            return await _collection
                 .Find(_ => true)
                 .ToListAsync();
        }

        public async Task<Circuit?> GetByIdCircuitAsync(Guid id) 
        {
            _logger.LogInformation("Searching circuit with Id {CircuitId}", id);

            return await _collection
                .Find(c => c.Id == id)
                .FirstOrDefaultAsync();
        }

        

    }
}
