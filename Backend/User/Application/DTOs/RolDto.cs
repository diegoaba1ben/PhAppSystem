using System;
using PhAppUser.Application.DTOs;

namespace PhAppUser.Application.DTOs
{
    public class RolDto
    {
        public Guid Id { get; set; } // Identificador único del rol
        public string Nombre { get; set; } = string.Empty; // Nombre del rol
        public string Descripcion { get; set; } = string.Empty; // Descripción del rol
        public DateTime FechaCreacion { get; set; } // Fecha de creación del rol

        // Relaciones simplificadas para el DTO (IDs en lugar de entidades completas)
        public Guid AreaId { get; set; } // Relación con el área (clave foránea)
    }
}
