using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profesional.Application.Services
{
    public interface IReporteService
    {
        Task<byte[]> GenerarReportePacientesExcelAsync();
    }
}