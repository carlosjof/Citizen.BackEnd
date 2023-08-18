using AutoMapper;
using PRUEBASB.Application.ViewModel;
using PRUEBASB.Domain.Entities;

namespace PRUEBASB.Application.AutoMapper
{
    public class MappingResource : Profile
    {
        public MappingResource()
        {
            CreateMap<CitizenCreateDto, Citizen>();

            CreateMap<CitizenUpdateDto, CitizenVMUpdate>();

            CreateMap<CitizenVMUpdate, CitizenVM>();
        }
    }
}
