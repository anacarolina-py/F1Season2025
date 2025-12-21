using Domain.Competition.Models.DTOs.Competitions;
using F1Season2025.Competition.Services.Interfaces;

namespace F1Season2025.Competition.Clients
{
    public class TeamServiceClient : ITeamServiceClient
    {
        private readonly HttpClient _httpClient;

        public TeamServiceClient(HttpClient httpClientTeams)
        {
            _httpClient = httpClientTeams;
        }

        public async Task<bool> ValidateSeasonAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<ValidateTeamsSeasonDto>("/api/Team/validate");

            if (result == null)
            {
                throw new InvalidOperationException("Could not validate teams for season start.");
            }

            return result.CanStart;
        }
    }
}
