using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace StraySafe.Nucleus.Database
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "StraySafe");

            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config.GetConnectionString("dev");
            optionsBuilder.UseNpgsql(connectionString);

            return new DataContext(optionsBuilder.Options, config);
        }
    }
}
