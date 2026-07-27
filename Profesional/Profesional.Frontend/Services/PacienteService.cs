using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Profesional.Frontend.Models;


namespace Profesional.Frontend.Services
{
    public class PacienteService
    {
        private readonly HttpClient _httpClient;

        public PacienteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET /api/Pacientes (con paginación y filtros)
        public async Task<PagedResponse<PacienteDto>?> GetPacientesAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string? search = null,
            bool? activo = true,
            string? sortBy = null,
            bool sortDescending = false)
        {
            var url = $"api/Pacientes?PageNumber={pageNumber}&PageSize={pageSize}&Activo={activo}";
            if (!string.IsNullOrEmpty(search))
                url += $"&Search={search}";
            if (!string.IsNullOrEmpty(sortBy))
                url += $"&SortBy={sortBy}&SortDescending={sortDescending}";

            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PagedResponse<PacienteDto>>();
            }

            var body = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(
                $"GET {url} devolvió {(int)response.StatusCode} {response.ReasonPhrase}. {body}");
        }

        // GET /api/Pacientes/{id}
        public async Task<PacienteDto?> GetPacienteByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/Pacientes/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PacienteDto>();
            }
            return null;
        }

        // POST /api/Pacientes
        public async Task<PacienteDto?> CreatePacienteAsync(PacienteCreateDto paciente)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Pacientes", paciente);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PacienteDto>();
            }
            return null;
        }

        // PUT /api/Pacientes/{id}
        public async Task<PacienteDto?> UpdatePacienteAsync(int id, PacienteCreateDto paciente)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Pacientes/{id}", paciente);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PacienteDto>();
            }
            return null;
        }

        // DELETE /api/Pacientes/{id}
        public async Task<bool> DeletePacienteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Pacientes/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<PacienteDetalleDto?> GetPacienteDetalleAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/Pacientes/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PacienteDetalleDto>();
            }
            return null;
        }
    }
}