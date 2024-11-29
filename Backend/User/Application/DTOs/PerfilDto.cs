using System;
using System.Collections.Generic;

namespace PhAppUser.Application.DTOs
{
    /// <summary>
    /// DTO encargado del manejo complejo de la combinatoria de entidades para el manejo de usuarios.
    /// </summary>
    public class PerfilDto
    {
        public Guid Id { get; set; }

        // Relación simplificada de identificadores o nombres de usuarios
        public List<UsuarioDto> Usuarios { get; set; } = new List<UsuarioDto>();

        // Relación con Rol: lista simplificada de nombre de roles
        public List<string> Roles { get; set; } = new List<string>();

        // Relación con Area: nombre del área.
        public string Area { get; set; } = string.Empty;
    }
}
