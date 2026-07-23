using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using Profesional.Application.DTOs;
using Profesional.Application.Interfaces;
using Profesional.Domain.Entities;
using BCrypt.Net;


namespace Profesional.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(IApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<UsuarioResponseDto> RegisterAsync(UsuarioRegisterDto dto)
        {
            // Verificar si el email ya existe
            var existe = await _context.Usuarios.AnyAsync(u => u.Email == dto.Email);
            if (existe)
                throw new Exception("El email ya está registrado");

            // Crear el usuario (hashear contraseña)
            var usuario = new Usuario
            {
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password), 
                Nombre = dto.Nombre,
                Rol = "Kinesiologo" // Por defecto
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            // Generar token
            var token = GenerarToken(usuario);

            return new UsuarioResponseDto
            {
                Id = usuario.Id,
                Email = usuario.Email,
                Nombre = usuario.Nombre,
                Rol = usuario.Rol,
                Token = token
            };
        }

        public async Task<UsuarioResponseDto> LoginAsync(UsuarioLoginDto dto)
        {
            // Buscar usuario por email
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (usuario == null)
                throw new Exception("Email o contraseña incorrectos");

            // Verificar contraseña
            var passwordValida = BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash);
            if (!passwordValida)
                throw new Exception("Email o contraseña incorrectos");

            // Generar token
            var token = GenerarToken(usuario);

            return new UsuarioResponseDto
            {
                Id = usuario.Id,
                Email = usuario.Email,
                Nombre = usuario.Nombre,
                Rol = usuario.Rol,
                Token = token
            };
        }

        private string GenerarToken(Usuario usuario)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "mi-clave-secreta-super-segura-para-desarrollo");
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Role, usuario.Rol)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8),
                Issuer = _configuration["Jwt:Issuer"] ?? "ProfesionalAPI",
                Audience = _configuration["Jwt:Audience"] ?? "ProfesionalClient",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}