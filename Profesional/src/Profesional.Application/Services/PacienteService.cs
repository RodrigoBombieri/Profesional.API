using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Profesional.Application.DTOs;
using Profesional.Domain.Entities;
using Profesional.Application.Interfaces;
using Profesional.Application.Helpers;

namespace Profesional.Application.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly IApplicationDbContext _context;

        public PacienteService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResponse<PacienteResponseDto>> GetAllAsync(PaginationParams paginationParams)
        {
            var query = _context.Pacientes
                .Where(p => p.Activo == (paginationParams.Activo ?? true)); // Filtro por Activo

            // Búsqueda por nombre o apellido
            if (!string.IsNullOrEmpty(paginationParams.Search))
            {
                var search = paginationParams.Search.ToLower();
                query = query.Where(p =>
                    p.Nombre.ToLower().Contains(search) ||
                    p.Apellido.ToLower().Contains(search) ||
                    p.DNI.Contains(search) ||
                    p.Email.ToLower().Contains(search));
            }

            // Ordenamiento
            if (!string.IsNullOrEmpty(paginationParams.SortBy))
            {
                query = paginationParams.SortBy.ToLower() switch
                {
                    "nombre" => paginationParams.SortDescending ? query.OrderByDescending(p => p.Nombre) : query.OrderBy(p => p.Nombre),
                    "apellido" => paginationParams.SortDescending ? query.OrderByDescending(p => p.Apellido) : query.OrderBy(p => p.Apellido),
                    "fecharegistro" => paginationParams.SortDescending ? query.OrderByDescending(p => p.FechaRegistro) : query.OrderBy(p => p.FechaRegistro),
                    _ => query.OrderBy(p => p.Id)
                };
            }
            else
            {
                query = query.OrderBy(p => p.Id);
            }

            // Paginación
            var totalRecords = await query.CountAsync();
            var pacientes = await query
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();

            var data = pacientes.Select(p => new PacienteResponseDto
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

            return new PagedResponse<PacienteResponseDto>(data, paginationParams.PageNumber, paginationParams.PageSize, totalRecords);
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