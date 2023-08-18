using PRUEBASB.Application.ViewModel;
using PRUEBASB.Domain.Entities;

namespace PRUEBASB.Application.Interface
{
    public interface IPruebaSBService
    {
        Task<PagedResponse<CitizenVM>> GetPagedCitizen(int page, int pageSize);
        Task<SuccessResponse> GetCitizen(string cin);
        Task<SuccessResponse> CreateCitizen(Citizen citizen);

        Task<Boolean> CitizenExist(string cin);
        Task<SuccessResponse> UpdateCitizen(string cin, CitizenVMUpdate citizen);
        Task<SuccessResponse> DeleteCitizen(string cin);
    }
}
