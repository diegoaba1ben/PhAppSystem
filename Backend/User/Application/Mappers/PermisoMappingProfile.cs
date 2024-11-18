using AutoMapper;
using PhAppUser.Domain.Entities;
using PhAppUser.Application.DTOs;

namespace PhAppUser.Application.Mappers
{
    public class PermisoMappingProfile : Profile
    {
       public PermisoMappingProfile()
       {
            // Mapear de Entidad a DTO
            CreateMap<Permiso, PermisoDto>();

            // Mapear de DTO a Entidad
            CreateMap<PermisoDto, Permiso>();
       }
    }
}