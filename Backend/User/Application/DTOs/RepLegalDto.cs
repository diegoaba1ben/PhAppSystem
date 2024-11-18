using System;
using PhAppUser.Domain.Enums;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Application.DTOs
{
    public class RepLegalDto : CuentaUsuarioDto
    {
        public string CertLegal { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
    }
}