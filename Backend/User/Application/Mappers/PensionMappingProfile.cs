using AutoMapper;
using PhAppUser.Domain.Entities;
using PhAppUser.Application.DTOs;

namespace PhAppUser.Application.Mappers
{
    public class PensionMappinProfile : Profile
    {
        public PensionMappinProfile()
        {
            // Mapeo desde Pension a PensionDto
            CreateMap<Pension, PensionDto>();

            // Mapeo desde PensionDto a Pension
            CreateMap<PensionDto, Pension>();
        }
    }
}