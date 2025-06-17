using Microsoft.AspNetCore.Http.Internal;

namespace StraySafe.Test.MockedServices;

public class FileMockFactory
{
    public static async Task<FormFile> GetFileWithExifData()
    {
        string imagePath = Path.Combine("Resources", "exifTest3.jpg");
        byte[] fileBytes = await File.ReadAllBytesAsync(imagePath);
        MemoryStream memoryStream = new(fileBytes);
        FormFile fileWithExifData = new(memoryStream, 0, memoryStream.Length, "Image", "exifTest3.jpg");
        return fileWithExifData;
    }

    public static FormFile GetEmptyFile()
    {
        return new FormFile(null, 0, 0, string.Empty, string.Empty);
    }
}
