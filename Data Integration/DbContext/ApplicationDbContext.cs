using Microsoft.EntityFrameworkCore;

namespace Data_Integration
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           // GlobalQueryFilters.CreateGlobalIsDeletedQueryFilter(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
    }
}