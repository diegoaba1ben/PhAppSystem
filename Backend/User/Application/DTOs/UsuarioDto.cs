using System;

namespace PhAppUser.Application.DTOs
{
    /// <summary>
    /// DTO que representa a un usuario simplificado para su inclusi�n en perfiles y otras consultas.
    /// </summary>
    public class UsuarioDto
    {
        public Guid Id { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public bool EsActivo { get; set; }
    }
}