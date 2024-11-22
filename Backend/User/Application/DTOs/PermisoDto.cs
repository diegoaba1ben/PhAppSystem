using System;

namespace PhAppUser.Application.DTOs
{
    public class PermisoDto
    {
        public Guid Id { get; set; } // Identificador único del permiso
        public string Codigo { get; set; } = string.Empty; // Código único del permiso
        public string Nombre { get; set; } = string.Empty; // Nombre descriptivo del permiso
        public string Descripcion { get; set; } = string.Empty; // Detalle sobre el permiso
        public DateTime FechaCreacion { get; set; } = DateTime.Now; // Fecha de creación del permiso
        
        // Nueva propiedad: Fecha de revocación (opcional)
        public DateTime? FechaRevocacion { get; set; }
    }
}
