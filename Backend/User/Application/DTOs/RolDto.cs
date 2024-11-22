using System;
using System.Collections.Generic;
using PhAppUser.Application.DTOs;

namespace PhAppUser.Application.DTOs
{
    public class RolDto
    {
        // Identificador único del rol
        public Guid Id { get; set; }

        // Nombre del rol
        public string Nombre { get; set; } = string.Empty;

        // Descripción del rol
        public string Descripcion { get; set; } = string.Empty;

        // Fecha de creación del rol (Fecha de asignación en términos de auditoría)
        public DateTime FechaCreacion { get; set; }

        // Nueva propiedad: Fecha de revocación
        public DateTime? FechaInactivacion { get; set; }

        // Relaciones simplificadas
        public Guid AreaId { get; set; } // Relación con el área (clave foránea)

        // Nueva relación: Lista de permisos asociados al rol
        public List<PermisoDto> Permisos { get; set; } = new List<PermisoDto>();
    }
}
