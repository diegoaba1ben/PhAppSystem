using System;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Application.DTOs
{
    /// <summary>
    /// DTO para el reporte de usuarios inactivos
    /// </summary>
    public class UsuarioInactivoDto
    {
        public string NombresCompletos { get; set; } = string.Empty;
        public string ApellidosCompletos { get; set; } = string.Empty;
        public string Identificacion { get; set; } = string.Empty;
        public DateTime FechaInactivacion { get; set; }
        public string MotivoInactivacion { get; set; } = string.Empty;
        public string? NombreUsuario { get; set; }
    }
}