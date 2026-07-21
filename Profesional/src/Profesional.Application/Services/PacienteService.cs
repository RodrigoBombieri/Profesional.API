using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Profesional.Application.DTOs;
using Profesional.Domain.Entities;
using Profesional.Application.Interfaces;

namespace Profesional.Application.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly IApplicationDbContext _context;

        public PacienteService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PacienteResponseDto>> GetAllAsync()
        {
            var pacientes = await _context.Pacientes
                .Where(p => p.Activo)
                .ToListAsync();

            return pacientes.Select(p => new PacienteResponseDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Apellido = p.Apellido,
                DNI = p.DNI,
                Telefono = p.Telefono,
                Email = p.Email,
                FechaRegistro = p.FechaRegistro,
                Activo = p.Activo
            });
        }

        public async Task<PacienteResponseDto?> GetByIdAsync(int id)
        {
            var paciente = await _context.Pacientes
                .FirstOrDefaultAsync(p => p.Id == id && p.Activo);

            if (paciente == null) return null;

            return new PacienteResponseDto
            {
                Id = paciente.Id,
                Nombre = paciente.Nombre,
                Apellido = paciente.Apellido,
                DNI = paciente.DNI,
                Telefono = paciente.Telefono,
                Email = paciente.Email,
                FechaRegistro = paciente.FechaRegistro,
                Activo = paciente.Activo
            };
        }

        public async Task<PacienteResponseDto> CreateAsync(PacienteCreateDto dto)
        {
            var paciente = new Paciente
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                DNI = dto.DNI,
                Telefono = dto.Telefono,
                Email = dto.Email,
                FechaRegistro = DateTime.Now,
                Activo = true
            };

            _context.Pacientes.Add(paciente);
            await _context.SaveChangesAsync();

            return new PacienteResponseDto
            {
                Id = paciente.Id,
                Nombre = paciente.Nombre,
                Apellido = paciente.Apellido,
                DNI = paciente.DNI,
                Telefono = paciente.Telefono,
                Email = paciente.Email,
                FechaRegistro = paciente.FechaRegistro,
                Activo = paciente.Activo
            };
        }

        public async Task<PacienteResponseDto?> UpdateAsync(int id, PacienteCreateDto dto)
        {
            var paciente = await _context.Pacientes
                .FirstOrDefaultAsync(p => p.Id == id && p.Activo);

            if (paciente == null) return null;

            paciente.Nombre = dto.Nombre;
            paciente.Apellido = dto.Apellido;
            paciente.DNI = dto.DNI;
            paciente.Telefono = dto.Telefono;
            paciente.Email = dto.Email;

            await _context.SaveChangesAsync();

            return new PacienteResponseDto
            {
                Id = paciente.Id,
                Nombre = paciente.Nombre,
                Apellido = paciente.Apellido,
                DNI = paciente.DNI,
                Telefono = paciente.Telefono,
                Email = paciente.Email,
                FechaRegistro = paciente.FechaRegistro,
                Activo = paciente.Activo
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var paciente = await _context.Pacientes
                .FirstOrDefaultAsync(p => p.Id == id && p.Activo);

            if (paciente == null) return false;

            // Soft delete: solo lo marcamos como inactivo
            paciente.Activo = false;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}