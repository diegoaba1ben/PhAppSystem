using System;
using PhAppUser.Application.DTOs;


namespace PhAppUser.Application.DTOs
{
    /// <summary>
    /// DTO para representar auditorías completas de usuarios.
    /// </summary>
    public class AuditoriaUsuarioDto
    {
        // Información básica del usuario
        public Guid UsuarioId { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Identificacion { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty; 
        public DateTime FechaRegistro { get; set; }
        public bool EsActivo { get; set; }
        public DateTime? FechaInactivacion { get; set; }

        // Historial de cambios en roles y permisos
        public List<CambioRolPermisoDto> HistorialRolesPermisos { get; set; } = new List<CambioRolPermisoDto>();

        // DTO interno para los cambios en roles y permisos
        public class CambioRolPermisoDto
        {
            public string Rol { get; set; } = string.Empty;
            public List<string> Permisos { get; set; } = new List<string>();
            public DateTime FechaRegistro { get; set; }
            public DateTime? FechaRevocacion { get; set; }
        }

        // Eventos importantes (auditables)
        public List<EventoAuditoriaDto> EventosAuditoria { get; set; } = new List<EventoAuditoriaDto>();

        // DTO interno para eventos específicos
        public class EventoAuditoriaDto
        {
            public string TipoEvento { get; set; } = string.Empty; // Creación, Cambio, Inactivación
            public DateTime FechaEvento { get; set; }
            public string Detalle { get; set; } = string.Empty; // Detalles del evento
        }
    }
}
