using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace StraySafe.Nucleus.Database
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<DataContext> optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            string? basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "StraySafe");
            IConfigurationRoot? config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<DataContext>()
                .Build();

            optionsBuilder.UseNpgsql(config.GetConnectionString("straySafe"));

            return new DataContext(optionsBuilder.Options, config);
        }
    }
}
