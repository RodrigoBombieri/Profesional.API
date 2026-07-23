using Profesional.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profesional.Application.Services
{
    public interface IAuthService
    {
        Task<UsuarioResponseDto> RegisterAsync(UsuarioRegisterDto dto);
        Task<UsuarioResponseDto> LoginAsync(UsuarioLoginDto dto);
    }
}