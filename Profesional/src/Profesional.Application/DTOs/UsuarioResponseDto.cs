using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profesional.Application.DTOs
{
    public class UsuarioResponseDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty; // Incluimos el JWT
    }
}
