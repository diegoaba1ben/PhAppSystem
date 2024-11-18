using AutoMapper;
using PhAppUser.Application.DTOs;
using PhAppUser.Domain.Entities;

namespace PhAppUser.Application.Mappers
{
    public class RepLegalMappingProfile : Profile
    {
        public RepLegalMappingProfile()
        {
            // Mapear de entidad a DTO
            CreateMap<RepLegal, RepLegalDto>();

            // Mapear de DTO a Entidad
            CreateMap<RepLegalDto, RepLegal>();
        }
    }
}