using AutoMapper;
using PhAppUser.Domain.Entities;
using PhAppUser.Aplication.DTOs;

namespace PhAppUser.Application.Mappers
{
    public class AreaMappingProfile : Profile
    {
        public AreaMappingProfile()
        {
            // Mapeo simplificado de Area a DTO y viceversa
            CreateMap<Area, AreaDto>().ReverseMap();
        }
    }
}