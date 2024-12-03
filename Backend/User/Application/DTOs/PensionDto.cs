using System;

namespace PhAppUser.Application.DTOs
{
    public class PensionDto
    {
        public Guid Id { get; set; } // Identificador único de la afiliación
        public string Numero { get; set; } = string.Empty; // Número de afiliación
        public string RazonSocialPension { get; set; } = string.Empty; // Nombre de la entidad de pensión
        public Guid CuentaUsuarioId { get; set; } // Relación con CuentaUsuario
    }
}
