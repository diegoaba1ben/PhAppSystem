using System;

namespace PhAppUser.Application.DTOs
{
    public class SaludDto
    {
        public Guid Id { get; set; } // Identificador único de la afiliación
        public string Numero { get; set; } = string.Empty; // Número de afiliación
        public string RazonSocialSalud { get; set; } = string.Empty; // Nombre de la entidad de salud
        public Guid CuentaUsuarioId { get; set; } // Relación con CuentaUsuario
        public bool EsActivo { get; set; } // Indicador de activo o inactivo
    }
}
