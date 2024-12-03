using AutoMapper;
using PhAppUser.Domain.Entities;
using PhAppUser.Application.DTOs;

namespace PhAppUser.Application.Mappers
{
    public class PensionMappingProfile : Profile
    {
        public PensionMappingProfile()
        {
            // Mapeo desde Pension a PensionDto
            CreateMap<Pension, PensionDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Numero, opt => opt.MapFrom(src => src.Numero))
                .ForMember(dest => dest.RazonSocialPension, opt => opt.MapFrom(src => src.RazonSocialPension))
                .ForMember(dest => dest.CuentaUsuarioId, opt => opt.MapFrom(src => src.CuentaUsuarioId));

            // Mapeo desde PensionDto a entidad Pension
            CreateMap<PensionDto, Pension>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Numero, opt => opt.MapFrom(src => src.Numero))
                .ForMember(dest => dest.RazonSocialPension, opt => opt.MapFrom(src => src.RazonSocialPension))
                .ForMember(dest => dest.CuentaUsuarioId, opt => opt.MapFrom(src => src.CuentaUsuarioId));
        }
    }
}
