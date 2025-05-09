using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using StraySafe.Data.Database;
using StraySafe.Data.Database.Models.Sightings;
using StraySafe.Logic.Sightings.Models;

namespace StraySafe.Logic.Sightings;

public class SightingClient
{
    private readonly DataContext _context;

    public SightingClient(DataContext context)
    {
        _context = context;
    }

    public SightingDetail? GetSightingDetailById(int id)
    {
        SightingDetail? detail = _context.SightingDetails.Where(x => x.Id == id).FirstOrDefault();
        return detail;
    }

    public List<SightingPreview> GetSightingPreviewsByCoordinates(Coordinates coordinates)
    {
        if (coordinates.Latitude == null || coordinates.Longitude == null)
        {
            return new List<SightingPreview>();
        }

        MapBoundingBox boundingBox = GetBoundingBox((double)coordinates.Latitude, (double)coordinates.Longitude, 1);
        List<SightingPreview> sightingPreviewsInRange = _context.SightingPreviews.Where(
                x => x.Coordinates.Latitude <= boundingBox.MaxLat &&
                x.Coordinates.Latitude >= boundingBox.MinLat &&
                x.Coordinates.Longitude <= boundingBox.MaxLng &&
                x.Coordinates.Longitude >= boundingBox.MinLng)
            .ToList();
        return sightingPreviewsInRange;
    }

    private MapBoundingBox GetBoundingBox(double lat, double lng, double radiusInMiles)
    {
        double latOffset = radiusInMiles / 69.0;
        double lngOffset = radiusInMiles / (69.0 * Math.Cos(lat * Math.PI / 180.0));

        double minLat = lat - latOffset;
        double maxLat = lat + latOffset;
        double minLng = lng - lngOffset;
        double maxLng = lng + lngOffset;

        return new MapBoundingBox()
        {
            MinLat = minLat,
            MaxLat = maxLat,
            MinLng = minLng,
            MaxLng = maxLng,
        };
    }
}
