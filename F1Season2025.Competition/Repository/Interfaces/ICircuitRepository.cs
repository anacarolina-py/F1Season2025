using Domain.Competition.Models.Entities;
using MongoDB.Bson;

namespace F1Season2025.Competition.Repository.Interfaces
{
    public interface ICircuitRepository
    {
        Task AddCircuitAsync(Circuit circuit);
        Task<Circuit?> GetCircuitByIdAsync(ObjectId id);
        Task<IEnumerable<Circuit>> GetAllCircuitsAsync();
        Task<bool> ExistCircuitNameCountryAsync(string nameCircuit, string country);
    }
}
