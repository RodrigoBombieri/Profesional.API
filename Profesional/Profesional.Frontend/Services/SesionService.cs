using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Profesional.Frontend.Models;

namespace Profesional.Frontend.Services
{
    public class SesionService
    {
        private readonly HttpClient _httpClient;

        public SesionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET /api/Sesiones/paciente/{pacienteId}
        public async Task<List<SesionDto>?> GetSesionesByPacienteAsync(int pacienteId)
        {
            var response = await _httpClient.GetAsync($"api/Sesiones/paciente/{pacienteId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<SesionDto>>();
            }
            return null;
        }

        // GET /api/Sesiones/{id}
        public async Task<SesionDto?> GetSesionByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/Sesiones/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<SesionDto>();
            }
            return null;
        }

        // POST /api/Sesiones
        public async Task<SesionDto?> CreateSesionAsync(SesionCreateDto sesion)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Sesiones", sesion);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<SesionDto>();
            }
            return null;
        }

        // PUT /api/Sesiones/{id}
        public async Task<SesionDto?> UpdateSesionAsync(int id, SesionCreateDto sesion)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Sesiones/{id}", sesion);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<SesionDto>();
            }
            return null;
        }

        // PATCH /api/Sesiones/{id}/completar
        public async Task<bool> CompletarSesionAsync(int id)
        {
            var response = await _httpClient.PatchAsync($"api/Sesiones/{id}/completar", null);
            return response.IsSuccessStatusCode;
        }

        // DELETE /api/Sesiones/{id}
        public async Task<bool> DeleteSesionAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Sesiones/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}