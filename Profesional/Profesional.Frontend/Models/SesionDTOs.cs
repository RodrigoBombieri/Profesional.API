using System;

namespace Profesional.Frontend.Models
{
    public class SesionDto
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public string PacienteNombre { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string TipoTratamiento { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
        public string? Evolucion { get; set; }
        public DateTime? ProximaCita { get; set; }
        public int DuracionMinutos { get; set; }
        public bool Completada { get; set; }
    }

    public class SesionCreateDto
    {
        public int PacienteId { get; set; }
        public DateTime Fecha { get; set; }
        public string TipoTratamiento { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
        public string? Evolucion { get; set; }
        public DateTime? ProximaCita { get; set; }
        public int DuracionMinutos { get; set; } = 30;
        public bool Completada { get; set; } = false;
    }
}