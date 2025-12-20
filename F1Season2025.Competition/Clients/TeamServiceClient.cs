using Domain.Competition.Models.DTOs.Competitions;
using System.Text.Json;

namespace F1Season2025.Competition.Clients
{
    public class TeamServiceClient
    {
        private readonly HttpClient _httpClient;

        public TeamServiceClient(HttpClient httpClientTeams)
        {
            _httpClient = httpClientTeams;
        }

        public async Task<bool> ValidateSeasonAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<ValidateTeamsSeasonDto>("/api/teams/validate-season"/*vai vim da API teams*/);

            if (result == null)
            {
                throw new InvalidOperationException("Could not validate teams for season start.");
            }

            return result.CanStart;
        }
    }
}
