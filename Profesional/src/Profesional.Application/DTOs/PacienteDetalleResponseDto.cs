using System.Collections.Generic;

namespace Profesional.Application.DTOs
{
    public class PacienteDetalleResponseDto : PacienteResponseDto
    {
        public List<SesionResponseDto> Sesiones { get; set; } = new();
    }
}
