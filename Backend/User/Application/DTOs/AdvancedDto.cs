using PhAppUser.Domain.Enums;

namespace PhAppUser.Application.DTOs
{
    public class AdvancedDto
    {
        /// <summary>
        /// DTO para representar los resultados de búsquedas avanzadas de usuarios.
        /// </summary>
        public class AdvancedUserDto
        {
            public Guid Id { get; set; }
            public string NombresCompletos { get; set; } = string.Empty;
            public string ApellidosCompletos { get; set; } = string.Empty;
            public string Identificacion { get; set; } = string.Empty;
            public string Direccion { get; set; } = string.Empty;
            public string Ciudad { get; set; } = string.Empty;
            public string Telefono { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public bool EsActivo { get; set; } = true;
            public DateTime FechaRegistro { get; set; } = DateTime.Now;
            public string NombreUsuario { get; set; } = string.Empty;
            public DateTime? FechaUltimoLogin { get; set; }
            public List<PerfilDto> Perfiles { get; set; } = new List<PerfilDto>();

            // Nuevos atributos relacionados con la afiliación y auditoría
            public Afiliacion Afiliacion { get; set; }
            public int? DiasPendientes { get; set; }
            public int Intento { get; set; }
            public bool Bloqueado { get; set; }
        }
    }
}
