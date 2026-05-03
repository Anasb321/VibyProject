using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using VibyApp.DB.Data;

namespace VibyApp.DB
{
    public class VibyDbContextFactory : IDesignTimeDbContextFactory<VibyDbContext>
    {
        public VibyDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<VibyDbContext>();
            optionsBuilder.UseSqlite("Data Source=viby.db");

            return new VibyDbContext(optionsBuilder.Options);
        }
    }
}
