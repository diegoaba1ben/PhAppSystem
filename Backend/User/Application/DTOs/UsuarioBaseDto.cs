using System;
using PhAppUser.Domain.Enums;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Application.DTOs
{
    public class UsuarioBaseDto
    {
        public Guid Id { get; set; }
        public string NombresCompletos { get; set; } = string.Empty;
        public string ApellidosCompletos { get; set; } = string.Empty;
        public string Identificacion { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool EsActivo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaInactivacion { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public DateTime? FechaUltimoLogin { get; set; }
        public TipoCuenta TipoCuenta { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<PerfilDto> Perfiles { get; set; } = new List<PerfilDto>();
    }
}