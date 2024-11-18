using AutoMapper;
using PhAppUser.Domain.Entities;
using PhAppUser.Application.DTOs;

namespace PhAppUser.Application.DTOs
{
    public class SaludMappingProfile : Profile
    {
        public SaludMappingProfile ()
        {
            // Mapeo desde Salud a SaludDto
            CreateMap<SaludBuilder, SaludDto>();

            // Mapeo desde SaludDto a entidad Salud
            CreateMap<SaludDto, Salud>();
        }
    }
}