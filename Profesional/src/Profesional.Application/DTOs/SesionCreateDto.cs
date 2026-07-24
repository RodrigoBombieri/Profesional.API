using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profesional.Application.DTOs
{
    public class SesionCreateDto
    {
        [Required(ErrorMessage = "El ID del paciente es obligatorio")]
        public int PacienteId { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El tipo de tratamiento es obligatorio")]
        [MaxLength(100, ErrorMessage = "El tipo de tratamiento no puede superar los 100 caracteres")]
        public string TipoTratamiento { get; set; } = string.Empty;

        public string? Observaciones { get; set; }

        public string? Evolucion { get; set; }

        public DateTime? ProximaCita { get; set; }

        [Range(1, 240, ErrorMessage = "La duración debe estar entre 1 y 240 minutos")]
        public int DuracionMinutos { get; set; } = 30;

        public bool Completada { get; set; } = false;
    }
}