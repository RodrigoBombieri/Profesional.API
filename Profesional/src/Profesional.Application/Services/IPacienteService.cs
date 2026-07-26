using System.Collections.Generic;
using System.Threading.Tasks;
using Profesional.Application.DTOs;
using Profesional.Application.Helpers;

namespace Profesional.Application.Services
{
    public interface IPacienteService
    {
        Task<PagedResponse<PacienteResponseDto>> GetAllAsync(PaginationParams paginationParams);
        Task<PacienteResponseDto?> GetByIdAsync(int id);
        Task<PacienteResponseDto> CreateAsync(PacienteCreateDto dto);
        Task<PacienteResponseDto?> UpdateAsync(int id, PacienteCreateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}