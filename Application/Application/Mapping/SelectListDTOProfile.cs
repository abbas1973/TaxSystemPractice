using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping
{
    public class SelectListDTOProfile : Profile
    {
        public SelectListDTOProfile()
        {
            CreateMap<Role, SelectListDTO>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title));

            CreateMap<UserRole, SelectListDTO>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Role.Title));

            CreateMap<Province, SelectListDTO>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name));

            CreateMap<City, SelectListDTO>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name));

        }
    }
}
