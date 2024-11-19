using AutoMapper;
using PhAppUser.Domain.Entities;
using PhAppUser.Application.DTOs;

namespace PhAppUser.Application.Mappers
{
    /// <summary>
    /// Perfil de mapeo para búsquedas avanzadas de usuarios.
    /// </summary>
    public class AdvancedUserMappingProfile : Profile
    {
        public AdvancedUserMappingProfile()
        {
            // Mapeo de CuentaUsuario a AdvancedUserDto
            CreateMap<CuentaUsuario, AdvancedDto.AdvancedUserDto>()
                .ForMember(dest => dest.NombresCompletos, opt => opt.MapFrom(src => src.NombresCompletos))
                .ForMember(dest => dest.ApellidosCompletos, opt => opt.MapFrom(src => src.ApellidosCompletos))
                .ForMember(dest => dest.Identificacion, opt => opt.MapFrom(src => src.Identificacion))
                .ForMember(dest => dest.Direccion, opt => opt.MapFrom(src => src.Direccion))
                .ForMember(dest => dest.Ciudad, opt => opt.MapFrom(src => src.Ciudad))
                .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Telefono))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.EsActivo, opt => opt.MapFrom(src => src.EsActivo))
                .ForMember(dest => dest.FechaRegistro, opt => opt.MapFrom(src => src.FechaRegistro))
                .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => src.NombreUsuario))
                .ForMember(dest => dest.FechaUltimoLogin, opt => opt.MapFrom(src => src.FechaUltimoLogin))
                .ForMember(dest => dest.Perfiles, opt => opt.MapFrom(src => src.Perfiles)); // Relación con perfiles

            // Mapeo de Perfil a PerfilDto
            CreateMap<Perfil, PerfilDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Area.Nombre)) // Se referencia Area.Nombre
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(r => r.Nombre).ToList())); // Lista de nombres de roles
        }
    }
}
