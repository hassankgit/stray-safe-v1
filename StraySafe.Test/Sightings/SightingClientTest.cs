using Integration.Supabase.Interfaces;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Data.Sqlite;
using Moq;
using StraySafe.Data.Database;
using StraySafe.Data.Database.Enums;
using StraySafe.Data.Database.Models.Sightings;
using StraySafe.Logic.Sightings;
using StraySafe.Logic.Sightings.Models;
using StraySafe.Test.MockedServices;

namespace StraySafe.Test.Sightings;

[TestClass]
public class SightingClientTest
{
    private DataContext? _context;
    private SqliteConnection? _connection;
    private Mock<ISupabaseService>? _supabaseServiceMock;
    private FormFile? _fileWithExifData;
    private SightingClient? _sightingClient;

    [TestInitialize]
    public async Task SetUp()
    {
        _context = DataContextMockFactory.Mock(out _connection);
        _supabaseServiceMock = SupabaseServiceMockFactory.Mock();
        _fileWithExifData = await FileMockFactory.GetFileWithExifData();
        _sightingClient = new SightingClient(_context, _supabaseServiceMock.Object);
    }

    [TestCleanup]
    public void CleanUp()
    {
        _context!.Dispose();
        _connection!.Close();
    }

    [TestMethod]
    public void GetSightingDetailById_CanGetById_ReturnsSightingDetail()
    {
        SightingDetailDto? actual = _sightingClient!.GetSightingDetailById(1);
        Assert.IsNotNull(actual);
        Assert.AreEqual(1, actual.Id);
        Assert.AreEqual("Mano", actual.Name);
    }

    [TestMethod]
    public void GetSightingDetailById_CanGetByNonexistentId_ThrowsException()
    {
        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            _sightingClient!.GetSightingDetailById(20);
        });
    }

    [TestMethod]
    public async Task UploadImage_CanUploadImage_ReturnsUploadResponseDto()
    {
        UploadResponseDto? actual = await _sightingClient!.UploadImage(_fileWithExifData!);
        UploadResponseDto expected = new()
        {
            Coordinates = new()
            {
                Latitude = 45.00,
                Longitude = -100.00
            },
            DateTime = new(2025, 1, 17, 12, 55, 0, DateTimeKind.Unspecified),
            Url = "https://www.google.com"
        };

        Assert.IsNotNull(actual);
        AssertEqual(expected, actual);
    }

    [TestMethod]
    public async Task CreateSighting_CanCreateSighting_CreatesSighting()
    {
        CreateSightingRequest request = new()
        {
            Name = "Sally",
            Species = "Dog",
            Breed = "Golden Retriever",
            DateTime = new(2025, 10, 10, 10, 10, 10, 10, DateTimeKind.Unspecified),
            Coordinates = new()
            {
                Latitude = -30,
                Longitude = 60,
            },
            Location = "Somewhere",
            ImageUrl = "https://www.google.com",
            Age = EAnimalAge.SIX_TO_TWELVE_MONTHS,
            Sex = EAnimalSex.MALE,
            Status = EAnimalStatus.STILL_ROAMING,
            Behavior = EAnimalBehavior.FRIENDLY,
            Health = EAnimalHealth.HEALTHY,
            Notes = "notesnotesnotes!"
        };

        CreateSightingResponseDto response = await _sightingClient!.CreateSighting(request);
        SightingDetail? detail = _context!.SightingDetails.FirstOrDefault(x => x.Id == response.SightingId);
        Assert.AreEqual(detail!.Id, response.SightingId);
    }

    private static void AssertEqual(UploadResponseDto expected, UploadResponseDto actual)
    {
        Assert.AreEqual(expected.Url, actual.Url);
        Assert.AreEqual(expected.DateTime, actual.DateTime);
        Assert.AreEqual(expected.Coordinates!.Latitude, actual.Coordinates!.Latitude);
        Assert.AreEqual(expected.Coordinates!.Longitude, actual.Coordinates!.Longitude);
    }
}
