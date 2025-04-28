using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StraySafe.Data.Database.Models.Users;

namespace StraySafe.Data.Database;

public class DataContext : IdentityDbContext<User>
{
    protected readonly IConfiguration _config;
    public DataContext(DbContextOptions<DataContext> options, IConfiguration config)
        : base(options)
    {
        _config = config;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("identity");
    }
}
