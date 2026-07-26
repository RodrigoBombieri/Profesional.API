using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profesional.Application.DTOs
{
    public class DashboardResponseDto
    {
        public int TotalPacientesActivos { get; set; }
        public int TotalSesionesMes { get; set; }
        public int ProximasCitas { get; set; } // Próximos 7 días
        public Dictionary<string, int> TratamientosMasUsados { get; set; } = new();
        public List<SesionResponseDto> UltimasSesiones { get; set; } = new();
    }
}