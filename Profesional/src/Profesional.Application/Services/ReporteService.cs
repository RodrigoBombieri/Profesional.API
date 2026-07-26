using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Profesional.Application.Interfaces;


namespace Profesional.Application.Services
{
    public class ReporteService : IReporteService
    {
        private readonly IApplicationDbContext _context;

        public ReporteService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<byte[]> GenerarReportePacientesExcelAsync()
        {
            // 1. Obtener los datos
            var pacientes = await _context.Pacientes
                .Where(p => p.Activo)
                .OrderBy(p => p.Apellido)
                .ThenBy(p => p.Nombre)
                .Select(p => new
                {
                    p.Id,
                    p.Nombre,
                    p.Apellido,
                    p.DNI,
                    p.Telefono,
                    p.Email,
                    p.FechaRegistro,
                    CantidadSesiones = p.Sesiones.Count()
                })
                .ToListAsync();

            // 2. Configurar EPPlus (licencia no comercial)
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                // 3. Crear la hoja de cálculo
                var worksheet = package.Workbook.Worksheets.Add("Pacientes");

                // 4. Definir los encabezados
                var headers = new[] { "ID", "Nombre", "Apellido", "DNI", "Teléfono", "Email", "Fecha Registro", "Sesiones" };
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                }

                // 5. Llenar los datos
                int row = 2;
                foreach (var p in pacientes)
                {
                    worksheet.Cells[row, 1].Value = p.Id;
                    worksheet.Cells[row, 2].Value = p.Nombre;
                    worksheet.Cells[row, 3].Value = p.Apellido;
                    worksheet.Cells[row, 4].Value = p.DNI;
                    worksheet.Cells[row, 5].Value = p.Telefono;
                    worksheet.Cells[row, 6].Value = p.Email;
                    worksheet.Cells[row, 7].Value = p.FechaRegistro.ToString("dd/MM/yyyy");
                    worksheet.Cells[row, 8].Value = p.CantidadSesiones;
                    row++;
                }

                // 6. Ajustar el ancho de las columnas
                worksheet.Cells.AutoFitColumns();

                // 7. Devolver el archivo como arreglo de bytes
                return await Task.FromResult(package.GetAsByteArray());
            }
        }
    }
}