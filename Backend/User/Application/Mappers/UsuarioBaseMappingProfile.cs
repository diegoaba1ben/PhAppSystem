using AutoMapper;
using PhAppUser.Application.DTOs;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Application.Mappers
{
    public class UsuarioBaseMappingProfile : Profile
    {
        public UsuarioBaseMappingProfile()
        {
            // Mapeo de CuentaUsuario a UsuarioBaseDto
            CreateMap<CuentaUsuario, UsuarioBaseDto>()
                .ForMember(dest => dest.NombresCompletos, opt => opt.MapFrom(src => src.NombresCompletos))
                .ForMember(dest => dest.ApellidosCompletos, opt => opt.MapFrom(src => src.ApellidosCompletos))
                .ForMember(dest => dest.Identificacion, opt => opt.MapFrom(src => src.Identificacion))
                .ForMember(dest => dest.Direccion, opt => opt.MapFrom(src => src.Direccion))
                .ForMember(dest => dest.Ciudad, opt => opt.MapFrom(src => src.Ciudad))
                .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Telefono))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.EsActivo, opt => opt.MapFrom(src => src.EsActivo))
                .ForMember(dest => dest.FechaRegistro, opt => opt.MapFrom(src => src.FechaRegistro))
                .ForMember(dest => dest.FechaInactivacion, opt => opt.MapFrom(src => src.FechaInactivacion))
                .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => src.NombreUsuario))
                .ForMember(dest => dest.FechaUltimoLogin, opt => opt.MapFrom(src => src.FechaUltimoLogin))
                .ForMember(dest => dest.TipoCuenta, opt => opt.MapFrom(src => src.TipoCuenta))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => src.FechaCreacion))
                .ForMember(dest => dest.Perfiles, opt => opt.MapFrom(src => src.Perfiles));
        }
    }
}
