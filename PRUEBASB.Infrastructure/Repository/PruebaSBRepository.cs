using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PRUEBASB.Application.Interface;
using PRUEBASB.Application.ViewModel;
using PRUEBASB.Domain.Entities;
using PRUEBASB.Persistence.Context;

namespace PRUEBASB.Infrastructure.Repository
{
    public class PruebaSBRepository : IPruebaSBRepository
    {
        private readonly PruebaSBDbContext _context;
        private readonly IMapper _mapper;

        public PruebaSBRepository(PruebaSBDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedResponse<CitizenVM>> GetPagedCitizen(int page, int pageSize)
        {
            try
            {
                var query = _context.Citizen.AsQueryable();
                var count = await query.Where(c => c.Status.Equals(true)).CountAsync();
                var totalPages = (int)Math.Ceiling((double)count / pageSize);

                var result = await query.Where(c => c.Status.Equals(true))
                                      .Skip((page - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToListAsync();

                var data = _mapper.Map<List<Citizen>, List<CitizenVM>>(result);

                var hasNext = page < totalPages;
                var hasPrevious = page > 1;

                return new PagedResponse<CitizenVM>
                {
                    Count = count,
                    TotalPage = totalPages,
                    HasNext = hasNext,
                    HasPrevious = hasPrevious,
                    Data = data,
                };

            }
            catch (Exception err)
            {

                return null;
            }
        }

        public async Task<Citizen> GetCitizen(string cin)
        {
            try
            {
                var result = await _context.Citizen.Where(c => c.Status.Equals(true)).FirstOrDefaultAsync(c => c.CIN.Equals(cin));
                if (result == null)
                {
                    return new Citizen
                    {
                        Status = false
                    };
                }

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Citizen> CreateCitizen(Citizen citizen)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {

                await _context.Citizen.AddAsync(citizen);
                await _context.SaveChangesAsync();

                transaction.Commit();

                return citizen;
            }
            catch (Exception ex)
            {

                transaction.Rollback();
                return null;
            }
        }

        public async Task<bool> CitizenExist(string cin)
        {
            try
            {
                var result = await _context.Citizen.AnyAsync(c => c.CIN.Equals(cin));
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<CitizenVMUpdate> UpdateCitizen(string cin, CitizenVMUpdate citizen)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var existingCitizen = await _context.Citizen.FirstOrDefaultAsync(c => c.CIN.Equals(cin));
                if (existingCitizen == null)
                {
                    return null;
                }

                citizen.CIN = cin;
                if (citizen.Status == null)
                {
                    citizen.Status = existingCitizen.Status;
                }
                existingCitizen.DtEdit = DateTime.Now;

                _context.Entry(existingCitizen).CurrentValues.SetValues(citizen);
                await _context.SaveChangesAsync();

                transaction.Commit();

                return citizen;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return null;
            }
        }

        public async Task<Citizen> DeleteCitizen(string cin)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var existingCitizen = await _context.Citizen.FirstOrDefaultAsync(c => c.CIN.Equals(cin));
                if (existingCitizen == null)
                {
                    return null;
                }

                existingCitizen.Status = false;
                existingCitizen.DtEdit = DateTime.Now;

                await _context.SaveChangesAsync();

                transaction.Commit();

                return existingCitizen;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return null;
            }
        }
    }
}
