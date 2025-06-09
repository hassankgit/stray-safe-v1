using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using StraySafe.Data.Database.Models.Sightings;

namespace StraySafe.Logic.ImageLogic
{
    public class ImageMetadataClient
    {
        public DateTime? GetDateTime(IFormFile file)
        {
            Stream stream = file.OpenReadStream();
            Image image = Image.Load(stream);
            ExifProfile? exifProfile = image.Metadata.ExifProfile;
            if (exifProfile == null)
            {
                return null;
            }

            string? dateTimeString = exifProfile.Values.FirstOrDefault(x => x.Tag == ExifTag.DateTimeOriginal)?.GetValue() as string;
            if (string.IsNullOrEmpty(dateTimeString))
            {
                return null;
            }
            
            if (DateTime.TryParseExact(dateTimeString,"yyyy:MM:dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime dateTaken))
            {
                return dateTaken;
            }

            return null;
        }
        public Coordinates? GetCoordinates(IFormFile file)
        {
            Coordinates coordinates = new Coordinates();

            Stream stream = file.OpenReadStream();
            Image image = Image.Load(stream);
            ExifProfile? exifProfile = image.Metadata.ExifProfile;
            if (exifProfile == null)
            {
                return null;
            }

            Rational[]? gpsLatitude = exifProfile.Values.FirstOrDefault(x => x.Tag == ExifTag.GPSLatitude)?.GetValue() as Rational[];
            Rational[]? gpsLongitude = exifProfile.Values.FirstOrDefault(x => x.Tag == ExifTag.GPSLongitude)?.GetValue() as Rational[];
            string? gpsLatitudeRef = exifProfile.Values.FirstOrDefault(x => x.Tag == ExifTag.GPSLatitudeRef)?.GetValue() as string;
            string? gpsLongitudeRef = exifProfile.Values.FirstOrDefault(x => x.Tag == ExifTag.GPSLongitudeRef)?.GetValue() as string;

            if (gpsLatitude != null && gpsLatitudeRef != null && gpsLongitude != null && gpsLongitudeRef != null)
            {
                double latitude = ConvertGpsToDecimal(gpsLatitude, gpsLatitudeRef);
                double longitude = ConvertGpsToDecimal(gpsLongitude, gpsLongitudeRef);

                coordinates.Latitude = latitude;
                coordinates.Longitude = longitude;
            }

            return coordinates;
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
