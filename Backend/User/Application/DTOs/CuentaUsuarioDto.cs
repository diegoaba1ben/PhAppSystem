using System;
using PhAppUser.Domain.Enums;
using PhAppUser.Domain.Entities;
using PhAppUser.Application.DTOs;

public class CuentaUsuarioDto : UsuarioDatosBasicosQDto
{
    public string Password { get; set; } = string.Empty;
    public string TarjProf { get; set; } = string.Empty;
    public bool? SujetoRetencion { get; set; }
    public TipoIdTrib? TipoIdTrib { get; set; }
    public string RazonSocial { get; set; } = string.Empty;
    public Afiliacion Afiliacion { get; set; }
    public int? DiasPendientes { get; set; }
}
