using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StraySafe.Nucleus.Database.Models.Users;

namespace StraySafe.Nucleus.Database;

public class DataContext : IdentityDbContext<User>
{
    protected readonly IConfiguration _configuration;
    public DataContext(DbContextOptions<DataContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetSection("straySafe")["connectionString"]);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("identity");
    }
}
