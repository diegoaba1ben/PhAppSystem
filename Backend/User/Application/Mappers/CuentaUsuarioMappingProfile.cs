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
                .ForMember(dest => dest.Perfiles, opt => opt.MapFrom(src => src.Perfiles))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => src.NombreUsuario))
                .ForMember(dest => dest.NombresCompletos, opt => opt.MapFrom(src => src.NombresCompletos))
                .ForMember(dest => dest.ApellidosCompletos, opt => opt.MapFrom(src => src.ApellidosCompletos))
                .ForMember(dest => dest.EsActivo, opt => opt.MapFrom(src => src.EsActivo))
                .ForMember(dest => dest.FechaRegistro, opt => opt.MapFrom(src => src.FechaRegistro))
                .ForMember(dest => dest.Direccion, opt => opt.MapFrom(src => src.Direccion))
                .ForMember(dest => dest.Ciudad, opt => opt.MapFrom(src => src.Ciudad))
                .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Telefono))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FechaInactivacion, opt => opt.MapFrom(src => src.FechaInactivacion))
                .ForMember(dest => dest.FechaUltimoLogin, opt => opt.MapFrom(src => src.FechaUltimoLogin))
                .ForMember(dest => dest.TipoCuenta, opt => opt.MapFrom(src => src.TipoCuenta))
                .ForMember(dest => dest.TarjProf, opt => opt.MapFrom(src => src.TarjProf))
                .ForMember(dest => dest.TipoContrato, opt => opt.MapFrom(src => src.TipoContrato))
                .ForMember(dest => dest.SujetoRetencion, opt => opt.MapFrom(src => src.SujetoRetencion))
                .ForMember(dest => dest.TipoIdTrib, opt => opt.MapFrom(src => src.TipoIdTrib))
                .ForMember(dest => dest.RazonSocial, opt => opt.MapFrom(src => src.RazonSocial))
                .ForMember(dest => dest.Afiliacion, opt => opt.MapFrom(src => src.Afiliacion))
                .ForMember(dest => dest.DiasPendientes, opt => opt.MapFrom(src => src.DiasPendientes))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => src.FechaCreacion));

            // Mapeo de Perfil a PerfilDto
            CreateMap<Perfil, PerfilDto>()
                .ForMember(dest => dest.Usuarios, opt => opt.MapFrom(src => src.CuentaUsuarios.Select(cu => cu.NombreUsuario).ToList()))
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Area.Nombre));

            // Mapeo de CuentaUsuarioDto a CuentaUsuario
            CreateMap<CuentaUsuarioDto, CuentaUsuario>()
                .ForMember(dest => dest.Perfiles, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => src.FechaCreacion))
                .ForMember(dest => dest.Direccion, opt => opt.MapFrom(src => src.Direccion))
                .ForMember(dest => dest.Ciudad, opt => opt.MapFrom(src => src.Ciudad))
                .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Telefono))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FechaInactivacion, opt => opt.MapFrom(src => src.FechaInactivacion))
                .ForMember(dest => dest.FechaUltimoLogin, opt => opt.MapFrom(src => src.FechaUltimoLogin))
                .ForMember(dest => dest.TipoCuenta, opt => opt.MapFrom(src => src.TipoCuenta))
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


