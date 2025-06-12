using AutoMapper;
using Integration.Supabase.Interfaces;
using Microsoft.AspNetCore.Http.Internal;
using Moq;
using StraySafe.Data.Database;
using StraySafe.Data.Database.Models.Sightings;
using StraySafe.Logic.ImageLogic;
using StraySafe.Logic.Mappers;
using StraySafe.Logic.Sightings;
using StraySafe.Logic.Sightings.Models;
using StraySafe.Test.MockedServices;

namespace StraySafe.Test.Sightings;

[TestClass]
public class SightingClientTest
{
    private DataContext? _context;
    private IMapper? _mapper;
    private Mock<ISupabaseService>? _supabaseServiceMock;
    private Mock<ImageMetadataClient>? _imageMetadataClientMock;

    private FormFile? _fileWithExifData;
    private SightingClient? _sightingClient;

    [TestInitialize]
    public async Task SetUp()
    {
        string imagePath = Path.Combine("Resources", "exifTest3.jpg");
        byte[] fileBytes = await File.ReadAllBytesAsync(imagePath);
        MemoryStream memoryStream = new(fileBytes);
        _fileWithExifData = new FormFile(memoryStream, 0, memoryStream.Length, "Image", "exifTest3.jpg");

        _context = DataContextMockFactory.Mock(Guid.NewGuid().ToString());
        _mapper = new MapperConfiguration(c => c.AddProfile<SightingDetailMapper>()).CreateMapper(); 
        _supabaseServiceMock = SupabaseServiceMockFactory.Mock();
        _imageMetadataClientMock = new Mock<ImageMetadataClient>();

        _sightingClient = new SightingClient(
            _context,
            _mapper,
            _supabaseServiceMock.Object,
            _imageMetadataClientMock.Object
        );
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
    public void GetSightingDetailById_CanGetByNonexistentId_ReturnsNullDto()
    {
        SightingDetailDto? actual = _sightingClient!.GetSightingDetailById(20);
        Assert.IsNull(actual);
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

    private static void AssertEqual(UploadResponseDto expected, UploadResponseDto actual)
    {
        Assert.AreEqual(expected.Url, actual.Url);
        Assert.AreEqual(expected.DateTime, actual.DateTime);
        Assert.AreEqual(expected.Coordinates!.Latitude, actual.Coordinates!.Latitude);
        Assert.AreEqual(expected.Coordinates!.Longitude, actual.Coordinates!.Longitude);
    }
}
