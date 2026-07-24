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
            var sesiones = await _context.Sesiones
                .Include(s => s.Paciente)
                .Where(s => s.Paciente.Activo)
                .OrderByDescending(s => s.Fecha)
                .ToListAsync();

            return sesiones.Select(s => MapToDto(s));
        }

        public async Task<IEnumerable<SesionResponseDto>> GetByPacienteIdAsync(int pacienteId)
        {
            var sesiones = await _context.Sesiones
                .Include(s => s.Paciente)
                .Where(s => s.PacienteId == pacienteId && s.Paciente.Activo)
                .OrderByDescending(s => s.Fecha)
                .ToListAsync();

            return sesiones.Select(s => MapToDto(s));
        }

        public async Task<SesionResponseDto?> GetByIdAsync(int id)
        {
            var sesion = await _context.Sesiones
                .Include(s => s.Paciente)
                .FirstOrDefaultAsync(s => s.Id == id && s.Paciente.Activo);

            return sesion == null ? null : MapToDto(sesion);
        }

        public async Task<SesionResponseDto> CreateAsync(SesionCreateDto dto)
        {
            // Verificar que el paciente existe y está activo
            var paciente = await _context.Pacientes
                .FirstOrDefaultAsync(p => p.Id == dto.PacienteId && p.Activo);

            if (paciente == null)
                throw new Exception("El paciente no existe o está inactivo");

            var sesion = new Sesion
            {
                PacienteId = dto.PacienteId,
                Fecha = dto.Fecha,
                TipoTratamiento = dto.TipoTratamiento,
                Observaciones = dto.Observaciones,
                Evolucion = dto.Evolucion,
                ProximaCita = dto.ProximaCita,
                DuracionMinutos = dto.DuracionMinutos,
                Completada = dto.Completada
            };

            _context.Sesiones.Add(sesion);
            await _context.SaveChangesAsync();

            // Recargar la sesión con el paciente incluido
            var sesionCreada = await _context.Sesiones
                .Include(s => s.Paciente)
                .FirstAsync(s => s.Id == sesion.Id);

            return MapToDto(sesionCreada);
        }

        public async Task<SesionResponseDto?> UpdateAsync(int id, SesionCreateDto dto)
        {
            var sesion = await _context.Sesiones
                .Include(s => s.Paciente)
                .FirstOrDefaultAsync(s => s.Id == id && s.Paciente.Activo);

            if (sesion == null)
                return null;

            // Verificar que el paciente existe y está activo
            var paciente = await _context.Pacientes
                .FirstOrDefaultAsync(p => p.Id == dto.PacienteId && p.Activo);

            if (paciente == null)
                throw new Exception("El paciente no existe o está inactivo");

            sesion.PacienteId = dto.PacienteId;
            sesion.Fecha = dto.Fecha;
            sesion.TipoTratamiento = dto.TipoTratamiento;
            sesion.Observaciones = dto.Observaciones;
            sesion.Evolucion = dto.Evolucion;
            sesion.ProximaCita = dto.ProximaCita;
            sesion.DuracionMinutos = dto.DuracionMinutos;
            sesion.Completada = dto.Completada;

            await _context.SaveChangesAsync();

            // Recargar la sesión con el paciente incluido
            var sesionActualizada = await _context.Sesiones
                .Include(s => s.Paciente)
                .FirstAsync(s => s.Id == sesion.Id);

            return MapToDto(sesionActualizada);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sesion = await _context.Sesiones
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sesion == null)
                return false;

            _context.Sesiones.Remove(sesion); // Borrado físico (las sesiones no se "eliminan" lógicamente)
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CompletarSesionAsync(int id)
        {
            var sesion = await _context.Sesiones
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sesion == null)
                return false;

            sesion.Completada = true;
            await _context.SaveChangesAsync();

            return true;
        }

        // Método privado para mapear de entidad a DTO
        private SesionResponseDto MapToDto(Sesion sesion)
        {
            return new SesionResponseDto
            {
                Id = sesion.Id,
                PacienteId = sesion.PacienteId,
                PacienteNombre = $"{sesion.Paciente.Nombre} {sesion.Paciente.Apellido}",
                Fecha = sesion.Fecha,
                TipoTratamiento = sesion.TipoTratamiento,
                Observaciones = sesion.Observaciones,
                Evolucion = sesion.Evolucion,
                ProximaCita = sesion.ProximaCita,
                DuracionMinutos = sesion.DuracionMinutos,
                Completada = sesion.Completada,
                FechaRegistro = DateTime.Now // Podrías agregar este campo a la entidad si lo deseas
            };
        }
    }
}
