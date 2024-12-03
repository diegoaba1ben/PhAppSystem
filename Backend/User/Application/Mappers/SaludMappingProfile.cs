using AutoMapper;
using PhAppUser.Domain.Entities;
using PhAppUser.Application.DTOs;

namespace PhAppUser.Application.Mappers
{
    public class SaludMappingProfile : Profile
    {
        public SaludMappingProfile()
        {
            // Mapeo desde Salud a SaludDto
            CreateMap<Salud, SaludDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Numero, opt => opt.MapFrom(src => src.Numero))
                .ForMember(dest => dest.RazonSocialSalud, opt => opt.MapFrom(src => src.RazonSocialSalud))
                .ForMember(dest => dest.CuentaUsuarioId, opt => opt.MapFrom(src => src.CuentaUsuarioId));

            // Mapeo desde SaludDto a entidad Salud
            CreateMap<SaludDto, Salud>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Numero, opt => opt.MapFrom(src => src.Numero))
                .ForMember(dest => dest.RazonSocialSalud, opt => opt.MapFrom(src => src.RazonSocialSalud))
                .ForMember(dest => dest.CuentaUsuarioId, opt => opt.MapFrom(src => src.CuentaUsuarioId));
        }
    }
}
