using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using StraySafe.Models;

namespace StraySafe.Services.ImageLogic
{
    public class ImageMetadataClient
    {
        public Coordinates GetCoordinates(IFormFile file)
        {
            Stream stream = file.OpenReadStream();
            Image image = Image.Load(stream);

            ExifProfile? exifProfile = image.Metadata.ExifProfile;

            if (exifProfile == null)
            {
                throw new Exception("No exif profile found for image.");
            }

            Rational[]? gpsLatitude = exifProfile.Values.FirstOrDefault(x => x.Tag == ExifTag.GPSLatitude)?.GetValue() as Rational[];
            Rational[]? gpsLongitude = exifProfile.Values.FirstOrDefault(x => x.Tag == ExifTag.GPSLongitude)?.GetValue() as Rational[];
            string? gpsLatitudeRef = exifProfile.Values.FirstOrDefault(x => x.Tag == ExifTag.GPSLatitudeRef)?.GetValue() as string;
            string? gpsLongitudeRef = exifProfile.Values.FirstOrDefault(x => x.Tag == ExifTag.GPSLongitudeRef)?.GetValue() as string;

            if (gpsLatitude != null && gpsLatitudeRef != null && gpsLongitude != null && gpsLongitudeRef != null)
            {
                double latitude = ConvertGpsToDecimal(gpsLatitude, gpsLatitudeRef);
                double longitude = ConvertGpsToDecimal(gpsLongitude, gpsLongitudeRef);
                
                return new Coordinates()
                {
                    Latitude = latitude,
                    Longitude = longitude,
                };
            }

            throw new Exception("No location found for image.");
        }

        private static double ConvertGpsToDecimal(Rational[] coordinates, string direction)
        {
            double gpsToDecimal = coordinates[0].ToDouble() +
                                   coordinates[1].ToDouble() / 60 +
                                   coordinates[2].ToDouble() / 3600;

            if (direction == "S" || direction == "W")
            {
                gpsToDecimal *= -1;
            }

            return gpsToDecimal;
        }
    }
}
