using AutoMapper;
using PhAppUser.Application.DTOs;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Application.Mappers
{
    public class RolMappingProfile : Profile
    {
        public RolMappingProfile()
        {
            // Mapeo de Rol a RolDto
            CreateMap<Rol, RolDto>()
                .ForMember(dest => dest.AreaId, opt => opt.MapFrom(src => src.AreaId));

            // Mapeo de RolDto a Rol
            CreateMap<RolDto, Rol>()
                .ForMember(dest => dest.AreaId, opt => opt.MapFrom(src => src.AreaId));
        }
    }
}
