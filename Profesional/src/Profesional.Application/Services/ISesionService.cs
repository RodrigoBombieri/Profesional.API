using Profesional.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profesional.Application.Services
{
    public interface ISesionService
    {
        Task<IEnumerable<SesionResponseDto>> GetAllAsync();
        Task<IEnumerable<SesionResponseDto>> GetByPacienteIdAsync(int pacienteId);
        Task<SesionResponseDto?> GetByIdAsync(int id);
        Task<SesionResponseDto> CreateAsync(SesionCreateDto dto);
        Task<SesionResponseDto?> UpdateAsync(int id, SesionCreateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> CompletarSesionAsync(int id);
    }
}