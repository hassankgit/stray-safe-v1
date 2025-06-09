using Microsoft.AspNetCore.Http.Internal;
using StraySafe.Data.Database.Models.Sightings;
using StraySafe.Logic.ImageLogic;

namespace StraySafe.Test.ImageLogic;

[TestClass]
public class ImageMetadataTest
{
    private ImageMetadataClient? _imageMetadataClient;
    private FormFile? _fileWithExifData;

    [TestInitialize]
    public async Task SetUpAsync()
    {
        _imageMetadataClient = new ImageMetadataClient();
        string imagePath = Path.Combine("Resources", "exifTest3.jpg");
        byte[] fileBytes = await File.ReadAllBytesAsync(imagePath);
        var memoryStream = new MemoryStream(fileBytes);
        _fileWithExifData = new FormFile(memoryStream, 0, memoryStream.Length, "Image", "exifTest3.jpg");
    }

    [TestMethod]
    public void GetDateTime_FromFile_GetsDateTimeFromExifProfile()
    {
        DateTime? result = _imageMetadataClient!.GetDateTime(_fileWithExifData!);
        DateTime expected = new(2025, 1, 17, 12, 55, 0, DateTimeKind.Unspecified);
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void GetCoordinates_FromFile_GetsCoordinatesFromExifProfile()
    {
        Coordinates? result = _imageMetadataClient!.GetCoordinates(_fileWithExifData!);
        Assert.IsNotNull(result);
        Assert.AreEqual(45, result.Latitude);
        Assert.AreEqual(-100, result.Longitude);
    }
}
