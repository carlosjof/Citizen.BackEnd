using PRUEBASB.Application.ViewModel;
using PRUEBASB.Domain.Entities;

namespace PRUEBASB.Application.Interface
{
    public interface IPruebaSBRepository
    {
        Task<PagedResponse<CitizenVM>> GetPagedCitizen(int page, int pageSize);
        Task<Citizen> GetCitizen(string cin);
        Task<Citizen> CreateCitizen(Citizen citizen);
        Task<Boolean> CitizenExist(string cin);
        Task<CitizenVMUpdate> UpdateCitizen(string cin, CitizenVMUpdate citizen);
        Task<Citizen> DeleteCitizen(string cin);
    }
}
