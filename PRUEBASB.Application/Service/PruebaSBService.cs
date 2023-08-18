using AutoMapper;
using PRUEBASB.Application.Interface;
using PRUEBASB.Application.ViewModel;
using PRUEBASB.Domain.Entities;
using System.Net;

namespace PRUEBASB.Application.Service
{
    public class PruebaSBService : IPruebaSBService
    {
        private readonly IPruebaSBRepository _pruebaSBRepository;
        private readonly IMapper _mapper;

        public PruebaSBService(IPruebaSBRepository pruebaSBRepository, IMapper mapper)
        {
            _pruebaSBRepository = pruebaSBRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<CitizenVM>> GetPagedCitizen(int page, int pageSize)
        {
            PagedResponse<CitizenVM> result;

            result = await _pruebaSBRepository.GetPagedCitizen(page, pageSize);

            return result;
        }

        public async Task<SuccessResponse> GetCitizen(string cin)
        { 
            var result = await _pruebaSBRepository.GetCitizen(cin);

            if (result == null)
            {
                return new SuccessResponse(false, null, HttpStatusCode.NotImplemented, null);
            }

            var data = _mapper.Map<Citizen, CitizenVM>(result);

            return new SuccessResponse(true, data, HttpStatusCode.OK, null);
        }

        public async Task<SuccessResponse> CreateCitizen(Citizen citizen)
        {
            var result = await _pruebaSBRepository.CreateCitizen(citizen);

            if (result == null)
            {
                return new SuccessResponse(false, null, HttpStatusCode.NotImplemented, null);
            }

            var data = _mapper.Map<Citizen, CitizenVM>(result);

            return new SuccessResponse(true, data, HttpStatusCode.OK, null);
        }

        public async Task<bool> CitizenExist(string cin)
        {
            var result = await _pruebaSBRepository.CitizenExist(cin);

            return result;
        }

        public async Task<SuccessResponse> UpdateCitizen(string cin, CitizenVMUpdate citizen)
        {
            var result = await _pruebaSBRepository.UpdateCitizen(cin, citizen);

            if (result == null)
            {
                return new SuccessResponse(false, null, HttpStatusCode.NotImplemented, null);
            }

            var data = _mapper.Map<CitizenVMUpdate, CitizenVM>(result);

            return new SuccessResponse(true, data, HttpStatusCode.OK, null);
        }

        public async Task<SuccessResponse> DeleteCitizen(string cin)
        {
            var result = await _pruebaSBRepository.DeleteCitizen(cin);

            if (result == null)
            {
                return new SuccessResponse(false, null, HttpStatusCode.NotImplemented, null);
            }

            var data = _mapper.Map<Citizen, CitizenVM>(result);

            return new SuccessResponse(true, data, HttpStatusCode.OK, null);
        }
    }
}
