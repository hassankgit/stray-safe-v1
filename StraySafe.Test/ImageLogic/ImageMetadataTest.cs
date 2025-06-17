using Microsoft.AspNetCore.Http.Internal;
using StraySafe.Data.Database.Models.Sightings;
using StraySafe.Logic.ImageLogic;
using StraySafe.Test.MockedServices;

namespace StraySafe.Test.ImageLogic;

[TestClass]
public class ImageMetadataTest
{
    private ImageMetadataClient? _imageMetadataClient;
    private FileMockFactory? _fileMockFactory;
    private FormFile? _fileWithExifData;
    private FormFile? _emptyFile;

    [TestInitialize]
    public async Task SetUpAsync()
    {
        _imageMetadataClient = new ImageMetadataClient();
        _fileMockFactory = new FileMockFactory();
        _fileWithExifData = await _fileMockFactory!.GetFileWithExifData();
        _emptyFile = _fileMockFactory.GetEmptyFile();
    }

    [TestMethod]
    public void GetDateTime_FromFile_GetsDateTimeFromExifProfile()
    {
        DateTime? actual = _imageMetadataClient!.GetDateTime(_fileWithExifData!);
        DateTime expected = new(2025, 1, 17, 12, 55, 0, DateTimeKind.Unspecified);
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetDateTime_FromEmptyFile_ReturnsNull()
    {
        DateTime? actual = _imageMetadataClient!.GetDateTime(_emptyFile!);
        Assert.IsNull(actual);
    }

    [TestMethod]
    public void GetCoordinates_FromFile_GetsCoordinatesFromExifProfile()
    {
        Coordinates? actual = _imageMetadataClient!.GetCoordinates(_fileWithExifData!);
        Assert.IsNotNull(actual);
        Assert.AreEqual(45, actual.Latitude);
        Assert.AreEqual(-100, actual.Longitude);
    }

    [TestMethod]
    public void GetCoordinates_FromEmptyFile_ReturnsNull()
    {
        Coordinates? actual = _imageMetadataClient!.GetCoordinates(_emptyFile!);
        Assert.IsNull(actual);
    }
}
