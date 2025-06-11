using Microsoft.AspNetCore.Http.Internal;
using StraySafe.Data.Database.Models.Sightings;
using StraySafe.Logic.ImageLogic;

namespace StraySafe.Test.ImageLogic;

[TestClass]
public class ImageMetadataTest
{
    private ImageMetadataClient? _imageMetadataClient;
    private FormFile? _fileWithExifData;
    private FormFile? _emptyFile;

    [TestInitialize]
    public async Task SetUpAsync()
    {
        _imageMetadataClient = new ImageMetadataClient();
        string imagePath = Path.Combine("Resources", "exifTest3.jpg");
        byte[] fileBytes = await File.ReadAllBytesAsync(imagePath);
        MemoryStream memoryStream = new(fileBytes);
        _fileWithExifData = new FormFile(memoryStream, 0, memoryStream.Length, "Image", "exifTest3.jpg");
        _emptyFile = new FormFile(null, 0, memoryStream.Length, string.Empty, string.Empty);
    }

    [TestMethod]
    public void GetDateTime_FromFile_GetsDateTimeFromExifProfile()
    {
        DateTime? result = _imageMetadataClient!.GetDateTime(_fileWithExifData!);
        DateTime expected = new(2025, 1, 17, 12, 55, 0, DateTimeKind.Unspecified);
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GetDateTime_FromEmptyFile_ReturnsNull()
    {
        DateTime? result = _imageMetadataClient!.GetDateTime(_emptyFile!);
        Assert.IsNull(result);
    }

    [TestMethod]
    public void GetCoordinates_FromFile_GetsCoordinatesFromExifProfile()
    {
        Coordinates? result = _imageMetadataClient!.GetCoordinates(_fileWithExifData!);
        Assert.IsNotNull(result);
        Assert.AreEqual(45, result.Latitude);
        Assert.AreEqual(-100, result.Longitude);
    }

    [TestMethod]
    public void GetCoordinates_FromEmptyFile_ReturnsNull()
    {
        Coordinates? result = _imageMetadataClient!.GetCoordinates(_emptyFile!);
        Assert.IsNull(result);
    }
}
