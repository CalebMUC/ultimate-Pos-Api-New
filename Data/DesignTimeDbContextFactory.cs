using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Ultimate_POS_Api.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<UltimateDBContext>
    {
        public UltimateDBContext CreateDbContext(string[] args)
        {
            // Build configuration (reads appsettings.json)
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<UltimateDBContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new UltimateDBContext(optionsBuilder.Options);
        }
    }
}
