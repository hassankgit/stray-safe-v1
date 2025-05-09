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
        .OwnsOne(x => x.Tags, tags =>
        {
            tags.Property(t => t.Status)
                .HasConversion<int>();

            tags.Property(t => t.Behavior)
                .HasConversion<int>();

            tags.Property(t => t.Health)
                .HasConversion<int>();
        });

        builder.Entity<SightingDetail>()
            .Property(x => x.Age)
            .HasConversion<int>();

        builder.Entity<SightingDetail>()
            .Property(x => x.Sex)
            .HasConversion<int>();

        base.OnModelCreating(builder);
    }
}
