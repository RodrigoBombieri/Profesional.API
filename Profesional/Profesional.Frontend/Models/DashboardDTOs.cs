using System.Collections.Generic;

namespace Profesional.Frontend.Models
{
    public class DashboardResponseDto
    {
        public int TotalPacientesActivos { get; set; }
        public int TotalSesionesMes { get; set; }
        public int ProximasCitas { get; set; }
        public Dictionary<string, int> TratamientosMasUsados { get; set; } = new();
        public List<SesionDto> UltimasSesiones { get; set; } = new();
    }
}