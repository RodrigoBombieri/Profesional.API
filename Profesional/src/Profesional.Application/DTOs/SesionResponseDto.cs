using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profesional.Application.DTOs
{
    public class SesionResponseDto
    {
        public int IdPaciente { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string TipoTratamiento { get; set; } = string.Empty;
    }
}
