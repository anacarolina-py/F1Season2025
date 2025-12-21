using Domain.Engeneering.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace Infrastructure.Engeneering.Data.Client
{
    public class TeamManagementClient
    {
        private readonly HttpClient _client;

        public TeamManagementClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<EngineeringInfoDTO?>> GetEngineeringInfo(int teamId)
        {
            var response = await _client.GetAsync
                ($"teams/engineeringinfo/{teamId}");

            if(!response.IsSuccessStatusCode)
            return Enumerable.Empty<EngineeringInfoDTO?>();

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<EngineeringInfoDTO>>();

            return result ?? Enumerable.Empty<EngineeringInfoDTO>();
        }
    }
}
