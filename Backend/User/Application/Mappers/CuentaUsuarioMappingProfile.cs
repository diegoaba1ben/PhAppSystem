using AutoMapper;
using PhAppUser.Application.DTOs;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Application.Mappers
{
    public class CuentaUsuarioMappingProfile : Profile
    {
        public CuentaUsuarioMappingProfile()
        {
            // Mapeo de CuentaUsuario a CuentaUsuarioDto
            CreateMap<CuentaUsuario, CuentaUsuarioDto>()
                .IncludeBase<CuentaUsuario, UsuarioBaseDto>() // Incluye el mapeo de UsuarioBaseDto
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.TarjProf, opt => opt.MapFrom(src => src.TarjProf))
                .ForMember(dest => dest.TipoContrato, opt => opt.MapFrom(src => src.TipoContrato))
                .ForMember(dest => dest.SujetoRetencion, opt => opt.MapFrom(src => src.SujetoRetencion))
                .ForMember(dest => dest.TipoIdTrib, opt => opt.MapFrom(src => src.TipoIdTrib))
                .ForMember(dest => dest.RazonSocial, opt => opt.MapFrom(src => src.RazonSocial))
                .ForMember(dest => dest.Afiliacion, opt => opt.MapFrom(src => src.Afiliacion))
                .ForMember(dest => dest.DiasPendientes, opt => opt.MapFrom(src => src.DiasPendientes));

        }

    }
}


