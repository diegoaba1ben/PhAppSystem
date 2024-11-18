using AutoMapper;
using PhAppUser.Application.DTOs;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Application.Mappers
{
    public class PerfilMappingProfile : Profile
    {
        public PerfilMappingProfile()
        {
            // Mapear de Entidad a DTO
            CreateMap<Perfil, PerfilDto>()
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Area != null ? src.Area.Nombre : string.Empty))
                .ForMember(dest => dest.Usuarios, opt => opt.MapFrom(src => src.CuentaUsuarios.Select(cu => cu.NombreUsuario).ToList()))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(rol => rol.Nombre).ToList()));

            // Mapear de DTO a Entidad
            CreateMap<PerfilDto, Perfil>()
                .ForMember(dest => dest.AreaId, opt => opt.Ignore()) // `AreaId` será asignado en otro lugar
                .ForMember(dest => dest.CuentaUsuarios, opt => opt.Ignore()) // Necesita lógica personalizada
                .ForMember(dest => dest.Roles, opt => opt.Ignore()); // Necesita lógica personalizada
        }
    }
}

