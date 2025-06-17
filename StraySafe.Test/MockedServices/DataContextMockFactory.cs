using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StraySafe.Data.Database;
using StraySafe.Data.Database.Enums;
using StraySafe.Data.Database.Models.Sightings;

namespace StraySafe.Test.MockedServices;

public static class DataContextMockFactory
{
    public static DataContext Mock(out SqliteConnection connection)
    {
        connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        DbContextOptions<DataContext> options = new DbContextOptionsBuilder<DataContext>()
            .UseSqlite(connection)
            .Options;

        IConfigurationRoot config = new ConfigurationBuilder().Build();
        DataContext context = new(options, config);
        context.Database.EnsureCreated();

        context.SightingDetails.AddRange(
            new SightingDetail
            {
                Id = 1,
                Name = "Mano",
                Species = "Cat",
                Breed = "Tabby",
                Age = EAnimalAge.SIX_TO_TWELVE_MONTHS,
                Sex = EAnimalSex.FEMALE,
                ImageUrl = "https://www.google.com",
                LastSpotted = new DateTime(2025, 12, 10, 5, 5, 5, DateTimeKind.Utc),
                Location = "Czechoslovakia",
                Tags = new SightingTags
                {
                    Status = EAnimalStatus.STILL_ROAMING,
                    Behavior = EAnimalBehavior.TIMID,
                    Health = EAnimalHealth.HEALTHY,
                },
                SubmittedById = "123456789",
                SubmittedByName = "Joe Mama"
            },
            new SightingDetail
            {
                Id = 2,
                Name = "Hercules",
                Species = "Cat",
                Breed = "Tuxedo",
                Age = EAnimalAge.TWO_TO_FIVE_YEARS,
                Sex = EAnimalSex.MALE,
                ImageUrl = "https://www.google.com",
                LastSpotted = new DateTime(2024, 10, 10, 5, 5, 5, DateTimeKind.Utc),
                Location = "3400 Lancaster, Philadelphia, PA, 19104",
                Tags = new SightingTags
                {
                    Status = EAnimalStatus.STILL_ROAMING,
                    Behavior = EAnimalBehavior.TIMID,
                    Health = EAnimalHealth.HEALTHY,
                },
                SubmittedById = "987654321",
                SubmittedByName = "Zeus"
            }
        );

        context.SightingPreviews.AddRange(
            new SightingPreview
            {
                Id = 1,
                Name = "Mano",
                Species = "Cat",
                Breed = "Tabby",
                ImageUrl = "https://www.google.com",
                LastSpotted = new DateTime(2025, 12, 10, 5, 5, 5, DateTimeKind.Utc),
                Coordinates = new Coordinates
                {
                    Latitude = -45.00,
                    Longitude = 100.00
                },
                SubmittedById = "123456789",
                SightingDetailId = 1
            },
            new SightingPreview
            {
                Id = 2,
                Name = "Hercules",
                Species = "Cat",
                Breed = "Tuxedo",
                ImageUrl = "https://www.google.com",
                LastSpotted = new DateTime(2024, 10, 10, 5, 5, 5, DateTimeKind.Utc),
                Coordinates = new Coordinates
                {
                    Latitude = -30.00,
                    Longitude = 70.00
                },
                SubmittedById = "987654321",
                SightingDetailId = 2
            }
        );

        context.SaveChanges();

        return context;
    }
}