using Microsoft.AspNetCore.Http.Internal;
using StraySafe.Data.Database.Models.Sightings;
using StraySafe.Logic.ImageLogic;
using StraySafe.Test.MockedServices;

namespace StraySafe.Test.ImageLogic;

[TestClass]
public class ImageMetadataTest
{
    private FormFile? _fileWithExifData;
    private FormFile? _emptyFile;

    [TestInitialize]
    public async Task SetUpAsync()
    {
        _fileWithExifData = await FileMockFactory.GetFileWithExifData();
        _emptyFile = FileMockFactory.GetEmptyFile();
    }

    [TestMethod]
    public void GetDateTime_FromFile_GetsDateTimeFromExifProfile()
    {
        DateTime? actual = ImageMetadataClient.GetDateTime(_fileWithExifData!);
        DateTime expected = new(2025, 1, 17, 12, 55, 0, DateTimeKind.Unspecified);
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetDateTime_FromEmptyFile_ReturnsNull()
    {
        DateTime? actual = ImageMetadataClient.GetDateTime(_emptyFile!);
        Assert.IsNull(actual);
    }

    [TestMethod]
    public void GetCoordinates_FromFile_GetsCoordinatesFromExifProfile()
    {
        Coordinates? actual = ImageMetadataClient.GetCoordinates(_fileWithExifData!);
        Assert.IsNotNull(actual);
        Assert.AreEqual(45, actual.Latitude);
        Assert.AreEqual(-100, actual.Longitude);
    }

    [TestMethod]
    public void GetCoordinates_FromEmptyFile_ReturnsNull()
    {
        Coordinates? actual = ImageMetadataClient.GetCoordinates(_emptyFile!);
        Assert.IsNull(actual);
    }
}
