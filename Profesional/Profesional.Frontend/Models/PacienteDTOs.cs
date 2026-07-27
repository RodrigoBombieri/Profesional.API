using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Profesional.Frontend.Models
{
    public class PacienteDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string DNI { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string ObraSocial { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; }
    }

    public class PacienteCreateDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El DNI es obligatorio")]
        public string DNI { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "El email no es válido")]
        public string Email { get; set; } = string.Empty;

        public string Direccion { get; set; } = string.Empty;
        public string ObraSocial { get; set; } = string.Empty;
    }

    public class PagedResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public List<T> Data { get; set; } = new();
    }
    public class PacienteDetalleDto : PacienteDto
    {
        public string Direccion { get; set; } = string.Empty;
        public string ObraSocial { get; set; } = string.Empty;
        public List<SesionDto> Sesiones { get; set; } = new();
    }

}