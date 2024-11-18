using System;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Application.DTOs
{
    public class SaludDto : CuentaUsuarioDto
    {
        public string Numero { get; set; } = string.Empty;
        public string RazonSocialSalud { get; set; } = string.Empty;
    }
}