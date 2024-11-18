using AutoMapper;
using PhAppUser.Application.DTOs;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Application.Mappers
{
    public class CuentaUsuarioMappingProfile : Profile
    {
        public CuentaUsuarioMappingProfile()
        {
            CreateMap<CuentaUsuario, CuentaUsuarioDto>(). ReverseMap();
        }
    }
}