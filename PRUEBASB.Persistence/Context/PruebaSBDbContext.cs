using Microsoft.EntityFrameworkCore;
using PRUEBASB.Domain.Entities;

namespace PRUEBASB.Persistence.Context
{
    public class PruebaSBDbContext : DbContext
    {
        public PruebaSBDbContext(DbContextOptions<PruebaSBDbContext> options) : base(options) { }

        public DbSet<Citizen> Citizen { get; set; }
    }
}
