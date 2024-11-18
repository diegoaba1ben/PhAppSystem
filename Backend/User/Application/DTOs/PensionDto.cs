using System;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Application.DTOs
{
    public class PensionDto : CuentaUsuarioDto
    {
        public string Numero { get; set; } = string.Empty;
        public string RazonSocialPension { get; set; } = string.Empty;
    }
}