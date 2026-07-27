using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Profesional.Frontend.Models;

namespace Profesional.Frontend.Services
{
    public class DashboardService
    {
        private readonly HttpClient _httpClient;

        public DashboardService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DashboardResponseDto?> GetDashboardDataAsync()
        {
            var response = await _httpClient.GetAsync("api/Dashboard");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<DashboardResponseDto>();
            }
            return null;
        }
    }
}