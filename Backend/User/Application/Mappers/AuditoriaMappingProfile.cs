using AutoMapper;
using PhAppUser.Domain.Entities;
using PhAppUser.Application.DTOs;

namespace PhAppUser.Application.Mappers
{
    public class AuditoriaMappingProfile : Profile
    {
        public AuditoriaMappingProfile()
        {
            CreateMap<CuentaUsuario, AuditoriaUsuarioDto>()
                .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => $"{src.NombresCompletos} {src.ApellidosCompletos}"))
                .ForMember(dest => dest.Identificacion, opt => opt.MapFrom(src => src.Identificacion))
                .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => src.NombreUsuario))
                .ForMember(dest => dest.FechaRegistro, opt => opt.MapFrom(src => src.FechaRegistro))
                .ForMember(dest => dest.EsActivo, opt => opt.MapFrom(src => src.EsActivo))
                .ForMember(dest => dest.FechaInactivacion, opt => opt.MapFrom(src => src.FechaInactivacion))
                .ForMember(dest => dest.HistorialRolesPermisos, opt => opt.MapFrom(src =>
                    src.Perfiles.SelectMany(p => p.Roles.Select(r => new AuditoriaUsuarioDto.CambioRolPermisoDto
                    {
                        Rol = r.Nombre,
                        Permisos = r.Permisos.Select(p => p.Nombre).ToList(),
                        FechaRegistro = r.FechaCreacion
                    }))))
                .ForMember(dest => dest.EventosAuditoria, opt => opt.MapFrom(src => GenerarEventosAuditoria(src)));
        }

        // Método para generar los eventos de auditoría
        private static List<AuditoriaUsuarioDto.EventoAuditoriaDto> GenerarEventosAuditoria(CuentaUsuario src)
        {
            var eventos = new List<AuditoriaUsuarioDto.EventoAuditoriaDto>
            {
                new AuditoriaUsuarioDto.EventoAuditoriaDto
                {
                    TipoEvento = "Creación",
                    FechaEvento = src.FechaRegistro,
                    Detalle = "Usuario creado en el sistema"
                }
            };

            if (src.FechaInactivacion.HasValue)
            {
                eventos.Add(new AuditoriaUsuarioDto.EventoAuditoriaDto
                {
                    TipoEvento = "Inactivación",
                    FechaEvento = src.FechaInactivacion.Value,
                    Detalle = "Usuario marcado como inactivo"
                });
            }

            return eventos;
        }
    }
}




