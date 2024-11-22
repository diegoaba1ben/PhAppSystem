using System;

namespace PhAppUser.Application.DTOs
{
    public class AfiliacionSSDto
    {
        public Guid UsuarioId { get; set; }
        public string NombresCompletos { get; set; } = string.Empty;
        public string ApellidosCompletos { get; set; } = string.Empty;
        public string Identificacion { get; set; } = string.Empty;
        public string Afiliacion { get; set; } = string.Empty; // Completa o Parcial
        public int? DiasPendientes { get; set; }
        public SaludDto? Salud { get; set; } // DTO de Salud
        public PensionDto? Pension { get; set; } // DTO de Pensión
    }
}
