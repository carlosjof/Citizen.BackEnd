using AutoMapper;
using PRUEBASB.Application.ViewModel;
using PRUEBASB.Domain.Entities;

namespace PRUEBASB.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<Citizen, CitizenVM>()
                .ForMember(dest => dest.CIN, src => src.MapFrom(c => c.CIN))
                .ForMember(dest => dest.Name, src => src.MapFrom(c => c.Name))
                .ForMember(dest => dest.LastName, src => src.MapFrom(c => c.LastName))
                .ForMember(dest => dest.Age, src => src.MapFrom(c => c.Age))
                .ForMember(dest => dest.Gender, src => src.MapFrom(c => c.Gender))
                .ForMember(dest => dest.Status, src => src.MapFrom(c => c.Status));

            CreateMap<Citizen, CitizenVMUpdate>();
        }
    }
}
