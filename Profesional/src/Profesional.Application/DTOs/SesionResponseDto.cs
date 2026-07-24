using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profesional.Application.DTOs
{
    public class SesionResponseDto
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public string PacienteNombre { get; set; } = string.Empty; // Nombre completo del paciente
        public DateTime Fecha { get; set; }
        public string TipoTratamiento { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
        public string? Evolucion { get; set; }
        public DateTime? ProximaCita { get; set; }
        public int DuracionMinutos { get; set; }
        public bool Completada { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
