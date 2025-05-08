using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StraySafe.Data.Database.Models.Sightings;

namespace StraySafe.Data.Database;

public class DataContext : DbContext
{
    protected readonly IConfiguration _config;

    public DbSet<SightingPreview> SightingPreviews { get; set; }
    public DbSet<SightingDetail> SightingDetails { get; set; }

    public DataContext(DbContextOptions<DataContext> options, IConfiguration config)
        : base(options)
    {
        _config = config;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<SightingPreview>()
        .OwnsOne(x => x.Coordinates);

        builder.Entity<SightingDetail>()
       .OwnsOne(x => x.Tags);

        base.OnModelCreating(builder);
    }
}
