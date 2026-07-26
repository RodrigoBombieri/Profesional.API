using Microsoft.EntityFrameworkCore;
using Profesional.Application.DTOs;
using Profesional.Application.Interfaces;
using Profesional.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Profesional.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IApplicationDbContext _context;

        public DashboardService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardResponseDto> GetDashboardDataAsync()
        {
            var ahora = DateTime.Now;
            var inicioMes = new DateTime(ahora.Year, ahora.Month, 1);
            var finMes = inicioMes.AddMonths(1).AddDays(-1);
            var fechaLimiteProximasCitas = ahora.AddDays(7);

            // 1. Total de pacientes activos
            var totalPacientesActivos = await _context.Pacientes
                .CountAsync(p => p.Activo);

            // 2. Total de sesiones del mes
            var totalSesionesMes = await _context.Sesiones
                .CountAsync(s => s.Fecha >= inicioMes && s.Fecha <= finMes);

            // 3. Próximas citas (próximos 7 días)
            var proximasCitas = await _context.Sesiones
                .CountAsync(s => s.ProximaCita.HasValue
                    && s.ProximaCita.Value >= ahora
                    && s.ProximaCita.Value <= fechaLimiteProximasCitas
                    && !s.Completada);

            // 4. Tratamientos más usados (top 5)
            var tratamientos = await _context.Sesiones
                .Where(s => s.Fecha >= inicioMes && s.Fecha <= finMes)
                .GroupBy(s => s.TipoTratamiento)
                .Select(g => new { Tratamiento = g.Key, Cantidad = g.Count() })
                .OrderByDescending(x => x.Cantidad)
                .Take(5)
                .ToDictionaryAsync(
                    x => x.Tratamiento,
                    x => x.Cantidad
                );

            // 5. Últimas 5 sesiones registradas
            var ultimasSesiones = await _context.Sesiones
                .Include(s => s.Paciente)
                .OrderByDescending(s => s.Fecha)
                .Take(5)
                .Select(s => new SesionResponseDto
                {
                    Id = s.Id,
                    PacienteId = s.PacienteId,
                    PacienteNombre = s.Paciente.Nombre + " " + s.Paciente.Apellido,
                    Fecha = s.Fecha,
                    TipoTratamiento = s.TipoTratamiento,
                    Observaciones = s.Observaciones,
                    Evolucion = s.Evolucion,
                    ProximaCita = s.ProximaCita,
                    DuracionMinutos = s.DuracionMinutos,
                    Completada = s.Completada
                })
                .ToListAsync();

            return new DashboardResponseDto
            {
                TotalPacientesActivos = totalPacientesActivos,
                TotalSesionesMes = totalSesionesMes,
                ProximasCitas = proximasCitas,
                TratamientosMasUsados = tratamientos,
                UltimasSesiones = ultimasSesiones
            };
        }
    }
}