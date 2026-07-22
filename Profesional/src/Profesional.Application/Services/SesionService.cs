using Microsoft.EntityFrameworkCore;
using Profesional.Application.DTOs;
using Profesional.Application.Interfaces;
using Profesional.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profesional.Application.Services
{
    public class SesionService : ISesionService
    {
        private readonly IApplicationDbContext _context;

        public SesionService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SesionResponseDto>> GetAllAsync()
        {
            var sesiones = await _context.Sesiones.ToListAsync();

            return sesiones.Select(s => new SesionResponseDto
            {
                IdPaciente = s.PacienteId,
                Fecha = s.Fecha,
                TipoTratamiento = s.TipoTratamiento
                // ETC
            });

        }

        public async Task<SesionResponseDto> GetByIdAsync(int id)
        {
            var sesion = await _context.Sesiones.FirstOrDefaultAsync(s => s.Id == id);
            
            if (sesion == null) return null;

            return new SesionResponseDto
            {
                IdPaciente = sesion.PacienteId,
                Fecha = sesion.Fecha,
                TipoTratamiento = sesion.TipoTratamiento
                // ETC
            };
        }

        public async Task<SesionResponseDto> CreateAsync(SesionCreateDto dto)
        {
            return new SesionResponseDto
            {
                IdPaciente = 1,
                Fecha = DateTime.Now,
                TipoTratamiento = "Tratamiento de ejemplo"
                // ETC
            };
        }

        public async Task<SesionResponseDto> UpdateAsync(int id, SesionCreateDto dto)
        {
            return new SesionResponseDto
            {
                IdPaciente = 1,
                Fecha = DateTime.Now,
                TipoTratamiento = "Tratamiento actualizado"
                // ETC
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return true;
        }
    }
}
