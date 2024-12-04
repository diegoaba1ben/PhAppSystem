using System;
using PhAppUser.Domain.Enums;
using PhAppUser.Domain.Entities;
using PhAppUser.Application.DTOs;
using FluentValidation;

namespace PhAppUser.Application.DTOs
{
    /// <summary>
    /// Representa a un usuario con información de afiliaciones a salud y pensión.
    /// </summary>
    public class UsuarioSegSocQDto
    {
        // Identificador único del usuario
        public Guid Id { get; set; }
        public string NombresCompletos { get; set; } = string.Empty;
        // Información de afiliación a Salud del usuario
        public SaludDto Salud  { get; set; } = new SaludDto();
        // Información de la afiliación a Pensión del usuario 
        public PensionDto Pension { get; set; } = new PensionDto();         
    }
}