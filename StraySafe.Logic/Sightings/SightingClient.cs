using AutoMapper;
using Integration.Supabase.Interfaces;
using Microsoft.AspNetCore.Http;
using StraySafe.Data.Database;
using StraySafe.Data.Database.Models.Sightings;
using StraySafe.Logic.Sightings.Models;

namespace StraySafe.Logic.Sightings;

public class SightingClient
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly ISupabaseService _supabaseService;

    public SightingClient(DataContext context,
                          IMapper mapper,
                          ISupabaseService supabaseService)
    {
        _context = context;
        _mapper = mapper;
        _supabaseService = supabaseService;
    }

    public SightingDetailDto? GetSightingDetailById(int id)
    {
        SightingDetail? detail = _context.SightingDetails.Where(x => x.Id == id).FirstOrDefault();
        SightingDetailDto dto = _mapper.Map<SightingDetailDto>(detail);
        return dto;
    }

    public async Task<string> UploadImage(IFormFile image)
    {
        return await _supabaseService.User.UploadImage(image);
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
