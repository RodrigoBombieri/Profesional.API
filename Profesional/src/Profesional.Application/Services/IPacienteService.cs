using System.Collections.Generic;
using System.Threading.Tasks;
using Profesional.Application.DTOs;

namespace Profesional.Application.Services
{
    public interface IPacienteService
    {
        Task<IEnumerable<PacienteResponseDto>> GetAllAsync();
        Task<PacienteResponseDto?> GetByIdAsync(int id);
        Task<PacienteResponseDto> CreateAsync(PacienteCreateDto dto);
        Task<PacienteResponseDto?> UpdateAsync(int id, PacienteCreateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}