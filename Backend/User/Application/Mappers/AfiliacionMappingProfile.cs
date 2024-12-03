using AutoMapper;
using PhAppUser.Application.DTOs;
using PhAppUser.Domain.Entities;
using PhAppUser.Domain.Enums;

namespace PhAppUser.Application.Mappers
{
    public class AfiliacionSSMappingProfile : Profile
    {
        public AfiliacionSSMappingProfile()
        {
            // Mapeo de CuentaUsuario a AfiliacionSSDto
            CreateMap<CuentaUsuario, AfiliacionSSDto>()
                .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NombresCompletos, opt => opt.MapFrom(src => src.NombresCompletos))
                .ForMember(dest => dest.ApellidosCompletos, opt => opt.MapFrom(src => src.ApellidosCompletos))
                .ForMember(dest => dest.Identificacion, opt => opt.MapFrom(src => src.Identificacion))
                .ForMember(dest => dest.Afiliacion, opt => opt.MapFrom(src => src.Afiliacion.ToString())) // Convertir enum a string
                .ForMember(dest => dest.DiasPendientes, opt => opt.MapFrom(src => src.DiasPendientes))
                .ForMember(dest => dest.Salud, opt => opt.MapFrom(src => src.Salud)) // Usar mapeo existente
                .ForMember(dest => dest.Pension, opt => opt.MapFrom(src => src.Pension)); // Usar mapeo existente

            // Mapeo inverso de AfiliacionSSDto a CuentaUsuario
            CreateMap<AfiliacionSSDto, CuentaUsuario>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UsuarioId))
                .ForMember(dest => dest.NombresCompletos, opt => opt.MapFrom(src => src.NombresCompletos))
                .ForMember(dest => dest.ApellidosCompletos, opt => opt.MapFrom(src => src.ApellidosCompletos))
                .ForMember(dest => dest.Identificacion, opt => opt.MapFrom(src => src.Identificacion))
                .ForMember(dest => dest.Afiliacion, opt => opt.MapFrom(src => Enum.Parse<Afiliacion>(src.Afiliacion))) // Convertir string a enum
                .ForMember(dest => dest.DiasPendientes, opt => opt.MapFrom(src => src.DiasPendientes))
                .ForMember(dest => dest.Salud, opt => opt.Ignore()) // Ignorar para manejar con lógica adicional
                .ForMember(dest => dest.Pension, opt => opt.Ignore()); // Ignorar para manejar con lógica adicional
        }
    }
}
