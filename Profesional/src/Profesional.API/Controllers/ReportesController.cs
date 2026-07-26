using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Profesional.Application.Services;

namespace Profesional.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportesController : ControllerBase
    {
        private readonly IReporteService _reporteService;

        public ReportesController(IReporteService reporteService)
        {
            _reporteService = reporteService;
        }

        [HttpGet("pacientes")]
        public async Task<IActionResult> ExportarPacientesExcel()
        {
            try
            {
                var fileBytes = await _reporteService.GenerarReportePacientesExcelAsync();
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Pacientes_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al generar el reporte", detalle = ex.Message });
            }
        }
    }
}